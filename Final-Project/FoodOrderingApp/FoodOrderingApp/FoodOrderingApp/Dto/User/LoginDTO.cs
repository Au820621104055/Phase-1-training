using System.ComponentModel.DataAnnotations;

namespace FoodOrderingApp.Dto.User
{
    public class LoginDTO
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
