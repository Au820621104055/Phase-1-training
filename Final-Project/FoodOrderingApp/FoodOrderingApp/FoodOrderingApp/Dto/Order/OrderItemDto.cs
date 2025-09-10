namespace FoodOrderingApp.Dto.Order
{
    public class OrderItemDto
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; internal set; }
        public string? MenuItemName { get; internal set; }
    }
}
