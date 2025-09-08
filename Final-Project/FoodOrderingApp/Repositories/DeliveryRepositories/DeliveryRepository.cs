using FoodOrderingApp.Context;
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

        public async Task<IEnumerable<Order>> GetAssignedOrders(int staffId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Restaurant)
                .Where(o => o.DeliveryPersonId == staffId)
                .ToListAsync();
        }

        public async Task<Order?> UpdateDeliveryStatus(int orderId, string status, int staffId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId && o.DeliveryPersonId == staffId);
            if (order == null) return null;

            order.DeliveryStatus = status;
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
