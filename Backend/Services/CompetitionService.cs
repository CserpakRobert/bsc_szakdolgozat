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

        private readonly UserService _userService;
        private readonly MariaDBContext _context;

        public CompetitionService( UserService userService, MariaDBContext context)
        {
            _userService = userService;
            _context = context;
        }


        public virtual async Task<int> CreateCompetition(CreateCompetitionDTO model)
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

            var result = await _context.Competitions.AddAsync(new Competition()
            {
                SubmissionStartDate = model.SubmissionStartDate,
                VotingStartDate = model.VotingStartDate,
                FinishDate = model.FinishDate,
                Title = model.Title,
                Description = model.Description,
                Creator = user,
                Moderators = [user],
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
        public virtual async Task<bool> VotesEnabled(int id)
        {
            var competition = await _context.Competitions.FindAsync(id);
            if (competition is null) return false;
            return competition.IsInVotingPhase;
        }
        public virtual async Task<bool> SubmissionEnabled(int id)
        {
            var competition = await _context.Competitions.FindAsync(id);
            if (competition is null) return false;
            return competition.IsInSubmissionPhase;
        }

        public virtual async Task<bool?> IsInPreparationPhase(int id)
        {
            var result = await _context.Competitions.FindAsync(id);
            if (result is null) return false;
            return result.IsInPreparationPhase;
        }

        public virtual async Task<bool> IsModerator(int competitionId, int userId)
        {
            return await _context.Moderators.AnyAsync(m => m.UserId == userId && m.CompetitionId == competitionId);
        }

        public virtual async Task<CompetitionList?> GetDetailsList(GetCompetitionListDTO listDTO)
        {

            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            var query = _context.Competitions
                .Include(comp => comp.Creator)
                .Include(comp => comp.Moderators)
                .AsQueryable();

            if (listDTO.UserName is not null)
            {
                query = query.Where(comp => comp.Creator.UserName == listDTO.UserName);
            }

            if (listDTO.Type is ("movie" or "music" or "shortstory"))
            {
                query = query.Where(comp => comp.IsMovieCompetition == (listDTO.Type == "movie") &&
                                            comp.IsMusicCompetition == (listDTO.Type == "music") &&
                                            comp.IsShortStoryCompetition == (listDTO.Type == "shortstory"));
            }
            if (listDTO.Phase is ("preparation" or "vote" or "finished"))
            {
                query = query.Where(comp => comp.IsInPreparationPhase == (listDTO.Phase == "preparation") &&
                                            comp.IsInVotingPhase == (listDTO.Phase == "vote") &&
                                            !(comp.IsInVotingPhase || comp.IsInPreparationPhase || comp.IsInSubmissionPhase) == (listDTO.Phase == "finished"));
            }
            if (listDTO.Phase is ("submission"))
                query = query.Where(comp => comp.IsInSubmissionPhase);


            var result = await query
                .Skip(listDTO.Page * 50)
                .Take(50)
                .OrderByDescending(c => c.Id)
                .Select(c => new CompetitionListDetails(c))
                .ToListAsync()
                ;
            var count = await query.CountAsync();

            return new CompetitionList() { pageCount = ((count - 1) / 50) + 1, Competitions = result };
        }

        public virtual async Task<GetOneCompetitionDTO> GetDetails(int id)
        {
            var user = await _userService.GetLoggedInUser();
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

            var ownSubmission = await _context.CollaboratedSubmissions
                .Include(sub => sub.Submission)
                .Where(sub => sub.UserId == user.Id && sub.Submission.CompetitionId == id)
                .Select(sub => sub.Submission.Id)
                .FirstOrDefaultAsync();
            competition.OwnSubmission = ownSubmission;

            return competition;
        }

    }
}
