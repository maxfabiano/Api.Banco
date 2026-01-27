using Api.Banco.Database.ContaCorrente.Application.Command;
using Api.Banco.Database.ContaCorrente.Context;
using Api.Banco.Database.ContaCorrente.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Application.Handlers
{
    public class TipoMovimentoHandler :
        IRequestHandler<CreateTipoMovimentoCommand, int>,
        IRequestHandler<UpdateTipoMovimentoCommand, bool>,
        IRequestHandler<DeleteTipoMovimentoCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public TipoMovimentoHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateTipoMovimentoCommand request, CancellationToken ct)
        {
            var tipoId = _context.TipoMovimento.AsNoTracking().Where(x => x.Id == request.Id).FirstOrDefault();
            if(tipoId != null)
            {
                return tipoId.Id;
            }
            var tipo = new TipoMovimento
            {
                Id = request.Id,
                Descricao = request.Descricao
            };
            
            _context.Set<TipoMovimento>().Add(tipo);
            await _context.SaveChangesAsync(ct);
            return tipo.Id;
        }

        public async Task<bool> Handle(UpdateTipoMovimentoCommand request, CancellationToken ct)
        {
            var tipo = await _context.Set<TipoMovimento>()
                .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (tipo == null) return false;

            tipo.Descricao = request.Descricao;

            return await _context.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> Handle(DeleteTipoMovimentoCommand request, CancellationToken ct)
        {
            var tipo = await _context.Set<TipoMovimento>()
                .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (tipo == null) return false;

            _context.Set<TipoMovimento>().Remove(tipo);

            return await _context.SaveChangesAsync(ct) > 0;
        }
    }
}
