using RealEstate.Infrastructure.Entities;

namespace RealEstate.Infrastructure.Repositories
{
    public interface IPropertyTraceRepository : IGenericRepository<PropertyTrace>
    {
        // Aquí podrías agregar métodos específicos, ej:
        // Task<IEnumerable<PropertyTrace>> GetTracesByPropertyIdAsync(Guid propertyId);
    }
}
