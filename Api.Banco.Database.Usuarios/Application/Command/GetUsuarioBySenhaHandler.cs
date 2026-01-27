using Api.Banco.Database.Usuarios.Application.Command;
using Api.Banco.Database.Usuarios.Context;
using Api.Banco.Database.Usuarios.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Banco.Database.Usuarios.Handlers
{
    public class GetUsuarioBySenhaHandler : IRequestHandler<GetUsuarioBySenhaQuery, UsuarioTb?>
    {
        private readonly UsuariosDbContext _context;

        public GetUsuarioBySenhaHandler(UsuariosDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioTb?> Handle(GetUsuarioBySenhaQuery request, CancellationToken ct)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.SenhaHash == request.SenhaHash, ct);
        }
    }
}