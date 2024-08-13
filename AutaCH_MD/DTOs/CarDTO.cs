namespace AutaCH_MD.DTOs
{
    public class CarDTO
    {
        public Guid CarId { get; set; }
        public DateTime? StartAuction { get; set; }
        public DateTime? EndAuction { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? Body { get; set; }
        public string? Color { get; set; }
        public int? Engine { get; set; }
        public string? Fuel { get; set; }
        public string? Transmission { get; set; }
        public int? Gears { get; set; }
        public int? Doors { get; set; }
        public int? Seats { get; set; }
        public string? Tires { get; set; }
        public int? Key { get; set; }
        public int? Year { get; set; }
        public int? Mileage { get; set; }
        public string? FirstRegister { get; set; }
        public string? ReferenceNumber { get; set; }
        public List<string>? Images { get; set; }
        public decimal? NewPrice { get; set; }
        public decimal? Value { get; set; }
        public decimal? RepairCosts { get; set; }
        public string? MoreInfo { get; set; }
        public string? SerialEquipment { get; set; }
        public bool? RegistrationCertificate { get; set; }
        public bool? Drive { get; set; }
        public bool? Damaged { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateTime { get; set; }
        public decimal UserBid { get; set; }
    }
}
