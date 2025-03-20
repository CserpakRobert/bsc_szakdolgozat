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
        public string TrailerLink { get; set; }
        [Required]
        public string MovieLink { get; set; }
        [Required]
        public string Creators { get; set; }
    }
}
