namespace SemanixEngine.Tests.FormsService.UnitTests
{
    [TestFixture]
    public class PublishFormHandlerIntegrationTests
    {

        private FormsDbContext _dbContext;
        private IHttpContextAccessor _httpContextAccessor;
        private Mock<IMapper> _mapperMock;
        private Mock<IEventBus> _eventBusMock;
        private Mock<IFormsMetrics> _formsMetricsMock;

        [SetUp]
        public void Setup()
        {
            // In-memory EF Core database
            var options = new DbContextOptionsBuilder<FormsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new FormsDbContext(options);

            // HttpContextAccessor with TenantId
            _httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext()
            };
            _httpContextAccessor.HttpContext.Items[HeaderKeys.TenantId] = "tenant-123";

            // Mock RenderingService via EventBus
            _eventBusMock = new Mock<IEventBus>();
            _eventBusMock
                .Setup(e => e.PublishAsync(It.IsAny<FormPublishedEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            _eventBusMock
                .Setup(e => e.PublishAsync(It.IsAny<FormUpdatedEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _mapperMock = new Mock<IMapper>();
            _mapperMock.Setup(Setup => Setup.Map<FormPublishedEvent>(It.IsAny<Form>()))
                .Returns(new FormPublishedEvent
                {
                    PublishedFormId = Guid.NewGuid(),
                    TenantId = "tenant-123",
                    Name = "Test Form",
                    PublishedAt = DateTime.UtcNow,
                    JsonPayload = "{\"sections\": [{\"sectionId\": \"personalInfo\", \"label\": \"Personal Information\", \"fields\": [{\"fieldId\": \"firstName\", \"label\": \"First Name\", \"type\": \"text\", \"required\": true, \"maxLength\": 50, \"order\": 1}]}]}"
                });

            _mapperMock.Setup(Setup => Setup.Map<FormUpdatedEvent>(It.IsAny<Form>()))
                .Returns(new FormUpdatedEvent
                {
                    UpdatedFormId = Guid.NewGuid(),
                    TenantId = "tenant-124",
                    Name = "Test Form-2",
                    UpdatedAt = DateTime.UtcNow,
                    JsonPayload = "{\"sections\": [{\"sectionId\": \"personalInfo\", \"label\": \"Personal Information\", \"fields\": [{\"fieldId\": \"firstName\", \"label\": \"First Name\", \"type\": \"text\", \"required\": true, \"maxLength\": 50, \"order\": 1}]}]}"
                });

            _formsMetricsMock = new Mock<IFormsMetrics>();
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Dispose();
        }

        [Test]
        public async Task Handle_WhenDraftFormExists_PublishesToRenderingService()
        {
            // Arrange
            var form = new Form
            {
                Id = Guid.NewGuid(),
                TenantId = "tenant-123",
                State = FormState.Draft,
                DateCreated = DateTime.UtcNow,
                Name = "Test Form",
                JsonPayload = "{\"sections\": [{\"sectionId\": \"personalInfo\", \"label\": \"Personal Information\", \"fields\": [{\"fieldId\": \"firstName\", \"label\": \"First Name\", \"type\": \"text\", \"required\": true, \"maxLength\": 50, \"order\": 1}]}]}"
            };
            _dbContext.Forms.Add(form);
            await _dbContext.SaveChangesAsync();

            var handler = new PublishFormHandler(_dbContext, _httpContextAccessor, _mapperMock.Object, _eventBusMock.Object, _formsMetricsMock.Object);
            var command = new PublishFormCommand { FormId = form.Id };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.ResponseCode, Is.EqualTo(200));
            _eventBusMock.Verify(e => e.PublishAsync(It.IsAny<FormPublishedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
            _eventBusMock.Verify(e => e.PublishAsync(It.IsAny<FormUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
