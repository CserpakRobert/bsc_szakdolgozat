using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("MusicDetails")]
    public class MusicDetails
    {
        public int Id { get; set; }

        [Required]
        public string Link { get; set; }

        public Submission Submission { get; set; }
    }
}
