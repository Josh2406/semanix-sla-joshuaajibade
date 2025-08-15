using Dapper;
using FormsService.Application.Interfaces;
using FormsService.Application.Models.Response;
using System.Data;

namespace FormsService.Infrastructure.Repository
{
    public class QueryRepository(IDbConnection conn) : IQueryRepository
    {
        private readonly IDbConnection _conn = conn;

        public async Task<FormDto?> GetByIdAsync(Guid id, string tenantId, CancellationToken ct)
        {
            var sql = @"SELECT Id, TenantId, EntityId, Name, Description, JsonPayload, Version, State
                        FROM Forms WHERE Id = @Id AND TenantId = @TenantId";

            return await _conn.QueryFirstOrDefaultAsync<FormDto>(sql, new { Id = id, TenantId = tenantId });
        }
    }
}
