using RealEstate.Infrastructure.Context;
using RealEstate.Infrastructure.Entities;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyTraceRepository : GenericRepository<PropertyTrace>, IPropertyTraceRepository
    {
        public PropertyTraceRepository(RealEstateDbContext context) : base(context)
        {
        }
    }
}
