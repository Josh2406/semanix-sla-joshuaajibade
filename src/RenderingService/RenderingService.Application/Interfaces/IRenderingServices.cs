using RenderingService.Domain.Entities;
using RenderingService.Domain.Models;
namespace RenderingService.Application.Interfaces
{
    public interface IRenderingServices
    {
        Task<bool> CreateOrUpdateRenderedForm(RenderedForm form, CancellationToken cancellationToken);
        Task<RenderedFormDto?> GetRenderedForm(Guid id, string tenantId, CancellationToken cancellationToken);
        Task<List<RenderedFormDto>> GetRenderedForms(string tenantId, CancellationToken cancellationToken);
    }
}
