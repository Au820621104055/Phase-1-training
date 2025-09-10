using System.ComponentModel.DataAnnotations;

namespace FoodOrderingApp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [Required]
        public string Role { get; set; } = "Customer"; 

        public string? PhoneNumber { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public ICollection<Order> CustomerOrders { get; set; } = new List<Order>();
        public ICollection<Order> DeliveryOrders { get; set; } = new List<Order>();


        public bool IsActive { get; set; } = true;
    }
}
