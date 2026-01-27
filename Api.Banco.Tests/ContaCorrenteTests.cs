using Moq;
using Xunit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Banco.ContaCorrente.Controllers.Api.Banco.ContaCorrente.Api.Controllers;
using Api.Banco.Database.ContaCorrente.Application.Command;
using Api.Banco.ContaCorrente.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

public class ContaCorrenteTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly MovimentacaoController _controller;

    public ContaCorrenteTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new MovimentacaoController(_mediatorMock.Object);

        // Mock de usuário logado (Claim IdUsuario)
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("IdUsuario", "1") }));
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
    }

    [Fact]
    public async Task Deposito_DeveRetornarOk_QuandoValorPositivo()
    {
        // Arrange
        var request = new MovimentacaoRequest ( 1, 100,  "Pix",  10 );
        _mediatorMock.Setup(m => m.Send(It.IsAny<CriarMovimentoCommand>(), default)).ReturnsAsync(true);

        // Act
        var result = await _controller.Deposito(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Depósito realizado com sucesso", okResult.Value);
    }
}