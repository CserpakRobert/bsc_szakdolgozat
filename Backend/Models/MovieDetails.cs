using Backend.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("MovieDetails")]
    public class MovieDetails
    {
        public int Id { get; set; }

        [Required]
        [YouTubeLink]
        public string TrailerLink { get; set; } = null!;

        [Required]
        [YouTubeLink]
        public string MovieLink { get; set; } = null!;

        [Required]
        public int SubmissionId { get; set; }

        [Required]
        public Submission Submission { get; set; } = null!;
    }
}
