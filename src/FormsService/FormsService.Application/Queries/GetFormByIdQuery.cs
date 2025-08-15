using FormsService.Application.Models.Response;
using MediatR;

namespace FormsService.Application.Queries
{
    public class GetFormByIdQuery: IRequest<FormDto>
    {
        public Guid FormId { get; set; }
    }
}
