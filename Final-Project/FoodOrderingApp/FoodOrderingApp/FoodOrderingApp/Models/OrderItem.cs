using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingApp.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; }

        public decimal Price { get; set; }
        public MenuItem MenuItem { get; set; }

        [Range(1, 50)]
        public int Quantity { get; set; }
    }
}
