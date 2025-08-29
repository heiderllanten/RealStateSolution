using RealEstate.Application.Interfaces;
using RealEstate.Application.Models;
using RealEstate.Infrastructure.Entities;
using RealEstate.Infrastructure.Repositories;

namespace RealEstate.Application.Services
{
    public class PropertyTraceService : IPropertyTraceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PropertyTraceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PropertyTraceModel> AddAsync(PropertyTraceModel model)
        {
            // Validar propiedad
            var property = await _unitOfWork.Properties.GetByIdAsync(model.PropertyId);
            if (property == null) return null;

            var trace = new PropertyTrace
            {
                PropertyId = model.PropertyId,
                DateSale = model.DateSale == default ? DateTime.UtcNow : model.DateSale,
                Name = model.Name,
                Value = model.Value,
                Tax = model.Tax
            };

            await _unitOfWork.PropertyTraces.AddAsync(trace);
            await _unitOfWork.CompleteAsync();

            model.Id = trace.Id;
            if (model.DateSale == default) model.DateSale = trace.DateSale;
            return model;
        }

        public async Task<IEnumerable<PropertyTraceModel>> GetByPropertyAsync(Guid propertyId)
        {
            var traces = await _unitOfWork.PropertyTraces.FindAsync(t => t.PropertyId == propertyId);
            return traces.Select(t => new PropertyTraceModel
            {
                Id = t.Id,
                PropertyId = t.PropertyId,
                DateSale = t.DateSale,
                Name = t.Name,
                Value = t.Value,
                Tax = t.Tax
            });
        }

        public async Task<PropertyTraceModel> GetByIdAsync(Guid traceId)
        {
            var trace = await _unitOfWork.PropertyTraces.GetByIdAsync(traceId);
            if (trace == null) return null;

            return new PropertyTraceModel
            {
                Id = trace.Id,
                PropertyId = trace.PropertyId,
                DateSale = trace.DateSale,
                Name = trace.Name,
                Value = trace.Value,
                Tax = trace.Tax
            };
        }
    }
}
