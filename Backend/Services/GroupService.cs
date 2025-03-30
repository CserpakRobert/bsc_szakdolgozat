using Backend.Models;
using Backend.Models.DTOs;
using Backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{

    /// <summary>
    /// For tables Group, SubmissionInvites, ClosedGroupJoinRequests
    /// </summary>
    public class GroupService
    {
        private readonly MariaDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserService _userService;
        private readonly CompetitionService _competitionService;

        public GroupService(IHttpContextAccessor httpContextAccessor,
            UserService userService,
            MariaDBContext context,
            CompetitionService competitionService
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _context = context;
            this._competitionService = competitionService;
        }

        public async Task<bool> CanJoin(int competitionId, int? userId = null)
        {

            userId ??= _userService.GetLoggedInUser().Id;
            if (userId is null) return false;

            var hasJoin = _context.Groups.Include(g => g.Participants)
                .Where(g => g.CompetitionId == competitionId)
                .AnyAsync(g => g.GroupOwnerId == userId || g.Participants.Any(p => p.Id == userId));

            var canJoin = _competitionService.IsModerator(competitionId, (int)userId);

            return await canJoin && await hasJoin;
        }
        public async Task<bool?> IsOwner(int groupId,string? userName = null)
        {
            User? user;
            if (userName is null)
            {
                user = await _userService.GetLoggedInUser();
            }
            else
            {
                user = await _userService.GetUserByName(userName);
            }
            if (user is null) return false;
            var group = await _context.Groups.FindAsync(groupId);
            if (group is null) return false;

            return group.GroupOwnerId == user.Id;
        }

        public async Task<int?> GetGroupsCompetition(int groupId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group is null) return null;
            return group.CompetitionId;

        }

        public async Task<int?> Create(int competitionId,CreateGroupDTO createGroup)
        {
            if (!await CanJoin(competitionId)) return null;
            var result = await _context.Groups.AddAsync(new Group()
            {
                CompetitionId = competitionId,
                Description = createGroup.Description,
                IsClosed = createGroup.IsClosed
            });
            await _context.SaveChangesAsync();
            if (result is null) return null;
            return result.Entity.Id;
        }

        public async Task<Group?> GetById(int groupId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            return group;
        }

        public async Task<int?> RequestJoin(int groupId, GroupRequestDTO request)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) return null;

            var group = await GetById(groupId);
            if (group is null) return null;

            var result =await _context.JoinRequests
                .AddAsync(new OpenGroupJoinRequests() { 
                    Description = request.Description,
                    Group = group,
                    RequestOwner = user
                });
            await _context.SaveChangesAsync();
            return result.Entity.Id;
        }

        public async Task<bool?> InviteUser(int groupId, string userName)
        {
            var sender = await _userService.GetLoggedInUser();
            if (sender is null) return null;

            var reciever = await _userService.GetUserByName(userName);
            if (reciever is null) return null;
            var group = await GetById(groupId);
            if (group is null) return null;


            var result = await _context.SubmissionInvites
                .AddAsync(new SubmissionInvites()
                {
                    Group = group,
                    RecieverUser = reciever,
                    SenderUser = sender
                });
            return result is null;
        }

        public async Task<bool?> Delete(int groupId)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) return null;

            var group = await _context.Groups
                .Include(g => g.Participants)
                .Include(g => g.JoinRequests)
                .Include(g => g.Invites)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group is null) return false;
             _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return true;
            
        }
    }
}
