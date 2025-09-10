using FoodOrderingApp.Dto.Payment;

namespace FoodOrderingApp.Dto.Order
{
    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = "";
        public string RestaurantName { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "";
        public List<OrderItemDetailDto> Items { get; set; } = new();
        public PaymentDto? Payment { get; set; }
        public decimal TotalAmount { get; set; }


    }
}
