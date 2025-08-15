using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using FormsService.Application.Commands;
using FormsService.Application.Constants;
using FormsService.Domain.Entities;
using FormsService.Domain.Enums;
using FormsService.Infrastructure.Handlers;
using FormsService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shared.Common.Contracts;
using Shared.Common.Events;

namespace SemanixEngine.Tests.FormsService.UnitTests
{
    [TestFixture]
    public class UpdateFormHandlerTests
    {
        private Mock<IEventBus> _eventBusMock;
        private Mock<IValidator<UpdateFormCommand>> _validatorMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IMapper> _mapperMock;
        private FormsDbContext _dbContext;
        private UpdateFormHandler _handler;

        private const string TenantId = "tenant-123";

        [SetUp]
        public void Setup()
        {
            // EF Core in-memory DB
            var options = new DbContextOptionsBuilder<FormsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new FormsDbContext(options);

            // HttpContextAccessor with TenantId
            var httpContext = new DefaultHttpContext();
            httpContext.Items[HeaderKeys.TenantId] = TenantId;
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            // Mocks
            _eventBusMock = new Mock<IEventBus>();
            _validatorMock = new Mock<IValidator<UpdateFormCommand>>();
            _mapperMock = new Mock<IMapper>();

            _handler = new UpdateFormHandler(
                _dbContext,
                _eventBusMock.Object,
                _validatorMock.Object,
                _httpContextAccessorMock.Object,
                _mapperMock.Object
            );
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Dispose();
        }

        [Test]
        public async Task Handle_InvalidValidation_Returns400()
        {
            // Arrange
            var command = new UpdateFormCommand { FormId = Guid.NewGuid() };
            var failures = new List<ValidationFailure> { new ValidationFailure("Name", "Name is required") };

            _validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(failures));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.ResponseCode, Is.EqualTo(400));
            Assert.That(result.ResponseMessage, Is.EqualTo("Bad Request"));
            Assert.That(result.Errors, Contains.Item("Name is required"));
        }

        [Test]
        public async Task Handle_FormNotFound_Returns404()
        {
            // Arrange
            var command = new UpdateFormCommand { FormId = Guid.NewGuid() };

            _validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult()); // Valid

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.ResponseCode, Is.EqualTo(404));
            Assert.That(result.ResponseMessage, Is.EqualTo("Form Not Found."));
        }

        [Test]
        public async Task Handle_ArchivedForm_Returns400()
        {
            // Arrange
            var form = new Form
            {
                Id = Guid.NewGuid(),
                TenantId = TenantId,
                State = FormState.Archived,
                Name = "Test Form-3",
                JsonPayload = "{\"sections\": [{\"sectionId\": \"personalInfo\", \"label\": \"Personal Information\", \"fields\": [{\"fieldId\": \"firstName\", \"label\": \"First Name\", \"type\": \"text\", \"required\": true, \"maxLength\": 50, \"order\": 1}]}]}\""
            };
            _dbContext.Forms.Add(form);
            await _dbContext.SaveChangesAsync();

            var command = new UpdateFormCommand { FormId = form.Id };

            _validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult()); // Valid

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.ResponseCode, Is.EqualTo(400));
            Assert.That(result.ResponseMessage, Is.EqualTo("Archived forms cannot be updated."));
        }

        [Test]
        public async Task Handle_ValidUpdate_UpdatesFormAndPublishesEvent()
        {
            // Arrange
            var form = new Form
            {
                Id = Guid.NewGuid(),
                TenantId = TenantId,
                State = FormState.Draft,
                Name = "Old Name",
                Description = "Old Desc",
                JsonPayload = "{}",
                Version = 1
            };
            _dbContext.Forms.Add(form);
            await _dbContext.SaveChangesAsync();

            var command = new UpdateFormCommand
            {
                FormId = form.Id,
                Name = "New Name",
                Description = "New Desc",
                JsonPayload = "{ updated: true }"
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult()); // Valid

            var updatedEvent = new FormUpdatedEvent();
            _mapperMock.Setup(m => m.Map<FormUpdatedEvent>(It.IsAny<Form>())).Returns(updatedEvent);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.ResponseCode, Is.EqualTo(200));
            Assert.That(result.ResponseMessage, Is.EqualTo("Form updated successfully."));

            var updatedForm = await _dbContext.Forms.FirstAsync(f => f.Id == form.Id);
            Assert.That(updatedForm.Name, Is.EqualTo("New Name"));
            Assert.That(updatedForm.Description, Is.EqualTo("New Desc"));
            Assert.That(updatedForm.JsonPayload, Is.EqualTo("{ updated: true }"));
            Assert.That(updatedForm.Version, Is.EqualTo(2)); // Incremented

            _eventBusMock.Verify(e => e.PublishAsync(updatedEvent, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Handle_ExceptionThrown_Returns500()
        {
            // Arrange
            var command = new UpdateFormCommand { FormId = Guid.NewGuid() };

            _validatorMock
                .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Validation service unavailable"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.ResponseCode, Is.EqualTo(500));
            Assert.That(result.Errors, Contains.Item("Validation service unavailable"));
        }
    }
}
