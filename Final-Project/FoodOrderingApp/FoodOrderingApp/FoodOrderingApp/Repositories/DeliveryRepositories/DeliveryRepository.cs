using FoodOrderingApp.Context;
using FoodOrderingApp.Dto.Order;
using FoodOrderingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingApp.Repositories.DeliveryRepositories
{
    public class DeliveryRepository:IDeliveryRepository
    {
        private readonly AppDBContext _context;

        public DeliveryRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DeliveryOrderDto>> GetAssignedOrders(int staffId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Restaurant)
                .Where(o => o.DeliveryPersonId == staffId)
                .Select(o => new DeliveryOrderDto
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    DeliveryStatus = o.DeliveryStatus,
                    CustomerName = o.Customer.FullName,
                    RestaurantName = o.Restaurant.Name
                })
                .ToListAsync();
        }

        public async Task<Order?> UpdateDeliveryStatus(int orderId, string status, int staffId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.DeliveryPersonId == staffId);

            if (order == null) return null;

            order.DeliveryStatus = status;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<DeliveryOrderDto>> GetAvailableOrdersAsync()
        {
            return await _context.Orders
                .Where(o => o.DeliveryPersonId == null && o.DeliveryStatus == "Accepted")
                .Select(o => new DeliveryOrderDto
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    DeliveryStatus = o.DeliveryStatus,
                    CustomerName = o.Customer.FullName,
                    RestaurantName = o.Restaurant.Name
                })
                .ToListAsync();
        }

        public bool AcceptOrder(int orderId, int deliveryPersonId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null || order.DeliveryPersonId != null)
                return false;

            order.DeliveryPersonId = deliveryPersonId;
            order.DeliveryStatus = "Accepted";
            _context.SaveChanges();
            return true;
        }

        public bool RejectOrder(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                return false;

            order.DeliveryStatus = "RejectedByDelivery";
            _context.SaveChanges();
            return true;
        }
    }
}
