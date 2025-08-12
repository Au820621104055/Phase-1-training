using System.ComponentModel.DataAnnotations;

namespace MovieTicket.Models
{
    public class movie
    {
        public int Id { get; set; }
        [StringLength(40,MinimumLength =5, ErrorMessage = "Name Length should be greater than 5") ]
        public string name { get; set; }
        public string gender { get; set; }

        public DateOnly dateofbirth { get; set; }
        [Range(5,100, ErrorMessage = "Age should be greater than 5")]
        public int age { get; set; }
    }
}
