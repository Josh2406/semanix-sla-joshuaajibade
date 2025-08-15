namespace FormsService.Application.Models.Request
{
    public class FieldDto
    {
        public string FieldId { get; set; } = default!;
        public string Label { get; set; } = default!;
        public string Type { get; set; } = default!;
        public int Order { get; set; }
        public bool? Required { get; set; }
        public int? MaxLength { get; set; }
    }
}
