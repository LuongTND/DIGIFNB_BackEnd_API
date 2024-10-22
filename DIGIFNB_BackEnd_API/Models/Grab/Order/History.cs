using System.ComponentModel.DataAnnotations;

namespace DIGIFNB_BackEnd_API.Models.Grab.Order
{
    public class History
    {
        [Key]
        [Required]
        public string LongOrderId { get; set; }
        [Required]
        public string ShortOrderId { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

    }
}
