using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("MovieDetails")]
    public class MovieDetails
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        public Submission Submission { get; set; }
    }
}
