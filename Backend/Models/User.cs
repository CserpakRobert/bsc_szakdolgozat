using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Backend.Models
{
    [Table("Users")]
    public class User: IdentityUser<int>
    {


        [MaxLength(25, ErrorMessage ="Felhasználó név nem lehet 26 karakternél hoszabb")]
        [Required]
        public override string UserName { get; set; }

        [EmailAddress(ErrorMessage ="Kérlek adj meg egy valós e-email címet")]
        [Required]
        public override string Email { get; set; }


        public ICollection<Submission> VotedSubmissions { get; set; } = [];


        public ICollection<Competition> CreatedCompetitions { get; set; } = [];
        public ICollection<Competition> ModeratedCompetitions { get; set; } = [];
        public ICollection<Submission> CreatedSubmissions { get; set; } = [];
        public ICollection<Submission> CollaboratedSubmissions { get; set; } = [];
        public ICollection<SubmissionInvites> SentInvites { get; set; } = [];
        public ICollection<SubmissionInvites> RecievedInvites { get; set; } = [];
        public ICollection<ClosedGroupJoinRequests> JoinRequests { get; set; } = [];
        public ICollection<Group> OwnedGroups { get; set; } = [];
        public ICollection<Group> ParticipatingGroups { get; set; } = [];




        //Remove not used fields
        public override string? PhoneNumber { get; set; } = null!;
        public override bool PhoneNumberConfirmed { get; set; } = false;
        public override bool TwoFactorEnabled { get; set; } = false;


    }
}
