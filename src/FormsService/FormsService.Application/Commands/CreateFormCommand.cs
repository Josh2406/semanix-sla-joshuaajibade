using FormsService.Application.Models.Response;
namespace FormsService.Application.Commands
{
    public class CreateFormCommand: IRequest<BaseResponse<CreateFormResponse>>
    {
        public string Name { get; set; } = default!;
        public string? Description {get; set;}
        public object JsonPayload {get; set;} = default!;
    }
}
