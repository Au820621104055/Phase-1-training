using BankProject.Model;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginRequest = BankProject.Model.LoginRequest;

namespace BankProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly IConfiguration _configuration;

        public AuthController(IHttpClientFactory httpFactory, IConfiguration configuration)
        {
            _httpFactory = httpFactory;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var client = _httpFactory.CreateClient();
            var fastApiUrl = "http://127.0.0.1:8000/login";

            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

            try
            {
                var resp = await client.PostAsync(fastApiUrl, content);
                if (!resp.IsSuccessStatusCode)
                {
                    var err = await resp.Content.ReadAsStringAsync();
                    return Unauthorized(new { message = "Invalid credentials", error = err });
                }

                var body = await resp.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(body);

                if (loginResponse == null)
                    return StatusCode(502, new { message = "Auth service returned invalid data" });

                var jwtSection = _configuration.GetSection("JwtSettings");
                var secret = jwtSection["SecretKey"];
                if (string.IsNullOrEmpty(secret) || secret.Length < 32)
                    return StatusCode(500, new { message = "Server JWT key misconfigured (must be >=32 chars)" });

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginResponse.Name),
                    new Claim(ClaimTypes.Role, loginResponse.Role)
                };

                var token = new JwtSecurityToken(
                    issuer: jwtSection["Issuer"],
                    audience: jwtSection["Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSection["ExpiryMinutes"])),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    message = "Login successful",
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    data = loginResponse
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error calling auth service", error = ex.Message });
            }
        }
    }
}
