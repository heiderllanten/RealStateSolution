namespace RealEstate.Application.Models
{
    public class PropertyTraceModel
    {
        public Guid Id { get; set; }
        public DateTime DateSale { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Value { get; set; }
        public double Tax { get; set; }
        public Guid PropertyId { get; set; }
    }
}
