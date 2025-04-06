using Backend.Models;
using Backend.Models.DTOs;
using Backend.Models.JoinTables;
using Backend.Persistence;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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

            var hasJoin = !await _context.Groups.Include(g => g.Participants)
                .Where(g => g.CompetitionId == competitionId)
                .AnyAsync(g => g.GroupOwnerId == userId || g.Participants.Any(p => p.Id == userId));

            var canJoin = !await _competitionService.IsModerator(competitionId, (int)userId);

            return canJoin && hasJoin;
        }
        public async Task<bool?> IsOwner(int groupId, string? userName = null)
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

        public async Task<int> Create(int competitionId, CreateGroupDTO createGroup)
        {
            if (await CanJoin(competitionId) == false)
                throw new ExceptionWithDescription("Erre a versenyre nem tudsz jelentkezni");

            if (await _competitionService.IsInPreparationPhase(competitionId) is null or false)
                throw new ExceptionWithDescription("A jelentkezési időszak már lejárt");

            var user = await _userService.GetLoggedInUser();
            if (user is null)
                throw new UserNotAuthorized("");

            var result = await _context.Groups.AddAsync(new Models.Group()
            {
                GroupOwner = user,
                CompetitionId = competitionId,
                Description = createGroup.Description,
                IsClosed = createGroup.IsClosed
            });

            var joinTable = await _context.UserParticipateGroups.AddAsync(new UserParticipateGroups()
            {
                Group = result.Entity,
                User = user
            });

            await _context.SaveChangesAsync();

            if (result is null)
                throw new ExceptionWithDescription("A csapat létrehozása sikertelen");
            return result.Entity.Id;
        }

        public async Task<GetGroupDTO> GetById(int groupId)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null)
                throw new UserNotAuthorized("");



            var group = await _context.Groups.Include(g => g.Participants)
                .Include(g => g.GroupOwner)
                .Include(g => g.JoinRequests)
                .Include(g => g.Invites)
                .Select(g => new GetGroupDTO()
                {
                    GroupId = g.Id,
                    CompetitionId = g.CompetitionId,
                    CompetitionTitle = g.Competition.Title,
                    Description = g.Description,
                    IsClosed = g.IsClosed,
                    OwnerName = g.GroupOwner.UserName,
                    InviteCount = g.Invites.Count,
                    RequestCount = g.JoinRequests.Count,
                    ParticipateNames = g.Participants.Select(p => p.UserName).ToList()
                })
                .FirstOrDefaultAsync(g => g.GroupId == groupId);

            if (group is null) throw new ExceptionWithDescription("A csapat nem található");
            if (group.IsClosed && group.OwnerName != user.UserName && !group.ParticipateNames.Any(p => p == user.UserName))
            {
                throw new ExceptionWithDescription("A csapat zárva van, csak a tagjai láthatják");
            }
            return group;
        }


        public async Task<bool> Invite(int groupId, string userName)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            var invitee = await _userService.GetUserByName(userName);
            if (invitee is null) throw new ExceptionWithDescription("A felhasználó nem található");

            var group = await _context.Groups
                .Include(g => g.Participants)
                .Include(g => g.GroupOwner)
                .Include(g => g.Invites)
                .FirstOrDefaultAsync(g => g.Id == groupId);
            if (group is null) throw new ExceptionWithDescription("A csapat nem található");
            if (group.GroupOwner.UserName != user.UserName)
            {
                throw new ExceptionWithDescription("Csak a csapat tulajdonosa hívhat meg résztvevőet");
            }
            if (!await CanJoin(group.CompetitionId, invitee.Id))
            {
                throw new ExceptionWithDescription("A megadott felhasználó nem tud jelentkezni erre a versenyre");
            }
            using var transaction = await _context.Database.BeginTransactionAsync();

            var invite = await _context.SubmissionInvites.AddAsync(new SubmissionInvites()
            {
                Group = group,
                RecieverUser = invitee,
                SenderUser = user
            });

            group.Invites.Add(invite.Entity);
            await _context.SaveChangesAsync();
            transaction.Commit();
            return true;
        }


        public async Task<IEnumerable<string>> getInvites(int groupId)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            if (await IsOwner(groupId, user.UserName) is null)
            {
                throw new ExceptionWithDescription("Csak a csapat tulajdonosa láthatja a meghívottakat");
            }

            var invites = await _context.SubmissionInvites
                .Include(si => si.RecieverUser)
                .Where(si => si.GroupId == groupId)
                .Select(si => si.RecieverUser.UserName)
                .ToListAsync();

            return invites;
        }

        internal async Task<IEnumerable<string>> getRequests(int groupId)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            if (await IsOwner(groupId, user.UserName) is null)
            {
                throw new ExceptionWithDescription("Csak a csapat tulajdonosa láthatja a jelentkezéseket");
            }

            var requests = await _context.JoinRequests
                .Include(jr => jr.RequestOwner)
                .Where(jr => jr.GroupId == groupId)
                .Select(jr => jr.RequestOwner.UserName).ToListAsync();

            return requests;
        }


        public async Task<bool> DeleteInvite(int groupId, string userName)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            var invitee = await _userService.GetUserByName(userName);
            if (invitee is null) throw new ExceptionWithDescription("A felhasználó nem található");

            var invite = await _context.SubmissionInvites.Where(si => si.GroupId == groupId && si.RecieverUserId == invitee.Id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return true;

        }


        public async Task<bool> RequestJoin(int groupId, string description)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            if (!await CanJoin(groupId))
                throw new ExceptionWithDescription("Erre a versenyre nem tudsz jelentkezni");

            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group is null) throw new ExceptionWithDescription("A csapat nem található");

            if (await _context.JoinRequests.AnyAsync(ji => ji.GroupId == groupId && ji.RequestOwnerId == user.Id))
            {
                throw new ExceptionWithDescription("Már jelentkezdtél");
            }

            var result = await _context.JoinRequests.AddAsync(new OpenGroupJoinRequests()
            {
                Group = group,
                RequestOwner = user,
                Description = description
            });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRequest(int groupId, string userName)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            if (await IsOwner(groupId, user.UserName) is null or false)
            {
                throw new ExceptionWithDescription("Csak a csapat tulajdonosa törölhetjelentkezést");
            }

            var result = await _context.JoinRequests
                .Include(jr => jr.RequestOwner)
                .Where(jr => jr.GroupId == groupId && jr.RequestOwner.UserName == userName)
                .ExecuteDeleteAsync();
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> HasJoin(int groupId)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            return await _context.JoinRequests.AnyAsync(ji => ji.GroupId == groupId && ji.RequestOwnerId == user.Id);


        }

        internal async Task<bool> AcceptRequest(int groupId, string userName)
        {

            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            if (await IsOwner(groupId, user.UserName) is null or false)
            {
                throw new ExceptionWithDescription("Csak a csapat tulajdonosa törölhetjelentkezést");
            }

            var group = await _context.Groups.FirstAsync(g => g.Id == groupId);
            if (group is null) throw new ExceptionWithDescription("A csapat nem található");

            var result = await _context.JoinRequests
                .Include(jr => jr.RequestOwner)
                .Where(jr => jr.GroupId == groupId && jr.RequestOwner.UserName == userName)
                .ExecuteDeleteAsync();

            if (result == 0) throw new ExceptionWithDescription("A jelentkezés nem található");

            var addUser = await _userService.GetUserByName(userName);
            if (addUser is null) throw new ExceptionWithDescription("A felhasználó nem található");

            var addResult = await _context.UserParticipateGroups.AddAsync(new UserParticipateGroups()
            {
                Group = group,
                User= addUser
            });

            await _context.SaveChangesAsync();

            return true;
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

        public async Task DeletePendingGroups(int userId, int groupId)
        {
            var toDeleteInvites = _context.SubmissionInvites.Where(si => si.GroupId == groupId && si.RecieverUserId == userId);
            var toDeleteRequests = _context.JoinRequests.Where(jr => jr.GroupId == groupId && jr.RequestOwnerId == userId);
            _context.SubmissionInvites.RemoveRange(toDeleteInvites);
            _context.JoinRequests.RemoveRange(toDeleteRequests);
            await _context.SaveChangesAsync();
        }


    }
}
