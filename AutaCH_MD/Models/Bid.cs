using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AutaCH_MD.Models
{
    public class Bid
    {
        [Key]
        [Required]
        public Guid BidId { get; set; } = Guid.NewGuid();
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [Required]
        [ForeignKey("Car")]
        public Guid CarId {  get; set; }
        [JsonIgnore]
        public Car Car {  get; set; }
        public decimal BidAmount {  get; set; }
        public DateTime DateTime { get; set; }
        public bool IsActive { get; set; }
    }
}
