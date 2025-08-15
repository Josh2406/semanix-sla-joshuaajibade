namespace FormsService.Infrastructure.Handlers
{
    public class UpdateFormHandler(FormsDbContext context, IEventBus eventBus, IValidator<UpdateFormCommand> validator, 
        IHttpContextAccessor accessor, IMapper mapper) : IRequestHandler<UpdateFormCommand, BaseResponse>
    {
        private readonly FormsDbContext _context = context;
        private readonly IEventBus _eventBus = eventBus;
        private readonly IValidator<UpdateFormCommand> _validator = validator;
        private readonly IHttpContextAccessor _contextAccessor = accessor;
        private readonly IMapper _mapper = mapper;

        public async Task<BaseResponse> Handle(UpdateFormCommand request, CancellationToken cancellationToken)
        {
            BaseResponse response;
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return new BaseResponse
                    {
                        Errors = [.. validationResult.Errors.Select(e => e.ErrorMessage)],
                        ResponseCode = 400,
                        ResponseMessage = "Bad Request"
                    };
                }

                var tenantId = _contextAccessor.HttpContext.Items[HeaderKeys.TenantId]!.ToString()!;
                var form = await _context.Forms.FirstOrDefaultAsync(x => x.Id == request.FormId && x.TenantId == tenantId, cancellationToken);
                if(form == null)
                {
                    return new BaseResponse
                    {
                        Errors = [ ],
                        ResponseCode = 404,
                        ResponseMessage = "Form Not Found."
                    };
                }

                if(form.State == FormState.Archived)
                {
                    return new BaseResponse
                    {
                        Errors = [],
                        ResponseCode = 400,
                        ResponseMessage = "Archived forms cannot be updated."
                    };
                }

                var previousState = form.State;

                form.Name = request.Name;
                form.Description = request.Description;
                form.JsonPayload = request.JsonPayload;
                form.Version++;
                form.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);

                var formUpdated = _mapper.Map<FormUpdatedEvent>(form);
                await _eventBus.PublishAsync(formUpdated, cancellationToken);

                response = new BaseResponse { Errors = [], ResponseCode = 200, ResponseMessage = "Form updated successfully." };
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
