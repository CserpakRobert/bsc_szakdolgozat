using Backend.Models;
using Backend.Models.DTOs;
using Backend.Persistence;
using Backend.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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


        public async Task<int> CreateCompetition(CreateCompetitionDTO model)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null)
            {
                throw new UserNotAuthorized("");
            }
            if (model.Category is not ("movie" or "music" or "shortstory"))
                throw new ExceptionWithDescription("Nem létező kategória");

            if (model.Description.Length > 3000 || model.Description.Length < 10)
                throw new ExceptionWithDescription("A leírásnak 10 és 3000 karakter között kell lennie");
            if (model.Title.Length < 10)
                throw new ExceptionWithDescription("A címnek legalább 10 karakter hosszúnak kell lennie");

            if (model.SubmissionStartDate < DateOnly.FromDateTime(DateTime.Now))
                throw new ExceptionWithDescription("Hibás dátum");
            if (model.VotingStartDate < model.SubmissionStartDate)
                throw new ExceptionWithDescription("Hibás dátum");
            if (model.FinishDate < model.VotingStartDate)
                throw new ExceptionWithDescription("Hibás dátum");

            var result = await _context.Competitions.AddAsync(new Competition() {
                SubmissionStartDate = model.SubmissionStartDate,
                VotingStartDate = model.VotingStartDate,
                FinishDate = model.FinishDate,
                Title = model.Title,
                Description = model.Description,
                Creator = user,
                Moderators = [user],
                IsPrivate = false,
                IsInPreparationPhase = true,
                IsMovieCompetition = model.Category == "movie",
                IsMusicCompetition = model.Category == "music",
                IsShortStoryCompetition = model.Category == "shortstory"
            });


            await _context.SaveChangesAsync();

            if (result is null)
                throw new ExceptionWithDescription("A verseny létrehozása sikertelen");

            return result.Entity.Id;
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
                .Skip(listDTO.Page * 50)
                .Take(50)
                ;
            var count = _context.Competitions
                .Where(correctPhase(listDTO.Phase))
                .Where(correctType(listDTO.Type))
                .Count();
            
            return new CompetitionList() { pageCount = ((count - 1) / 50) + 1, Competitions = result };
        }

        public async Task<GetOneCompetitionDTO> GetDetails(int id)
        {
            var user = _userService.GetLoggedInUser();
            if (user is null)
            {
                throw new ExceptionWithDescription("Bejelentkezés nélkül nem elérhető");
            }
            var competition = await _context.Competitions
                .Include(comp => comp.Creator)
                .Include(comp => comp.Moderators)
                .Where(comp => comp.Id == id)
                .Select(c => new GetOneCompetitionDTO(c))
                .FirstOrDefaultAsync();

            if (competition is null) throw new ExceptionWithDescription("Ez a verseny nem található");
            return competition;
        }

    }
}
