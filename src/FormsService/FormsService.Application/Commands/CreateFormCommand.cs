using FormsService.Application.Models.Response;
namespace FormsService.Application.Commands
{
    public class CreateFormCommand: IRequest<BaseResponse<CreateFormResponse>>
    {
        public string Name { get; set; } = default!;
        public string? Description {get; set;}
        public string JsonPayload {get; set;} = default!;
    }
}
