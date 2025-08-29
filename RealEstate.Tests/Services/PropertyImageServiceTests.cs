using Moq;
using NUnit.Framework;
using RealEstate.Application.Services;
using RealEstate.Infrastructure.Entities;
using RealEstate.Infrastructure.Repositories;

namespace RealEstate.Tests.Services
{
    public class PropertyImageServiceTests
    {
        private Mock<IUnitOfWork> _mockUow;
        private Mock<IPropertyImageRepository> _mockImageRepo;
        private Mock<IPropertyRepository> _mockPropertyRepo;
        private PropertyImageService _service;

        [SetUp]
        public void Setup()
        {
            _mockImageRepo = new Mock<IPropertyImageRepository>();
            _mockPropertyRepo = new Mock<IPropertyRepository>();

            _mockUow = new Mock<IUnitOfWork>();
            _mockUow.Setup(u => u.PropertyImages).Returns(_mockImageRepo.Object);
            _mockUow.Setup(u => u.Properties).Returns(_mockPropertyRepo.Object);

            _service = new PropertyImageService(_mockUow.Object);
        }

        [Test]
        public async Task AddAsync_ShouldReturnImage_WhenPropertyExists()
        {
            var propertyId = Guid.NewGuid();
            _mockPropertyRepo.Setup(r => r.GetByIdAsync(propertyId)).ReturnsAsync(new Property { Id = propertyId });

            _mockImageRepo.Setup(r => r.AddAsync(It.IsAny<PropertyImage>())).Returns(Task.CompletedTask);
            _mockUow.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _service.AddAsync(propertyId, "/images/properties/test.jpg");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Url, Is.EqualTo("/images/properties/test.jpg"));
        }
    }
}
