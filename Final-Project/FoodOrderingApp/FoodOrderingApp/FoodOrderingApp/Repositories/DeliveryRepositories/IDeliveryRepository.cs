using FoodOrderingApp.Dto.Order;
using FoodOrderingApp.Models;

namespace FoodOrderingApp.Repositories.DeliveryRepositories
{
    public interface IDeliveryRepository
    {
        Task<IEnumerable<DeliveryOrderDto>> GetAssignedOrders(int staffId);
        Task<Order?> UpdateDeliveryStatus(int orderId, string status, int staffId);
        Task<IEnumerable<DeliveryOrderDto>> GetAvailableOrdersAsync();
        bool AcceptOrder(int orderId, int deliveryPersonId);
        bool RejectOrder(int orderId);
    }
}
