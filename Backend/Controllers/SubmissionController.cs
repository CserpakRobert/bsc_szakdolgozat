using Backend.Models;
using Backend.Models.DTOs;
using Backend.Persistence;
using Backend.Services;
using Backend.Utils;
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
            private readonly SubmissionService _submissionService;

            public SubmissionController(
                             SubmissionService submissionService)
            {
                _submissionService = submissionService;
            }


            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            [HttpPost("list")]
            public virtual async Task<IActionResult> GetSubmissionList([FromBody] SubmissionListGetModel listGetModel)
            {
                try
                {
                    return Ok(await _submissionService.GetSubmissionList(listGetModel));
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
            [HttpGet("{id}")]
            public virtual async Task<IActionResult> GetSubmission(int id)
            {
                try
                {
                    return Ok(await _submissionService.GetSubmission(id));
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
            [HttpGet("ownSubmission/{competitionId}")]
            public virtual async Task<IActionResult> GetOwnSubmission(int competitionId)
            {
                try
                {
                    return Ok(await _submissionService.GetOwnSubmission(competitionId));
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
            [HttpGet("type/{id}")]
            public virtual async Task<IActionResult> GetSubmissionType(int id)
            {
                try
                {
                    return Ok(await _submissionService.GetType(id));
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
            [HttpPost("create/movie")]
            public virtual async Task<IActionResult> CreateSubmission([FromBody] ModifyMovieDetails model)
            {
                try
                {
                    return Ok(await _submissionService.ModifyMovieSubmission(model));
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
            [HttpPost("create/music")]
            public virtual async Task<IActionResult> CreateMusicSubmission([FromBody] ModifyMusicDetails model)
            {
                try
                {
                    return Ok(await _submissionService.ModifyMusicSubmission(model));
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
            [HttpPost("create/story")]
            public virtual async Task<IActionResult> CreateSubmission([FromBody] ModifyShortStoryDetails model)
            {
                try
                {
                    return Ok(await _submissionService.ModifyShortStorySubmission(model));
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
            [HttpPost("vote/{id}")]
            public virtual async Task<IActionResult> VoteForSubmission(int id)
            {

                try
                {
                    await _submissionService.VoteForSubmission(id);
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
            [HttpPost("unvote/{id}")]
            public virtual async Task<IActionResult> UnVoteForSubmission(int id)
            {

                try
                {
                    await _submissionService.UnVoteForSubmission(id);
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
        }

    }


}

