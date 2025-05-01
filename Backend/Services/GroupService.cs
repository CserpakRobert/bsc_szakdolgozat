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
        private readonly UserService _userService;

        public GroupService(
            UserService userService,
            MariaDBContext context
            )
        {
            _userService = userService;
            _context = context;
        }

        public virtual async Task<bool> CanJoin(int competitionId, int? userId = null)
        {

            if (userId is null)
            {
                var user = await _userService.GetLoggedInUser();
                if (user is null) return false;
                userId = user.Id;
            }

            if (userId is null) return false;

            var hasJoin = !await _context.Groups
                .Include(g => g.Participants)
                .Where(g => g.CompetitionId == competitionId)
                .AnyAsync(g => g.Participants.Any(p => p.Id == userId));

            var canJoin = !await _context.Moderators.AnyAsync(m => m.UserId == userId && m.CompetitionId == competitionId);

            return canJoin && hasJoin;
        }

        public virtual async Task<GetSmallDetailsDTO> GroupSmallDetails(int competitionId, int page)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");
            var groups = await _context.Groups
                .Where(c => c.CompetitionId == competitionId && !c.IsClosed)
                .Include(g => g.Participants)
                .Include(g => g.GroupOwner)
                .Skip(page * 50)
                .Take(50)
                .Select(g => new GroupSmallDetailsDTO(g))
                .ToListAsync();
            if (groups is null) throw new ExceptionWithDescription("Nincsenek csapatok a versenyhez");

            var count = await _context.Groups
                .Where(c => c.CompetitionId == competitionId && !c.IsClosed)
                .CountAsync();

            return new GetSmallDetailsDTO()
            {
                Groups = groups,
                Count = ((count - 1) / 50) + 1
            };

        }
        public virtual async Task<bool?> IsOwner(int groupId, string? userName = null)
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

        public virtual async Task<int?> GetGroupsCompetition(int groupId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group is null) return null;
            return group.CompetitionId;

        }

        public virtual async Task<int> Create(int competitionId, CreateGroupDTO createGroup)
        {
            if (await CanJoin(competitionId) is false)
                throw new ExceptionWithDescription("Erre a versenyre nem tudsz jelentkezni");

            if ( _context.Competitions.Find(competitionId)?.IsInPreparationPhase is null or false)
                throw new ExceptionWithDescription("A jelentkezési időszak már lejárt");

            var user = await _userService.GetLoggedInUser();
            if (user is null)
                throw new UserNotAuthorized("");

            if (createGroup.Description.Length > 3000 || createGroup.Description.Length < 10)
                throw new ExceptionWithDescription("A leírásnak 10 és 3000 karakter között kell lennie");

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

        public virtual async Task<GetGroupDTO> GetById(int groupId)
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
            if (group.IsClosed
                && (group.OwnerName != user.UserName
                && !group.ParticipateNames.Any(p => p == user.UserName)
                && !await _context.SubmissionInvites.AnyAsync(si => si.RecieverUserId == user.Id))
                )
            {
                throw new ExceptionWithDescription("A csapat zárva van, csak a tagjai láthatják");
            }
            return group;
        }


        public virtual async Task<bool> Invite(int groupId, string userName)
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


        public virtual async Task<IEnumerable<string>> getInvites(int groupId)
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

        public virtual async Task<IEnumerable<string>> getRequests(int groupId)
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


        public virtual async Task<bool> DeleteInvite(int groupId, string userName)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            var invitee = await _userService.GetUserByName(userName);
            if (invitee is null) throw new ExceptionWithDescription("A felhasználó nem található");

            var invite = await _context.SubmissionInvites.Where(si => si.GroupId == groupId && si.RecieverUserId == invitee.Id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return true;

        }


        public virtual async Task<bool> RequestJoin(int groupId, string description)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            if (!await CanJoin(groupId))
                throw new ExceptionWithDescription("Erre a versenyre nem tudsz jelentkezni");

            if (description.Length > 3000 || description.Length < 10)
                throw new ExceptionWithDescription("A leírásnak 10 és 3000 karakter között kell lennie");

            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group is null) throw new ExceptionWithDescription("A csapat nem található");

            if (await CanJoin(group.CompetitionId, user.Id) is false)
            {
                throw new ExceptionWithDescription("Nem jelentkezhetsz a saját versenyedre");
            }

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

        public virtual async Task<bool> DeleteRequest(int groupId, string userName)
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

        public virtual async Task<bool> HasJoin(int groupId)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            return await _context.JoinRequests.AnyAsync(ji => ji.GroupId == groupId && ji.RequestOwnerId == user.Id);


        }

        public virtual async Task<bool> AcceptRequest(int groupId, string userName)
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
                User = addUser
            });

            await _context.SaveChangesAsync();

            return true;
        }

        public virtual async Task<bool> IsInvited(int groupId)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            return await _context.SubmissionInvites.AnyAsync(ji => ji.GroupId == groupId && ji.RecieverUserId == user.Id);
        }

        public virtual async Task<bool> AcceptInvite(int groupId)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            if (!await _context.SubmissionInvites.AnyAsync(si => si.RecieverUserId == user.Id || si.GroupId == groupId))
                throw new ExceptionWithDescription("Ebbe a csoportba nincs meghívásod");

            var deleteR = await _context.SubmissionInvites
                .Where(si => si.RecieverUserId == user.Id && si.GroupId == groupId)
                .ExecuteDeleteAsync();
            var addResult = await _context.UserParticipateGroups.AddAsync(new UserParticipateGroups()
            {
                GroupId = groupId,
                UserId = user.Id
            });
            await _context.SaveChangesAsync();
            return true;

        }
        public virtual async Task<bool> DeclineInvite(int groupId)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            if (!await _context.SubmissionInvites.AnyAsync(si => si.RecieverUserId == user.Id || si.GroupId == groupId))
                throw new ExceptionWithDescription("Ebbe a csoportba nincs meghívásod");

            var deleteR = await _context.SubmissionInvites
                .Where(si => si.RecieverUserId == user.Id && si.GroupId == groupId)
                .ExecuteDeleteAsync();

            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<string> GetDescription(int groupId, string userName)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");
            if (await IsOwner(groupId, user.UserName) is null)
            {
                throw new ExceptionWithDescription("Csak a csapat tulajdonosa láthatja a jelentkezéseket");
            }
            var result = await _context.JoinRequests
                .Include(jr => jr.RequestOwner)
                .Where(jr => jr.GroupId == groupId && jr.RequestOwner.UserName == userName)
                .Select(jr => jr.Description)
                .FirstOrDefaultAsync();

            if (result is null) throw new ExceptionWithDescription("A jelentkezés nem található");
            return result;
        }

        public virtual async Task<GetSmallDetailsDTO> GetInvitedGroups()
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");
            var groups = await _context.SubmissionInvites
                .Include(si => si.Group)
                .Where(si => si.RecieverUserId == user.Id)
                .Select(si => new GroupSmallDetailsDTO(si.Group)).ToListAsync();

            return new GetSmallDetailsDTO()
            {
                Groups = groups,
                Count = ((groups.Count - 1) / 50) + 1
            };
        }

        public virtual async Task<object?> UserOpenGroups(string username, int page)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            var queriedUser = await _userService.GetUserByName(username);
            if (queriedUser is null) throw new ExceptionWithDescription("A felhasználó nem található");

            var groups = await _context.Groups
                .Include(g => g.Participants)
                .Include(g => g.GroupOwner)
                .Where(group => group.Participants.Any(p => p.Id == queriedUser.Id) && !group.IsClosed)
                .Skip(page * 50)
                .Take(50)
                .Select(g => new GroupSmallDetailsDTO(g))
                .ToListAsync();

            if (groups is null) throw new ExceptionWithDescription("Nincsenek csapatok a versenyhez");

            var count = groups.Count;

            return new GetSmallDetailsDTO()
            {
                Groups = groups,
                Count = ((count - 1) / 50) + 1
            };
        }

        public virtual async Task<int> GetOwnGroup(int competitionId)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");


            var groupId = await _context.Groups
                .Include(g => g.Participants)
                .Where(g => g.CompetitionId == competitionId && g.Participants.Any(p => p.Id == user.Id))
                .Select(g => g.Id)
                .FirstOrDefaultAsync();

            return groupId;
        }

        public virtual async Task<object?> GetPrivateGroups(int page)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            var queriedUser = await _userService.GetUserByName(user.UserName);
            if (queriedUser is null) throw new ExceptionWithDescription("A felhasználó nem található");

            var groups = await _context.Groups
                .Include(g => g.Participants)
                .Include(g => g.GroupOwner)
                .Where(group => group.Participants.Any(p => p.Id == queriedUser.Id) && group.IsClosed)
                .Skip(page * 50)
                .Take(50)
                .Select(g => new GroupSmallDetailsDTO(g))
                .ToListAsync();

            if (groups is null) throw new ExceptionWithDescription("Nincsenek csapatok a versenyhez");

            var count = groups.Count;

            return new GetSmallDetailsDTO()
            {
                Groups = groups,
                Count = ((count - 1) / 50) + 1
            };
        }
        public virtual async Task<object?> GetInvitedGroups(int page)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            var queriedUser = await _userService.GetUserByName(user.UserName);
            if (queriedUser is null) throw new ExceptionWithDescription("A felhasználó nem található");

            var groups = await _context.Groups
                .Include(g => g.Invites)
                .Include(g => g.GroupOwner)
                .Where(group => group.Invites.Any(p => p.RecieverUserId == queriedUser.Id))
                .Skip(page * 50)
                .Take(50)
                .Select(g => new GroupSmallDetailsDTO(g))
                .ToListAsync();

            if (groups is null) throw new ExceptionWithDescription("Nincsenek csapatok a versenyhez");

            var count = groups.Count;

            return new GetSmallDetailsDTO()
            {
                Groups = groups,
                Count = ((count - 1) / 50) + 1
            };
        }
        public virtual async Task<object?> GetJoinedGroups(int page)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            var queriedUser = await _userService.GetUserByName(user.UserName);
            if (queriedUser is null) throw new ExceptionWithDescription("A felhasználó nem található");

            var groups = await _context.Groups
                .Include(g => g.JoinRequests)
                .Include(g => g.GroupOwner)
                .Where(group => group.JoinRequests.Any(p => p.RequestOwnerId == queriedUser.Id))
                .Skip(page * 50)
                .Take(50)
                .Select(g => new GroupSmallDetailsDTO(g))
                .ToListAsync();

            if (groups is null) throw new ExceptionWithDescription("Nincsenek csapatok a versenyhez");

            var count = groups.Count;

            return new GetSmallDetailsDTO()
            {
                Groups = groups,
                Count = ((count - 1) / 50) + 1
            };
        }
    }
}
