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

        public string Description { get; set; } = "";
    }

    public class GetGroupDTO
    {
        public int CompetitionId { get; set; }
        public int GroupId { get; set; }
        public string CompetitionTitle { get; set; } = null!;

        public string OwnerName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public bool IsClosed { get; set; }
        public ICollection<string> ParticipateNames { get; set; } = [];

        public int InviteCount { get; set; }
        public int RequestCount { get; set; }


    }
    public class GetSmallDetailsDTO
    {
        public IEnumerable<GroupSmallDetailsDTO> Groups { get; set; } = null!;
        public int Count { get; set; }

    }
    public class GroupSmallDetailsDTO
    {
        public int GroupId { get; set; }
        public int CompetitionId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public int ParticipateCount { get; set; }

        public GroupSmallDetailsDTO(Group Group)
        {
            GroupId = Group.Id;
            CompetitionId = Group.CompetitionId;
            OwnerName = Group.GroupOwner.UserName;
            ParticipateCount = Group.Participants.Count;
        }
    }
}
