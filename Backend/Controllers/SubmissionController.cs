using Backend.Models;
using Backend.Persistence;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Backend.Controllers
{
    namespace Backend.Controllers
    {
        [Microsoft.AspNetCore.Mvc.Route("api/submission")]
        [ApiController]
        public class SubmissionController : ControllerBase
        {
            private readonly UserManager<User> _userManager;
            private readonly IConfiguration _configuration;
            private readonly MariaDBContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly UserService _userService;
            private readonly SubmissionService _submissionService;

            public SubmissionController(UserManager<User> userManager,
                MariaDBContext mariaDbContext,
                             IConfiguration configuration,
                             IHttpContextAccessor httpContextAccessor,
                             UserService userService,
                             SubmissionService submissionService)
            {
                this._userManager = userManager;
                this._configuration = configuration;
                this._context = mariaDbContext;
                this._httpContextAccessor = httpContextAccessor;
                this._userService = userService;
                _submissionService = submissionService;
            }

            public class CreateMusicDetailsModel
            {
                public string Link { get; set; }
            }
            public class CreateMovieDetailsModel
            {
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

            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            [HttpPost("create")]
            public async Task<IActionResult> CreateSubmission([FromBody] CreateSubmissionModel model)
            {
                var user = await _userService.GetLoggedInUser();
                if (user is null)
                {
                    return Unauthorized("Invalid Credentials");
                }
                if (model.musicDetails == null && model.MovieDetails == null)
                {
                    return BadRequest();
                }
                var canSubmit = await _userService.CanSubmitOnCompetition(model.CompetitionId, user.Id);
                if (!canSubmit)
                {
                    return BadRequest("cant submit");
                }
                using var transaction = await _context.Database.BeginTransactionAsync();
                if (model.musicDetails != null)
                {
                    var musicDetails = new MusicDetails() { Link = model.musicDetails.Link };
                    await _context.MusicDetails.AddAsync(musicDetails);
                    await _context.SaveChangesAsync();

                    if (musicDetails == null)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest();
                    }

                    var submissionResult = await _context.AddAsync(new Submission()
                    {
                        CompetitionId = model.CompetitionId,
                        OwnerId = user.Id,
                        Title = model.Title,
                        Description = model.Description,
                        MusicDetailsId = musicDetails.Id
                    });

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return Ok(new { Id = submissionResult.Entity.Id });

                }
                if (model.MovieDetails != null)
                {
                    var movieDetails = new MovieDetails() { TrailerLink = model.MovieDetails.TrailerLink, MovieLink = model.MovieDetails.TrailerLink };
                    await _context.MoviesDetails.AddAsync(movieDetails);
                    await _context.SaveChangesAsync();

                    if (movieDetails == null)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest();
                    }

                    var submissionResult = await _context.AddAsync(new Submission()
                    {
                        CompetitionId = model.CompetitionId,
                        OwnerId = user.Id,
                        Title = model.Title,
                        Description = model.Description,
                        MovieDetailsId = movieDetails.Id
                    });

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return Ok(new { Id = submissionResult.Entity.Id });

                }
                return BadRequest();
            }

            public class GetSubmissionmodel
            {
                public bool CanVote { get; set; }
                public bool HasVote { get; set; }
                public string Title { get; set; }
                public string Description { get; set; }
                public int Votes { get; set; }
                public CreateMusicDetailsModel? musicDetails { get; set; }
                public CreateMovieDetailsModel? MovieDetails { get; set; }
            }
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            [HttpGet("{id}")]
            public async Task<IActionResult> GetSubmission(int id)
            {
                var submission = await _context.Submissions.
                    Include(sub => sub.VotedUsers)
                    .Include(sub => sub.MovieDetails)
                    .Include(sub => sub.MusicDetails)
                    .FirstOrDefaultAsync(sub => sub.Id == id);
                if (submission is null)
                {
                    return BadRequest("Submission not found");
                }
                if (submission.MovieDetails is not null)
                {
                    return Ok(new GetSubmissionmodel()
                    {
                        CanVote = await _submissionService.CanVote(id),
                        HasVote = await _submissionService.HasVote(id),
                        Title = submission.Title,
                        Description = submission.Description,
                        Votes = submission.VoteCount,
                        MovieDetails = new CreateMovieDetailsModel()
                        {
                            MovieLink = submission.MovieDetails.MovieLink,
                            TrailerLink = submission.MovieDetails.TrailerLink
                        }

                    });
                }
                if (submission.MusicDetails is not null)
                {
                    return Ok(new GetSubmissionmodel()
                    {
                        CanVote = await _submissionService.CanVote(id),
                        HasVote = await _submissionService.HasVote(id),
                        Title = submission.Title,
                        Description = submission.Description,
                        Votes = submission.VotedUsers.Count,
                        musicDetails = new CreateMusicDetailsModel()
                        {
                            Link = submission.MusicDetails.Link,
                        }

                    });
                }
                return BadRequest("Unexpected details error");
            }


            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            [HttpPost("vote/{id}")]
            public async Task<IActionResult> VoteForSubmission(int id)
            {

                var result = await _submissionService.VoteForSubmission(id);
                if (result)
                {
                    return Ok();
                }
                return BadRequest("Cant Vote for this submission");

            }

            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            [HttpPost("unvote/{id}")]
            public async Task<IActionResult> UnVoteForSubmission(int id)
            {

                var result = await _submissionService.UnVoteForSubmission(id);
                if (result)
                {
                    return Ok();
                }
                return BadRequest("No vote on submision");

            }
        }

    }


}

