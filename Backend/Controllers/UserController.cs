using Backend.Models;
using Backend.Models.DTOs;
using Backend.Services;
using Backend.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            this._userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                await _userService.RegisterUser(model);
                return Ok();
            }
            catch (ExceptionWithDescription e)
            {

                return BadRequest(new { errorMessage = e.Message });
            }
            catch (UserNotAuthorized)
            {
                return Unauthorized();
            }

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{name}")]
        public async Task<IActionResult> UserById(string name)
        {
            var result = await _userService.GetUserByName(name);
            if (result is null)
            {
                return BadRequest("Nem talált felhasználó");
            }
            return Ok(new { result.Id, result.UserName });
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            var result = await _userService.LoginUser(model);

            if (result == null)
            {
                return Unauthorized("Bejelnentkezés sikertelen");
            }

            return Ok(result);
        }


    }
}

