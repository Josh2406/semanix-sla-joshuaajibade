using FormsService.Domain.Enums;

namespace FormsService.Domain.Entities
{
    public class Form
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string TenantId { get; set; } = default!;
        public string? EntityId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string JsonPayload { get; set; } = default!;
        public int Version { get; set; } = 1;
        public FormState State { get; set; } = FormState.Draft;
    }
}
