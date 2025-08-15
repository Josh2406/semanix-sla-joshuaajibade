using RenderingService.Domain.Entities;
namespace RenderingService.Application.Interfaces
{
    public interface IRenderingServices
    {
        Task<bool> CreateOrUpdateRenderedForm(RenderedForm form, CancellationToken cancellationToken);
        Task<RenderedForm?> GetRenderedForm(Guid id, string tenantId, CancellationToken cancellationToken);
        Task<List<RenderedForm>> GetRenderedForms(string tenantId, CancellationToken cancellationToken);
    }
}
