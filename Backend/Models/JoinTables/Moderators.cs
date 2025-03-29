using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.JoinTables
{
    [Table("Moderators")]
    public class Moderators
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }
    }
}
