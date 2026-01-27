using Api.Banco.Database.Transferencia.Context;
using Api.Banco.Database.Transferencia.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.Transferencia.Application.Query
{
    public record GetTransferenciasQuery() : IRequest<IEnumerable<TransferenciaTb>>;

    public class GetTransferenciasHandler : IRequestHandler<GetTransferenciasQuery, IEnumerable<TransferenciaTb>>
    {
        private readonly TransferenciaDbContext _context;
        public GetTransferenciasHandler(TransferenciaDbContext context) => _context = context;

        public async Task<IEnumerable<TransferenciaTb>> Handle(GetTransferenciasQuery request, CancellationToken ct) =>
            await _context.Transferencias
                .AsNoTracking()
                .OrderByDescending(x => x.DataMovimento)
                .Take(1000)
                .ToListAsync(ct);
    }
}
