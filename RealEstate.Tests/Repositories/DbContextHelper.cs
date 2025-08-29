using Microsoft.EntityFrameworkCore;
using RealEstate.Infrastructure.Context;

namespace RealEstate.Tests.Repositories
{
    public static class DbContextHelper
    {
        public static RealEstateDbContext GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<RealEstateDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new RealEstateDbContext(options);
        }
    }
}
