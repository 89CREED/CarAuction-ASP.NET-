using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AutaCH_MD.Models
{
    public class CarInformation
    {
        [Key]
        [Required]
        public Guid InfoId { get; set; } = Guid.NewGuid();
        [ForeignKey("CarId")]
        [JsonIgnore]
        public Guid CarId { get; set; }
        public Car Car { get; set; }
        public string Make {  get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Body { get; set; }
        public string Fuel { get; set; }
        public int Engine {  get; set; }
        public int Doors { get; set; }
        public int Seats { get; set; }
        public string Condition { get; set; }
        public string Transmission { get; set; }
        public int Gears { get; set; }
        public decimal NewPrice { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal Value { get; set; }
        public string Tires { get; set; }
        public string Interior { get; set; }
        public string ServiceBook { get; set; }
        public string PreviousDamages { get; set; }
        public string MoreInfo { get; set; }
        public string SerialEquipment { get; set; }
        public decimal Repair { get; set; }
        public int Key { get; set; }
        public bool RegistrationCertificate { get; set; }
        public bool Drive { get; set; }
        public bool Damaged {  get; set; }
        public bool IsActive { get; set; }
        public DateTime DateTime { get; set; }

        public virtual ICollection<Bid> Bids { get; set; }


    }
}
