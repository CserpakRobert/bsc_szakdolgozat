using Backend.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("MusicDetails")]
    public class MusicDetails
    {
        [Required]
        public int Id { get; set; }

        [YouTubeLink]
        [Required]
        public string Link { get; set; }


        [Required]
        public int SubmissionId { get; set; }

        [Required]
        public Submission Submission { get; set; } 
    }
}
