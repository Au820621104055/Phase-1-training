using FoodOrderingApp.Context;
using FoodOrderingApp.Dto;
using FoodOrderingApp.Dto.Order;
using FoodOrderingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingApp.Repositories.OrderRepositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDBContext _context;

        public OrderRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task<OrderDTO> AddAsync(OrderDTO orderDto)
        {
            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                RestaurantId = orderDto.RestaurantId,
                OrderDate = orderDto.OrderDate,
                DeliveryPersonId = orderDto.DeliveryPersonId,
                DeliveryStatus = orderDto.DeliveryStatus
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            orderDto.OrderId = order.OrderId;
            return orderDto;
        }

        public async Task<IEnumerable<OrderDTO>?> GetById(int restaurantId)
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Restaurant)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .Where(u => u.RestaurantId == restaurantId)
                .ToListAsync();

            if (orders == null || !orders.Any()) return null;

            var orderDTOs = orders.Select(order => new OrderDTO
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.FullName,
                RestaurantId = order.RestaurantId,
                RestaurantName = order.Restaurant.Name,
                DeliveryPersonId = order.DeliveryPersonId,
                DeliveryPersonName = order.DeliveryPerson != null ? order.DeliveryPerson.FullName : "",
                OrderDate = order.OrderDate,
                DeliveryStatus = order.DeliveryStatus,
                orderdetail = order.OrderItems.Select(i => new OrderItemDetailDto
                {
                    ItemName = i.MenuItem.Name,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            });

            return orderDTOs;
        }

        public async Task<List<OrderDTO>> GetAllAsync()
        {
            return await _context.Orders.Include(o => o.OrderItems)
                .Select(o => new OrderDTO
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.FullName,
                    RestaurantName = o.Restaurant.Name,
                    DeliveryPersonName = o.DeliveryPerson.FullName,
                    RestaurantId = o.RestaurantId,
                    OrderDate = o.OrderDate,
                    DeliveryPersonId = o.DeliveryPersonId,
                    DeliveryStatus = o.DeliveryStatus,
                })
                .ToListAsync();
        }

        public async Task<OrderDTO> UpdateAsync(OrderDTO orderDto)
        {
            var order = await _context.Orders.FindAsync(orderDto.OrderId);
            if (order == null) throw new Exception("Order not found");

            order.CustomerId = orderDto.CustomerId;
            order.RestaurantId = orderDto.RestaurantId;
            order.OrderDate = orderDto.OrderDate;
            order.DeliveryPersonId = orderDto.DeliveryPersonId;
            order.DeliveryStatus = orderDto.DeliveryStatus;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return orderDto;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        Task<OrderDTO?> IOrderRepository.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}