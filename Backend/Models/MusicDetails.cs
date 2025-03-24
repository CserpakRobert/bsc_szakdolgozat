using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("MusicDetails")]
    public class MusicDetails
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string TrailerLink { get; set; }
        [Required]
        public string MovieLink { get; set; }

        [Required]
        public Submission Submission { get; set; }

    }
}
