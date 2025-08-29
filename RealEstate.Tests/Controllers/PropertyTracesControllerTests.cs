using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RealEstate.Api.Controllers;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models;

namespace RealEstate.Tests.Controllers
{
    [TestFixture]
    public class PropertyTracesControllerTests
    {
        private Mock<IPropertyTraceService> _mockService;
        private PropertyTracesController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IPropertyTraceService>();
            _controller = new PropertyTracesController(
                _mockService.Object,
                Mock.Of<Microsoft.Extensions.Logging.ILogger<PropertyTracesController>>());
        }

        [Test]
        public async Task AddTrace_ShouldReturnOk_WhenPropertyExists()
        {
            // Arrange
            var model = new PropertyTraceModel { PropertyId = Guid.NewGuid() };
            var created = new PropertyTraceModel { Id = Guid.NewGuid(), PropertyId = model.PropertyId };

            _mockService.Setup(s => s.AddAsync(model))
                .ReturnsAsync(created);

            // Act
            var result = await _controller.AddTrace(model);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var returned = okResult.Value as PropertyTraceModel;
            Assert.That(returned, Is.Not.Null);
            Assert.That(returned.Id, Is.EqualTo(created.Id));
        }

        [Test]
        public async Task AddTrace_ShouldReturnNotFound_WhenPropertyDoesNotExist()
        {
            // Arrange
            var model = new PropertyTraceModel { PropertyId = Guid.NewGuid() };

            _mockService.Setup(s => s.AddAsync(model))
                .ReturnsAsync((PropertyTraceModel)null);

            // Act
            var result = await _controller.AddTrace(model);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetByProperty_ShouldReturnOk_WithTraces()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            var traces = new List<PropertyTraceModel>
            {
                new PropertyTraceModel { Id = Guid.NewGuid(), PropertyId = propertyId }
            };

            _mockService.Setup(s => s.GetByPropertyAsync(propertyId))
                .ReturnsAsync(traces);

            // Act
            var result = await _controller.GetByProperty(propertyId);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var returned = okResult.Value as List<PropertyTraceModel>;
            Assert.That(returned, Is.Not.Null);
            Assert.That(returned.Count, Is.EqualTo(1));
            Assert.That(returned.First().PropertyId, Is.EqualTo(propertyId));
        }

        [Test]
        public async Task GetById_ShouldReturnOk_WhenTraceExists()
        {
            // Arrange
            var traceId = Guid.NewGuid();
            var trace = new PropertyTraceModel { Id = traceId, PropertyId = Guid.NewGuid() };

            _mockService.Setup(s => s.GetByIdAsync(traceId))
                .ReturnsAsync(trace);

            // Act
            var result = await _controller.GetById(traceId);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var returned = okResult.Value as PropertyTraceModel;
            Assert.That(returned, Is.Not.Null);
            Assert.That(returned.Id, Is.EqualTo(traceId));
        }

        [Test]
        public async Task GetById_ShouldReturnNotFound_WhenTraceDoesNotExist()
        {
            // Arrange
            var traceId = Guid.NewGuid();

            _mockService.Setup(s => s.GetByIdAsync(traceId))
                .ReturnsAsync((PropertyTraceModel)null);

            // Act
            var result = await _controller.GetById(traceId);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
    }
}
