namespace FormsService.Application.Models.Request
{
    public class SectionDto
    {
        public string SectionId { get; set; } = default!;
        public List<FieldDto> Fields { get; set; } = default!;
        public string Title { get; set; } = default!;
    }
}
