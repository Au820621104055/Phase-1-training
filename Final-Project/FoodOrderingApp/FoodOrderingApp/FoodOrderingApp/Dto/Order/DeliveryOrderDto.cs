namespace FoodOrderingApp.Dto.Order
{
    public class DeliveryOrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryStatus { get; set; }
        public string CustomerName { get; set; }
        public string RestaurantName { get; set; }
    }
}
