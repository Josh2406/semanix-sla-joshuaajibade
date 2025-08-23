using System.Text.Json;

namespace FormsService.Infrastructure.Repository
{
    public class QueryRepository(IDbConnection conn) : IQueryRepository
    {
        private readonly IDbConnection _conn = conn;

        public async Task<FormDto?> GetByIdAsync(Guid id, string tenantId, CancellationToken ct)
        {
            var sql = @"SELECT * FROM Forms WHERE Id = @Id AND TenantId = @TenantId";
            var result = await _conn.QueryFirstOrDefaultAsync<Form>(sql, new { Id = id, TenantId = tenantId });
            var payload = result?.JsonPayload == null ? null : 
                JsonSerializer.Deserialize<object>(result.JsonPayload, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result == null ? null : new FormDto
            {
                Id = result.Id,
                Description = result.Description,
                State = result.State.ToString(),
                JsonPayload = payload!,
                EntityId = result.EntityId,
                TenantId = tenantId,
                Version = result.Version,
                Name = result.Name
            };
        }
    }
}
