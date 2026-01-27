using Api.Banco.Database.Tarifas.Context;
using Api.Banco.Database.Tarifas.Domain;
using Api.Banco.Database.Tarifas.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.Tarifas.Application.Command
{
    public record CreateTarifaCommand(int IdContaCorrente, decimal Valor) : IRequest<int>;

    public class CreateTarifaHandler : IRequestHandler<CreateTarifaCommand, int>
    {
        private readonly TarifasDbContext _context;
        public CreateTarifaHandler(TarifasDbContext context) => _context = context;

        public async Task<int> Handle(CreateTarifaCommand request, CancellationToken ct)
        {
            var tarifa = new TarifaTb
            {
                IdContaCorrente = request.IdContaCorrente,
                Valor = request.Valor,
                DataMovimento = DateTime.UtcNow
            };

            _context.Tarifas.Add(tarifa);
            await _context.SaveChangesAsync(ct);
            return tarifa.IdTarifa;
        }
    }
}
