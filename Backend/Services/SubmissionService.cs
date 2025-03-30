using Backend.Models;
using Backend.Models.DTOs;
using Backend.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


        public async Task<Submission?> ModifySubmission(IModifySubmission modifySubmission)
        {
            if (modifySubmission is not (ModifyMovieDetails or ModifyMusicDetails or ModifyShortStoryDetails))
            {
                return null;
            }

            var submission = await _context.Submissions
                .Include(sub => sub.MovieDetails)
                .Include(sub => sub.MusicDetails)
                .Include(sub => sub.ShortStoryDetails)
                .Include(sub => sub.Competition)
                .FirstOrDefaultAsync(sub => sub.Id == modifySubmission.Id);

            if (submission is null || submission.Competition.IsInSubmissionPhase)
            {
                return null;
            }

            submission.IsSubmitted = modifySubmission.Submitted;
            submission.Title = modifySubmission.Title;
            submission.Description = modifySubmission.Description;


            // Movie
            if (submission.MovieDetails is not null && modifySubmission is ModifyMovieDetails modifyMovieSubmission)
            {
                var competition = await _context.Competitions.FindAsync(submission.CompetitionId);
                if (competition is not null && competition.IsInSubmissionPhase && competition.IsMovieCompetition)
                {
                    submission.MovieDetails.MovieLink = modifyMovieSubmission.MovieLink;
                    submission.MovieDetails.TrailerLink = modifyMovieSubmission.TrailerLink;
                }
                else return null;
            }

            // Music
            if (submission.MusicDetails is not null && modifySubmission is ModifyMusicDetails modifyMusicSubmission)
            {
                var competition = await _context.Competitions.FindAsync(submission.CompetitionId);
                if (competition is not null && competition.IsInSubmissionPhase && competition.IsMovieCompetition)
                {
                    submission.MusicDetails.Link = modifyMusicSubmission.Link;
                }
                else return null;
            }

            // ShortStory
            if (submission.ShortStoryDetails is not null
                && modifySubmission is ModifyShortStoryDetails modifyShortStorySubmission)
            {
                var competition = await _context.Competitions.FindAsync(submission.CompetitionId);
                if (competition is not null && competition.IsInSubmissionPhase && competition.IsMovieCompetition)
                {
                    submission.ShortStoryDetails.Story = modifyShortStorySubmission.Story;
                }
                else return null;
            }

            await _context.SaveChangesAsync();
            return submission;
        }

        public async Task<bool> VoteForSubmission(int id)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) return false;

            var submission = await _context.Submissions.FirstOrDefaultAsync(sub => sub.Id == id);
            if (submission is null) return false;

            var inVotingPhase = await _context.Competitions.Where(c => c.Id == submission.CompetitionId).Select(c => c.IsInVotingPhase).FirstOrDefaultAsync();
            if (!inVotingPhase) return false;

            var hasVoted = await _context.Submissions.Where(s => s.Id == submission.Id).AnyAsync(sub => sub.VotedUsers.Any(c => c.Id == user.Id));
            if (hasVoted) return false;

            var eligibleForVoting = !await _context.Submissions.Where(s => s.Id == submission.Id).AnyAsync(sub => sub.Creators.Any(c => c.Id == user.Id));
            if (!eligibleForVoting) return false;

            submission.VoteCount++;
            submission.VotedUsers.Add(user);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnVoteForSubmission(int id)
        {
            var user = await _userService.GetLoggedInUser();
            if (user is null) return false;

            var submission = await _context.Submissions.Include(sub => sub.VotedUsers).FirstOrDefaultAsync(sub => sub.Id == id);
            if (submission is null) return false;

            var inVotingPhase = await _context.Competitions.Where(c => c.Id == submission.CompetitionId).Select(c => c.IsInVotingPhase).FirstOrDefaultAsync();
            if (!inVotingPhase) return false;

            var hasVoted = await _context.Submissions.Where(s => s.Id == submission.Id).AnyAsync(sub => sub.VotedUsers.Any(c => c.Id == user.Id));
            if (!hasVoted) return false;

            var eligibleForVoting = !await _context.Submissions.Where(s => s.Id == submission.Id).AnyAsync(sub => sub.Creators.Any(c => c.Id == user.Id));
            if (!eligibleForVoting) return false;

            submission.VoteCount--;
            submission.VotedUsers.Remove(user);
            await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Submission?> GetById(int id)
        {
            return await _context.Submissions.FindAsync(id);
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
            var user = _userService.GetLoggedInUser();
            var submission = await _context.Competitions.FirstAsync(e => e.Id == id);
            if (user is null || submission.CreatorId != user.Id) return false;
            return true;
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
