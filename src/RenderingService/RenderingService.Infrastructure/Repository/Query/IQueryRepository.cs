using RenderingService.Domain.Models;

namespace RenderingService.Infrastructure.Repository.Query
{
    public interface IQueryRepository
    {
        Task<RenderedFormDto?> GetRenderedFormById(Guid id, string tenantId, CancellationToken cancellationToken);
        Task<List<RenderedFormDto>> GetRenderedFormsByTenant(string tenantId, CancellationToken cancellationToken);
    }
}
