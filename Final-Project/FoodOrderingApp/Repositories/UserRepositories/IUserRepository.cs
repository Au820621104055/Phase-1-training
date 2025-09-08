using FoodOrderingApp.Dto.User;
using FoodOrderingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingApp.Repositories.UserRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> AddAsync(User user);
        Task<IEnumerable<Restaurant>> BrowseRestaurants(string? cuisine = null);
        Task<IEnumerable<MenuItem>> GetMenu(int restaurantId);
        Task<Order> PlaceOrder(Order order, List<OrderItem> items);
        Task<Payment> MakePayment(Payment payment);
        Task<Order?> TrackOrder(int orderId);
    }
}
