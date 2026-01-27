using Api.Banco.Database.Tarifas.Application.Command;
using Api.Banco.Database.Transferencia.Application.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Tests
{
    public class TarifasETramferenciaTests
    {
        [Fact]
        public async Task PostTarifa_DeveRetornarSucesso()
        {
            var mediatorMock = new Mock<IMediator>();
            var controller = new Api.Banco.Tarifas.Controllers.TarifasController(mediatorMock.Object);
            var command = new CreateTarifaCommand(1,100);

            mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(1);

            var result = await controller.PostTarifa(command);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Transferir_DeveChamarMediator()
        {
            var mediatorMock = new Mock<IMediator>();
            var controller = new Api.Banco.Transferencia.Controllers.TransferenciaController(mediatorMock.Object);
            var command = new EfetuarTransferenciaCommand(1,2,1);

            mediatorMock.Setup(m => m.Send(It.IsAny<EfetuarTransferenciaCommand>(), default)).ReturnsAsync(true);

            var result = await controller.Transferir(command);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
