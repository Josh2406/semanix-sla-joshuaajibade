namespace FormsService.API.Controllers
{
    [Route("api/v1/forms")]
    [ApiController]
    public class FormsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFormCommand command)
        {
            var response = await _mediator.Send(command);
            return response.ResponseCode == 200 ? Ok(response) : response.ResponseCode == 500 ? StatusCode(500, response) :
                BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFormCommand command)
        {
            command.FormId = id;
            var response = await _mediator.Send(command);
            return response.ResponseCode == 200 ? Ok(response) : response.ResponseCode == 500 ? StatusCode(500, response) :
                BadRequest(response);
        }

        [HttpPost("{id}/publish")]
        public async Task<IActionResult> Publish(Guid id)
        {
            var response = await _mediator.Send(new PublishFormCommand { FormId = id});
            return response.ResponseCode == 200 ? Ok(response) : response.ResponseCode == 500 ? StatusCode(500, response) :
                BadRequest(response);
        }

        [HttpPost("{id}/archive")]
        public async Task<IActionResult> Archive(Guid id)
        {
            var response = await _mediator.Send(new ArchiveFormCommand { FormId = id });
            return response.ResponseCode == 200 ? Ok(response) : response.ResponseCode == 500 ? StatusCode(500, response) :
                BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await _mediator.Send(new GetFormByIdQuery { FormId = id });
            return response.ResponseCode == 200 ? Ok(response) : response.ResponseCode == 500 ? StatusCode(500, response) :
                BadRequest(response);
        }
    }
}
