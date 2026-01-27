using Api.Banco.Database.ContaCorrente.Context;
using Api.Banco.Database.ContaCorrente.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Application.Command
{
    public record AbrirContaCommand(int IdUsuario) : IRequest<int>;

    public class AbrirContaHandler : IRequestHandler<AbrirContaCommand, int>
    {
        private readonly ApplicationDbContext _context;
        public AbrirContaHandler(ApplicationDbContext context) => _context = context;

        public async Task<int> Handle(AbrirContaCommand request, CancellationToken ct)
        {
            var novaConta = new ContaCorrenteTb
            {
                IdUsuario = request.IdUsuario,
                Saldo = 0,
                Ativo = true, 
                IsDeleted = false
            };

            _context.ContasCorrentes.Add(novaConta);
            await _context.SaveChangesAsync(ct);

            return novaConta.IdContaCorrente;
        }
    }
}
