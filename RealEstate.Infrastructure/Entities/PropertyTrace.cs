namespace RealEstate.Infrastructure.Entities
{
    public class PropertyTrace
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DateSale { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Value { get; set; }
        public double Tax { get; set; }

        // Relación con propiedad
        public Guid PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
