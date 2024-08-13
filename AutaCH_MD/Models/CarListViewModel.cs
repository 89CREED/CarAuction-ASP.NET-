namespace AutaCH_MD.Models
{
    public class CarListViewModel
    {
        public IEnumerable<Car> Cars { get; set; }
        public Guid CarId { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
        public int? MileageFrom { get; set; }
        public int? MileageTo { get; set; }
        public DateTime EndAuction { get; set; }
        public string TimeRemaining { get; set; } 
    }

}
