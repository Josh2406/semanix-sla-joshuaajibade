using FormsService.Application.Models.Request;
using System.Text.Json;

namespace FormsService.Infrastructure.Handlers
{
    public class CreateFormHandler(FormsDbContext context, IValidator<CreateFormCommand> validator, IHttpContextAccessor contextAccessor) : 
            IRequestHandler<CreateFormCommand, BaseResponse<CreateFormResponse>>
    {
        private readonly FormsDbContext _context = context;
        private readonly IValidator<CreateFormCommand> _validator = validator;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;


        public async Task<BaseResponse<CreateFormResponse>> Handle(CreateFormCommand request, CancellationToken cancellationToken)
        {
            BaseResponse<CreateFormResponse> response;
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return new BaseResponse<CreateFormResponse>
                    {
                        Errors = [.. validationResult.Errors.Select(e => e.ErrorMessage)],
                        ResponseCode = 400,
                        ResponseMessage = "Bad Request"
                    };
                }

                var tenantId = _contextAccessor.HttpContext.Items[HeaderKeys.TenantId]!.ToString()!;
                string? entityId = _contextAccessor.HttpContext.Items.TryGetValue(HeaderKeys.EntityId, out var v) ? v?.ToString() : null;

                var form = new Form
                {
                    TenantId = tenantId,
                    EntityId = entityId,
                    Name = request.Name,
                    Description = request.Description,
                    JsonPayload = JsonSerializer.Serialize(request.JsonPayload, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                };

                _context.Forms.Add(form);
                await _context.SaveChangesAsync(cancellationToken);

                response = new BaseResponse<CreateFormResponse>
                {
                    Data = new CreateFormResponse { FormId = form.Id },
                    ResponseCode = 200,
                    ResponseMessage = "Form created successfully."
                };
                Log.Information("Form created successfully. FormId: {FormId}, TenantId: {TenantId}, EntityId: {EntityId}", form.Id, tenantId, entityId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating form. TenantId: {TenantId}, EntityId: {EntityId}", 
                    _contextAccessor.HttpContext.Items[HeaderKeys.TenantId], 
                    _contextAccessor.HttpContext.Items.TryGetValue(HeaderKeys.EntityId, out var v) ? v?.ToString() : null);
                response = new BaseResponse<CreateFormResponse>
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
