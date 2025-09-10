using FoodOrderingApp.Context;
using FoodOrderingApp.Dto;
using FoodOrderingApp.Dto.Restaurant;
using FoodOrderingApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<Restaurant> Add(Restaurant res)
        {
            _context.AddAsync(res);
            await _context.SaveChangesAsync();
            return res;

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
        public async Task<IEnumerable<Restaurant>> GetByOwnerAsync(int ownerId)
        {
            return await _context.Restaurants
                                 .Where(r => r.OwnerId == ownerId)
                                 .Include(r => r.MenuItems)
                                 .ToListAsync();
        }
        public async Task<RestaurantValidation> SubmitForValidation(RestaurantValidation restaurant)
        {
            restaurant.Status = "Pending";
            _context.ValidRes.Add(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<IEnumerable<RestaurantValidation>> GetPendingRestaurants()
        {
            return await _context.ValidRes.Where(r => r.Status == "Pending").ToListAsync();
        }

        public async Task<RestaurantValidation> UpdateRestaurantStatus(int id, string status)
        {
            var restaurant = await _context.ValidRes.FindAsync(id);
            if (restaurant == null) return null;
            restaurant.Status = status;
            await _context.SaveChangesAsync();
            return restaurant;
        }
        public async Task<IEnumerable<RestaurantWithStatusDto>> GetMyRestaurantsWithStatus(int ownerId)
        {
            var restaurants = await _context.Restaurants
                .Where(r => r.OwnerId == ownerId)
                .Join(_context.ValidRes,
                      r => r.RestaurantId,
                      v => v.RestaurantId,
                      (r, v) => new RestaurantWithStatusDto
                      {
                          RestaurantId = r.RestaurantId,
                          Name = r.Name,
                          Address = r.Address,
                          CuisineType = r.CuisineType,
                          PhoneNumber = r.PhoneNumber,
                          OwnerId = r.OwnerId,
                          Status = v.Status
                      })
                .ToListAsync();

            return restaurants;
        }
    }
}

