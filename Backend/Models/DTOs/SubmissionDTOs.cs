namespace Backend.Models.DTOs
{
    public class GetSusbmission
    {

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsSubmitted { get; set; }
        public int Votes { get; set; }
        public int Placement { get; set; }
        public bool CanVote { get; set; }
        public bool HasVote { get; set; }

        public CreateMusicDetailsModel? MusicDetails { get; set; }
        public CreateMovieDetailsModel? MovieDetails { get; set; }
        public CreateShortStoryDetailsModel? ShortStoryDetails { get; set; }

        public string CreatorName { get; set; } = null!;
        public ICollection<string> ParticipateNames { get; set; } = [];


    }


    public class CreateMusicDetailsModel
    {
        public string Link { get; set; } = null!;
    }
    public class CreateMovieDetailsModel
    {
        public string TrailerLink { get; set; } = null!;
        public string MovieLink { get; set; } = null!;
    }
    public class CreateShortStoryDetailsModel
    {
        public string Story { get; set; } = null!;
    }
    public class CreateSubmissionModel
    {
        public int CompetitionId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public CreateMusicDetailsModel? musicDetails { get; set; }
        public CreateMovieDetailsModel? MovieDetails { get; set; }

        public CreateShortStoryDetailsModel? ShortStoryDetails { get; set; }
        public ICollection<string> InvitedUsers { get; set; } = [];
    }
    public class SubmissionListGetModel
    {
        public string? UserName { get; set; }

        public int? CompetitionId { get; set; }

        public string? Phase { get; set; }
        public bool SortByPlacement { get; set; }

        public bool SortByVoteCount { get; set; }

        public bool SortByDate { get; set; }

        public int Page { get; set; }


    }
    public class SubmissionListModel {
        public int pageCount { get; set; }
        public ICollection<SubmissionListDetailsModel> Submissions { get; set; } = [];

    }
    public class SubmissionListDetailsModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Type { get; set; } = "";
        public string Phase { get; set; } = "";
        public int Placement { get; set; }
        public int VoteCount { get; set; }


    }
    public partial interface IModifySubmission
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }

    public class ModifyMovieDetails : IModifySubmission
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? MovieLink { get; set; }
        public string? TrailerLink { get; set; }
    }
    public class ModifyMusicDetails : IModifySubmission
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Link { get; set; }
    }
    public class ModifyShortStoryDetails : IModifySubmission
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Story { get; set; }

    }
}
