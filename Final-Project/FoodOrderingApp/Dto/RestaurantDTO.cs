namespace FoodOrderingApp.Dto
{
    public class RestaurantDTO
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string CuisineType { get; set; } = "";
        public int OwnerId { get; set; }
        public string OwnerName { get; set; } = "";
    }
}
