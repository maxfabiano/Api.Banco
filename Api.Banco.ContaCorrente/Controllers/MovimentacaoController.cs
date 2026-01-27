
using Api.Banco.ContaCorrente.DTOs;
using Api.Banco.Database.ContaCorrente.Application.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Security.Claims;
namespace Api.Banco.ContaCorrente.Controllers
{

    namespace Api.Banco.ContaCorrente.Api.Controllers
    {
        [ApiController]
        [Route("api/conta")]
        [Authorize]
        public class MovimentacaoController : ControllerBase
        {
            private readonly IMediator _mediator;

            public MovimentacaoController(IMediator mediator) => _mediator = mediator;

            [HttpPost("deposito")]
            public async Task<IActionResult> Deposito([FromBody] MovimentacaoRequest request)
            {
                if(request.Valor <= 0)
                {
                    return BadRequest("O valor do depósito deve ser maior que zero.");
                }
                var idUsuario = int.Parse(User.FindFirst("IdUsuario")?.Value); 
                if (request.IdUsuario != null)
                {
                    idUsuario = request.IdUsuario.Value;
                }
                var tipo1 = new CreateTipoMovimentoCommand(1, "Depósito");
                var tipo2 = new CreateTipoMovimentoCommand(2, "Saque");
                await _mediator.Send(tipo1);
                await _mediator.Send(tipo2);
                var command = new CriarMovimentoCommand(idUsuario, request.Valor, 1, request.Descricao,request.IdConta);
                var result = await _mediator.Send(command);

                return result ? Ok("Depósito realizado com sucesso") : BadRequest("Erro ao processar depósito");
            }
            [HttpPost("abrir-conta")]
            public async Task<IActionResult> AbrirConta([FromBody] AbrirContaRequest request)
            {
                try
                {
                    var command = new AbrirContaCommand(request.IdUsuarioPrincipal);
                    var idGerado = await _mediator.Send(command);

                    return Ok(new
                    {
                        Message = "Conta aberta com sucesso!",
                        IdConta = idGerado
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = $"Erro ao abrir conta: {ex.Message}" });
                }
            }
            [HttpGet("saldo")]
            public async Task<IActionResult> GetSaldo()
            {
                var claimId = User.FindFirst("IdUsuario")?.Value;

                if (string.IsNullOrEmpty(claimId))
                    return Unauthorized(new { message = "Token inválido ou ID da conta ausente" });

                var idConta = int.Parse(claimId);

                var query = new GetSaldoQuery(idConta);
                var resultado = await _mediator.Send(query);

                return resultado != null ? Ok(resultado) : NotFound("Conta não encontrada");
            }
            
            [HttpPost("saque")]
            public async Task<IActionResult> Saque([FromBody] MovimentacaoRequest request)
            {
                if(request.Valor <= 0)
                {
                    return BadRequest("O valor do saque deve ser maior que zero.");
                }
                var IdUsuario = int.Parse(User.FindFirst("IdUsuario")?.Value);
                if (request.IdUsuario != null)
                {
                    IdUsuario = request.IdUsuario.Value;
                }
                var command = new CriarMovimentoCommand(IdUsuario, request.Valor, 2, request.Descricao,request.IdConta);
                var result = await _mediator.Send(command);

                return result ? Ok("Saque realizado com sucesso") : BadRequest("Erro ao processar saque");
            }
        }
    }
}
