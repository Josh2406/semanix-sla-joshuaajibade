namespace RenderingService.Infrastructure.Repository.Command
{
    public interface ICommandRepository
    {
        Task<bool> CreateOrUpdateRenderedForm(RenderedForm form, CancellationToken cancellationToken);
    }
}
