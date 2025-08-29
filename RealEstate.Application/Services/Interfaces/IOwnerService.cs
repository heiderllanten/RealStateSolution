using RealEstate.Application.Models;

namespace RealEstate.Application.Interfaces
{
    public interface IOwnerService
    {
        Task<OwnerModel?> GetByIdAsync(Guid id);
        Task<OwnerModel> CreateAsync(OwnerModel model);
        Task<PagedResult<OwnerModel>> GetAllAsync(int page = 1, int pageSize = 10);
    }
}
