using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingApp.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Precision(18, 2)]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = "Cash"; 

        [Required]
        public string PaymentStatus { get; set; } = "Pending"; 

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}
