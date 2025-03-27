using Backend.Models;
using Backend.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class CompetitionService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserService _userService;
        private readonly MariaDBContext _context;

        public CompetitionService(IHttpContextAccessor httpContextAccessor, UserService userService, MariaDBContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _context = context;
        }

        public async Task<bool> VotesEnabled(int id)
        {
            var competition = await _context.Competitions.FirstAsync(e => e.Id == id);
            return competition.IsInVotingPhase;
        }
        public async Task<bool> SubmissionEnabled(int id)
        {
            var competition = await _context.Competitions.FirstAsync(e => e.Id == id);
            return competition.IsInSubmissionPhase;
        }


    }
}
