namespace FormsService.Application.Models.Response
{
    public class FormDto
    {
        public Guid Id { get; set; }
        public string TenantId { get; set; } = default!;
        public string? EntityId { get; set; }
        public string? Description { get; set; }
        public string JsonPayload { get; set; } = default!;
        public int Version { get; set; }
        public FormState State { get; set; }
    }
}
