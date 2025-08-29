using NUnit.Framework;
using RealEstate.Infrastructure.Context;
using RealEstate.Infrastructure.Entities;
using RealEstate.Infrastructure.Repositories;

namespace RealEstate.Tests.Repositories
{
    public class OwnerRepositoryTests
    {
        private RealEstateDbContext _context;
        private OwnerRepository _repository;

        [SetUp]
        public void Setup()
        {
            _context = DbContextHelper.GetInMemoryDbContext(Guid.NewGuid().ToString());
            _repository = new OwnerRepository(_context);
        }

        [Test]
        public async Task AddAsync_ShouldAddOwner()
        {
            var owner = new Owner { Id = Guid.NewGuid(), Name = "John Doe" };

            await _repository.AddAsync(owner);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(owner.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("John Doe"));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnOwners()
        {
            await _repository.AddAsync(new Owner { Id = Guid.NewGuid(), Name = "Jane Doe" });
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync();

            Assert.That(result, Is.Not.Empty);
        }
    }
}
