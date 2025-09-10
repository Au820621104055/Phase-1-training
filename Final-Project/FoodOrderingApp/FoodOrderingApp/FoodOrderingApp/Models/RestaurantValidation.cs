using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingApp.Models
{
    public class RestaurantValidation
    {
        [Key]
        public int ValidationId { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public Restaurant Restaurant { get; set; }
    }
}
