namespace FoodOrderingApp.Dto.Menu
{
    public class MenuItemDto
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }

        public int RestaurantId { get; set; }

        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
    }
}
