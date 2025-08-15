namespace RenderingService.API.Controllers
{
    [Route("events/forms")]
    [ApiController]
    public class EventsController(IRenderingServices renderingService) : ControllerBase
    {
        private readonly IRenderingServices _renderingService = renderingService;
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        [HttpPost]
        public async Task<IActionResult> Receive([FromBody] object @event)
        {
            var eventJson = @event.ToString();
            if (string.IsNullOrEmpty(eventJson)) return BadRequest();

            RenderedForm form = null;
            var published = JsonSerializer.Deserialize<FormPublishedEvent>(eventJson, _jsonSerializerOptions);
            if (published != null && published.PublishedAt != DateTime.MinValue)
            {
                form = new RenderedForm
                {
                    Description = published.Description,
                    EntityId = published.EntityId,
                    Id = published.FormId,
                    JsonPayload = published.JsonPayload,
                    LastEvent = EventActions.PUBLISH,
                    Name = published.Name,
                    TenantId = published.TenantId,
                    UpdatedAtUtc = DateTime.UtcNow,
                    Version = published.Version
                };
            }
            else
            {
                var updated = JsonSerializer.Deserialize<FormUpdatedEvent>(eventJson, _jsonSerializerOptions);
                if (updated != null && updated.UpdatedAt != DateTime.MinValue)
                {
                    form = new RenderedForm
                    {
                        Description = updated.Description,
                        EntityId = updated.EntityId,
                        Id = updated.FormId,
                        JsonPayload = updated.JsonPayload,
                        LastEvent = EventActions.UPDATE,
                        Name = updated.Name,
                        TenantId = updated.TenantId,
                        UpdatedAtUtc = DateTime.UtcNow,
                        Version = updated.Version
                    };
                }
            }

            if(form == null)
            {
                return BadRequest("Unknown Event Received");
            }

            await _renderingService.CreateOrUpdateRenderedForm(form, default);
            return Ok();
        }
    }
}
