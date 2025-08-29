using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models;
using RealEstate.Infrastructure.Entities;
using RealEstate.Infrastructure.Repositories;

namespace RealEstate.Application.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OwnerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OwnerModel?> GetByIdAsync(Guid id)
        {
            var owner = await _unitOfWork.Owners.GetByIdAsync(id);

            if (owner == null)
                return null;

            return new OwnerModel
            {
                Id = owner.Id,
                Name = owner.Name,
                Address = owner.Address,
                Birthday = owner.Birthday
            };
        }

        public async Task<OwnerModel> CreateAsync(OwnerModel model)
        {
            var owner = new Owner
            {
                Name = model.Name,
                Address = model.Address,
                Photo = model.Photo,
                Birthday = model.Birthday
            };

            await _unitOfWork.Owners.AddAsync(owner);
            await _unitOfWork.CompleteAsync();

            model.Id = owner.Id;
            return model;
        }

        public async Task<PagedResult<OwnerModel>> GetAllAsync(int page = 1, int pageSize = 10)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _unitOfWork.Owners.Query();

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(o => o.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OwnerModel
                {
                    Id = o.Id,
                    Name = o.Name,
                    Address = o.Address,
                    Photo = o.Photo,
                    Birthday = o.Birthday
                })
                .ToListAsync();

            return new PagedResult<OwnerModel>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

    }
}
