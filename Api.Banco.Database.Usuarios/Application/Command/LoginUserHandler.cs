using Api.Banco.Database.Usuarios.Context;
using Api.Banco.Database.Usuarios.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net; 

namespace Api.Banco.Database.Usuarios.Handlers
{
    public record LoginUserQuery(string Login, string Senha) : IRequest<UsuarioTb?>;

    public class LoginUserHandler : IRequestHandler<LoginUserQuery, UsuarioTb?>
    {
        private readonly UsuariosDbContext _context;

        public LoginUserHandler(UsuariosDbContext context) => _context = context;

        public async Task<UsuarioTb?> Handle(LoginUserQuery request, CancellationToken ct)
        {
            try
            {
                var user = await _context.Usuarios
                    .FirstOrDefaultAsync(u => (u.Nome == request.Login || u.Numero.ToString() == request.Login), ct);

                if (user == null) return null;
 if (!BCrypt.Net.BCrypt.Verify(request.Senha, user.SenhaHash))
                    return null;

                return user;
            }
            catch (Exception ex) { 
            throw new Exception("Erro ao autenticar usuário: " + ex.Message);
            }
        }
    }
}