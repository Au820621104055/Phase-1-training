using FoodOrderingApp.Models;

namespace FoodOrderingApp.Repositories.DeliveryRepositories
{
    public interface IDeliveryRepository
    {
        Task<IEnumerable<Order>> GetAssignedOrders(int staffId);
        Task<Order?> UpdateDeliveryStatus(int orderId, string status, int staffId);
    }
}
