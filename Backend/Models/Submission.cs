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
        [DefaultValue(0)]
        public int Votes { get; set; }

        [Required]
        public User Owner { get; set; }
        [Required]
        public int OwnerId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public Competition Competition { get; set; }
        [Required]
        public int CompetitionId { get; set; }
        [Required]
        public ICollection<User> Creators { get; set; }
        public MusicDetails? MusicDetails { get; set; }
        public int? MusicDetailsId { get; set; }
        public MovieDetails? MovieDetails { get; set; }
        public int? MovieDetailsId { get; set; }

        public ICollection<InvitedSubmissionUsers> Invites { get; set; }
    }

    [Table("InvitedSubmissionUsers")]
    public class InvitedSubmissionUsers
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public Submission Submission { get; set; }

        public int CreatorId { get; set; }
        public User Creator { get; set; }
    }


}
