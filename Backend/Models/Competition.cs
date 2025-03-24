using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Competition")]
    public class Competition 
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool IsPrivate { get; set; }
        [Required]
        public User Creator { get; set; }
        [Required]
        public int CreatorId { get; set; }

        [Required]
        public ICollection<User> Moderators { get; set; }

        public ICollection<Submission> Submissions { get; set; }



    }
    
}
