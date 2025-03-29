using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class ShortStoryDetails
    {

        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(20000)]
        [StringLength(20000, ErrorMessage=("Egy történet maximum 20000 karakter lehet"))]

        public string Story { get; set; }

        [Required]
        public int SubmissionId { get; set; }

        [Required]
        public Submission Submission { get; set; }
    }
}
