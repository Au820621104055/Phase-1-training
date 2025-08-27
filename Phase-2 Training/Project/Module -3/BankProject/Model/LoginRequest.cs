using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BankProject.Model
{
    public class LoginRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
