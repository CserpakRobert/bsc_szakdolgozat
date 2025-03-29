using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.JoinTables
{

    [Table("UserParticipateGroups")]
    public class UserParticipateGroups
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
