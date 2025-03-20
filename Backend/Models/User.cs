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


        [MaxLength(22)]
        [Required]
        public override string UserName { get; set; }

        [EmailAddress]
        [Required]
        public override string Email { get; set; }

        public ICollection<Submission> OwnedSubmissions { get; set; }
        public ICollection<Submission> CollaboratedSubmissions { get; set; }

        public ICollection<Competition> ModeratedCompetitions { get; set; }

        public ICollection<Competition> OwnedCompetiotions { get; set; }

    }
}
