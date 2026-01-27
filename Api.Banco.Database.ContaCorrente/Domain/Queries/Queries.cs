using Api.Banco.Database.ContaCorrente.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Domain.Queries
{
    public record GetAllContasQuery() : IRequest<IEnumerable<ContaCorrenteTb>>;
    public record GetAllMovimentosQuery() : IRequest<IEnumerable<Movimento>>;
    public record GetAllTiposMovimentoQuery() : IRequest<IEnumerable<TipoMovimento>>;
    public record GetAllIdempotenciasQuery() : IRequest<IEnumerable<Idempotencia>>;
    public record GetAllRolesQuery() : IRequest<IEnumerable<Role>>;
    public record GetAllAuditLogsQuery() : IRequest<IEnumerable<AuditLog>>;
}
