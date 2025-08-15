using FormsService.Application.Constants;
using FormsService.Application.Contracts;
using FormsService.Application.Models.Response;
using FormsService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FormsService.Infrastructure.Handlers
{
    public class GetByIdHandler(IHttpContextAccessor accessor, IQueryRepository repository) : 
            IRequestHandler<GetFormByIdQuery, BaseResponse<FormDto>>
    {
        private readonly IHttpContextAccessor _contextAccessor = accessor;
        private readonly IQueryRepository _repository = repository;

        public async Task<BaseResponse<FormDto>> Handle(GetFormByIdQuery request, CancellationToken cancellationToken)
        {
            BaseResponse<FormDto> response;
            try
            {
                var tenantId = _contextAccessor.HttpContext.Items[HeaderKeys.TenantId]!.ToString()!;
                var form = await _repository.GetByIdAsync(request.FormId, tenantId, cancellationToken);
                if(form == null)
                {
                    return new BaseResponse<FormDto>()
                    {
                        Errors = [],
                        ResponseCode = 404,
                        ResponseMessage = "Form Not Found."
                    };
                }

                response = new BaseResponse<FormDto>
                {
                    Data = form,
                    Errors = [],
                    ResponseCode = 200,
                    ResponseMessage = "Form retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                response = new BaseResponse<FormDto>
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
