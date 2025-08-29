using RealEstate.Application.Models;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyService
    {
        Task<PropertyModel?> GetByIdAsync(Guid id);
        Task<PropertyModel> CreateAsync(PropertyModel model);
        Task<PropertyModel> UpdateAsync(Guid id, PropertyModel model);
        Task<PropertyModel> ChangePriceAsync(Guid id, double newPrice);
        Task<PropertyImageModel> AddImageAsync(Guid propertyId, string imageUrl);
        Task<PagedResult<PropertyModel>> GetAllAsync(string? name = null, string? address = null, double? minPrice = null, double? maxPrice = null, int page = 1, int pageSize = 10);
    }
}
