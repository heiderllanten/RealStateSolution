using System;

namespace RealEstate.Infrastructure.Entities
{
    public class PropertyImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string File { get; set; } = string.Empty;
        public bool Enabled { get; set; } = false;

        public Guid PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
