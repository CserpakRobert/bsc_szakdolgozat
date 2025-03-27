using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("MovieDetails")]
    public class MovieDetails
    {
        public int Id { get; set; }

        [Required]
        public string TrailerLink { get; set; }

        [Required]
        public string MovieLink { get; set; }
        public Submission Submission { get; set; }
    }
}
