using Backend.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Groups")]
    public class Group
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int CompetitionId { get; set; }

        [Required]
        public Competition Competition { get; set; }

        [Required]
        public int GroupOwnerId { get; set; }

        [Required]
        public User GroupOwner { get; set; }


        [Required]
        [ValidDescription]
        [MaxLength(3000)]
        public string Description { get; set; }

        [Required]
        public bool IsClosed { get; set; } = false;

        public ICollection<User> Participants { get; set; } = [];

        public ICollection<OpenGroupJoinRequests> JoinRequests { get; set; } = [];

        public ICollection<SubmissionInvites> Invites { get; set; } = [];


    }
}
