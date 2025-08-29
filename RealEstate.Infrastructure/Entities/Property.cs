using System;
using System.Collections.Generic;

namespace RealEstate.Infrastructure.Entities
{
    public class Property
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Price { get; set; }
        public string CodeInternational { get; set; } = string.Empty;
        public int Year { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Relaciones
        public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
        public ICollection<PropertyTrace> PropertyTraces { get; set; } = new List<PropertyTrace>();

        public Guid OwnerId { get; set; }
        public Owner Owner { get; set; }
    }
}
