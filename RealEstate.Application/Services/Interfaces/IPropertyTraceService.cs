using RealEstate.Application.Models;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyTraceService
    {
        Task<PropertyTraceModel> AddAsync(PropertyTraceModel model);
        Task<IEnumerable<PropertyTraceModel>> GetByPropertyAsync(Guid propertyId);
        Task<PropertyTraceModel> GetByIdAsync(Guid traceId);

    }
}
