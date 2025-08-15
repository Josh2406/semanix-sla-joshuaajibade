using FormsService.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FormsService.Domain.Entities
{
    public class Form
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, StringLength(100)]
        public string TenantId { get; set; } = default!;

        [StringLength(100)]
        public string? EntityId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = default!;

        [StringLength(500)]
        public string? Description { get; set; }

        public string JsonPayload { get; set; } = default!;

        public int Version { get; set; } = 1;

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? PublishedAt { get; set; }

        public FormState State { get; set; } = FormState.Draft;
    }
}
