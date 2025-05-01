using Backend.Models;
using Backend.Models.DTOs;
using Backend.Persistence;
using Backend.Services;
using Backend.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Backend.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/group")]
    [ApiController]
    public class GroupController : ControllerBase
    {

        private readonly GroupService _groupService;

        public GroupController(
            GroupService groupService
            )
        {
            this._groupService = groupService;
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
                return BadRequest(new { errorMessage = e.Message });
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
            catch (UserNotAuthorized)
            {
                return Unauthorized();
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("privateGroups/{page}")]
        public async Task<IActionResult> GetPrivateGroups(int page)
        {
            try
            {
                return Ok(await _groupService.GetPrivateGroups(page));
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
        [HttpGet("invitedGroups/{page}")]
        public async Task<IActionResult> GetInvitedGroups(int page)
        {
            try
            {
                return Ok(await _groupService.GetInvitedGroups(page));
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
        [HttpGet("joinedGroups/{page}")]
        public async Task<IActionResult> GetJoinedGroups(int page)
        {
            try
            {
                return Ok(await _groupService.GetJoinedGroups(page));
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
        [HttpPost("smallDetails/{competitionId}")]
        public async Task<IActionResult> GetSmallDetails(int competitionId, [FromBody] int page)
        {
            try
            {
                return Ok(await _groupService.GroupSmallDetails(competitionId, page));
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
        [HttpGet("ownGroup/{competitionId}")]
        public async Task<IActionResult> GetSmallDetails(int competitionId)
        {
            try
            {
                return Ok(await _groupService.GetOwnGroup(competitionId));
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
        [HttpPost("useropengroups/{username}")]
        public async Task<IActionResult> GetSmallDetails(string username, [FromBody] int page)
        {
            try
            {
                return Ok(await _groupService.UserOpenGroups(username, page));
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
        [HttpGet("isInvited/{groupId}")]
        public async Task<IActionResult> IsInvited(int groupId)
        {
            try
            {
                return Ok(await _groupService.IsInvited(groupId));
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
        public async Task<IActionResult> DeleteInvite(int groupId, [FromBody] InviteDTO invite)
        {

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
        [HttpPost("invites/accept/{groupId}")]
        public async Task<IActionResult> AcceptInvite(int groupId)
        {
            try
            {
                return Ok(await _groupService.AcceptInvite(groupId));
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
        [HttpPost("invites/decline/{groupId}")]
        public async Task<IActionResult> DeclineInvite(int groupId)
        {
            try
            {
                return Ok(await _groupService.DeclineInvite(groupId));
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
        public async Task<IActionResult> RequestJoin(int groupId)
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
        [HttpPost("requests/{groupId}")]
        public async Task<IActionResult> GetDescription(int groupId, [FromBody] InviteDTO invite)
        {

            try
            {
                return Ok(await _groupService.GetDescription(groupId, invite.UserName));
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


    }
}
