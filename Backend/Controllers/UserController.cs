using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Web;

namespace Backend.Controllers
{

    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public UserController(UserManager<User> userManager,
                         SignInManager<User> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [HttpPost("user/register")]
        public async Task<IActionResult> Register([FromBody] JsonElement body)
        {


                var user = new Models.User { Email = body.GetProperty("email").GetString(), UserName = body.GetProperty("username").GetString() };
                var result = await _userManager.CreateAsync(user, body.GetProperty("password").GetString());
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(new { message = "User registered successfully!" });
                }
                return BadRequest(result.Errors);
            
        }
    }
}
