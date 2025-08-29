namespace RealEstate.Application.Models
{
    public class PropertyModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Price { get; set; }
        public string CodeInternational { get; set; } = string.Empty;
        public int Year { get; set; }
        public Guid OwnerId { get; set; }

        public List<PropertyImageModel> Images { get; set; } = new();
    }
}
