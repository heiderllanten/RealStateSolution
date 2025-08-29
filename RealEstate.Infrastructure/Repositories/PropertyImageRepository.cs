using RealEstate.Infrastructure.Context;
using RealEstate.Infrastructure.Entities;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyImageRepository : GenericRepository<PropertyImage>, IPropertyImageRepository
    {
        public PropertyImageRepository(RealEstateDbContext context) : base(context)
        {
        }
    }
}
