namespace FoodOrderingApp.Dto.Restaurant
{
    public class RestaurantBrowseDto
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; } = "";
        public string CuisineType { get; set; } = "";
        public string Address { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
    }
}
