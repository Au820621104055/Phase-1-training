using FoodOrderingApp.Context;
using FoodOrderingApp.Dto;
using FoodOrderingApp.Dto.Menu;
using FoodOrderingApp.Dto.Restaurant;
using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.RestaurantRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodOrderingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "RestaurantOwner")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantRepo;
        private readonly AppDBContext _context;

        public RestaurantController(IRestaurantRepository restaurantRepo,AppDBContext _context)
        {
            _restaurantRepo = restaurantRepo;
            this._context = _context;
        }

        [HttpGet("profile")]
        public async Task<ActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim.Value, out int ownerId)) return BadRequest("Invalid user ID in token.");

            var profile = await _restaurantRepo.GetProfile(ownerId);
            if (profile == null) return NotFound("Profile not found.");

            return Ok(new
            {
                profile.RestaurantId,
                profile.Name,
                profile.CuisineType,
                profile.Address,
                profile.PhoneNumber
            });
        }

        [HttpPut("profile")]
        public async Task<ActionResult> UpdateProfile(UpdateRestaurantDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim.Value, out int ownerId)) return BadRequest("Invalid user ID in token.");

            var profile = await _restaurantRepo.GetProfile(ownerId);
            if (profile == null) return NotFound("Profile not found.");

            profile.Name = dto.Name;
            profile.CuisineType = dto.CuisineType;
            profile.Address = dto.Address;
            profile.PhoneNumber = dto.PhoneNumber;

            var updated = await _restaurantRepo.UpdateProfile(profile);
            return Ok(updated);
        }

        [HttpPost("menu")]
        public async Task<ActionResult> AddMenuItem(MenuItemDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim.Value, out int ownerId)) return BadRequest("Invalid user ID in token.");

            var restaurant = await _context.Restaurants
                    .FirstOrDefaultAsync(r => r.RestaurantId == dto.RestaurantId);
            if (restaurant == null) return NotFound("Restaurant not found.");

            var item = new MenuItem
            {
                RestaurantId = restaurant.RestaurantId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable
            };

            var added = await _restaurantRepo.AddMenuItem(item);
            return Ok(added);
        }

        [HttpPut("menu/{id}")]
        public async Task<ActionResult> UpdateMenuItem(int id, MenuItemDto dto)
        {
            var item = new MenuItem
            {
                MenuItemId = id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable
            };

            var updated = await _restaurantRepo.UpdateMenuItem(item);
            if (updated == null) return NotFound("Menu item not found.");

            return Ok(updated);
        }

        [HttpDelete("menu/{id}")]
        public async Task<ActionResult> DeleteMenuItem(int id)
        {
            var deleted = await _restaurantRepo.DeleteMenuItem(id);
            if (!deleted) return NotFound("Menu item not found.");

            return Ok(new { message = "Deleted successfully" });
        }


        [HttpGet("menu/{id}")]
        public async Task<ActionResult> GetMenuById(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim.Value, out int ownerId)) return BadRequest("Invalid user ID in token.");

            var restaurant = await _restaurantRepo.GetProfile(ownerId);
            if (restaurant == null) return NotFound("Restaurant not found.");

            var menuItem = await _restaurantRepo.GetMenuItemById(id);
            if (menuItem == null || menuItem.RestaurantId != restaurant.RestaurantId)
                return NotFound("Menu item not found for this restaurant.");

            return Ok(menuItem);
        }

        [HttpGet("orders")]
        public async Task<ActionResult> GetOrders()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim.Value, out int ownerId)) return BadRequest("Invalid user ID in token.");

            var restaurant = await _restaurantRepo.GetProfile(ownerId);
            if (restaurant == null) return NotFound("Restaurant not found.");

            var orders = await _restaurantRepo.GetOrders(restaurant.RestaurantId);
            return Ok(orders.Select(o => new
            {
                o.OrderId,
                o.OrderDate,
                o.DeliveryStatus,
                Items = o.OrderItems.Select(oi => new
                {
                    oi.MenuItem.Name,
                    oi.Quantity,
                    Price = oi.MenuItem.Price
                })
            }));
        }

        [HttpGet("ordersbyid/{id}")]
        public async Task<ActionResult> GetOrderById(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim.Value, out int ownerId)) return BadRequest("Invalid user ID in token.");

            var restaurant = await _restaurantRepo.GetProfile(ownerId);
            if (restaurant == null) return NotFound("Restaurant not found.");

            var order = await _restaurantRepo.GetOrderById(id);
            if (order == null || order.RestaurantId != restaurant.RestaurantId)
                return NotFound("Order not found for this restaurant.");

            return Ok(new
            {
                order.OrderId,
                order.OrderDate,
                order.DeliveryStatus,
                Items = order.OrderItems.Select(oi => new
                {
                    oi.MenuItem.Name,
                    oi.Quantity,
                    Price = oi.MenuItem.Price
                })
            });
        }

        [HttpPut("orders/{id}/status")]
        public async Task<ActionResult> UpdateOrderStatus(int id, UpdateStatus dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim.Value, out int ownerId)) return BadRequest("Invalid user ID in token.");

            var updated = await _restaurantRepo.UpdateOrderStatus(id, dto.Status);
            if (updated == null) return NotFound("Order not found.");

            return Ok(new { updated.OrderId, updated.DeliveryStatus });
        }

        [HttpGet("AllRestaurant")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetAll()
        {
            var res = await _restaurantRepo.GetAllAsync();
            return Ok(res);
        }
        [HttpPost("addrestaurant")]
        public async Task<ActionResult> AddRestaurant([FromBody] RestaurantDTO resDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var restaurant = new Restaurant
            {
                Name = resDto.Name,
                Address = resDto.Address,
                PhoneNumber = resDto.PhoneNumber,
                CuisineType = resDto.CuisineType,
                OwnerId = resDto.OwnerId
            };

            var addRes = await _restaurantRepo.Add(restaurant);
            return Ok(addRes);
        }

        [HttpGet("MyRestaurants")]
        public async Task<IActionResult> GetMyRestaurants()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("User ID not found in token.");
            if (!int.TryParse(userIdClaim.Value, out int ownerId)) return BadRequest("Invalid user ID in token.");

            if (userIdClaim == null)
                return Unauthorized();

            int id = int.Parse(userIdClaim.Value);

            var restaurants = await _restaurantRepo.GetMyRestaurantsWithStatus(id);
            return Ok(restaurants);
        }

        [HttpPost("submit-for-validation")]
        public async Task<ActionResult> SubmitRestaurant(RestaurantSubmissionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Step 1: Save the restaurant
            var restaurant = new Restaurant
            {
                Name = dto.Name,
                Address = dto.Address,
                CuisineType = dto.CuisineType,
                PhoneNumber = dto.PhoneNumber,
                OwnerId = dto.OwnerId
            };

            var savedRestaurant = await _restaurantRepo.Add(restaurant);

            // Step 2: Save its validation record
            var validation = new RestaurantValidation
            {
                RestaurantId = savedRestaurant.RestaurantId,
                Status = "Pending",
                SubmittedAt = DateTime.UtcNow
            };

            await _restaurantRepo.SubmitForValidation(validation);

            return Ok(new { message = "Restaurant submitted for validation", restaurant = savedRestaurant });
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetPendingRestaurants()
        {
            var pending = await _restaurantRepo.GetPendingRestaurants();
            return Ok(pending);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateStatus(int id, UpdateStatus dto)
        {
            var updated = await _restaurantRepo.UpdateRestaurantStatus(id, dto.Status);
            if (updated == null) return NotFound("Restaurant not found");
            return Ok(new { message = $"Restaurant {dto.Status}", updated });
        }

    }
}
