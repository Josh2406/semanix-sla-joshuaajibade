namespace FormsService.Domain.Events
{
    public class FormPublishedEvent
    {
        public Guid FormId { get; set; }
        public string TenantId { get; set; } = default!;
        public string? EntityId { get; set; }
        public int Version { get; set; }
        public string JsonPayload { get; set; } = default!;
    }
}
