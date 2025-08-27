using BankProject.Context;
using BankProject.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using static BankProject.Model.Dto.CustomerDto;

namespace BankProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CustomerController(AppDbContext context) => _context = context;

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCustomer([FromBody] CreateCustomerDto dto)
        {
            var customer = new Customers
            {
                Name = dto.Name,
                Age = dto.Age,
                BankAccounts = dto.Accounts.Select(a => new BankAccount
                {
                    AccountNumber = a.AccountNumber,
                    Amount = a.Amount,
                    CreatedAt = a.CreatedAt ?? DateTime.UtcNow
                }).ToList()
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Customer added", customer = new { customer.Id, customer.Name, customer.Age } });
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _context.Customers
                .Include(c => c.BankAccounts)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Age,
                    Accounts = c.BankAccounts.Select(a => new
                    {
                        a.Id,
                        a.AccountNumber,
                        a.Amount,
                        a.CreatedAt
                    }).ToList()
                })
                .ToListAsync();

            return Ok(customers);
        }
    }
}
