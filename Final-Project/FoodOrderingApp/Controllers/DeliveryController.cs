using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.DeliveryRepositories;
using FoodOrderingApp.Dto.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using UpdateStatus = FoodOrderingApp.Dto.Restaurant.UpdateStatus;

namespace FoodOrderingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "DeliveryStaff")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryRepository _deliveryRepo;

        public DeliveryController(IDeliveryRepository deliveryRepo)
        {
            _deliveryRepo = deliveryRepo;
        }

        [HttpGet("assigned-orders")]
        public async Task<ActionResult> GetAssignedOrders()
        {
            var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var orders = await _deliveryRepo.GetAssignedOrders(staffId);

            return Ok(orders.Select(o => new
            {
                o.OrderId,
                Customer = o.Customer.FullName,
                Restaurant = o.Restaurant.Name,
                o.OrderDate,
                o.DeliveryStatus
            }));
        }

        [HttpPut("orders/{id}/status")]
        public async Task<ActionResult> UpdateDeliveryStatus(int id, [FromBody] UpdateStatus dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Status))
                return BadRequest("Status is required.");

            var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var updated = await _deliveryRepo.UpdateDeliveryStatus(id, dto.Status, staffId);

            if (updated == null) return NotFound("Order not found or not assigned to you");

            return Ok(new
            {
                updated.OrderId,
                updated.DeliveryStatus
            });
        }
    }
}
