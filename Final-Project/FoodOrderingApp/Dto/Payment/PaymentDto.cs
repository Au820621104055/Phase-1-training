namespace FoodOrderingApp.Dto.Payment
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "";
        public string PaymentStatus { get; set; } = "";
        public DateTime PaymentDate { get; set; }
        public int OrderId { get; internal set; }
    }
}
