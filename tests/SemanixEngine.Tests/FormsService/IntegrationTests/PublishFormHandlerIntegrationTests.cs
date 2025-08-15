//using AutoMapper;
//using FormsService.Application.Commands;
//using FormsService.Application.Constants;
//using FormsService.Domain.Entities;
//using FormsService.Domain.Enums;
//using FormsService.Infrastructure.Handlers;
//using FormsService.Infrastructure.Persistence;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using Shared.Common.Contracts;
//using Shared.Common.Events;

//namespace SemanixEngine.Tests.FormsService.UnitTests
//{


//    [TestFixture]
//    public class PublishFormHandlerIntegrationTests
//    {
//        private FormsDbContext _dbContext;
//        private IHttpContextAccessor _httpContextAccessor;
//        private IMapper _mapper;
//        private TestEventBus _eventBus;

//        [TearDown]
//        public void TearDown()
//        {
//            _dbContext?.Dispose();
//        }

//        [SetUp]
//        public void Setup()
//        {
//            // In-memory EF Core database
//            var options = new DbContextOptionsBuilder<FormsDbContext>()
//                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                .Options;
//            _dbContext = new FormsDbContext(options);

//            // HttpContextAccessor with TenantId
//            _httpContextAccessor = new HttpContextAccessor
//            {
//                HttpContext = new DefaultHttpContext()
//            };
//            _httpContextAccessor.HttpContext.Items[HeaderKeys.TenantId] = "tenant-123";

//            // AutoMapper configuration
//            var mapperConfig = new MapperConfiguration(cfg =>
//            {
//                cfg.CreateMap<Form, FormPublishedEvent>()
//                    .ForMember(x => x.FormId, opt => opt.MapFrom(x => x.Id));
//                cfg.CreateMap<Form, FormUpdatedEvent>()
//                    .ForMember(x => x.FormId, opt => opt.MapFrom(x => x.Id));
//            });
//            _mapper = mapperConfig.CreateMapper();

//            // Test EventBus
//            _eventBus = new TestEventBus();
//        }

//        [Test]
//        public async Task Handle_PublishesForm_WhenDraftExists()
//        {
//            // Arrange
//            var form = new Form
//            {
//                Id = Guid.NewGuid(),
//                TenantId = "tenant-123",
//                State = FormState.Draft,
//                DateCreated = DateTime.UtcNow
//            };
//            _dbContext.Forms.Add(form);
//            await _dbContext.SaveChangesAsync();

//            var handler = new PublishFormHandler(_dbContext, _httpContextAccessor, _mapper, _eventBus);
//            var command = new PublishFormCommand { FormId = form.Id };

//            // Act
//            var response = await handler.Handle(command, CancellationToken.None);

//            // Assert
//            Assert.That(response.ResponseCode, Is.EqualTo(200));
//            Assert.That(response.ResponseMessage, Is.EqualTo("Form published successfully"));

//            var updatedForm = await _dbContext.Forms.FirstAsync();
//            Assert.That(updatedForm.State, Is.EqualTo(FormState.Published));

//            Assert.That(_eventBus.PublishedEvents.Count, Is.EqualTo(2));
//            Assert.That(_eventBus.PublishedEvents[0], Is.TypeOf<FormPublishedEvent>());
//            Assert.That(_eventBus.PublishedEvents[1], Is.TypeOf<FormUpdatedEvent>());
//        }

//        private class TestEventBus : IEventBus
//        {
//            public List<object> PublishedEvents { get; } = new();

//            public Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
//            {
//                PublishedEvents.Add(@event);
//                return Task.CompletedTask;
//            }
//        }
//    }

//}
