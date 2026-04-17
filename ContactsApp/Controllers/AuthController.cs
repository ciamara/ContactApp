using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace ContactsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // simplified authorization
            if (model.Username == "admin" && model.Password == "admin123")
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VerySecretKeyForSecurity1234567890!"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "contactsapp",
                    audience: "users",
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: creds
                );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return Unauthorized();
        }
    }

    public class LoginModel { public string Username { get; set; } public string Password { get; set; } }
}
