namespace FoodOrderingApp.Dto.Order
{
    public class CreateOrderDto
    {
        public int RestaurantId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public string? SpecialInstructions { get; set; } 
    }
}
