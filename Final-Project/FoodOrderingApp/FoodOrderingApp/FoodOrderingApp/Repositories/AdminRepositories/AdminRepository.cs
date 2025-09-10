using FoodOrderingApp.Context;
using FoodOrderingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingApp.Repositories.AdminRepositories
{
    public class AdminRepository:IAdminRepository
    {
        private readonly AppDBContext _context;

        public AdminRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> UpdateUserStatus(int userId, bool isActive)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            user.IsActive = isActive;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserById(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Restaurant)
                .Include(o => o.DeliveryPerson)
                .ToListAsync();
        }

        public async Task<IEnumerable<Restaurant>> GetAllRestaurants()
        {
            return await _context.Restaurants.ToListAsync();
        }

        public async Task<User?> AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateUser(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);
            if (existingUser == null) return null;

            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;
            existingUser.IsActive = user.IsActive;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RestaurantValidation>> GetPendingRestaurants()
        {
            return await _context.ValidRes
                .Where(r => r.Status == "Pending")
                .ToListAsync();
        }

        public async Task<RestaurantValidation?> UpdateRestaurantStatus(int restaurantId, string status)
        {
            var restaurant = await _context.ValidRes
                .FirstOrDefaultAsync(r => r.RestaurantId == restaurantId);

            if (restaurant == null) return null;

            restaurant.Status = status;
            await _context.SaveChangesAsync();

            return restaurant;
        }
    }
}

