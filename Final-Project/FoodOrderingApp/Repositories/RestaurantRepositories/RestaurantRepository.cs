using FoodOrderingApp.Context;
using FoodOrderingApp.Dto;
using FoodOrderingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingApp.Repositories.RestaurantRepositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly AppDBContext _context;

        public RestaurantRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Restaurant?> GetProfile(int ownerId)
        {
            return await _context.Restaurants.FirstOrDefaultAsync(r => r.OwnerId == ownerId);
        }

        public async Task<Restaurant> UpdateProfile(Restaurant restaurant)
        {
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<MenuItem> AddMenuItem(MenuItem item)
        {
            _context.MenuItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<MenuItem?> UpdateMenuItem(MenuItem item)
        {
            var existing = await _context.MenuItems.FindAsync(item.MenuItemId);
            if (existing == null) return null;

            existing.Name = item.Name;
            existing.Description = item.Description;
            existing.Price = item.Price;
            existing.IsAvailable = item.IsAvailable;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteMenuItem(int menuItemId)
        {
            var item = await _context.MenuItems.FindAsync(menuItemId);
            if (item == null) return false;

            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<MenuItem>> GetMenu(int restaurantId)
        {
            return await _context.MenuItems
                .Where(m => m.RestaurantId == restaurantId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrders(int restaurantId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem)
                .Where(o => o.RestaurantId == restaurantId)
                .ToListAsync();
        }

        public async Task<Order?> UpdateOrderStatus(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return null;

            order.DeliveryStatus = status;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<MenuItem> GetMenuItemById(int id)
        {
            return await _context.MenuItems
                .FirstOrDefaultAsync(m => m.MenuItemId == id);
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }
        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            return await _context.Restaurants.Include(m=>m.MenuItems).ToListAsync();
        }
    }
}

