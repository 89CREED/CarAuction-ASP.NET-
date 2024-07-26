using System.ComponentModel.DataAnnotations;

namespace AutaCH_MD.Models
{
    public class Car
    {
        [Key]
        [Required]
        public Guid CarId { get; set; } = Guid.NewGuid();
        public DateTime StartAuction { get; set; }
        public DateTime EndAuction { get; set; }
        public string Make {  get; set; }
        public string Model { get; set; }
        public string Body { get; set; }
        public string FirstRegister { get; set; }
        public int Year { get; set; }
        public decimal Mileage { get; set; }
        public string ReferenceNumber { get; set; } 
        public List<string> Images {  get; set; }
        public decimal Price { get; set; }
        public bool IsActive {  get; set; }
        public DateTime DateTime { get; set; }

        public ICollection<Bid> Bids { get; set; }
        public CarInformation CarInformation { get; set; }

    }
}
