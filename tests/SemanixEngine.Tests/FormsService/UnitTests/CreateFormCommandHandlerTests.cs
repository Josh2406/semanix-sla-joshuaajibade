using FluentValidation;
using FluentValidation.Results;
using FormsService.Application.Commands;
using FormsService.Application.Constants;
using FormsService.Infrastructure.Handlers;
using FormsService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace SemanixEngine.Tests.FormsService.UnitTests
{
    [TestFixture]
    public class CreateFormHandlerTests
    {
        private FormsDbContext _dbContext;
        private Mock<IValidator<CreateFormCommand>> _validatorMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<FormsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new FormsDbContext(options);

            _validatorMock = new Mock<IValidator<CreateFormCommand>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            // Setup HttpContext with required headers
            var httpContext = new DefaultHttpContext();
            httpContext.Items[HeaderKeys.TenantId] = "tenant-123";
            httpContext.Items[HeaderKeys.EntityId] = "entity-456";
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Dispose();
        }

        [Test]
        public async Task Handle_ShouldReturn200_WhenValidationPasses()
        {
            // Arrange
            var command = new CreateFormCommand
            {
                Name = "Test Form",
                Description = "Test description",
                JsonPayload = "{ \"fields\": [] }"
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var handler = new CreateFormHandler(_dbContext, _validatorMock.Object, _httpContextAccessorMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.ResponseCode, Is.EqualTo(200));
            Assert.That(result.Data.FormId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(_dbContext.Forms.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Handle_ShouldReturn400_WhenValidationFails()
        {
            // Arrange
            var command = new CreateFormCommand
            {
                Name = "",
                Description = "",
                JsonPayload = ""
            };

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required")
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(validationFailures));

            var handler = new CreateFormHandler(_dbContext, _validatorMock.Object, _httpContextAccessorMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.ResponseCode, Is.EqualTo(400));
            Assert.That(result.Errors, Contains.Item("Name is required"));
            Assert.That(_dbContext.Forms.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task Handle_ShouldReturn500_WhenExceptionThrown()
        {
            // Arrange
            var command = new CreateFormCommand
            {
                Name = "Test",
                Description = "Desc",
                JsonPayload = "{}"
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<CreateFormCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Something bad happened"));

            var handler = new CreateFormHandler(_dbContext, _validatorMock.Object, _httpContextAccessorMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.ResponseCode, Is.EqualTo(500));
            Assert.That(result.Errors.First(), Is.EqualTo("Something bad happened"));
        }
    }
}
