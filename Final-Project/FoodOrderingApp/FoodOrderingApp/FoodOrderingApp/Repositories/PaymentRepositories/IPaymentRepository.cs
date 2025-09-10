using FoodOrderingApp.Dto;
using FoodOrderingApp.Models;

namespace FoodOrderingApp.Repositories.DeliveryRepositories
{
    public interface IPaymentRepository
    {
        Task<PaymentDTO> AddAsync(PaymentDTO paymentdto);
        Task<PaymentDTO?> GetByIdAsync(int id);
        Task<List<PaymentDTO>> GetAllAsync();
        Task<PaymentDTO> UpdateAsync(PaymentDTO paymentdto);
        Task<bool> DeleteAsync(int id);
    }
}
