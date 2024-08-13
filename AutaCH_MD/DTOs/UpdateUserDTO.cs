using System.ComponentModel.DataAnnotations;

namespace AutaCH_MD.DTOs
{
    public class UpdateUserDTO
    {
        public Guid UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string HouseNumber { get; set; }
        [Required]
        public string ApartmentNumber { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

    }
}
