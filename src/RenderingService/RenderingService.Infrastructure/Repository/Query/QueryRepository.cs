using AutoMapper;
using RenderingService.Domain.Models;

namespace RenderingService.Infrastructure.Repository.Query
{
    public class QueryRepository(IDbConnection conn, IMapper mapper) : IQueryRepository
    {
        private readonly IDbConnection _conn = conn;
        private readonly IMapper _mapper = mapper;

        public async Task<RenderedFormDto?> GetRenderedFormById(Guid id, string tenantId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT TOP 1 * FROM RenderedForms WHERE Id = @Id AND TenantId = @TenantId";
            var result = await _conn.QueryFirstOrDefaultAsync<RenderedForm>(sql, new { Id = id, TenantId = tenantId });
            return result == null ? null : _mapper.Map<RenderedFormDto>(result);
        }

        public async Task<List<RenderedFormDto>> GetRenderedFormsByTenant(string tenantId, CancellationToken cancellationToken)
        {
            var sql = @"SELECT * FROM RenderedForms WHERE TenantId = @TenantId";
            var result = (await _conn.QueryAsync<RenderedForm>(sql, new { TenantId = tenantId })).AsList();
            if(result != null)
            {
                result = [.. result.OrderByDescending(x=>x.UpdatedAtUtc)];
            }

            return result == null ? [] : _mapper.Map<List<RenderedFormDto>>(result);
        }
    }
}
