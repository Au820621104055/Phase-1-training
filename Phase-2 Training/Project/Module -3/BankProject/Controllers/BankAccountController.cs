using BankProject.Context;
using BankProject.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using static BankProject.Model.Dto.BankAccountDto;

namespace BankProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BankAccountController(AppDbContext context) => _context = context;

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAccount([FromBody] CreateBankAccountDto dto)
        {
            if (dto.CustomerId == 0)
                return BadRequest(new { message = "CustomerId is required in CreateBankAccountDto" });

            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            if (customer == null)
                return NotFound(new { message = $"Customer with id {dto.CustomerId} not found" });

            var account = new BankAccount
            {
                AccountNumber = dto.AccountNumber,
                Amount = dto.Amount,
                CreatedAt = dto.CreatedAt ?? DateTime.UtcNow,
                CustomerId = dto.CustomerId
            };

            _context.BankAccounts.Add(account);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Account added", account = new { account.Id, account.AccountNumber, account.Amount } });
        }

        [HttpGet("by-customer/{customerId}")]
        [Authorize]
        public async Task<IActionResult> GetAccountsByCustomer(int customerId)
        {
            var accounts = await _context.BankAccounts
                .Where(a => a.CustomerId == customerId)
                .Select(a => new {
                    a.Id,
                    a.AccountNumber,
                    a.Amount,
                    a.CreatedAt,
                    a.CustomerId
                }).ToListAsync();

            if (!accounts.Any())
                return NotFound(new { message = $"No accounts found for customer {customerId}" });

            return Ok(accounts);
        }

        [HttpGet("balance")]
        [Authorize]
        public async Task<IActionResult> GetMyTotalBalance()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "Invalid token" });

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Name == username);
            if (customer == null)
                return NotFound(new { message = $"Customer '{username}' not found." });

            var totalBalance = await _context.BankAccounts
                .Where(a => a.CustomerId == customer.Id)
                .SumAsync(a => (decimal?)a.Amount) ?? 0m;

            return Ok(new { name = username, totalBalance });
        }
    }
}
