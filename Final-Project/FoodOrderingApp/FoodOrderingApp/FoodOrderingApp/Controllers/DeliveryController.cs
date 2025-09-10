using FoodOrderingApp.Dto.Order;
using FoodOrderingApp.Dto.Restaurant;
using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.DeliveryRepositories;
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
        public async Task<ActionResult<IEnumerable<DeliveryOrderDto>>> GetAssignedOrders()
        {
            var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var orders = await _deliveryRepo.GetAssignedOrders(staffId);
            return Ok(orders);
        }

        [HttpPut("orders/{id}/status")]
        public async Task<ActionResult<DeliveryOrderDto>> UpdateDeliveryStatus(int id, [FromBody] UpdateStatus dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Status))
                return BadRequest("Status is required.");

            var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var updated = await _deliveryRepo.UpdateDeliveryStatus(id, dto.Status, staffId);

            if (updated == null) return NotFound("Order not found or not assigned to you");

            return Ok(new DeliveryOrderDto
            {
                OrderId = updated.OrderId,
                OrderDate = updated.OrderDate,
                DeliveryStatus = updated.DeliveryStatus,
                CustomerName = updated.Customer?.FullName ?? "",
                RestaurantName = updated.Restaurant?.Name ?? ""
            });
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<DeliveryOrderDto>>> GetAvailableOrders()
        {
            var orders = await _deliveryRepo.GetAvailableOrdersAsync();
            return Ok(orders);
        }

        [HttpPost("accept/{orderId}/{deliveryPersonId}")]
        public IActionResult AcceptOrder(int orderId, int deliveryPersonId)
        {
            var success = _deliveryRepo.AcceptOrder(orderId, deliveryPersonId);
            if (!success) return BadRequest("Unable to accept order");
            return Ok(new UpdateStatus { Status = "Order accepted and assigned" });
        }

        [HttpPost("reject/{orderId}")]
        public IActionResult RejectOrder(int orderId)
        {
            var success = _deliveryRepo.RejectOrder(orderId);
            if (!success) return BadRequest("Unable to reject order");
            return Ok(new { Message = "Order rejected" });
        }
    }
}
