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
        [HttpPost("join/{groupId}")]
        public async Task<IActionResult> JoinGroup(int groupId, GroupRequestDTO request)
        {
            var group = await _groupService.GetById(groupId);
            if (group is null || group.IsClosed) return BadRequest("A csapat nem található");

            var result = await _groupService.RequestJoin(groupId, request);
            if (result is null) return BadRequest("A jelentkezés sikertelen");

            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("request/{groupId}")]
        public async Task<IActionResult> InviteUser(int groupId, [FromBody] InviteDTO invitedUser)
        {
            var inviteUser = await _userService.GetUserByName(invitedUser.UserName);
            if (inviteUser is null) return BadRequest("A megadott felhasználó nem található");
            if (await _groupService.CanJoin(inviteUser.Id)) return BadRequest("A megadott felhasználó már jelentkezett erre a versenyre");

            var result = await _groupService.InviteUser(groupId,inviteUser.UserName);
            if (result is null) return BadRequest("A meghívás sikertelen");

            return Ok();
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
