using FoodOrderingApp.Context;
using FoodOrderingApp.Dto;
using FoodOrderingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingApp.Repositories.MenuRepositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly AppDBContext _context;

        public MenuItemRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<MenuItemDTO> AddAsync(MenuItemDTO MenuItemDTO)
        {
            var menuItem = new MenuItem
            {
                Name = MenuItemDTO.Name,
                Description = MenuItemDTO.Description,
                Price = MenuItemDTO.Price,
                IsAvailable = MenuItemDTO.IsAvailable,
                RestaurantId = MenuItemDTO.RestaurantId
            };

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();

            MenuItemDTO.MenuItemId = menuItem.MenuItemId;
            return MenuItemDTO;
        }

        public async Task<MenuItemDTO?> GetByIdAsync(int id)
        {
            var menuItem = await _context.MenuItems
                .Include(m => m.Restaurant)
                .FirstOrDefaultAsync(m => m.MenuItemId == id);

            if (menuItem == null) return null;

            return new MenuItemDTO
            {
                MenuItemId = menuItem.MenuItemId,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                IsAvailable = menuItem.IsAvailable,
                RestaurantId = menuItem.RestaurantId,
                RestaurantName = menuItem.Restaurant.Name
            };
        }

        public async Task<List<MenuItemDTO>> GetAllAsync()
        {
            var menuItems = await _context.MenuItems.ToListAsync();
            return menuItems.Select(m => new MenuItemDTO
            {
                MenuItemId = m.MenuItemId,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                IsAvailable = m.IsAvailable,
                RestaurantId = m.RestaurantId
            }).ToList();
        }

        public async Task<MenuItemDTO> UpdateAsync(MenuItemDTO MenuItemDTO)
        {
            var menuItem = await _context.MenuItems.FindAsync(MenuItemDTO.MenuItemId);
            if (menuItem == null) throw new KeyNotFoundException("MenuItem not found");

            menuItem.Name = MenuItemDTO.Name;
            menuItem.Description = MenuItemDTO.Description;
            menuItem.Price = MenuItemDTO.Price;
            menuItem.IsAvailable = MenuItemDTO.IsAvailable;
            menuItem.RestaurantId = MenuItemDTO.RestaurantId;

            _context.MenuItems.Update(menuItem);
            await _context.SaveChangesAsync();

            return MenuItemDTO;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null) return false;

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
