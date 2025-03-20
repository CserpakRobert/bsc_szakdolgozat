using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Submissions")]
    public class Submission
    {
        [Required]
        [DefaultValue(0)]
        public int Votes { get; set; }

        [Required]
        [ForeignKey("OwnerId")]
        public User Owner { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Competition Competition { get; set; }

        [Required]
        public ICollection<User> Creators { get; set; }
        public MusicDetails? MusicDetails { get; set; }
        public MovieDetails? MovieDetails { get; set; }
    }

}
