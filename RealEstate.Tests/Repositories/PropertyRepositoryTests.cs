using NUnit.Framework;
using RealEstate.Infrastructure.Context;
using RealEstate.Infrastructure.Entities;
using RealEstate.Infrastructure.Repositories;

namespace RealEstate.Tests.Repositories
{
    public class PropertyRepositoryTests
    {
        private RealEstateDbContext _context;
        private PropertyRepository _repository;

        [SetUp]
        public void Setup()
        {
            _context = DbContextHelper.GetInMemoryDbContext(Guid.NewGuid().ToString());
            _repository = new PropertyRepository(_context);
        }

        [Test]
        public async Task AddAsync_ShouldAddProperty()
        {
            var property = new Property { Id = Guid.NewGuid(), Name = "Casa 123", Price = 100000 };

            await _repository.AddAsync(property);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(property.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Casa 123"));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnProperties()
        {
            await _repository.AddAsync(new Property { Id = Guid.NewGuid(), Name = "Depto", Price = 200000 });
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync();

            Assert.That(result, Is.Not.Empty);
        }
    }
}
