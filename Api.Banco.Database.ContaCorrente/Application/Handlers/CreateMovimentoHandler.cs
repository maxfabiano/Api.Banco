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
    public class CreateMovimentoHandler : IRequestHandler<CreateMovimentoCommand, long>
    {
        private readonly ApplicationDbContext _context;
        public CreateMovimentoHandler(ApplicationDbContext context) => _context = context;

        public async Task<long> Handle(CreateMovimentoCommand request, CancellationToken ct)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct);

            try
            {
                var movimento = new Movimento
                {
                    IdUsuario = request.ContaId,
                    IdTipoMovimento = request.TipoId,
                    Valor = request.Valor,
                    DataMovimento = DateTime.UtcNow
                };

                _context.Movimentos.Add(movimento);
                await _context.SaveChangesAsync(ct);

                await transaction.CommitAsync(ct);
                return movimento.IdMovimento;
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw; 
            }
        }
    }
}
