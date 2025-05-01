using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.JoinTables
{
    [Table("Moderators")]
    public class Moderators
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int CompetitionId { get; set; }
        public Competition Competition { get; set; } = null!;
    }
}
