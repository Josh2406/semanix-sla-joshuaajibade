using FormsService.Application.Constants;

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
                await ctx.Response.WriteAsync($"Missing required header: {HeaderKeys.TenantId}");
                return;
            }
            ctx.Items[HeaderKeys.TenantId] = tenant.ToString();
            if (ctx.Request.Headers.TryGetValue(HeaderKeys.EntityId, out var entity))
                ctx.Items[HeaderKeys.EntityId] = entity.ToString();
            await _next(ctx);
        }
    }
}
