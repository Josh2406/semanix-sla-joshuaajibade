using RenderingService.Application.Interfaces;
using RenderingService.Domain.Entities;
using RenderingService.Domain.Models;
using RenderingService.Infrastructure.Repository.Command;
using RenderingService.Infrastructure.Repository.Query;
using Serilog;

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
                Log.Error(ex, "Error creating or updating rendered form. FormId: {FormId}, TenantId: {TenantId}", form.Id, form.TenantId);
                return false;
            }
        }

        public async Task<RenderedFormDto?> GetRenderedForm(Guid id, string tenantId, CancellationToken cancellationToken)
        {
            try
            {
                return await _query.GetRenderedFormById(id, tenantId, cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving rendered form. FormId: {FormId}, TenantId: {TenantId}", id, tenantId);  
                return null;
            }
        }

        public async Task<List<RenderedFormDto>> GetRenderedForms(string tenantId, CancellationToken cancellationToken)
        {
            try
            {
                return await _query.GetRenderedFormsByTenant(tenantId, cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Error("Error retrieving rendered forms for tenant {TenantId}: {Message}", tenantId, ex.Message);
                return [];
            }
        }
    }
}
