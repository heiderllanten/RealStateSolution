using Moq;
using NUnit.Framework;
using RealEstate.Application.Models;
using RealEstate.Application.Services;
using RealEstate.Infrastructure.Entities;
using RealEstate.Infrastructure.Repositories;

namespace RealEstate.Tests.Services
{
    public class OwnerServiceTests
    {
        private Mock<IUnitOfWork> _mockUow;
        private Mock<IOwnerRepository> _mockOwnerRepo;
        private OwnerService _service;

        [SetUp]
        public void Setup()
        {
            _mockOwnerRepo = new Mock<IOwnerRepository>();
            _mockUow = new Mock<IUnitOfWork>();
            _mockUow.Setup(u => u.Owners).Returns(_mockOwnerRepo.Object);

            _service = new OwnerService(_mockUow.Object);
        }

        [Test]
        public async Task CreateAsync_ShouldReturnOwnerModel()
        {
            var model = new OwnerModel { Name = "Juan Pérez" };

            _mockOwnerRepo.Setup(r => r.AddAsync(It.IsAny<Owner>())).Returns(Task.CompletedTask);
            _mockUow.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _service.CreateAsync(model);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Juan Pérez"));
        }
    }
}
