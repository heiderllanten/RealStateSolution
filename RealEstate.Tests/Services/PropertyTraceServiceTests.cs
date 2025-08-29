using Moq;
using NUnit.Framework;
using RealEstate.Application.Models;
using RealEstate.Application.Services;
using RealEstate.Infrastructure.Entities;
using RealEstate.Infrastructure.Repositories;
using System.Linq.Expressions;

namespace RealEstate.Tests.Services
{
    public class PropertyTraceServiceTests
    {
        private Mock<IUnitOfWork> _mockUow;
        private Mock<IPropertyTraceRepository> _mockTraceRepo;
        private Mock<IPropertyRepository> _mockPropertyRepo;
        private PropertyTraceService _service;

        [SetUp]
        public void Setup()
        {
            _mockTraceRepo = new Mock<IPropertyTraceRepository>();
            _mockPropertyRepo = new Mock<IPropertyRepository>();

            _mockUow = new Mock<IUnitOfWork>();
            _mockUow.Setup(u => u.PropertyTraces).Returns(_mockTraceRepo.Object);
            _mockUow.Setup(u => u.Properties).Returns(_mockPropertyRepo.Object);

            _service = new PropertyTraceService(_mockUow.Object);
        }

        [Test]
        public async Task AddAsync_ShouldReturnTrace_WhenPropertyExists()
        {
            var propertyId = Guid.NewGuid();
            var traceModel = new PropertyTraceModel
            {
                PropertyId = propertyId,
                Value = 100000,
                Tax = 5000,
                Name = "Venta inicial"
            };

            _mockPropertyRepo.Setup(r => r.GetByIdAsync(propertyId)).ReturnsAsync(new Property { Id = propertyId });
            _mockTraceRepo.Setup(r => r.AddAsync(It.IsAny<PropertyTrace>())).Returns(Task.CompletedTask);
            _mockUow.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var result = await _service.AddAsync(traceModel);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Venta inicial"));
        }

        [Test]
        public async Task GetByPropertyAsync_ShouldReturnList()
        {
            var propertyId = Guid.NewGuid();
            _mockTraceRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<PropertyTrace, bool>>>()))
                          .ReturnsAsync(new List<PropertyTrace>
                          {
                              new PropertyTrace { Id = Guid.NewGuid(), PropertyId = propertyId, Name = "Venta A", Value = 100000, Tax = 5000 }
                          });

            var result = await _service.GetByPropertyAsync(propertyId);

            Assert.That(result.Count(), Is.EqualTo(1));
        }
    }
}
