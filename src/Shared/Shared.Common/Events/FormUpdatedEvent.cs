namespace Shared.Common.Events
{
    public class FormUpdatedEvent
    {
        public Guid FormId { get; set; }
        public string TenantId { get; set; } = default!;
        public string? EntityId { get; set; }
        public int Version { get; set; }
        public string JsonPayload { get; set; } = default!;
        public string? Description { get; set; }
        public string Name { get; set; } = default!;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
