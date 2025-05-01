using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.JoinTables
{

    [Table("UserParticipateGroups")]
    public class UserParticipateGroups
    {

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;
    }
}
