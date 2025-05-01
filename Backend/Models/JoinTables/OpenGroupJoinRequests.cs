using Backend.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.JoinTables
{

    [Table("JoinRequests")]
    public class OpenGroupJoinRequests
    {

        [Required]
        public int RequestOwnerId { get; set; }
        [Required]
        public User RequestOwner { get; set; } = null!;

        [Required]
        [ValidDescription]
        [MaxLength(3000)]
        public string Description { get; set; } = null!;


        [Required]
        public int GroupId { get; set; }

        [Required]
        public Group Group { get; set; } = null!;
    }
}
