
using Backend.Models;
using Backend.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class UserService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly MariaDBContext _context;

        public UserService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, MariaDBContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _context = context;
        }

        public async Task<bool> IsCreatorInCompetition(int competitionId, int userId)
        {
            var competition = await _context.Competitions.FirstAsync(comp => comp.Id == competitionId);

            return competition != null && competition.CreatorId == userId;
        }

        public async Task<bool> IsModeratorInCompetition(int competitionId, int userId)
        {
            var competition = await _context.Competitions.Include(comp => comp.Moderators).FirstAsync(comp => comp.Id == competitionId);

            return competition != null && competition.Moderators.Any(mod=>mod.Id == userId);
        }

        public async Task<bool> CanSubmitOnCompetition(int competitionId, int userId)
        {
            var canSubmit =  await _context.Competitions
                //.Include(comp => comp.Submissions)
                .AnyAsync(comp => 
                comp.Submissions.Any(sub => sub.OwnerId == userId 
                    || sub.Creators.Any(crea => crea.Id == userId)) 
                 || comp.Moderators.Any(mod => mod.Id == userId)
                 || comp.CreatorId == userId
                );

            return !canSubmit;
        }

        public async Task<User?> GetLoggedInUser()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (String.IsNullOrEmpty(userId))
            {
                return null;
            }
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return null;
            }
            return user;
        }
    }
}
