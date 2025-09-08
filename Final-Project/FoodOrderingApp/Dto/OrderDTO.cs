using FoodOrderingApp.Dto.Order;

namespace FoodOrderingApp.Dto
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = "";
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public int? DeliveryPersonId { get; set; }
        public string DeliveryPersonName { get; set; } = "";
        public string DeliveryStatus { get; set; } = "";
        public PaymentDTO? Payment { get; set; }

        public List<OrderItemDetailDto> orderdetail { get; set; } = new List<OrderItemDetailDto>();

        public List<OrderItemDto> Items { get; set; } = new();
    }
}
