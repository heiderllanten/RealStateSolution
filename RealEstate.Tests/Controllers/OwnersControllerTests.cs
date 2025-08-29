using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RealEstate.Api.Controllers;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models;

namespace RealEstate.Tests.Controllers
{
    [TestFixture]
    public class OwnersControllerTests
    {
        private Mock<IOwnerService> _mockOwnerService;
        private OwnersController _controller;

        [SetUp]
        public void Setup()
        {
            _mockOwnerService = new Mock<IOwnerService>();
            _controller = new OwnersController(_mockOwnerService.Object,
                Mock.Of<Microsoft.Extensions.Logging.ILogger<OwnersController>>());
        }

        [Test]
        public async Task Create_ShouldReturnCreatedResult_WhenOwnerIsValid()
        {
            // Arrange
            var model = new OwnerModel { Name = "Owner1" };
            var createdOwner = new OwnerModel { Id = Guid.NewGuid(), Name = "Owner1" };

            _mockOwnerService
                .Setup(s => s.CreateAsync(model))
                .ReturnsAsync(createdOwner);

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.That(result, Is.TypeOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult.ActionName, Is.EqualTo(nameof(_controller.GetById)));
            Assert.That(createdResult.Value, Is.EqualTo(createdOwner));
        }

        [Test]
        public async Task GetAll_ShouldReturnOkResult_WithOwners()
        {
            // Arrange
            var owners = new PagedResult<OwnerModel>
            {
                TotalCount = 1,
                Items = new List<OwnerModel>
                {
                    new OwnerModel { Id = Guid.NewGuid(), Name = "Owner1" }
                }
            };

            _mockOwnerService
                .Setup(s => s.GetAllAsync(1, 10))
                .ReturnsAsync(owners);

            // Act
            var result = await _controller.GetAll(1, 10);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var returnedOwners = okResult.Value as PagedResult<OwnerModel>;
            Assert.That(returnedOwners, Is.Not.Null);
            Assert.That(returnedOwners.TotalCount, Is.EqualTo(1));
            Assert.That(returnedOwners.Items.First().Name, Is.EqualTo("Owner1"));
        }

        [Test]
        public async Task GetById_ShouldReturnOk_WhenOwnerExists()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var owner = new OwnerModel { Id = ownerId, Name = "Owner1" };

            _mockOwnerService
                .Setup(s => s.GetByIdAsync(ownerId))
                .ReturnsAsync(owner);

            // Act
            var result = await _controller.GetById(ownerId);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var returnedOwner = okResult.Value as OwnerModel;
            Assert.That(returnedOwner, Is.Not.Null);
            Assert.That(returnedOwner.Id, Is.EqualTo(ownerId));
        }

        [Test]
        public async Task GetById_ShouldReturnNotFound_WhenOwnerDoesNotExist()
        {
            // Arrange
            var ownerId = Guid.NewGuid();

            _mockOwnerService
                .Setup(s => s.GetByIdAsync(ownerId))
                .ReturnsAsync((OwnerModel)null);

            // Act
            var result = await _controller.GetById(ownerId);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
    }
}
