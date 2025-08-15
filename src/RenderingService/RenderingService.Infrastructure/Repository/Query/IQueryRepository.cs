namespace RenderingService.Infrastructure.Repository.Query
{
    public interface IQueryRepository
    {
        Task<RenderedForm?> GetRenderedFormById(Guid id, string tenantId, CancellationToken cancellationToken);
        Task<List<RenderedForm>> GetRenderedFormsByTenant(string tenantId, CancellationToken cancellationToken);
    }
}
