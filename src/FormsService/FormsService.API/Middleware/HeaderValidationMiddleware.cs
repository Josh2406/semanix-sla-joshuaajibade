using System.Text.Json;

namespace FormsService.API.Middleware
{
    public class HeaderValidationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext ctx)
        {
            if (!ctx.Request.Headers.TryGetValue(HeaderKeys.TenantId, out var tenant) || string.IsNullOrWhiteSpace(tenant))
            {
                ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
                ctx.Response.ContentType = "application/json";
                var errorResponse = new
                {
                    ResponseCode = 400,
                    ResponseMessage = "Bad Request",
                    Errors = new List<string> { "Missing required header: " + HeaderKeys.TenantId }
                };

                var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                });
                await ctx.Response.WriteAsync(json);
                return;
            }
            ctx.Items[HeaderKeys.TenantId] = tenant.ToString();
            if (ctx.Request.Headers.TryGetValue(HeaderKeys.EntityId, out var entity))
                ctx.Items[HeaderKeys.EntityId] = entity.ToString();
            await _next(ctx);
        }
    }
}
