using FormsService.Application.Models.Response;

namespace FormsService.Application.Interfaces
{
    public interface IQueryRepository
    {
        Task<FormDto?> GetByIdAsync(Guid id, string tenantId, CancellationToken ct);
    }
}
