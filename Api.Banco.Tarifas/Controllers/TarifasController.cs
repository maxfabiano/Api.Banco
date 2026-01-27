using Api.Banco.Database.Tarifas.Application.Command;
using Api.Banco.Database.Tarifas.Application.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Banco.Tarifas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarifasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TarifasController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> PostTarifa([FromBody] CreateTarifaCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetTarifasQuery());
            return Ok(result);
        }
    }
}