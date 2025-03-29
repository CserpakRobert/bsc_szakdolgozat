using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("InvitedSubmissionUsers")]
    public class SubmissionInvites
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public User SenderUser { get; set; }

        [Required]
        public User RecieverUser { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public Group Group { get; set; }
    }
}
