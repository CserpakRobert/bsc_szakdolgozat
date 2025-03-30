using Backend.Models;
using Backend.Models.DTOs;
using Backend.Persistence;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    namespace Backend.Controllers
    {
        [Microsoft.AspNetCore.Mvc.Route("api/submission")]
        [ApiController]
        public class SubmissionController : ControllerBase
        {
            private readonly MariaDBContext _context;
            private readonly SubmissionService _submissionService;

            public SubmissionController(UserManager<User> userManager,
                MariaDBContext mariaDbContext,
                             SubmissionService submissionService)
            {
                this._context = mariaDbContext;
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
            [HttpPost("modify")]
            public async Task<IActionResult> CreateSubmission([FromBody] IModifySubmission model)
            {
                if (!await _submissionService.CanEdit(model.Id))
                {
                    BadRequest("Ezt a pályázatot nem szerkesztheted!");
                }
                var result = _submissionService.ModifySubmission(model);
                if (result is not null)
                {
                    return Ok();
                }
                return BadRequest("Probléma a módosítással");
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
                    return BadRequest(new {ErrorMessage = "Submission not found" });
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
                return BadRequest(new { ErrorMessage = "Unexpected details error" });
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

