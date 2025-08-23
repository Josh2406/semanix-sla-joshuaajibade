namespace RenderingService.Domain.Models
{
    public class RenderedFormDto
    {
        public Guid Id { get; set; }

        public string TenantId { get; set; } = default!;

        public string? EntityId { get; set; }

        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public object JsonPayload { get; set; } = default!;

        public int Version { get; set; }

        public DateTime UpdatedAtUtc { get; set; }

        public string LastEvent { get; set; } = default!;
    }
}
