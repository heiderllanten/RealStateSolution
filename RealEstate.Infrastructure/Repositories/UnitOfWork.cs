using RealEstate.Infrastructure.Context;

namespace RealEstate.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RealEstateDbContext _context;

        public IPropertyRepository Properties { get; }
        public IPropertyImageRepository PropertyImages { get; }
        public IOwnerRepository Owners { get; }
        public IPropertyTraceRepository PropertyTraces { get; }

        public UnitOfWork(
            RealEstateDbContext context,
            IPropertyRepository propertyRepository,
            IPropertyImageRepository propertyImageRepository,
            IOwnerRepository ownerRepository,
            IPropertyTraceRepository propertyTraceRepository)
        {
            _context = context;
            Properties = propertyRepository;
            PropertyImages = propertyImageRepository;
            Owners = ownerRepository;
            PropertyTraces = propertyTraceRepository;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
