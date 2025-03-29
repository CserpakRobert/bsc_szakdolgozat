using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.JoinTables
{
    [Table("CollaboratedSubmissions")]
    public class CollaboratedSubmissions
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int SubmissionId { get; set; }
        public Submission Submission { get; set; }
    }
}
