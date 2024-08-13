using System.ComponentModel.DataAnnotations;

namespace AutaCH_MD.DTOs
{
    public class AdminUpdateUserDTO
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }   
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Login {  get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateTime { get; set; }
        public string? Type { get; set; }

    }
}
