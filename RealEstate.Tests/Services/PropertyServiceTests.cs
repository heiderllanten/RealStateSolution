using Moq;
using NUnit.Framework;
using RealEstate.Application.Models;
using RealEstate.Application.Services;
using RealEstate.Infrastructure.Entities;
using RealEstate.Infrastructure.Repositories;

namespace RealEstate.Tests.Services
{
    public class PropertyServiceTests
    {
        private Mock<IUnitOfWork> _mockUow;
        private Mock<IPropertyRepository> _mockPropertyRepo;
        private PropertyService _service;

        [SetUp]
        public void Setup()
        {
            _mockPropertyRepo = new Mock<IPropertyRepository>();
            _mockUow = new Mock<IUnitOfWork>();
            _mockUow.Setup(u => u.Properties).Returns(_mockPropertyRepo.Object);

            _service = new PropertyService(_mockUow.Object);
        }

        [Test]
        public async Task CreateAsync_ShouldReturnPropertyModel_WhenCreated()
        {
            var model = new PropertyModel { Name = "Casa Test", Price = 100000, OwnerId = Guid.NewGuid() };

            _mockPropertyRepo.Setup(r => r.AddAsync(It.IsAny<Property>())).Returns(Task.CompletedTask);
            _mockUow.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _service.CreateAsync(model);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Casa Test"));
        }

        [Test]
        public async Task ChangePriceAsync_ShouldUpdatePrice()
        {
            var propertyId = Guid.NewGuid();
            var property = new Property { Id = propertyId, Name = "Depto", Price = 150000 };

            _mockPropertyRepo.Setup(r => r.GetByIdAsync(propertyId)).ReturnsAsync(property);
            _mockUow.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _service.ChangePriceAsync(propertyId, 200000);

            Assert.That(result.Price, Is.EqualTo(200000));
        }
    }
}
