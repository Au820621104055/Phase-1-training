using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingApp.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = "";

        [Required]
        public string Address { get; set; } = "";

        [Required, Phone]
        public string PhoneNumber { get; set; } = "";

        [MaxLength(50)]
        public string CuisineType { get; set; } = "";

        [ForeignKey("Owner")]
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
