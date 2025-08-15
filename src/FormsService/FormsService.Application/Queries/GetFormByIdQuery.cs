using FormsService.Application.Models.Response;
using MediatR;

namespace FormsService.Application.Queries
{
    public class GetFormByIdQuery: IRequest<BaseResponse<FormDto>>
    {
        public Guid FormId { get; set; }
    }
}
