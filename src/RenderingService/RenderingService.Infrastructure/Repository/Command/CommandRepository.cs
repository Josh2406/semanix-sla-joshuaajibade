namespace RenderingService.Infrastructure.Repository.Command
{
    public class CommandRepository(RenderingDbContext context) : ICommandRepository
    {
        private readonly RenderingDbContext _context = context;

        public async Task<bool> CreateOrUpdateRenderedForm(RenderedForm form, CancellationToken cancellationToken)
        {
            var response = false;
            try
            {
                var existingForm = await _context.RenderedForms.FindAsync([form.Id], cancellationToken);
                if (existingForm != null)
                {
                    existingForm.EntityId = form.EntityId;
                    existingForm.JsonPayload = form.JsonPayload;
                    existingForm.Version = form.Version;
                    existingForm.UpdatedAtUtc = DateTime.UtcNow;
                    _context.RenderedForms.Update(existingForm);
                    _context.Entry(existingForm).State = EntityState.Modified;
                }
                else
                {
                    _context.RenderedForms.Add(form);
                }
                var rowsChanged = await _context.SaveChangesAsync(cancellationToken);
                response = rowsChanged > 0;
            }
            catch (Exception ex)
            {

            }
            return response;
        }
    }
}
