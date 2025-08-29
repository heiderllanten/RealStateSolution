using RealEstate.Application.Interfaces;
using RealEstate.Application.Models;
using RealEstate.Infrastructure.Entities;
using RealEstate.Infrastructure.Repositories;

namespace RealEstate.Application.Services
{
    public class PropertyImageService : IPropertyImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PropertyImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PropertyImageModel> AddAsync(Guid propertyId, string imageUrl)
        {
            var property = await _unitOfWork.Properties.GetByIdAsync(propertyId);
            if (property == null) return null;

            var image = new PropertyImage
            {
                PropertyId = propertyId,
                File = imageUrl
            };

            await _unitOfWork.PropertyImages.AddAsync(image);
            await _unitOfWork.CompleteAsync();

            return new PropertyImageModel
            {
                Id = image.Id,
                Url = image.File
            };
        }

        public async Task<PropertyImageModel> UpdateUrlAsync(Guid imageId, string newUrl)
        {
            var image = await _unitOfWork.PropertyImages.GetByIdAsync(imageId);
            if (image == null) return null;

            image.File = newUrl;
            _unitOfWork.PropertyImages.Update(image);
            await _unitOfWork.CompleteAsync();

            return new PropertyImageModel
            {
                Id = image.Id,
                Url = image.File
            };
        }

        public async Task<bool> RemoveAsync(Guid imageId)
        {
            var image = await _unitOfWork.PropertyImages.GetByIdAsync(imageId);
            if (image == null) return false;

            _unitOfWork.PropertyImages.Remove(image);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<PropertyImageModel>> GetByPropertyAsync(Guid propertyId)
        {
            var images = await _unitOfWork.PropertyImages.FindAsync(i => i.PropertyId == propertyId);
            return images.Select(i => new PropertyImageModel
            {
                Id = i.Id,
                Url = i.File
            });
        }

        public async Task<PropertyImageModel> GetByIdAsync(Guid imageId)
        {
            var image = await _unitOfWork.PropertyImages.GetByIdAsync(imageId);
            if (image == null) return null;

            return new PropertyImageModel
            {
                Id = image.Id,
                Url = image.File
            };
        }
    }
}
