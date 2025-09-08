using FoodOrderingApp.Context;
using FoodOrderingApp.Dto;
using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.DeliveryRepositories;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingApp.Repositories.PaymentRepositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDBContext _context;

        public PaymentRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task<PaymentDTO> AddAsync(PaymentDTO paymentDto)
        {
            var payment = new Payment
            {
                OrderId = paymentDto.OrderId,
                Amount = paymentDto.Amount,
                PaymentStatus = paymentDto.PaymentStatus,
                PaymentDate = paymentDto.PaymentDate
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            paymentDto.PaymentId = payment.PaymentId;
            return paymentDto;
        }

        public async Task<PaymentDTO?> GetByIdAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return null;

            return new PaymentDTO
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                PaymentMethod = payment.PaymentMethod,
                Amount = payment.Amount,
                PaymentStatus = payment.PaymentStatus,
                PaymentDate = payment.PaymentDate
            };
        }

        public async Task<List<PaymentDTO>> GetAllAsync()
        {
            return await _context.Payments
                .Select(p => new PaymentDTO
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    Amount = p.Amount,
                    PaymentStatus = p.PaymentStatus,
                    PaymentDate = p.PaymentDate
                })
                .ToListAsync();
        }

        public async Task<PaymentDTO> UpdateAsync(PaymentDTO paymentDto)
        {
            var payment = await _context.Payments.FindAsync(paymentDto.PaymentId);
            if (payment == null) throw new Exception("Payment not found");

            payment.OrderId = paymentDto.OrderId;
            payment.Amount = paymentDto.Amount;
            payment.PaymentStatus = paymentDto.PaymentStatus;
            payment.PaymentDate = paymentDto.PaymentDate;

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();

            return paymentDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return false;

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
