using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIGIFNB_BackEnd_API.Models.Grab.Feedback
{
    public class Feedback
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackId { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public string Review { get; set; }
        [Required]
        public string Customer { get; set; }
        [Required]
        public string CustomerID { get; set; }
        [Required]
        public string Store { get; set; }
        [Required]
        public string? Type { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
