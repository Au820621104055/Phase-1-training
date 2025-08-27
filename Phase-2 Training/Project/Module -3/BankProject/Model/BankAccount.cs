using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankProject.Model
{
    public class BankAccount
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string AccountNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int CustomerId { get; set; }

        public Customers Customer { get; set; }
    }
}
