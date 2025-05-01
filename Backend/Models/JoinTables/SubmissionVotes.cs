using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.JoinTables
{

    [Table("SubmissionVotes")]
    public class SubmissionVotes
    {

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int SubmissionId { get; set; }
        public Submission Submission { get; set; } = null!;
    }
}
