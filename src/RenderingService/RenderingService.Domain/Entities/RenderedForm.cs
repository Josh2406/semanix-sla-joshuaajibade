using System.ComponentModel.DataAnnotations;
namespace RenderingService.Domain.Entities
{
    public class RenderedForm
    {
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string TenantId { get; set; } = default!;

        [StringLength(100)]
        public string? EntityId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = default!;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public string JsonPayload { get; set; } = default!;

        public int Version { get; set; }

        public DateTime UpdatedAtUtc { get; set; }


        [Required, StringLength(10)]
        public string LastEvent { get; set; } = default!;
    }
}
