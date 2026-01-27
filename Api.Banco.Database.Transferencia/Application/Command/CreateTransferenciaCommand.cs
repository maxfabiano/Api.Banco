using Api.Banco.Database.Transferencia.Context;
using Api.Banco.Database.Transferencia.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.Transferencia.Application.Command
{
    public record CreateTransferenciaCommand(int Origem, int Destino, decimal Valor) : IRequest<int>;

    public class CreateTransferenciaHandler : IRequestHandler<CreateTransferenciaCommand, int>
    {
        private readonly TransferenciaDbContext _context;
        public CreateTransferenciaHandler(TransferenciaDbContext context) => _context = context;

        public async Task<int> Handle(CreateTransferenciaCommand request, CancellationToken ct)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct);

            try
            {
                var transferencia = new TransferenciaTb
                {
                    IdContaCorrenteOrigem = request.Origem,
                    IdContaCorrenteDestino = request.Destino,
                    Valor = request.Valor,
                    DataMovimento = DateTime.UtcNow
                };

                _context.Transferencias.Add(transferencia);
                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);

                return transferencia.IdTransferencia;
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }
    }
}
