using Backend.Models;
using Backend.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class SubmissionService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserService _userService;
        private readonly MariaDBContext _context;

        public SubmissionService(IHttpContextAccessor httpContextAccessor, UserService userService, MariaDBContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _context = context;
        }

        public async Task<bool> CanVote(int id)
        {

            var user = await _userService.GetLoggedInUser();
            var submission = await _context.Submissions
                .Include(sub => sub.Competition)
                .Include(sub => sub.Creators)
                .FirstAsync(sub => sub.Id == id);
            if (user == null || submission == null) return false;

            return submission.Competition.IsInVotingPhase 
                && !(submission.OwnerId == user.Id 
                    || submission.Creators.Any(c => c.Id == user.Id) 
                    || await HasVote(id)
                    );
        }

        public async Task<bool> HasVote(int id)
        {
            var user = await _userService.GetLoggedInUser();
            var submission = await _context.Submissions
                .Include(sub => sub.VotedUsers)
                .FirstAsync(sub => sub.Id == id);
            if (user == null || submission == null) return false;
            return submission.VotedUsers.Any(u => u.Id == user.Id);
        }
        public async Task<bool> CanEdit(int id)
        {
            var competition = await _context.Competitions.FirstAsync(e => e.Id == id);
            return competition.IsInSubmissionPhase;
        }

        //public async Task<ICollection<Submission>> ListUser(int userId,
        //    bool owner = true,
        //    bool creator = true,
        //    int page = 0,
        //    int perPage = 50)
        //{

        //    var result = await _context.Users.Find(userId);

        //}

    }
}
