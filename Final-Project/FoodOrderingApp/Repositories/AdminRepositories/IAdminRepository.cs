using FoodOrderingApp.Models;

namespace FoodOrderingApp.Repositories.AdminRepositories
{
    public interface IAdminRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User?> GetUserById(int userId);
        Task<User?> AddUser(User user);
        Task<User?> UpdateUser(User user);
        Task<User?> UpdateUserStatus(int userId, bool isActive);
        Task<bool> DeleteUser(int userId);
        Task<IEnumerable<Order>> GetAllOrders();
        Task<IEnumerable<Restaurant>> GetAllRestaurants();
    }
}
