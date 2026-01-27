using Api.Banco.Database.ContaCorrente.Context;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Application.Command
{
    public record UpdateContaCommand(long Id, string Nome, bool Ativo) : IRequest<bool>;

    public class UpdateContaHandler : IRequestHandler<UpdateContaCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        public UpdateContaHandler(ApplicationDbContext context) => _context = context;

        public async Task<bool> Handle(UpdateContaCommand request, CancellationToken ct)
        {
            var conta = await _context.ContasCorrentes.FindAsync(new object[] { request.Id }, ct);
            if (conta == null) return false;

            conta.Ativo = request.Ativo;

            return await _context.SaveChangesAsync(ct) > 0;
        }
    }
}
