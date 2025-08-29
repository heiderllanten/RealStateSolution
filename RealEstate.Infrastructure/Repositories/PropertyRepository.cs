using RealEstate.Infrastructure.Context;
using RealEstate.Infrastructure.Entities;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(RealEstateDbContext context) : base(context)
        {
        }

        public IQueryable<Property> Query()
        {
            return _context.Properties.AsQueryable();
        }
    }
}
