using FoodOrderingApp.Dto;
using FoodOrderingApp.Models;

namespace FoodOrderingApp.Repositories.MenuRepositories
{
    public interface IMenuItemRepository
    {
        Task<MenuItemDTO> AddAsync(MenuItemDTO menuitemdto);
        Task<MenuItemDTO?> GetByIdAsync(int id);
        Task<List<MenuItemDTO>> GetAllAsync();
        Task<MenuItemDTO> UpdateAsync(MenuItemDTO menuitemdto);
        Task<bool> DeleteAsync(int id);
    }
}
