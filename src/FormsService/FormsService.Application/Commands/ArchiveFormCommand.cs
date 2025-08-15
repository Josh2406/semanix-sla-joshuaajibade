using FormsService.Application.Models.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FormsService.Application.Commands
{
    public class ArchiveFormCommand: IRequest<BaseResponse>
    {
        [Required]
        public Guid FormId { get; set; }
    }
}
