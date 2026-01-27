using Api.Banco.Database.Transferencia.Application.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Transferencia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferenciaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransferenciaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Transferir([FromBody] EfetuarTransferenciaCommand command)
        {
            var resultado = await _mediator.Send(command);

            if (resultado)
                return Ok(new { Message = "Transferência enviada para processamento" });

            return BadRequest("Erro ao processar transferência");
        }
    }
}
