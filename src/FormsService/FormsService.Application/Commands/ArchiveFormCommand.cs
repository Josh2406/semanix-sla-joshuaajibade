namespace FormsService.Application.Commands
{
    public class ArchiveFormCommand: IRequest<BaseResponse>
    {
        [Required]
        public Guid FormId { get; set; }
    }
}
