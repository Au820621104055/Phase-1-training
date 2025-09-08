using FoodOrderingApp.Context;
using FoodOrderingApp.Dto.User;
using FoodOrderingApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace FoodOrderingApp.Repositories.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _context;

        public UserRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> updateuserinfo(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<Restaurant>> BrowseRestaurants(string cuisine)
        {
            var query = _context.Restaurants.AsQueryable();
            if (!string.IsNullOrEmpty(cuisine))
                query = query.Where(r => r.CuisineType.Contains(cuisine));
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<MenuItem>> GetMenu(int restaurantId)
        {
            return await _context.MenuItems
                .Where(m => m.RestaurantId == restaurantId && m.IsAvailable)
                .ToListAsync();
        }

        public async Task<Order> PlaceOrder(Order order, List<OrderItem> items)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in items)
            {
                item.OrderId = order.OrderId;
                _context.OrderItems.Add(item);
            }
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Payment> MakePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Order?> TrackOrder(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }
    }
}

