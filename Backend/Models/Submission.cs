using Backend.Utils;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Submissions")]
    public class Submission
    {
        public int Id { get; set; }

        [Required]
        public int VoteCount { get; set; } = 0;

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public User Owner { get; set; }

        [Required]
        [Title]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [ValidDescription]
        [MaxLength(3000)]
        public string Description { get; set; }


        //Table connections
        [Required]
        public Competition Competition { get; set; }

        [Required]
        public int CompetitionId { get; set; }

        public ICollection<User> Creators { get; set; }

        public ICollection<User> VotedUsers { get; set; } = [];

        //Details
        public int? MusicDetailsId { get; set; }
        public MusicDetails? MusicDetails { get; set; }


        public int? MovieDetailsId { get; set; }
        public MovieDetails? MovieDetails { get; set; }


    }

}
