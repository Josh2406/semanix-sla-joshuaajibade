namespace FormsService.Infrastructure.Handlers
{
    public class PublishFormHandler(FormsDbContext context, IHttpContextAccessor accessor, IMapper mapper, IEventBus eventBus): 
            IRequestHandler<PublishFormCommand, BaseResponse>
    {
        private readonly FormsDbContext _context = context;
        private readonly IHttpContextAccessor _contextAccessor = accessor;
        private readonly IMapper _mapper = mapper;
        private readonly IEventBus _eventBus = eventBus;

        public async Task<BaseResponse> Handle(PublishFormCommand request, CancellationToken cancellationToken)
        {
            BaseResponse response;
            try
            {
                var tenantId = _contextAccessor.HttpContext.Items[HeaderKeys.TenantId]!.ToString()!;
                var form = await _context.Forms.FirstOrDefaultAsync(x => x.Id == request.FormId && x.TenantId == tenantId, cancellationToken);
                if (form == null)
                {
                    return new BaseResponse
                    {
                        Errors = [],
                        ResponseCode = 404,
                        ResponseMessage = "Form Not Found."
                    };
                }

                if (form.State != FormState.Draft)
                {
                    return new BaseResponse
                    {
                        Errors = [],
                        ResponseCode = 400,
                        ResponseMessage = "The form is either archived or already published."
                    };
                }

                form.State = FormState.Published;
                form.UpdatedAt = DateTime.UtcNow;
                form.PublishedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);

                var formPublished = _mapper.Map<FormPublishedEvent>(form);
                await _eventBus.PublishAsync(formPublished, cancellationToken);

                var formUpdated = _mapper.Map<FormUpdatedEvent>(form);
                await _eventBus.PublishAsync(formUpdated, cancellationToken);

                response = new BaseResponse { Errors = [], ResponseCode = 200, ResponseMessage = "Form published successfully" };
            }
            catch (Exception ex)
            {
                response = new BaseResponse
                {
                    Errors = [ex.Message],
                    ResponseCode = 500,
                    ResponseMessage = "An error occurred while processing your request. Please contact sysadmin."
                };
            }
            return response;
        }
    }
}
