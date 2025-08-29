using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RealEstate.Api.Controllers;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models;

namespace RealEstate.Tests.Controllers
{
    [TestFixture]
    public class PropertiesControllerTests
    {
        private Mock<IPropertyService> _mockPropertyService;
        private PropertiesController _controller;

        [SetUp]
        public void Setup()
        {
            _mockPropertyService = new Mock<IPropertyService>();
            _controller = new PropertiesController(
                _mockPropertyService.Object,
                Mock.Of<Microsoft.Extensions.Logging.ILogger<PropertiesController>>());
        }

        [Test]
        public async Task Create_ShouldReturnCreatedResult_WhenPropertyIsValid()
        {
            // Arrange
            var model = new PropertyModel { Name = "House 1" };
            var created = new PropertyModel { Id = Guid.NewGuid(), Name = "House 1" };

            _mockPropertyService.Setup(s => s.CreateAsync(model))
                .ReturnsAsync(created);

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.That(result, Is.TypeOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult.ActionName, Is.EqualTo(nameof(_controller.GetById)));
            Assert.That(createdResult.Value, Is.EqualTo(created));
        }

        [Test]
        public async Task Update_ShouldReturnOk_WhenPropertyExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var model = new PropertyModel { Name = "Updated House" };
            var updated = new PropertyModel { Id = id, Name = "Updated House" };

            _mockPropertyService.Setup(s => s.UpdateAsync(id, model))
                .ReturnsAsync(updated);

            // Act
            var result = await _controller.Update(id, model);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(updated));
        }

        [Test]
        public async Task Update_ShouldReturnNotFound_WhenPropertyDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var model = new PropertyModel { Name = "Nonexistent" };

            _mockPropertyService.Setup(s => s.UpdateAsync(id, model))
                .ReturnsAsync((PropertyModel)null);

            // Act
            var result = await _controller.Update(id, model);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task ChangePrice_ShouldReturnOk_WhenPropertyExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var newPrice = 250000.0;
            var property = new PropertyModel { Id = id, Price = newPrice };

            _mockPropertyService.Setup(s => s.ChangePriceAsync(id, newPrice))
                .ReturnsAsync(property);

            // Act
            var result = await _controller.ChangePrice(id, newPrice);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var returned = okResult.Value as PropertyModel;
            Assert.That(returned, Is.Not.Null);
            Assert.That(returned.Price, Is.EqualTo(newPrice));
        }

        [Test]
        public async Task ChangePrice_ShouldReturnNotFound_WhenPropertyDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var newPrice = 100000.0;

            _mockPropertyService.Setup(s => s.ChangePriceAsync(id, newPrice))
                .ReturnsAsync((PropertyModel)null);

            // Act
            var result = await _controller.ChangePrice(id, newPrice);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task GetAll_ShouldReturnOk_WithProperties()
        {
            // Arrange
            var properties = new PagedResult<PropertyModel>
            {
                TotalCount = 1,
                Items = new List<PropertyModel> { new PropertyModel { Id = Guid.NewGuid(), Name = "House 1" } }
            };

            _mockPropertyService.Setup(s => s.GetAllAsync(null, null, null, null, 1, 10))
                .ReturnsAsync(properties);

            // Act
            var result = await _controller.GetAll(null, null, null, null);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var returned = okResult.Value as PagedResult<PropertyModel>;
            Assert.That(returned, Is.Not.Null);
            Assert.That(returned.TotalCount, Is.EqualTo(1));
            Assert.That(returned.Items.First().Name, Is.EqualTo("House 1"));
        }

        [Test]
        public async Task GetById_ShouldReturnOk_WhenPropertyExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var property = new PropertyModel { Id = id, Name = "House 1" };

            _mockPropertyService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync(property);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var returned = okResult.Value as PropertyModel;
            Assert.That(returned, Is.Not.Null);
            Assert.That(returned.Id, Is.EqualTo(id));
        }

        [Test]
        public async Task GetById_ShouldReturnNotFound_WhenPropertyDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mockPropertyService.Setup(s => s.GetByIdAsync(id))
                .ReturnsAsync((PropertyModel)null);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
    }
}
