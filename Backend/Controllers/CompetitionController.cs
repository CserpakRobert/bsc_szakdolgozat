using Backend.Models;
using Backend.Persistence;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/competition")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly MariaDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public CompetitionController(UserManager<User> userManager,
            MariaDBContext mariaDbContext,
                         IConfiguration configuration,
                         IHttpContextAccessor httpContextAccessor)
        {
            this._userManager = userManager;
            this._configuration = configuration;
            this._context = mariaDbContext;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IEnumerable<Competition>> GetAll()
        {
            return _context.Competitions.Where(competition => competition.IsPrivate == false).Skip(0);
        }


        public class CreateCompetitionModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("create")]

        public async Task<IActionResult> Create([FromBody] CreateCompetitionModel model)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Debug.WriteLine($"user id: {userId}");
            if (String.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid credentials.");
            }
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {

                return Unauthorized("Invalid credentials.");
            }

            var result = await _context.Competitions.AddAsync(new Competition() { Title = model.Title, Description = model.Description, Creator = user, Moderators = [user], IsPrivate = false });
            await _context.SaveChangesAsync();

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result.Entity.Id);
            
        }
    }
}
