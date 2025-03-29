namespace Backend.Models.DTOs
{
    public class SubmissionDTOs
    {

    }
    public partial interface IModifySubmission
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
    }

    public interface IModifyMovieDetails : IModifySubmission
    {
        public string MovieLink { get; set; }
        public string TrailerLink { get; set; }
    }
    public interface IModifyMusicDetails : IModifySubmission
    {
        public string Link { get; set; }
    }
    public interface IModifyShortStoryDetails : IModifySubmission
    {
        public string Story { get; set; }

    }
}
