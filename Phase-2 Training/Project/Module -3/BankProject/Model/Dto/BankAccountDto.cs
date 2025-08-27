namespace BankProject.Model.Dto
{
    public class BankAccountDto
    {
        public class CreateBankAccountDto
        {
            public string AccountNumber { get; set; }
            public decimal Amount { get; set; }
            public DateTime? CreatedAt { get; set; }
            public int CustomerId { get; set; } // optional when used inside CreateCustomerDto
        }

        public class BankAccountResponseDto
        {
            public int Id { get; set; }
            public string AccountNumber { get; set; }
            public decimal Amount { get; set; }
            public DateTime CreatedAt { get; set; }
            public int CustomerId { get; set; }
        }
    }
}
