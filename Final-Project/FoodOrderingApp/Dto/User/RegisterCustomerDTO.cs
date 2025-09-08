using System.ComponentModel.DataAnnotations;

namespace FoodOrderingApp.Dto.User
{
    public class RegisterCustomerDTO
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "Customer";
        public string? PhoneNumber { get; set; }
    }
}
