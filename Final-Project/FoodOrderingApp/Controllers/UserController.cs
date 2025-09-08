using FoodOrderingApp.Context;
using FoodOrderingApp.Dto.Menu;
using FoodOrderingApp.Dto.Order;
using FoodOrderingApp.Dto.Payment;
using FoodOrderingApp.Dto.Restaurant;
using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.UserRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodOrderingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Customer")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _customerRepo;
        private readonly AppDBContext _context;

        public UserController(IUserRepository customerRepo, AppDBContext context)
        {
            _customerRepo = customerRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _customerRepo.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _customerRepo.GetByIdAsync(id);
            if (user == null)
                return NotFound($"User with Id={id} not found.");
            return Ok(user);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            var user = await _customerRepo.GetByEmailAsync(email);
            if (user == null)
                return NotFound($"User with Email={email} not found.");
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser([FromBody] User user)
        {
            var createdUser = await _customerRepo.AddAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId }, createdUser);
        }

        [HttpGet("restaurants")]
        public async Task<ActionResult<IEnumerable<RestaurantBrowseDto>>> GetRestaurants([FromQuery] string? cuisine = null)
        {
            var restaurants = await _customerRepo.BrowseRestaurants(cuisine);

            return Ok(restaurants.Select(r => new RestaurantBrowseDto
            {
                RestaurantId = r.RestaurantId,
                Name = r.Name,
                CuisineType = r.CuisineType,
                Address = r.Address,
                PhoneNumber = r.PhoneNumber
            }));
        }

        [HttpGet("restaurant/{id}/menu")]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenu(int id)
        {
            var menu = await _customerRepo.GetMenu(id);
            return Ok(menu.Select(m => new MenuItemDto
            {
                MenuItemId = m.MenuItemId,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                RestaurantId=m.RestaurantId,
                IsAvailable = m.IsAvailable
            }));
        }

        //[Authorize(Roles = "Customer")]
        //[HttpPost("order")]
        //public async Task<ActionResult<OrderResponseDto>> PlaceOrder(CreateOrderDto dto)
        //{
        //    if (dto == null || dto.Items == null || !dto.Items.Any())
        //        return BadRequest("Invalid order data");

        //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        //    if (userIdClaim == null) return Unauthorized();
        //    int userId = int.Parse(userIdClaim.Value);

        //    var restaurant = _context.Restaurants.Find(dto.RestaurantId);
        //    if (restaurant == null) return NotFound("Restaurant not found");

        //    var order = new Order
        //    {
        //        CustomerId = userId,
        //        RestaurantId = dto.RestaurantId,
        //        OrderDate = DateTime.UtcNow,
        //        DeliveryStatus = "Pending"
        //    };

        //    var orderItems = dto.Items.Select(i =>
        //    {
        //        var menuItem = _context.MenuItems.Find(i.MenuItemId);
        //        if (menuItem == null) throw new Exception($"MenuItem {i.MenuItemId} not found");
        //        return new OrderItem
        //        {
        //            MenuItemId = i.MenuItemId,
        //            Quantity = i.Quantity,
        //            Price = menuItem.Price
        //        };
        //    }).ToList();

        //    var placedOrder = await _customerRepo.PlaceOrder(order, orderItems);

        //    var response = new OrderResponseDto
        //    {
        //        OrderId = placedOrder.OrderId,
        //        RestaurantName = restaurant.Name,
        //        OrderDate = placedOrder.OrderDate,
        //        Status = placedOrder.DeliveryStatus,
        //        Items = orderItems.Select(oi =>
        //        {
        //            var menuItem = _context.MenuItems.Find(oi.MenuItemId)!;
        //            return new OrderItemDetailDto
        //            {
        //                ItemName = menuItem.Name,
        //                Quantity = oi.Quantity,
        //                Price = menuItem.Price
        //            };
        //        }).ToList()
        //    };

        //    return Ok(response);
        //}

        [Authorize(Roles = "Customer")]
        [HttpPost("order")]
        public async Task<ActionResult<OrderResponseDto>> PlaceOrder(CreateOrderDto dto)
        {
            if (dto == null || dto.Items == null || !dto.Items.Any())
                return BadRequest("Invalid order data");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            var restaurant = _context.Restaurants.Find(dto.RestaurantId);
            if (restaurant == null) return NotFound("Restaurant not found");

            var order = new Order
            {
                CustomerId = userId,
                RestaurantId = dto.RestaurantId,
                OrderDate = DateTime.UtcNow,
                DeliveryStatus = "Pending"
            };

            var orderItems = dto.Items.Select(i =>
            {
                var menuItem = _context.MenuItems.Find(i.MenuItemId);
                if (menuItem == null) throw new Exception($"MenuItem {i.MenuItemId} not found");
                return new OrderItem
                {
                    MenuItemId = i.MenuItemId,
                    Quantity = i.Quantity,
                    Price = menuItem.Price
                };
            }).ToList();

            var placedOrder = await _customerRepo.PlaceOrder(order, orderItems);

            // ✅ Calculate total amount
            var totalAmount = orderItems.Sum(oi => oi.Price * oi.Quantity);

            // ✅ Create Payment immediately
            var payment = new Payment
            {
                OrderId = placedOrder.OrderId,
                Amount = totalAmount,
                PaymentMethod = "CARD", // or from dto.PaymentMethod if passed along
                PaymentStatus = "Paid",
                PaymentDate = DateTime.UtcNow
            };

            await _customerRepo.MakePayment(payment);

            // ✅ Response including payment info
            var response = new OrderResponseDto
            {
                OrderId = placedOrder.OrderId,
                RestaurantName = restaurant.Name,
                OrderDate = placedOrder.OrderDate,
                Status = placedOrder.DeliveryStatus,
                Items = orderItems.Select(oi =>
                {
                    var menuItem = _context.MenuItems.Find(oi.MenuItemId)!;
                    return new OrderItemDetailDto
                    {
                        ItemName = menuItem.Name,
                        Quantity = oi.Quantity,
                        Price = menuItem.Price
                    };
                }).ToList(),
                Payment = new PaymentDto
                {
                    OrderId = placedOrder.OrderId,
                    Amount = totalAmount,
                    PaymentMethod = payment.PaymentMethod,
                    PaymentStatus = payment.PaymentStatus,
                    PaymentDate = payment.PaymentDate
                }
            };

            return Ok(response);
        }


        [Authorize(Roles = "Customer")]
        [HttpPost("payment")]
        public async Task<ActionResult> MakePayment(PaymentDto dto)
        {
            if (dto == null) return BadRequest("Payment data is required");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            var order = await _customerRepo.TrackOrder(dto.OrderId);
            if (order == null) return NotFound("Order not found");

            if (order.CustomerId != userId)
                return Forbid("You cannot pay for someone else's order");

            if (order.Payment != null)
                return BadRequest("Payment already made");

            var totalAmount = order.OrderItems.Sum(oi => oi.MenuItem.Price * oi.Quantity);

            var payment = new Payment
            {
                OrderId = order.OrderId,
                Amount = totalAmount,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = "Paid",
                PaymentDate = DateTime.UtcNow
            };

            await _customerRepo.MakePayment(payment);

            return Ok(new { message = "Payment successful", amount = totalAmount, orderId = order.OrderId });
        }


        [HttpGet("order/{id}/status")]
        public async Task<ActionResult> TrackOrder(int id)
        {
            var order = await _customerRepo.TrackOrder(id);
            if (order == null) return NotFound("Order not found");

            return Ok(new
            {
                OrderId = order.OrderId,
                Status = order.DeliveryStatus,
                UpdatedAt = order.OrderDate
            });
        }

    }
}
