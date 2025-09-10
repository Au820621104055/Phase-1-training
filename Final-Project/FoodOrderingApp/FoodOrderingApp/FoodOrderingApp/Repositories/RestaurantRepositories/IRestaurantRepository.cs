using FoodOrderingApp.Dto;
using FoodOrderingApp.Dto.Restaurant;
using FoodOrderingApp.Models;
using Microsoft.AspNetCore.Mvc;

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
        Task<Restaurant> Add(Restaurant restaurant);
        Task<IEnumerable<MenuItem>> GetMenu(int restaurantId);
        Task<MenuItem> GetMenuItemById(int id);
        Task<Order> GetOrderById(int id);
        Task<IEnumerable<RestaurantWithStatusDto>> GetMyRestaurantsWithStatus(int ownerId);
        Task<IEnumerable<Order>> GetOrders(int restaurantId);
        Task<Order?> UpdateOrderStatus(int orderId, string status);
        Task<IEnumerable<Restaurant>> GetByOwnerAsync(int ownerId);
        Task<RestaurantValidation> SubmitForValidation(RestaurantValidation restaurant);
        Task<IEnumerable<RestaurantValidation>> GetPendingRestaurants();
        Task<RestaurantValidation> UpdateRestaurantStatus(int id, string status);
    }
}
