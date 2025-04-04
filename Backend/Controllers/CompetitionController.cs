using Backend.Models;
using Backend.Models.DTOs;
using Backend.Persistence;
using Backend.Services;
using Backend.Utils;
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
        private readonly MariaDBContext _context;
        private readonly UserService _userService;
        private readonly CompetitionService _competitionService;

        public CompetitionController(UserManager<User> userManager,
            MariaDBContext mariaDbContext,
                CompetitionService competitionService,
                         IConfiguration configuration,
                         IHttpContextAccessor httpContextAccessor,
                         UserService userService)
        {
            this._context = mariaDbContext;
            this._userService = userService;
            this._competitionService = competitionService;
        }

        //[HttpGet]
        //public async Task<IEnumerable<Competition>> GetAll()
        //{
        //    return _context.Competitions.Where(competition => competition.IsPrivate == false).Skip(0);
        //}



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("list")]
        public async Task<IActionResult> GetListDetails([FromBody]GetCompetitionListDTO listArgs)
        {
            Debug.WriteLine(listArgs);
            var result = await _competitionService.GetDetailsList(listArgs);
            if (result is null) return BadRequest("nem találhatóak versenyek");
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCompetitionDTO model)
        {

            var user = await _userService.GetLoggedInUser();
            if (user is null)
            {
                return Unauthorized("Invalid Credentials");
            }
            if (model.Category is not ("movie" or "music" or "shortstory")) return BadRequest();
            var result = await _context.Competitions.AddAsync(new Competition()
            {
                Title = model.Title,
                Description = model.Description,
                Creator = user,
                Moderators = [user],
                IsPrivate = false,
                IsInPreparationPhase = true,
                IsInSubmissionPhase = true,
                IsInVotingPhase = true,
                IsMovieCompetition = model.Category == "movie",
                IsMusicCompetition = model.Category == "music",
                IsShortStoryCompetition = model.Category == "shortstory"

            });
            await _context.SaveChangesAsync();

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(new { Id = result.Entity.Id });

        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await _competitionService.GetDetails(id));
            }
            catch (ExceptionWithDescription e)
            {

                return BadRequest(new { errorMessage= e.Message });
            }

        }

    }
}
