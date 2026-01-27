using Api.Banco.Database.Tarifas.Context;
using Api.Banco.Database.Tarifas.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.Tarifas.Application.Handlers
{
    public record GetTarifasQuery() : IRequest<IEnumerable<TarifaTb>>;

    public class GetTarifasHandler : IRequestHandler<GetTarifasQuery, IEnumerable<TarifaTb>>
    {
        private readonly TarifasDbContext _context;
        public GetTarifasHandler(TarifasDbContext context) => _context = context;

        public async Task<IEnumerable<TarifaTb>> Handle(GetTarifasQuery request, CancellationToken ct) =>
            await _context.Tarifas
                .AsNoTracking()
                .OrderByDescending(x => x.DataMovimento)
                .Take(1000)
                .ToListAsync(ct);
    }
}
