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
    public class CriarMovimentoHandler : IRequestHandler<CriarMovimentoCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public CriarMovimentoHandler(ApplicationDbContext context) => _context = context;

        public async Task<bool> Handle(CriarMovimentoCommand request, CancellationToken ct)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(ct);

            try
            {
                var conta = await _context.ContasCorrentes
                    .FirstOrDefaultAsync(c => c.IdUsuario == request.IdUsuario, ct);

                if(request.IdConta.HasValue)
                {
                    conta = await _context.ContasCorrentes
                        .FirstOrDefaultAsync(c => c.IdContaCorrente == request.IdConta.Value, ct);
                }

                if (conta == null) return false;

                if (request.IdTipoMovimento == 2 && conta.Saldo < request.Valor)
                    throw new Exception("Saldo insuficiente para esta operação.");

                if (request.IdTipoMovimento == 1) conta.Saldo += request.Valor;
                else conta.Saldo -= request.Valor;

                var movimento = new Movimento
                {
                    IdUsuario = request.IdUsuario,
                    Descricao = request.Descricao,
                    IdTipoMovimento = request.IdTipoMovimento,
                    Valor = request.Valor,
                    DataMovimento = DateTime.UtcNow
                };

                _context.Movimentos.Add(movimento);
                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);

                return true;
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }
    }
}
