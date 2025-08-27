using static BankProject.Model.Dto.BankAccountDto;

namespace BankProject.Model.Dto
{
    public class CustomerDto
    {
        public class CreateCustomerDto
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public List<CreateBankAccountDto> Accounts { get; set; } = new();
        }

        public class CustomerResponseDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public List<BankAccountResponseDto> Accounts { get; set; } = new();
        }
    }
}
