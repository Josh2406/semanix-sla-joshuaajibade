namespace RenderingService.API.Controllers
{
    [Route("rendered-forms")]
    [ApiController]
    public class RenderedFormsController(IRenderingServices renderingService) : ControllerBase
    {
        private readonly IRenderingServices _renderingService = renderingService;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string tenant)
        {
            if (string.IsNullOrWhiteSpace(tenant))
                return BadRequest(new BaseResponse<List<RenderedForm>>
                {
                    Data = [],
                    ResponseCode = 400,
                    ResponseMessage = "Bad Request. Tenant must be valid"
                });

            var list = await _renderingService.GetRenderedForms(tenant, CancellationToken.None);
            return Ok(new BaseResponse<List<RenderedFormDto>>
            {
                Data = list,
                ResponseCode = 200,
                ResponseMessage = "OK"
            });
        }
    }
}
