using Backend.Models;
using Backend.Models.DTOs;
using Backend.Persistence;
using Backend.Services;
using Backend.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/group")]
    [ApiController]
    public class GroupController : ControllerBase
    {

        private readonly GroupService _groupService;
        private readonly CompetitionService _competitionService;
        private readonly UserService _userService;

        public GroupController(
            GroupService groupService,
            CompetitionService competitionService,
            UserService userService
            )
        {
            this._groupService = groupService;
            this._competitionService = competitionService;
            this._userService = userService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("create/{competitionId}")]
        public async Task<IActionResult> CreateClosed(int competitionId, [FromBody] CreateGroupDTO createGroup)
        {
            try
            {
                return Ok(await _groupService.Create(competitionId, createGroup));
            }
            catch (ExceptionWithDescription e)
            {
                return BadRequest(new  { errorMessage= e.Message });
            }
            catch (UserNotAuthorized)
            {
                return Unauthorized();
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public async Task<IActionResult> CreateClosed(int id)
        {
            try
            {
                return Ok(await _groupService.GetById(id));
            }
            catch (ExceptionWithDescription e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("invite/{groupId}")]
        public async Task<IActionResult> Invite(int groupId, [FromBody] InviteDTO invite)
        {

            try
            {
                return Ok(await _groupService.Invite(groupId, invite.UserName));
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
        [HttpGet("invites/{groupId}")]
        public async Task<IActionResult> Invites(int groupId)
        {

            try
            {
                return Ok(await _groupService.getInvites(groupId));
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
        [HttpPost("invites/delete/{groupId}")]
        public async Task<IActionResult> DeleteInvite(int groupId, [FromBody]InviteDTO invite)
        {

            try
            {
                return Ok(await _groupService.DeleteInvite(groupId,invite.UserName));
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
        [HttpPost("invites/accept/{groupId}")]
        public async Task<IActionResult> AcceptInvite(int groupId, [FromBody] InviteDTO invite)
        {
            //TODO!
            try
            {
                return Ok(await _groupService.DeleteInvite(groupId, invite.UserName));
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
        [HttpPost("join/{groupId}")]
        public async Task<IActionResult> RequestJoin(int groupId, [FromBody] GroupRequestDTO request)
        {

            try
            {
                return Ok(await _groupService.RequestJoin(groupId, request.Description));
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
        [HttpGet("hasjoin/{groupId}")]
        public async Task<IActionResult> RequestJoin(int groupId )
        {

            try
            {
                return Ok(await _groupService.HasJoin(groupId));
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
        [HttpGet("requests/{groupId}")]
        public async Task<IActionResult> Requests(int groupId)
        {

            try
            {
                return Ok(await _groupService.getRequests(groupId));
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
        [HttpPost("requests/delete/{groupId}")]
        public async Task<IActionResult> DeleteRequest(int groupId, [FromBody] InviteDTO invite)
        {

            try
            {
                return Ok(await _groupService.DeleteRequest(groupId, invite.UserName));
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
        [HttpPost("requests/accept/{groupId}")]
        public async Task<IActionResult> AcceptRequest(int groupId, [FromBody] InviteDTO invite)
        {
            try
            {
                return Ok(await _groupService.AcceptRequest(groupId, invite.UserName));
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
        [HttpPost("delete/{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var groupOwner = await _groupService.IsOwner(groupId);
            if (groupOwner is null or false) return BadRequest("Csak saját csapatot törölhetsz");

            var result = await _groupService.Delete(groupId);
            if (result is null or false) return BadRequest("A törlés sikertelen");

            return Ok();
        }

    }
}
