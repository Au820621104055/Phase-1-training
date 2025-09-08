namespace FoodOrderingApp.Dto
{
    public class MenuItemDTO
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }

        public int RestaurantId { get; set; }
        public string? RestaurantName { get; set; }
        public int Quantity { get; internal set; }
    }
}
