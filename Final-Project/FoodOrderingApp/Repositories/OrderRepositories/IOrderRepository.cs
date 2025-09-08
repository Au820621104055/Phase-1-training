using FoodOrderingApp.Dto;
using FoodOrderingApp.Models;

namespace FoodOrderingApp.Repositories.OrderRepositories
{
    public interface IOrderRepository
    {
        Task<OrderDTO> AddAsync(OrderDTO orderdto);
        Task<IEnumerable<OrderDTO>> GetById(int id);
        Task<OrderDTO?> GetByIdAsync(int id);
        Task<List<OrderDTO>> GetAllAsync();
        Task<OrderDTO> UpdateAsync(OrderDTO orderdto);
        Task<bool> DeleteAsync(int id);
    }
}