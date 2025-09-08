using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingApp.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public User Customer { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("DeliveryPerson")]
        public int? DeliveryPersonId { get; set; }
        public User? DeliveryPerson { get; set; }

        [Required]
        public string DeliveryStatus { get; set; } = "Pending";

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Payment Payment { get; set; }
    }
}
