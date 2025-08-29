using RealEstate.Infrastructure.Context;
using RealEstate.Infrastructure.Entities;

namespace RealEstate.Infrastructure.Repositories
{
    public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
    {
        public OwnerRepository(RealEstateDbContext context) : base(context)
        {
        }
    }
}
