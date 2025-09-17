using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.AdminRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepo;

        public AdminController(IAdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

        [HttpPost("users")]
        public async Task<ActionResult> AddUser([FromBody] User user)
        {
            var addedUser = await _adminRepo.AddUser(user);
            return Ok(addedUser);
        }

        [HttpPut("users/{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] User user)
        {
            user.UserId = id;
            var updated = await _adminRepo.UpdateUser(user);
            if (updated == null) return NotFound("User not found");
            return Ok(updated);
        }

        [HttpDelete("users/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var deleted = await _adminRepo.DeleteUser(id);
            if (!deleted) return NotFound("User not found");
            return Ok(new { message = "User deleted successfully" });
        }


        [HttpGet("dashboard/orders")]
        public async Task<ActionResult> GetAllOrders()
        {
            var orders = await _adminRepo.GetAllOrders();
            return Ok(orders.Select(o => new
            {
                o.OrderId,
                Customer = o.Customer.FullName,
                customerId = o.CustomerId,
                customerName = o.Customer.FullName,
                Restaurant = o.Restaurant.Name,
                DeliveryStaff = o.DeliveryPerson?.FullName ?? "Not Assigned",
                o.DeliveryStatus,
                o.OrderDate
            }));
        }

        [HttpGet("dashboard/restaurants")]
        public async Task<ActionResult> GetAllRestaurants()
        {
            var restaurants = await _adminRepo.GetAllRestaurants();
            return Ok(restaurants.Select(r => new
            {
                r.RestaurantId,
                r.Name,
                r.CuisineType,
                r.Address,
                r.PhoneNumber
            }));
        }


        [HttpGet("users")]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _adminRepo.GetAllUsers();
            return Ok(users.Select(u => new
            {
                u.UserId,
                u.FullName,
                u.Email,
                u.Role,
                u.IsActive
            }));
        }

        [HttpPut("users/{id}/status")]
        public async Task<ActionResult> UpdateUserStatus(int id, bool isActive)
        {
            var updated = await _adminRepo.UpdateUserStatus(id, isActive);
            if (updated == null) return NotFound("User not found");

            return Ok(new
            {
                updated.UserId,
                updated.FullName,
                updated.IsActive
            });
        }

        [HttpGet("restaurants/pending")]
        public async Task<ActionResult> GetPendingRestaurants()
        {
            var pending = await _adminRepo.GetPendingRestaurants();
            return Ok(pending);
        }

        [HttpPatch("restaurants/{id}/status")]
        public async Task<ActionResult> UpdateRestaurantStatus(int id, [FromQuery] string status)
        {
            var updated = await _adminRepo.UpdateRestaurantStatus(id, status);
            if (updated == null) return NotFound("Restaurant not found");

            return Ok(new
            {
                updated.RestaurantId,
                updated.Status
            });
        }
    }

}

