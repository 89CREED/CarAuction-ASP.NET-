namespace AutaCH_MD.DTOs
{
    public class BidDTO
    {
        public Guid BidId { get; set; }
        public Guid UserId { get; set; }
        public Guid CarId { get; set; }
        public decimal BidAmount { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsActive { get; set; }


        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Make {  get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
    }
}
