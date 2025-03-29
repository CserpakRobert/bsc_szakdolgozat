using Backend.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{

    [Table("ClosedGroupJoinRequests")]
    public class ClosedGroupJoinRequests
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public User RequestOwner { get; set; }

        [Required]
        [ValidDescription]
        public string Description { get; set; }


        [Required]
        public int GroupId { get; set; }

        [Required]
        public Group Group { get; set; }
    }
}
