using FoodOrderingApp.Dto;
using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.MenuRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer,Restaurant")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public MenuItemController(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _menuItemRepository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _menuItemRepository.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MenuItemDTO menuItem)
        {
            var created = await _menuItemRepository.AddAsync(menuItem);
            return CreatedAtAction(nameof(Get), new { id = created.MenuItemId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MenuItemDTO menuItem)
        {
            if (id != menuItem.MenuItemId) return BadRequest();
            var updated = await _menuItemRepository.UpdateAsync(menuItem);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _menuItemRepository.DeleteAsync(id);
            return NoContent();
        }
    }

}
