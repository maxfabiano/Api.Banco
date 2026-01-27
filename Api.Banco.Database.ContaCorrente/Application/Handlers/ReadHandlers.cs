using Api.Banco.Database.ContaCorrente.Context;
using Api.Banco.Database.ContaCorrente.Domain.Entities;
using Api.Banco.Database.ContaCorrente.Domain.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Application.Handlers
{
    public class ReadHandlers :
        IRequestHandler<GetAllContasQuery, IEnumerable<ContaCorrenteTb>>,
        IRequestHandler<GetAllMovimentosQuery, IEnumerable<Movimento>>,
        IRequestHandler<GetAllTiposMovimentoQuery, IEnumerable<TipoMovimento>>,
        IRequestHandler<GetAllIdempotenciasQuery, IEnumerable<Idempotencia>>,
        IRequestHandler<GetAllRolesQuery, IEnumerable<Role>>,
        IRequestHandler<GetAllAuditLogsQuery, IEnumerable<AuditLog>>
    {
        private readonly ApplicationDbContext _context;

        public ReadHandlers(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<ContaCorrenteTb>> Handle(GetAllContasQuery request, CancellationToken ct) =>
            await _context.ContasCorrentes.AsNoTracking().Take(1000).ToListAsync(ct);

        public async Task<IEnumerable<Movimento>> Handle(GetAllMovimentosQuery request, CancellationToken ct) =>
            await _context.Movimentos.AsNoTracking().OrderByDescending(x => x.DataMovimento).Take(1000).ToListAsync(ct);

        public async Task<IEnumerable<TipoMovimento>> Handle(GetAllTiposMovimentoQuery request, CancellationToken ct) =>
            await _context.Set<TipoMovimento>().AsNoTracking().Take(1000).ToListAsync(ct);

        public async Task<IEnumerable<Idempotencia>> Handle(GetAllIdempotenciasQuery request, CancellationToken ct) =>
            await _context.Idempotencias.AsNoTracking().OrderByDescending(x => x.DataCriacao).Take(1000).ToListAsync(ct);

        public async Task<IEnumerable<Role>> Handle(GetAllRolesQuery request, CancellationToken ct) =>
            await _context.Roles.AsNoTracking().Take(1000).ToListAsync(ct);

        public async Task<IEnumerable<AuditLog>> Handle(GetAllAuditLogsQuery request, CancellationToken ct) =>
            await _context.AuditLogs.AsNoTracking().OrderByDescending(x => x.Timestamp).Take(1000).ToListAsync(ct);
    }
}
