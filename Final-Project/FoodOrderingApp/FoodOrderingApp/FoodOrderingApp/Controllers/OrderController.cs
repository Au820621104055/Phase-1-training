using FoodOrderingApp.Context;
using FoodOrderingApp.Dto;
using FoodOrderingApp.Dto.Order;
using FoodOrderingApp.Dto.Payment;
using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.OrderRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer,RestaurantOwner")]
    public class OrderController:ControllerBase
    {

        private readonly IOrderRepository _orderRepository;
        private readonly AppDBContext _context;

        public OrderController(IOrderRepository orderRepository, AppDBContext _context)
        {
            _orderRepository = orderRepository;
            this._context = _context;
        }

        [HttpPost("order")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDTO dto)
        {
            var order = new Order
            {
                CustomerId = dto.CustomerId,
                RestaurantId = dto.RestaurantId,
                OrderDate = DateTime.Now,
                DeliveryStatus = "Pending",
                OrderItems = dto.Items.Select(i => new OrderItem
                {
                    MenuItemId = i.MenuItemId,
                    Quantity = i.Quantity,
                    Price = _context.MenuItems
                                    .Where(m => m.MenuItemId == i.MenuItemId)
                                    .Select(m => m.Price)
                                    .FirstOrDefault()
                }).ToList()
            };

            var totalAmount = order.OrderItems.Sum(x => x.Price * x.Quantity);

            var payment = new Payment
            {
                Amount = totalAmount,
                PaymentStatus = "Pending"
            };

            order.Payment = payment;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                orderId = order.OrderId,
                paymentId = order.Payment.PaymentId,
                amount = order.Payment.Amount
            });

        }

        [HttpPut("cancel/{orderId}")]

        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return NotFound("Order not found");

            if (order.DeliveryStatus != "Pending")
                return BadRequest("Order cannot be cancelled as it is already processed");

            order.DeliveryStatus = "Cancelled";
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        [HttpGet("order/{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.MenuItem)
                .Include(o => o.Customer)
                .Include(o => o.Restaurant)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound();

            return Ok(new
            {
                order.OrderId,
                order.CustomerId,
                CustomerName = order.Customer?.FullName ?? "",
                order.RestaurantId,
                RestaurantName = order.Restaurant?.Name ?? "",
                order.OrderDate,
                order.DeliveryStatus,
                items = order.OrderItems.Select(i => new
                {
                    i.MenuItemId,
                    i.Quantity,
                    i.Price,
                    FullName = i.MenuItem.Name
                }),
                payment = order.Payment
            });
        }


        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.MenuItem)
                .Include(o => o.Customer)
                .Include(o => o.Restaurant)
                .Include(o => o.Payment)
                .ToListAsync();

            var result = orders.Select(order => new
            {
                order.OrderId,
                order.CustomerId,
                CustomerName = order.Customer?.FullName ?? "",
                order.RestaurantId,
                RestaurantName = order.Restaurant?.Name ?? "",
                order.OrderDate,
                order.DeliveryStatus,
                order.DeliveryPersonId,
                DeliveryPersonName = order.DeliveryPerson?.FullName ?? "",
                items = order.OrderItems.Select(i => new
                {
                    i.MenuItemId,
                    i.Quantity,
                    i.Price,
                    Name = i.MenuItem.Name
                }),
                payment = order.Payment
            });

            return Ok(result);
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim.Value, out int customerId))
                return BadRequest("Invalid user ID in token.");

            var orders = await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(i => i.MenuItem)
                .Include(o => o.Restaurant)
                .Include(o => o.Payment)
                .Include(o => o.Customer)
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();

            var result = orders.Select(order => new OrderResponseDto
            {
                OrderId = order.OrderId,
                CustomerName = order.Customer?.FullName ?? "",
                RestaurantName = order.Restaurant?.Name ?? "",
                OrderDate = order.OrderDate,
                Status = order.DeliveryStatus,
                Items = order.OrderItems.Select(i => new OrderItemDetailDto
                {
                    ItemName = i.MenuItem.Name,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList(),
                Payment = order.Payment == null ? null : new PaymentDto
                {
                    PaymentId = order.Payment.PaymentId,
                    Amount = order.Payment.Amount,
                    PaymentMethod = order.Payment.PaymentMethod,
                    PaymentStatus = order.Payment.PaymentStatus,
                    PaymentDate = order.Payment.PaymentDate
                },
                TotalAmount = order.OrderItems.Sum(i => i.Price * i.Quantity)
            }).ToList();

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var restaurantIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier); 
            if (restaurantIdClaim == null) return Unauthorized("Restaurant ID not found in token.");

            if (!int.TryParse(restaurantIdClaim.Value, out int restaurantId))
                return BadRequest("Invalid restaurant ID in token.");

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.MenuItem)
                .Where(o => o.RestaurantId == id)
                .ToListAsync();

            if (!orders.Any()) return Ok(new List<OrderDTO>());

            var order = orders.Select(order => new OrderDTO
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                RestaurantId = order.RestaurantId,
                OrderDate = order.OrderDate,
                DeliveryPersonId = order.DeliveryPersonId,
                DeliveryStatus = order.DeliveryStatus,
                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    MenuItemId = i.MenuItemId,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    MenuItemName = i.MenuItem != null ? i.MenuItem.Name : ""
                }).ToList()
            }).ToList();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderDTO order)
        {
            var created = await _orderRepository.AddAsync(order);
            return CreatedAtAction(nameof(Get), new { id = created.OrderId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDTO order)
        {
            if (id != order.OrderId) return BadRequest();
            var updated = await _orderRepository.UpdateAsync(order);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

