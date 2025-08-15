namespace FormsService.Application.Commands
{
    public class PublishFormCommand: IRequest<BaseResponse>
    {
        [Required]
        public Guid FormId { get; set; }
    }
}
