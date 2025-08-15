using FluentValidation;
using FormsService.Application.Commands;
using FormsService.Application.Models.Response;
using FormsService.Domain.Entities;
using FormsService.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;

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
            BaseResponse <CreateFormResponse> response;
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

                var tenantId = _contextAccessor.HttpContext.Items[""]!.ToString()!;
                string? entityId = _contextAccessor.HttpContext.Items.TryGetValue("", out var v) ? v?.ToString() : null;

                var form = new Form
                {
                    TenantId = tenantId,
                    EntityId = entityId,
                    Name = request.Name,
                    Description = request.Description,
                    JsonPayload = request.JsonPayload,
                };
                _context.Forms.Add(form);
                await _context.SaveChangesAsync(cancellationToken);

                response = new BaseResponse<CreateFormResponse>
                {
                    Data = new CreateFormResponse { FormId = form.Id },
                    ResponseCode = 200,
                    ResponseMessage = "Form created successfully"
                };
            }
            catch (Exception ex)
            {
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
