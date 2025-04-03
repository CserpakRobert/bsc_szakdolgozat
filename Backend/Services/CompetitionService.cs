using Backend.Models;
using Backend.Models.DTOs;
using Backend.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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

        public async Task<bool?> IsInPreparationPhase(int id)
        {
            var result = await _context.Competitions.FindAsync(id);
            if (result is null) return null;
            return result.IsInPreparationPhase;
        }

        public async Task<bool> IsModerator(int competitionId, int userId)
        {
            return await _context.Moderators.AnyAsync(m => m.UserId == userId && m.CompetitionId == competitionId);
        }

        internal async Task<CompetitionList?> GetDetailsList(GetCompetitionListDTO listDTO)
        {
            Debug.WriteLine(listDTO.Phase);
            Func<string, Func<Competition, bool>> correctType = (type) => (comp) =>
            {
                return type switch
                {
                    "movie" => comp.IsMovieCompetition,
                    "music" => comp.IsMusicCompetition,
                    "shortstory" => comp.IsShortStoryCompetition,
                    _ => true,
                };
            };
            Func<string, Func<Competition, bool>> correctPhase = (phase) => (comp) =>
            {
                return phase switch
                {
                    "preparation" => comp.IsInPreparationPhase,
                    "vote" => comp.IsInVotingPhase,
                    "finished" => !(comp.IsInVotingPhase || comp.IsInPreparationPhase || comp.IsInSubmissionPhase),
                    _ => !comp.IsInSubmissionPhase,
                };
            };
            var result = _context.Competitions
                .Where(correctPhase(listDTO.Phase))
                .Where(correctType(listDTO.Type))
                .Select(c => new CompetitionListDetails(c))
                .Skip(listDTO.Page * 4)//TODO change to 50
                .Take(4)
                ;
            var count = _context.Competitions
                .Where(correctPhase(listDTO.Phase))
                .Where(correctType(listDTO.Type))
                .Count();
            return new CompetitionList() { pageCount = ((count - 1) / 4) + 1, Competitions = result };
        }

    }
}
