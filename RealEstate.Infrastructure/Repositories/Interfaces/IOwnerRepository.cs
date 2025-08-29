using RealEstate.Infrastructure.Entities;

namespace RealEstate.Infrastructure.Repositories
{
    public interface IOwnerRepository : IGenericRepository<Owner>
    {
        // Aquí podrías agregar métodos específicos, ej:
        // Task<IEnumerable<Owner>> GetOwnersWithPropertiesAsync();
    }
}
