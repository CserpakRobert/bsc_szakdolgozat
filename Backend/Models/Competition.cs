using Backend.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Competitions")]
    public class Competition
    {
        public int Id { get; set; }

        [Required]
        [Title]
        [MaxLength(200)]
        public string Title { get; set; } = "";

        [Required]
        [Description]
        [MaxLength(3000)]
        public string Description { get; set; } = "";

        //Phases
        [Required]
        [DefaultValue(true)]
        public bool IsInPreparationPhase { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool IsInSubmissionPhase { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsInVotingPhase { get; set; }

        //Dates
        [Required]
        public DateOnly SubmissionStartDate { get; set; }
        [Required]
        public DateOnly VotingStartDate { get; set; }
        [Required]
        public DateOnly FinishDate { get; set; }

        //Type
        [DefaultValue(false)]
        public bool IsMovieCompetition { get; set; }

        [DefaultValue(false)]
        public bool IsMusicCompetition { get; set; }     
        
        [DefaultValue(false)]
        public bool IsShortStoryCompetition { get; set; }

        //Table connections

        [Required]
        public int CreatorId { get; set; }

        [Required]
        public User Creator { get; set; } = null!;

        public ICollection<User> Moderators { get; set; } = [];

        public ICollection<Submission> Submissions { get; set; } = [];

        public ICollection<Group> Groups { get; set; } = [];



        [NotMapped]
        public DateOnly? RelevantDate { get {
                if (IsInPreparationPhase) return SubmissionStartDate;
                if (IsInVotingPhase) return FinishDate;
                if (IsInSubmissionPhase) return VotingStartDate;
                return null;
            } 
        }
        [NotMapped]
        public string Phase { get {
                if (IsInPreparationPhase) return "preparation";
                if (IsInVotingPhase) return "voting";
                if (IsInSubmissionPhase) return "submit";
                return "finished";
            } }

        [NotMapped]
        public string Type { get {
                if (IsMovieCompetition) return "movie";
                if (IsMusicCompetition) return "music";
                if (IsShortStoryCompetition) return "shortstory";
                return "";
            } }
    }

}
