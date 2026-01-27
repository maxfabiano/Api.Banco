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
    public record CreateContaCommand(int Numero, string Nome, string Senha, string Salt) : IRequest<long>;

    public class CreateContaHandler : IRequestHandler<CreateContaCommand, long>
    {
        private readonly ApplicationDbContext _context;
        public CreateContaHandler(ApplicationDbContext context) => _context = context;

        public async Task<long> Handle(CreateContaCommand request, CancellationToken ct)
        {
            var conta = new ContaCorrenteTb
            {
                Ativo = true,
                CreatedAt = DateTime.UtcNow
            };
            _context.ContasCorrentes.Add(conta);
            await _context.SaveChangesAsync(ct);
            return conta.IdContaCorrente;
        }
    }
}
