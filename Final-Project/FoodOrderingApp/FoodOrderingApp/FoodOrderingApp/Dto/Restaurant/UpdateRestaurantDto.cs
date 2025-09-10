namespace FoodOrderingApp.Dto.Restaurant
{
    public class UpdateRestaurantDto
    {
        public string Name { get; set; }
        public string CuisineType { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class RestaurantWithStatusDto
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CuisineType { get; set; }
        public string PhoneNumber { get; set; }
        public int OwnerId { get; set; }
        public string Status { get; set; }
    }
}
