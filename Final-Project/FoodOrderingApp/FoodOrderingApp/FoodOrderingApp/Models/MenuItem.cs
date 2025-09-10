using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingApp.Models
{
    public class MenuItem
    {
        [Key]
        public int MenuItemId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        [Precision(18, 2)]
        [Range(1, 9999)]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
