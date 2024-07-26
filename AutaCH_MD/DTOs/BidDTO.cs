namespace AutaCH_MD.DTOs
{
    public class BidDTO
    {
        public Guid BidId { get; set; }
        public Guid UserId { get; set; }
        public Guid CarId { get; set; }
        public decimal BidAmount { get; set; }
    }
}
