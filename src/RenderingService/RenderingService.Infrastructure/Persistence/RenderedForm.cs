namespace RenderingService.Infrastructure.Persistence
{
    public class RenderedForm
    {
        public Guid Id { get; set; }
        public string TenantId { get; set; } = default!;
        public string? EntityId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string JsonPayload { get; set; } = default!;
        public int Version { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
    }
}
