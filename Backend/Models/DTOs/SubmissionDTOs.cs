namespace Backend.Models.DTOs
{
    public class SubmissionDTOs
    {

    }
    public partial interface IModifySubmission
    {
        public int Id { get; set; }

        public bool Submitted { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
    }

    public class ModifyMovieDetails : IModifySubmission
    {
        public int Id { get; set ; }
        public bool Submitted { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string MovieLink { get; set; } = null!;
        public string TrailerLink { get; set; } = null!;
    }
    public class ModifyMusicDetails : IModifySubmission
    {
        public int Id { get; set; }
        public bool Submitted { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Link { get; set; } = null!;
    }
    public class ModifyShortStoryDetails : IModifySubmission
    {
        public int Id { get; set; }
        public bool Submitted { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Story { get; set; } = null!;

    }
}
