using System.ComponentModel.DataAnnotations;

namespace AutaCH_MD.Models
{
    public class User
    {
        [Key]
        [Required]
        public Guid UserId { get; set; } = Guid.NewGuid();
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode {  get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string HouseNumber { get; set; }
        [Required]
        public string ApartmentNumber { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public string Login {  get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public bool TermsAccepted { get; set; }
        [Required]
        public bool AknowledgeAccepted {  get; set; }
        public bool IsActive { get; set; }
        public DateTime DateTime { get; set; }
        public string? Type { get; set; }

        public ICollection<Bid> Bids { get; set; }
    }
}
