using Api.Banco.Usuarios.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api.Banco.Tests
{
    public class AuthTests
    {
        [Fact]
        public async Task Register_DeveRetornarErro_SeCpfInvalido()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var configMock = new Mock<IConfiguration>();
            var controller = new Api.Banco.Usuarios.Controllers.AuthController(mediatorMock.Object, configMock.Object);
            var request = new RegisterUserRequest("Teste", DateTime.UtcNow.Millisecond, "123", "123");

            // Act
            var result = await controller.Register(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            // Verifica se a resposta contém erro de documento inválido
        }
    }
}
