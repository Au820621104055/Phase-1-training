using FoodOrderingApp.Dto.User;
using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.UserRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;

    public AuthController(IUserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register(RegisterCustomerDTO dto)
    {
        var existing = await _userRepository.GetByEmailAsync(dto.Email, HashPassword(dto.Password));
        if (existing != null)
            return BadRequest("Email already registered");

        var hashedPassword = HashPassword(dto.Password);

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Password = hashedPassword,
            Role = dto.Role,
            PhoneNumber = dto.PhoneNumber
        };

        var createdUser = await _userRepository.AddAsync(user);

        var response = new UserDTO
        {
            UserId = createdUser.UserId,
            FullName = createdUser.FullName,
            Email = createdUser.Email,
            Role = createdUser.Role,
            PhoneNumber = createdUser.PhoneNumber
        };

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginDTO dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email,HashPassword(dto.Password));
        if (user == null)
            return Unauthorized("Invalid credentials");

        var token = GenerateJwtToken(user);

        return Ok(new { token });
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role)
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(6),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}

