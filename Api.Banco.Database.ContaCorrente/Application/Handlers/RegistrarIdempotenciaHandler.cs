using Api.Banco.Database.ContaCorrente.Application.Command;
using Api.Banco.Database.ContaCorrente.Context;
using Api.Banco.Database.ContaCorrente.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Application.Handlers
{
    public class RegistrarIdempotenciaHandler : IRequestHandler<RegistrarIdempotenciaCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        public RegistrarIdempotenciaHandler(ApplicationDbContext context) => _context = context;

        public async Task<bool> Handle(RegistrarIdempotenciaCommand request, CancellationToken ct)
        {
            var idempotencia = new Idempotencia
            {
                ChaveIdempotencia = request.Chave,
                Requisicao = request.Requisicao,
                Resultado = request.Resultado,
                DataCriacao = DateTime.UtcNow
            };

            _context.Idempotencias.Add(idempotencia);
            return await _context.SaveChangesAsync(ct) > 0;
        }
    }
}
