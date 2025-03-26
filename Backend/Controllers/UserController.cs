using Backend.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Backend.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        public UserController(UserManager<User> userManager,
                         SignInManager<User> signInManager,
                         IConfiguration configuration)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._configuration = configuration;
        }
        public class RegisterModel
        {


            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {

                var user = new Models.User { Email = model.Email, UserName=model.Username };
                var result = await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(new { message = "User registered successfully!" });
                }
                return BadRequest(result.Errors);
            
        }
        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var email = model.Email;
            var password = model.Password;
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = GenerateJwt(user);
            return Ok(new { Token = token });
        }

        private string GenerateJwt(Backend.Models.User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                 }),
                Expires = DateTime.UtcNow.AddDays(double.Parse(_configuration["Jwt:ExpirationDays"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}

