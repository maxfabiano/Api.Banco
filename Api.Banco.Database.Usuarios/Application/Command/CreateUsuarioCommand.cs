using Api.Banco.Database.Usuarios.Context;
using Api.Banco.Database.Usuarios.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.Usuarios.Application.Command
{
    public record CreateUsuarioCommand(string Nome, int Numero, string Cpf, string SenhaHash, string Salt) : IRequest<int>;

    public class CreateUsuarioHandler : IRequestHandler<CreateUsuarioCommand, int>
    {
        private readonly UsuariosDbContext _context;
        public CreateUsuarioHandler(UsuariosDbContext context) => _context = context;

        public async Task<int> Handle(CreateUsuarioCommand request, CancellationToken ct)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct);

            try
            {
                var usuario = new UsuarioTb
                {
                    Numero = request.Numero,
                    CPF = request.Cpf,
                    Nome = request.Nome,
                    SenhaHash = request.SenhaHash,
                    Salt = request.Salt
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);

                return usuario.IdUsuario;
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync(ct);
                throw new Exception(ex.Message);
            }
        }
    }
}
