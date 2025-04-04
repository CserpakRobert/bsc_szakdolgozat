namespace Backend.Models.DTOs
{
    public class CreateGroupDTO
    {
        public string Description { get; set; } = null!;

        public bool IsClosed { get; set; }

    }
    public class InviteDTO
    {
        public string UserName { get; set; } = null!;
    }
    public class GroupRequestDTO
    {
        public string Description { get; set; }
    }

    public class GetGroupDTO
    {
        public int CompetitionId { get; set; }
        public string CompetitionTitle { get; set; } = null!;

        public string OwnerName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public bool IsClosed { get; set; }
        public ICollection<string> ParticipateNames { get; set; } = [];


    }
}
