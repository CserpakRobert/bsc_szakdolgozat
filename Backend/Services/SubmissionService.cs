using Backend.Models;
using Backend.Models.DTOs;
using Backend.Persistence;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Services
{
    public class SubmissionService
    {

        private readonly UserService _userService;
        private readonly MariaDBContext _context;

        public SubmissionService(
            UserService userService,
            MariaDBContext context)
        {
            _userService = userService;
            _context = context;
        }

        public virtual async Task<SubmissionListModel> GetSubmissionList(SubmissionListGetModel listGetModel)
        {

            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            var query = _context.Submissions
                .Include(sub => sub.Competition)
                .Include(sub => sub.Creators)
                .Include(sub => sub.Owner)
                .AsQueryable();

            if (listGetModel.CompetitionId != null)
                query = query.Where(sub => sub.CompetitionId == listGetModel.CompetitionId);

            if (listGetModel.UserName != null)
                query = query.Where(sub => sub.Creators.Any(c => c.UserName == listGetModel.UserName));

            var test = query.ToList();
            switch (listGetModel.Phase)
            {
                case "finished":
                    query = query.Where(sub => 
                    !sub.Competition.IsInPreparationPhase
                    && !sub.Competition.IsInSubmissionPhase
                    && !sub.Competition.IsInVotingPhase
                    );
                    break;
                case "submission":
                    query = query.Where(sub => sub.Competition.IsInSubmissionPhase && sub.Creators.Any(c=>c.Id == user.Id));
                    break;
                case "voting":
                    query = query.Where(sub => sub.Competition.IsInVotingPhase);
                    break;
            }

            if (listGetModel.SortByPlacement)
                query = query.OrderBy(sub => sub.Id).OrderBy(sub => sub.Placement);
            else if (listGetModel.SortByVoteCount)
                query = query.OrderBy(sub => sub.Id).OrderByDescending(sub => sub.VoteCount);
            else if (listGetModel.SortByDate)
                query = query.OrderBy(sub => sub.Id).OrderByDescending(sub => sub.Competition.RelevantDate);
            else
                query = query.OrderBy(sub => sub.Id);

            var submissions = query.Select(sub => new SubmissionListDetailsModel
                {
                    Id = sub.Id,
                    Title = sub.Title,
                    Type = sub.Competition.Type,
                    Placement = sub.Placement,
                    VoteCount = sub.VoteCount,
                    Phase = sub.Competition.Phase,

                }).AsQueryable();

            var count = await query.CountAsync();

            var getSubmissions = await submissions.Skip(listGetModel.Page * 50).Take(50).ToListAsync();

            return new SubmissionListModel
            {
                pageCount = ((count - 1) / 50) + 1,
                Submissions = getSubmissions
            };
        }
        public virtual async Task<GetSusbmission> GetSubmission(int id)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            var submission = await _context.Submissions
                .Include(sub => sub.MovieDetails)
                .Include(sub => sub.MusicDetails)
                .Include(sub => sub.ShortStoryDetails)
                .Include(sub => sub.Competition)
                .Include(sub => sub.Creators)
                .Include(sub => sub.Owner)
                .FirstOrDefaultAsync(sub => sub.Id == id);

            if (submission is null) throw new ExceptionWithDescription("Nem található");

            if (submission.Competition.IsInSubmissionPhase)
            {
                if (!submission.Creators.Any(c => c.Id == user.Id))
                {
                    throw new ExceptionWithDescription("Ezt a pályázatot még nem tekintheted meg");
                }
            }

            var canVote = await CanVote(id);
            var hasVote = await HasVote(id);

            return new GetSusbmission
            {
                Id = submission.Id,
                Title = submission.Title,
                Description = submission.Description,
                IsSubmitted = submission.IsSubmitted,
                Votes = submission.VoteCount,
                CanVote = canVote,
                HasVote = hasVote,
                MusicDetails = submission.MusicDetails != null ? new CreateMusicDetailsModel
                {
                    Link = submission.MusicDetails.Link
                } : null,
                MovieDetails = submission.MovieDetails != null ? new CreateMovieDetailsModel
                {
                    TrailerLink = submission.MovieDetails.TrailerLink,
                    MovieLink = submission.MovieDetails.MovieLink
                } : null,
                ShortStoryDetails = submission.ShortStoryDetails != null ? new CreateShortStoryDetailsModel
                {
                    Story = submission.ShortStoryDetails.Story
                } : null,
                CreatorName = submission.Owner.UserName,
                ParticipateNames = submission.Creators.Select(c => c.UserName).ToList(),
                Placement = submission.Placement
            };

        }

        public virtual async Task<bool> VoteForSubmission(int id)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            var submission = await _context.Submissions.FirstOrDefaultAsync(sub => sub.Id == id);
            if (submission is null) throw new ExceptionWithDescription("Nem található");

            var inVotingPhase = await _context.Competitions.Where(c => c.Id == submission.CompetitionId).Select(c => c.IsInVotingPhase).FirstOrDefaultAsync();
            if (!inVotingPhase) throw new ExceptionWithDescription("A verseny nincs szavazási fázisban");

            var hasVoted = await _context.SubmissionVotes.AnyAsync(sv => sv.UserId == user.Id && sv.SubmissionId == id);
            if (hasVoted) throw new ExceptionWithDescription("Erre a pályázatra már leadtad a szavazatod");

            var eligibleForVoting = !await _context.Submissions
                                                .Where(s => s.Id == submission.Id)
                                                .AnyAsync(sub => sub.Creators
                                                    .Any(c => c.Id == user.Id));
            if (!eligibleForVoting) throw new ExceptionWithDescription("Nem szavazhatsz a saját pályázatodra");

            submission.VoteCount++;
            submission.VotedUsers.Add(user);

            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> UnVoteForSubmission(int id)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            var submission = await _context.Submissions.FirstOrDefaultAsync(sub => sub.Id == id);
            if (submission is null) throw new ExceptionWithDescription("Nem található");

            var inVotingPhase = await _context.Competitions.Where(c => c.Id == submission.CompetitionId).Select(c => c.IsInVotingPhase).FirstOrDefaultAsync();
            if (!inVotingPhase) throw new ExceptionWithDescription("A verseny nincs szavazási fázisban");

            var hasVoted = await _context.SubmissionVotes.AnyAsync(sv => sv.UserId == user.Id && sv.SubmissionId == id);
            if (!hasVoted) throw new ExceptionWithDescription("Erre a pályázatra még nem leadtad a szavazatod");

            var eligibleForVoting = !await _context.Submissions
                                                .Where(s => s.Id == submission.Id)
                                                .AnyAsync(sub => sub.Creators
                                                .Any(c => c.Id == user.Id));
            if (!eligibleForVoting) throw new ExceptionWithDescription("Nem szavazhatsz a saját pályázatodra");

            submission.VoteCount--;
            await _context.SubmissionVotes.Where(sv => sv.UserId == user.Id && sv.SubmissionId == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();

            return true;
        }

        public virtual async Task<Submission?> GetById(int id)
        {
            return await _context.Submissions.FindAsync(id);
        }



        public virtual async Task<bool> CanVote(int id)
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

        public virtual async Task<bool> HasVote(int id)
        {
            var user = await _userService.GetLoggedInUser();
            var submission = await _context.Submissions
                .Include(sub => sub.VotedUsers)
                .FirstAsync(sub => sub.Id == id);
            if (user == null || submission == null) return false;
            return submission.VotedUsers.Any(u => u.Id == user.Id);
        }
        public virtual async Task<bool> CanEdit(int id)
        {
            var user = _userService.GetLoggedInUser();
            var submission = await _context.Competitions.FirstAsync(e => e.Id == id);
            if (user is null || submission.CreatorId != user.Id) return false;
            return true;
        }

        public virtual async Task<string> GetType(int id)
        {
            var submission = await _context.Submissions.FirstOrDefaultAsync(sub => sub.Id == id);
            if (submission is null) throw new ExceptionWithDescription("Nem található");

            var competition = await _context.Competitions.FirstOrDefaultAsync(comp => comp.Id == submission.CompetitionId);
            if (competition is null) throw new ExceptionWithDescription("Nem található verseny");

            return competition.Type;

        }
        #region Modifies
        public virtual async Task<int> ModifyMovieSubmission(ModifyMovieDetails modifySubmission)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            var submission = await _context.Submissions
                .Include(sub => sub.MovieDetails)
                .Include(sub => sub.Competition)
                .FirstOrDefaultAsync(sub => sub.Id == modifySubmission.Id);

            if (submission is null) throw new ExceptionWithDescription("Nem található pályázat");
            if (!submission.Competition.IsInSubmissionPhase)
            {
                throw new ExceptionWithDescription("A verseny nincs beadási fázisban");
            }

            if (submission.IsSubmitted) throw new ExceptionWithDescription("A pályázat már le lett adva");

            if (submission.OwnerId != user.Id)
            {
                throw new ExceptionWithDescription("Csak a pályázat készítője adhatja be a pályázatot");
            }


            if (modifySubmission.Title.Length > 200 || modifySubmission.Title.Length < 10)
            {
                throw new ExceptionWithDescription("A cím hossza nem megfelelő");
            }
            submission.Title = modifySubmission.Title;

            if (modifySubmission.Description.Length > 3000 || modifySubmission.Description.Length < 10)
            {
                throw new ExceptionWithDescription("A leírás hossza nem megfelelő");
            }
            submission.Description = modifySubmission.Description;

            submission.MovieDetails = new MovieDetails();

            if (modifySubmission.MovieLink is not null && Utils.Utils.IsCorrectVideoId(modifySubmission.MovieLink))
            {
                submission.MovieDetails.MovieLink = modifySubmission.MovieLink;
            }
            else
            {
                throw new ExceptionWithDescription("Hiba a film linkkel");
            }

            if (modifySubmission.TrailerLink is not null && Utils.Utils.IsCorrectVideoId(modifySubmission.TrailerLink))
            {
                submission.MovieDetails.TrailerLink = modifySubmission.TrailerLink;
            }
            else
            {
                throw new ExceptionWithDescription("Hiba a trailer linkkel");
            }
            submission.IsSubmitted = true;

            await _context.SaveChangesAsync();
            return submission.Id;
        }

        public virtual async Task<int> GetOwnSubmission(int competitionId)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            var submission = await _context.Submissions
                .Include(sub => sub.Competition)
                .Where(sub => sub.CompetitionId == competitionId && sub.Creators.Any(c => c.Id == user.Id))
                .Select(sub => sub.Id).FirstOrDefaultAsync();

            return submission;
        }
        public virtual async Task<int> ModifyMusicSubmission(ModifyMusicDetails modifySubmission)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            var submission = await _context.Submissions
                .Include(sub => sub.MusicDetails)
                .Include(sub => sub.Competition)
                .FirstOrDefaultAsync(sub => sub.Id == modifySubmission.Id);

            if (submission is null) throw new ExceptionWithDescription("Nem található pályázat");
            if (!submission.Competition.IsInSubmissionPhase)
            {
                throw new ExceptionWithDescription("A verseny nincs beadási fázisban");
            }

            if (submission.IsSubmitted) throw new ExceptionWithDescription("A pályázat már le lett adva");

            if (submission.OwnerId != user.Id)
            {
                throw new ExceptionWithDescription("Csak a pályázat készítője adhatja be a pályázatot");
            }


            if (modifySubmission.Title.Length > 200 || modifySubmission.Title.Length < 10)
            {
                throw new ExceptionWithDescription("A cím hossza nem megfelelő");
            }
            submission.Title = modifySubmission.Title;

            if (modifySubmission.Description.Length > 3000 || modifySubmission.Description.Length < 10)
            {
                throw new ExceptionWithDescription("A leírás hossza nem megfelelő");
            }
            submission.Description = modifySubmission.Description;

            submission.MusicDetails = new MusicDetails();

            if (modifySubmission.Link is not null && Utils.Utils.IsCorrectVideoId(modifySubmission.Link))
            {
                submission.MusicDetails.Link = modifySubmission.Link;
            }
            else
            {
                throw new ExceptionWithDescription("Hiba a zene linkkel");
            }

            submission.IsSubmitted = true;
            await _context.SaveChangesAsync();
            return submission.Id;
        }
        public virtual async Task<int> ModifyShortStorySubmission(ModifyShortStoryDetails modifySubmission)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) throw new UserNotAuthorized("");

            var submission = await _context.Submissions
                .Include(sub => sub.ShortStoryDetails)
                .Include(sub => sub.Competition)
                .FirstOrDefaultAsync(sub => sub.Id == modifySubmission.Id);

            if (submission is null) throw new ExceptionWithDescription("Nem található pályázat");
            if (!submission.Competition.IsInSubmissionPhase)
            {
                throw new ExceptionWithDescription("A verseny nincs beadási fázisban");
            }

            if (submission.IsSubmitted) throw new ExceptionWithDescription("A pályázat már le lett adva");

            if (submission.OwnerId != user.Id)
            {
                throw new ExceptionWithDescription("Csak a pályázat készítője adhatja be a pályázatot");
            }


            if (modifySubmission.Title.Length > 200 || modifySubmission.Title.Length < 10)
            {
                throw new ExceptionWithDescription("A cím hossza nem megfelelő");
            }
            submission.Title = modifySubmission.Title;

            if (modifySubmission.Description.Length > 3000 || modifySubmission.Description.Length < 10)
            {
                throw new ExceptionWithDescription("A leírás hossza nem megfelelő");
            }
            submission.Description = modifySubmission.Description;

            submission.ShortStoryDetails = new ShortStoryDetails();

            if (modifySubmission.Story is not null
                && modifySubmission.Story.Length > 10
                && modifySubmission.Story.Length <= 20000)
            {
                submission.ShortStoryDetails.Story = modifySubmission.Story;
            }
            else
            {
                throw new ExceptionWithDescription("A történet hossza nem megfelelő");
            }

            submission.IsSubmitted = true;
            await _context.SaveChangesAsync();
            return submission.Id;
        }


        #endregion
    }
}
