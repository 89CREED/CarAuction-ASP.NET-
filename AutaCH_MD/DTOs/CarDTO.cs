namespace AutaCH_MD.DTOs
{
    public class CarDTO
    {
        public Guid CarId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Body { get; set; }
        public int Year { get; set; }
        public decimal Mileage { get; set; }
        public List<string> Images { get; set; }
    }
}
