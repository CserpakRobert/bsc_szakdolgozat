using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    [Table("Submissions")]
    public class Submission
    {
        [Required]
        public int votes { get; set; }
        
        public MovieDetails? MovieDetails {get;set;}
    }
}
