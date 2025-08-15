using RenderingService.Application.Interfaces;
using RenderingService.Domain.Entities;
using RenderingService.Infrastructure.Repository.Command;
using RenderingService.Infrastructure.Repository.Query;

namespace RenderingService.Application.Implementations
{
    public class RenderingServices(IQueryRepository query, ICommandRepository command) : IRenderingServices
    {
        private readonly IQueryRepository _query = query;
        private readonly ICommandRepository _command = command;

        public async Task<bool> CreateOrUpdateRenderedForm(RenderedForm form, CancellationToken cancellationToken)
        {
            try
            {
                return await _command.CreateOrUpdateRenderedForm(form, cancellationToken);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<RenderedForm?> GetRenderedForm(Guid id, string tenantId, CancellationToken cancellationToken)
        {
            try
            {
                return await _query.GetRenderedFormById(id, tenantId, cancellationToken);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<RenderedForm>> GetRenderedForms(string tenantId, CancellationToken cancellationToken)
        {
            try
            {
                return await _query.GetRenderedFormsByTenant(tenantId, cancellationToken);
            }
            catch (Exception ex)
            {
                return [];
            }
        }
    }
}
