using NUnit.Framework;
using RealEstate.Infrastructure.Context;
using RealEstate.Infrastructure.Entities;
using RealEstate.Infrastructure.Repositories;

namespace RealEstate.Tests.Repositories
{
    public class PropertyTraceRepositoryTests
    {
        private RealEstateDbContext _context;
        private PropertyTraceRepository _repository;

        [SetUp]
        public void Setup()
        {
            _context = DbContextHelper.GetInMemoryDbContext(Guid.NewGuid().ToString());
            _repository = new PropertyTraceRepository(_context);
        }

        [Test]
        public async Task AddAsync_ShouldAddTrace()
        {
            var trace = new PropertyTrace { Id = Guid.NewGuid(), PropertyId = Guid.NewGuid(), Name = "Venta inicial", Value = 100000, Tax = 5000 };

            await _repository.AddAsync(trace);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(trace.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Venta inicial"));
        }
    }
}
