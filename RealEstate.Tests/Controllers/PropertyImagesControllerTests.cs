using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RealEstate.Api.Controllers;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models;

namespace RealEstate.Tests.Controllers
{
    [TestFixture]
    public class PropertyImagesControllerTests
    {
        private Mock<IPropertyImageService> _mockService;
        private PropertyImagesController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IPropertyImageService>();
            _controller = new PropertyImagesController(_mockService.Object,
                Mock.Of<Microsoft.Extensions.Logging.ILogger<PropertyImagesController>>());
        }

        [Test]
        public async Task AddImage_ShouldReturnOk_WhenPropertyExists()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            var mockFile = new Mock<IFormFile>();
            var content = "Fake image content";
            var fileName = "test.jpg";

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            mockFile.Setup(f => f.OpenReadStream()).Returns(stream);
            mockFile.Setup(f => f.FileName).Returns(fileName);
            mockFile.Setup(f => f.Length).Returns(stream.Length);

            var resultModel = new PropertyImageModel { Id = Guid.NewGuid(), Url = "/images/properties/test.jpg" };
            _mockService.Setup(s => s.AddAsync(propertyId, It.IsAny<string>()))
                .ReturnsAsync(resultModel);

            // Act
            var result = await _controller.AddImage(propertyId, mockFile.Object);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var returned = okResult.Value as PropertyImageModel;
            Assert.That(returned, Is.Not.Null);
            Assert.That(returned.Id, Is.EqualTo(resultModel.Id));
        }

        [Test]
        public async Task AddImage_ShouldReturnBadRequest_WhenFileIsNull()
        {
            // Act
            var result = await _controller.AddImage(Guid.NewGuid(), null);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UpdateImage_ShouldReturnOk_WhenImageExists()
        {
            // Arrange
            var imageId = Guid.NewGuid();
            var newUrl = "/images/properties/new.jpg";
            var resultModel = new PropertyImageModel { Id = imageId, Url = newUrl };

            _mockService.Setup(s => s.UpdateUrlAsync(imageId, newUrl))
                .ReturnsAsync(resultModel);

            // Act
            var result = await _controller.UpdateImage(imageId, newUrl);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var returned = okResult.Value as PropertyImageModel;
            Assert.That(returned, Is.Not.Null);
            Assert.That(returned.Url, Is.EqualTo(newUrl));
        }

        [Test]
        public async Task UpdateImage_ShouldReturnNotFound_WhenImageDoesNotExist()
        {
            // Arrange
            var imageId = Guid.NewGuid();
            var newUrl = "/images/properties/new.jpg";

            _mockService.Setup(s => s.UpdateUrlAsync(imageId, newUrl))
                .ReturnsAsync((PropertyImageModel)null);

            // Act
            var result = await _controller.UpdateImage(imageId, newUrl);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task DeleteImage_ShouldReturnNoContent_WhenDeleted()
        {
            // Arrange
            var imageId = Guid.NewGuid();
            var image = new PropertyImageModel
            {
                Id = imageId,
                Url = "/images/properties/test.jpg"
            };

            // Mockear GetByIdAsync para devolver la imagen
            _mockService.Setup(s => s.GetByIdAsync(imageId))
                .ReturnsAsync(image);

            // Mockear RemoveAsync para devolver true
            _mockService.Setup(s => s.RemoveAsync(imageId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteImage(imageId);

            // Assert
            Assert.That(result, Is.TypeOf<NoContentResult>());

            // Verificar que los métodos del servicio fueron llamados
            _mockService.Verify(s => s.GetByIdAsync(imageId), Times.Once);
            _mockService.Verify(s => s.RemoveAsync(imageId), Times.Once);
        }


        [Test]
        public async Task DeleteImage_ShouldReturnNotFound_WhenImageDoesNotExist()
        {
            // Arrange
            var imageId = Guid.NewGuid();
            _mockService.Setup(s => s.RemoveAsync(imageId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteImage(imageId);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetByProperty_ShouldReturnOk_WithImages()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            var images = new List<PropertyImageModel>
            {
                new PropertyImageModel { Id = Guid.NewGuid(), Url = "/images/properties/1.jpg" }
            };

            _mockService.Setup(s => s.GetByPropertyAsync(propertyId))
                .ReturnsAsync(images);

            // Act
            var result = await _controller.GetByProperty(propertyId);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var returned = okResult.Value as List<PropertyImageModel>;
            Assert.That(returned, Is.Not.Null);
            Assert.That(returned.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetById_ShouldReturnOk_WhenImageExists()
        {
            // Arrange
            var imageId = Guid.NewGuid();
            var image = new PropertyImageModel { Id = imageId, Url = "/images/properties/1.jpg" };

            _mockService.Setup(s => s.GetByIdAsync(imageId))
                .ReturnsAsync(image);

            // Act
            var result = await _controller.GetById(imageId);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var returned = okResult.Value as PropertyImageModel;
            Assert.That(returned, Is.Not.Null);
            Assert.That(returned.Id, Is.EqualTo(imageId));
        }

        [Test]
        public async Task GetById_ShouldReturnNotFound_WhenImageDoesNotExist()
        {
            // Arrange
            var imageId = Guid.NewGuid();
            _mockService.Setup(s => s.GetByIdAsync(imageId)).ReturnsAsync((PropertyImageModel)null);

            // Act
            var result = await _controller.GetById(imageId);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
    }
}
