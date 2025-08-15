namespace Shared.Common.Events
{
    public class FormPublishedEvent
    {
        public Guid FormId { get; set; }
        public string TenantId { get; set; } = default!;
        public string? EntityId { get; set; }
        public int Version { get; set; }
        public string JsonPayload { get; set; } = default!;
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    }
}
