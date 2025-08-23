namespace SemanixEngine.Tests.FormsService.UnitTests
{
    [TestFixture]
    public class PublishFormHandlerTests
    {
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IEventBus> _eventBusMock;
        private FormsDbContext _dbContext;
        private PublishFormHandler _handler;
        private Mock<IFormsMetrics> _metricsMock;

        private const string TenantId = "tenant-123";

        [SetUp]
        public void Setup()
        {
            // Setup in-memory EF Core DB
            var options = new DbContextOptionsBuilder<FormsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new FormsDbContext(options);

            // Setup HttpContextAccessor
            var httpContext = new DefaultHttpContext();
            httpContext.Items[HeaderKeys.TenantId] = TenantId;
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            // Setup mocks
            _mapperMock = new Mock<IMapper>();
            _eventBusMock = new Mock<IEventBus>();
            _metricsMock = new Mock<IFormsMetrics>();

            _handler = new PublishFormHandler(_dbContext, _httpContextAccessorMock.Object, _mapperMock.Object, _eventBusMock.Object, 
                _metricsMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Dispose();
        }

        [Test]
        public async Task Handle_FormNotFound_Returns404()
        {
            // Arrange
            var command = new PublishFormCommand { FormId = Guid.Empty };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.ResponseCode, Is.EqualTo(404));
            Assert.That(result.ResponseMessage, Is.EqualTo("Form Not Found."));
        }

        [Test]
        public async Task Handle_FormNotInDraft_Returns400()
        {
            // Arrange
            var form = new Form
            {
                Id = Guid.NewGuid(),
                TenantId = TenantId,
                State = FormState.Published,
                JsonPayload = "{\"sections\": [{\"sectionId\": \"personalInfo\", \"label\": \"Personal Information\", \"fields\": [{\"fieldId\": \"firstName\", \"label\": \"First Name\", \"type\": \"text\", \"required\": true, \"maxLength\": 50, \"order\": 1}]}]}\"",
                Name = "Test Form"
            };
            _dbContext.Forms.Add(form);
            await _dbContext.SaveChangesAsync();

            var command = new PublishFormCommand { FormId = form.Id };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.ResponseCode, Is.EqualTo(400));
            Assert.That(result.ResponseMessage, Is.EqualTo("The form is either archived or already published."));
        }

        [Test]
        public async Task Handle_DraftForm_PublishesEventsAndReturns200()
        {
            // Arrange
            var form = new Form
            {
                Id = Guid.NewGuid(),
                TenantId = TenantId,
                State = FormState.Draft,
                Name = "Test Form-2",
                JsonPayload = "{\"sections\": [{\"sectionId\": \"personalInfo\", \"label\": \"Personal Information\", \"fields\": [{\"fieldId\": \"firstName\", \"label\": \"First Name\", \"type\": \"text\", \"required\": true, \"maxLength\": 50, \"order\": 1}]}]}\""
            };
            _dbContext.Forms.Add(form);
            await _dbContext.SaveChangesAsync();

            var formPublishedEvent = new FormPublishedEvent();
            var formUpdatedEvent = new FormUpdatedEvent();

            _mapperMock.Setup(m => m.Map<FormPublishedEvent>(It.IsAny<Form>())).Returns(formPublishedEvent);
            _mapperMock.Setup(m => m.Map<FormUpdatedEvent>(It.IsAny<Form>())).Returns(formUpdatedEvent);

            var command = new PublishFormCommand { FormId = form.Id };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.ResponseCode, Is.EqualTo(200));
            Assert.That(result.ResponseMessage, Is.EqualTo("Form published successfully"));

            _eventBusMock.Verify(e => e.PublishAsync(formPublishedEvent, It.IsAny<CancellationToken>()), Times.Once);
            _eventBusMock.Verify(e => e.PublishAsync(formUpdatedEvent, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Handle_ExceptionThrown_Returns500()
        {
            // Arrange
            var form = new Form
            {
                Id = Guid.NewGuid(),
                TenantId = TenantId,
                State = FormState.Draft,
                Name = "Test Form-3",
                JsonPayload = "{\"sections\": [{\"sectionId\": \"personalInfo\", \"label\": \"Personal Information\", \"fields\": [{\"fieldId\": \"firstName\", \"label\": \"First Name\", \"type\": \"text\", \"required\": true, \"maxLength\": 50, \"order\": 1}]}]}\""
            };
            _dbContext.Forms.Add(form);
            await _dbContext.SaveChangesAsync();

            _mapperMock.Setup(m => m.Map<FormPublishedEvent>(It.IsAny<Form>())).Throws(new Exception("Mapping failed"));

            var command = new PublishFormCommand { FormId = form.Id };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.ResponseCode, Is.EqualTo(500));
            Assert.That(result.Errors, Does.Contain("Mapping failed"));
        }
    }
}
