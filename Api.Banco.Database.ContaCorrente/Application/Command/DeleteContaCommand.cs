using Api.Banco.Database.ContaCorrente.Context;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Application.Command
{
    public record DeleteContaCommand(long Id) : IRequest<bool>;

    public class DeleteContaHandler : IRequestHandler<DeleteContaCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        public DeleteContaHandler(ApplicationDbContext context) => _context = context;

        public async Task<bool> Handle(DeleteContaCommand request, CancellationToken ct)
        {
            var conta = await _context.ContasCorrentes.FindAsync(new object[] { request.Id }, ct);
            if (conta == null) return false;

            _context.ContasCorrentes.Remove(conta); 
            return await _context.SaveChangesAsync(ct) > 0;
        }
    }
}
