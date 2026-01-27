using Api.Banco.Database.ContaCorrente.Context;
using Api.Banco.Database.ContaCorrente.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Application.Command
{
    public class GetSaldoHandler : IRequestHandler<GetSaldoQuery, SaldoResponse?>
    {
        private readonly ApplicationDbContext _context;

        public GetSaldoHandler(ApplicationDbContext context) => _context = context;

        public async Task<SaldoResponse?> Handle(GetSaldoQuery request, CancellationToken ct)
        {
            var conta = await _context.ContasCorrentes
                .AsNoTracking() 
                .Where(c => c.IdUsuario == request.IdUsuario)
                .Select(c => new SaldoResponse(c.IdContaCorrente, c.Saldo,DateTime.UtcNow))
                .FirstOrDefaultAsync(ct);

            return conta;
        }
    }
}
