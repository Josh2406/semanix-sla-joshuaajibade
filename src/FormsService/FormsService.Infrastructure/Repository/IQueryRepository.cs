namespace FormsService.Infrastructure.Repository
{
    public interface IQueryRepository
    {
        Task<FormDto?> GetByIdAsync(Guid id, string tenantId, CancellationToken ct);
    }
}
