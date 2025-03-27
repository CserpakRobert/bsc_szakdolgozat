using Backend.Models;
using Backend.Persistence;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/competition")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly MariaDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserService _userService;

        public CompetitionController(UserManager<User> userManager,
            MariaDBContext mariaDbContext,
                         IConfiguration configuration,
                         IHttpContextAccessor httpContextAccessor,
                         UserService userService)
        {
            this._userManager = userManager;
            this._configuration = configuration;
            this._context = mariaDbContext;
            this._httpContextAccessor = httpContextAccessor;
            this._userService = userService;
        }

        //[HttpGet]
        //public async Task<IEnumerable<Competition>> GetAll()
        //{
        //    return _context.Competitions.Where(competition => competition.IsPrivate == false).Skip(0);
        //}


        public class CreateCompetitionModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("create")]

        public async Task<IActionResult> Create([FromBody] CreateCompetitionModel model)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null)
            {
                return Unauthorized("Invalid Credentials");
            }
            if (model.Category != "movie" && model.Category != "music") return BadRequest();
            var result = await _context.Competitions.AddAsync(new Competition()
            {
                Title = model.Title,
                Description= model.Description,
                Creator = user,
                Moderators = [user],
                IsPrivate = false,
                isMovieCompetition = model.Category == "movie",
                isMusicCompetition = model.Category == "music"
                
            });
            await _context.SaveChangesAsync();

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(new { Id = result.Entity.Id });

        }
        public class GetOneCompetitionModel
        {
            public bool IsCreator { get; set; } = false;
            public bool IsModerator { get; set; } = false;
            public bool CanSubmit { get; set; } = true;
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public string Creatorname { get; set; } = "";
            public ICollection<string> ModeratorNames = [];

            public bool VotingEnabled { get; set; } = false;
            public bool SubmissionsEnabled { get; set; } = false;



        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null)
            {
                return Unauthorized("Invalid Credentials");
            }

            var competition = await _context.Competitions
                .Include(comp => comp.Creator)
                .Include(comp => comp.Moderators)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (competition == null)
            {
                return NotFound("Competition not found");
            }
            var result = new GetOneCompetitionModel();

            result = new GetOneCompetitionModel()
            {
                IsCreator = await _userService.IsCreatorInCompetition(competition.Id, user.Id),
                IsModerator = await _userService.IsModeratorInCompetition(competition.Id, user.Id),
                CanSubmit = await _userService.CanSubmitOnCompetition(competition.Id, user.Id),
                Title = competition.Title,
                Description = competition.Description,
                Creatorname = competition.Creator.UserName,
                ModeratorNames = [.. competition.Moderators.Select(moderator => moderator.UserName)],
                VotingEnabled = competition.IsInVotingPhase,
                SubmissionsEnabled = competition.IsInSubmissionPhase


            };

            return Ok(result);

        }

    }
}
