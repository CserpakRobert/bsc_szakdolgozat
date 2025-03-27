using Backend.Models;
using Backend.Persistence;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Backend.Controllers
{
    namespace Backend.Controllers
    {
        [Microsoft.AspNetCore.Mvc.Route("api/competition")]
        [ApiController]
        public class SubmissionController : ControllerBase
        {
            private readonly UserManager<User> _userManager;
            private readonly IConfiguration _configuration;
            private readonly MariaDBContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly UserService _userService;

            public SubmissionController(UserManager<User> userManager,
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

            public class CreateMusicDetailsModel
            {
                public string Link { get; set; }
            }
            public class CreateMovieDetailsModel
            {
                public string Link { get; set; }
                public string TrailerLink { get; set; }
                public string MovieLink { get; set; }
            }

            public class CreateSubmissionModel
            {
                public int CompetitionId { get; set; }
                public string Title { get; set; }
                public string Description { get; set; }
                public CreateMusicDetailsModel? musicDetails { get; set; }
                public CreateMovieDetailsModel? MovieDetails { get; set; }
                public ICollection<string> InvitedUsers { get; set; } = [];
            }

            [Authorize]
            [HttpPost("/submission/create")]
            public async Task<IActionResult> CreateSubmission([FromBody] CreateSubmissionModel model)
            {
                var user = await _userService.GetLoggedInUser();
                if (user is null)
                {
                    return Unauthorized("Invalid Credentials");
                }
                if (model.musicDetails == null ^ model.MovieDetails == null)
                {
                    return BadRequest();
                }
                var canSubmit = await _userService.CanSubmitOnCompetition(model.CompetitionId, user.Id);
                if (!canSubmit)
                {
                    return BadRequest("cant submit");
                }

                if (model.musicDetails != null)
                {
                    var musicDetails = new MusicDetails() { Link = model.musicDetails.Link };
                    await _context.MusicDetails.AddAsync(musicDetails);
                    if (musicDetails == null) return BadRequest();
                    var submissionResult = await _context.AddAsync(new Submission()
                    {
                        CompetitionId = model.CompetitionId,
                        OwnerId = user.Id,
                        Title = model.Title,
                        Description = model.Description,
                        MusicDetailsId = musicDetails.Id
                    });
                    await _context.SaveChangesAsync();
                    return Ok(new { Id = submissionResult.Entity.Id });

                }
                if (model.MovieDetails != null)
                {
                    var movieDetails = new MovieDetails() { TrailerLink = model.MovieDetails.TrailerLink, MovieLink = model.MovieDetails.TrailerLink  };
                    await _context.MoviesDetails.AddAsync(movieDetails);
                    if (movieDetails == null) return BadRequest();
                    var submissionResult = await _context.AddAsync(new Submission()
                    {
                        CompetitionId = model.CompetitionId,
                        OwnerId = user.Id,
                        Title = model.Title,
                        Description = model.Description,
                        MovieDetailsId = movieDetails.Id
                    });
                    await _context.SaveChangesAsync();
                    return Ok(new { Id = submissionResult.Entity.Id });

                }
                return BadRequest();
            }
        }
    }
}
