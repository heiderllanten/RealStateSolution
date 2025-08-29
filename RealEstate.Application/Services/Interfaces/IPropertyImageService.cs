using RealEstate.Application.Models;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyImageService
    {
        Task<PropertyImageModel> AddAsync(Guid propertyId, string imageUrl);
        Task<PropertyImageModel> UpdateUrlAsync(Guid imageId, string newUrl);
        Task<bool> RemoveAsync(Guid imageId);
        Task<IEnumerable<PropertyImageModel>> GetByPropertyAsync(Guid propertyId);
        Task<PropertyImageModel> GetByIdAsync(Guid imageId);
    }
}
