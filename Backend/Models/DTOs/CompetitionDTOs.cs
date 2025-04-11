namespace Backend.Models.DTOs
{

    public class GetCompetitionListDTO
    {
        public string Type { get; set; } = "";
        public string Phase { get; set; } = "";
        public int Page { get; set; } = 0;
    }

    public class CompetitionList
    {
        public int pageCount { get; set; } = 0;
        public IEnumerable<CompetitionListDetails>? Competitions { get; set; } = null;
    }
    public class CompetitionListDetails
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateOnly? RelevantDate { get; set; }
        public string Phase { get; set; }
        public string Type { get; set; }

        public CompetitionListDetails(Competition competition)
        {
            Id = competition.Id;
            Title = competition.Title;
            RelevantDate = competition.RelevantDate;
            Phase = competition.Phase;
            this.Type = competition.Type;
        }
    }
    public class CompetitionStates
    {
        public bool IsInPreparationPhase { get; set; }

        public bool IsInSubmissionPhase { get; set; }

        public bool IsInVotingPhase { get; set; }
    }
    public class CreateCompetitionDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public DateOnly SubmissionStartDate { get; set; }
        public DateOnly VotingStartDate { get; set; }
        public DateOnly FinishDate { get; set; }
    }
    public class GetOneCompetitionDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Creatorname { get; set; } = "";
        public string Phase { get; set; }
        public string Type { get; set; }
        public DateOnly? RelevantDate { get; set; }

        public int OwnSubmission { get; set; }


        public GetOneCompetitionDTO(Competition competition)
        {
            this.Creatorname = competition.Creator.UserName;
            this.Id = competition.Id;
            this.Title = competition.Title;
            this.Description = competition.Description;
            this.RelevantDate = competition.RelevantDate;
            this.Phase = competition.Phase;
            this.Type = competition.Type;


        }

    }


}
