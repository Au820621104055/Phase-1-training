using FoodOrderingApp.Dto;
using FoodOrderingApp.Models;

namespace FoodOrderingApp.Repositories.RestaurantRepositories
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<Restaurant?> GetProfile(int ownerId);
        Task<Restaurant> UpdateProfile(Restaurant restaurant);

        Task<MenuItem> AddMenuItem(MenuItem item);
        Task<MenuItem?> UpdateMenuItem(MenuItem item);
        Task<bool> DeleteMenuItem(int menuItemId);
        Task<IEnumerable<MenuItem>> GetMenu(int restaurantId);
        Task<MenuItem> GetMenuItemById(int id);
        Task<Order> GetOrderById(int id);

        Task<IEnumerable<Order>> GetOrders(int restaurantId);
        Task<Order?> UpdateOrderStatus(int orderId, string status);
    }
}
