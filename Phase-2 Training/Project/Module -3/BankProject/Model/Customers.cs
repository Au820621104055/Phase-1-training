using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankProject.Model
{
    public class Customers
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int Age { get; set; }

        [JsonIgnore]
        public List<BankAccount> BankAccounts { get; set; } = new();
    }
}
