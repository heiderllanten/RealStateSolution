using NUnit.Framework;
using RealEstate.Infrastructure.Context;
using RealEstate.Infrastructure.Entities;
using RealEstate.Infrastructure.Repositories;

namespace RealEstate.Tests.Repositories
{
    public class PropertyImageRepositoryTests
    {
        private RealEstateDbContext _context;
        private PropertyImageRepository _repository;

        [SetUp]
        public void Setup()
        {
            _context = DbContextHelper.GetInMemoryDbContext(Guid.NewGuid().ToString());
            _repository = new PropertyImageRepository(_context);
        }

        [Test]
        public async Task AddAsync_ShouldAddImage()
        {
            var image = new PropertyImage { Id = Guid.NewGuid(), File = "/images/test.jpg", PropertyId = Guid.NewGuid() };

            await _repository.AddAsync(image);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(image.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.File, Is.EqualTo("/images/test.jpg"));
        }
    }
}
