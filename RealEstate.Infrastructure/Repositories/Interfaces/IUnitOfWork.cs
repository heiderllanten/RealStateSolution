using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Repositories
{
    public interface IUnitOfWork
    {
        IPropertyRepository Properties { get; }
        IPropertyImageRepository PropertyImages { get; }
        IOwnerRepository Owners { get; }
        IPropertyTraceRepository PropertyTraces { get; }

        Task<int> CompleteAsync();
    }
}
