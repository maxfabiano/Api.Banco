using Api.Banco.Database.Usuarios.Application.Command;
using Api.Banco.Database.Usuarios.Domain.Entities;
using Api.Banco.Database.Usuarios.Handlers;
using Api.Banco.Usuarios.Dtos;
using Api.Banco.Usuarios.Utils;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Banco.Usuarios.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public AuthController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            try
            {
                
                if (!CpfValidator.IsValid(request.CPF))
                    return BadRequest(new ErrorResponse("CPF Inválido", "INVALID_DOCUMENT"));

                
                var command = new CreateUsuarioCommand(
                    request.Nome,
                    request.Numero,
                    request.CPF,
                    BCrypt.Net.BCrypt.HashPassword(request.Senha), 
                    Guid.NewGuid().ToString() 
                );

                var id = await _mediator.Send(command);
                return Ok(new { Message = "Usuário cadastrado com sucesso", Id = id });
            }
            catch (Exception ex)
            {
                
                return BadRequest(new ErrorResponse($"Erro ao cadastrar usuário: {ex.Message}", "USER_REGISTRATION_FAILED"));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _mediator.Send(new LoginUserQuery(request.Login, request.Senha));

            if (user == null)
                return Unauthorized(new ErrorResponse("Credenciais inválidas", "USER_UNAUTHORIZED"));

            var token = GenerateJwtToken(user);
            return Ok(new LoginResponse(token.Value, token.Expiration, user.IdUsuario));
        }

        private (string Value, DateTime Expiration) GenerateJwtToken(UsuarioTb user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
            var expiration = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:ExpirationInMinutes"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Nome),
                    new Claim("IdUsuario", user.IdUsuario.ToString()) 
                }),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token), expiration);
        }
    }
}