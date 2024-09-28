using System.ComponentModel.DataAnnotations;

namespace DIGIFNB_BackEnd_API.Models.Grab.Order
{
    public class Upcoming
    {
        [Key]
        [Required]
        public string OrderId { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public string Items { get; set; }
        [Required]
        public string Driver { get; set; }
        [Required]
        public string PickupIn { get; set; }
        [Required]
        public string Receipt { get; set; }

    }
}
