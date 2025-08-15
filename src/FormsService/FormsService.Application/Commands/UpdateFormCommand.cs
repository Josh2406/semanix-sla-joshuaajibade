using FormsService.Application.Models.Response;
using MediatR;

namespace FormsService.Application.Commands
{
    public class UpdateFormCommand: IRequest<BaseResponse>
    {
        public Guid FormId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string JsonPayload { get; set; } = default!;
    }
}
