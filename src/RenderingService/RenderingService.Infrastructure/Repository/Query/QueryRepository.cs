namespace RenderingService.Infrastructure.Repository.Query
{
    public class QueryRepository(IDbConnection conn) : IQueryRepository
    {
        private readonly IDbConnection _conn = conn;

        public async Task<RenderedForm?> GetRenderedFormById(Guid id, string tenantId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT TOP 1 * FROM RenderedForms WHERE Id = @Id AND TenantId = @TenantId";
            return await _conn.QueryFirstOrDefaultAsync<RenderedForm>(sql, new { Id = id, TenantId = tenantId });
        }

        public async Task<List<RenderedForm>> GetRenderedFormsByTenant(string tenantId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT * FROM RenderedForms WHERE TenantId = @TenantId";
            var result = (await _conn.QueryAsync<RenderedForm>(sql, new { TenantId = tenantId })).AsList();
            if(result != null)
            {
                result = [.. result.OrderByDescending(x=>x.UpdatedAtUtc)];
            }
            return result ?? [];
        }
    }
}
