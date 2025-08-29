using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models;
using RealEstate.Infrastructure.Entities;
using RealEstate.Infrastructure.Repositories;

namespace RealEstate.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PropertyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PropertyModel?> GetByIdAsync(Guid id)
        {
            var property = await _unitOfWork.Properties.GetByIdAsync(id);

            if (property == null)
                return null;

            return new PropertyModel
            {
                Id = property.Id,
                Name = property.Name,
                Address = property.Address,
                Price = property.Price,
                CodeInternational = property.CodeInternational,
                Year = property.Year,
                OwnerId = property.OwnerId
            };
        }


        public async Task<PropertyModel> CreateAsync(PropertyModel model)
        {
            var property = new Property
            {
                Name = model.Name,
                Address = model.Address,
                Price = model.Price,
                CodeInternational = model.CodeInternational,
                Year = model.Year,
                OwnerId = model.OwnerId
            };

            await _unitOfWork.Properties.AddAsync(property);
            await _unitOfWork.CompleteAsync();

            model.Id = property.Id;
            return model;
        }

        public async Task<PropertyModel> UpdateAsync(Guid id, PropertyModel model)
        {
            var property = await _unitOfWork.Properties.GetByIdAsync(id);
            if (property == null) return null;

            property.Name = model.Name;
            property.Address = model.Address;
            property.Price = model.Price;
            property.CodeInternational = model.CodeInternational;
            property.Year = model.Year;
            property.OwnerId = model.OwnerId;
            property.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Properties.Update(property);
            await _unitOfWork.CompleteAsync();

            return model;
        }

        public async Task<PropertyModel> ChangePriceAsync(Guid id, double newPrice)
        {
            var property = await _unitOfWork.Properties.GetByIdAsync(id);
            if (property == null) return null;

            property.Price = newPrice;
            property.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Properties.Update(property);
            await _unitOfWork.CompleteAsync();

            return new PropertyModel
            {
                Id = property.Id,
                Name = property.Name,
                Price = property.Price
            };
        }

        public async Task<PropertyImageModel> AddImageAsync(Guid propertyId, string imageUrl)
        {
            var property = await _unitOfWork.Properties.GetByIdAsync(propertyId);
            if (property == null) return null;

            var image = new PropertyImage
            {
                File = imageUrl,
                PropertyId = propertyId
            };

            await _unitOfWork.PropertyImages.AddAsync(image);
            await _unitOfWork.CompleteAsync();

            return new PropertyImageModel { Id = image.Id, Url = image.File };
        }

        public async Task<PagedResult<PropertyModel>> GetAllAsync(string? name = null, string? address = null, double? minPrice = null, double? maxPrice = null, int page = 1, int pageSize = 10)
        {
            var query = _unitOfWork.Properties.Query();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            if (!string.IsNullOrEmpty(address))
                query = query.Where(p => p.Address.Contains(address));

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PropertyModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Address = p.Address,
                    Price = p.Price,
                    CodeInternational = p.CodeInternational,
                    Year = p.Year,
                    OwnerId = p.OwnerId
                })
                .ToListAsync();

            return new PagedResult<PropertyModel>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

    }
}
