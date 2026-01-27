using Api.Banco.Database.Transferencia.Context;
using Api.Banco.Database.Transferencia.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;

namespace Api.Banco.Database.Transferencia.Application.Command
{
    public class EfetuarTransferenciaHandler : IRequestHandler<EfetuarTransferenciaCommand, bool>
    {
        private readonly TransferenciaDbContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseUrl;
        public EfetuarTransferenciaHandler(
            IConfiguration configuration,
            TransferenciaDbContext context,
            IHttpClientFactory clientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
            _baseUrl = configuration["ServiceUrls:ContaCorrente"];
        }

        public async Task<bool> Handle(EfetuarTransferenciaCommand request, CancellationToken ct)
        {
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(_baseUrl);
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"];
            client.DefaultRequestHeaders.Add("Authorization", token.ToString());

            var saqueResponse = await client.PostAsJsonAsync("api/conta/saque", new
            {
                IdConta = request.IdContaCorrenteOrigem,
                Valor = request.Valor,
                Descricao = $"Transferência para conta {request.IdContaCorrenteDestino}"
            }, ct);

            if (!saqueResponse.IsSuccessStatusCode) return false;

            var depositoResponse = await client.PostAsJsonAsync("api/conta/deposito", new
            {
                IdConta= request.IdContaCorrenteDestino,
                Valor = request.Valor,
                Descricao = $"Recebido de conta {request.IdContaCorrenteOrigem}"
            }, ct);

            if (!depositoResponse.IsSuccessStatusCode)
            {
                await client.PostAsJsonAsync("api/conta/deposito", new
                {
                    IdConta = request.IdContaCorrenteOrigem,
                    Valor = request.Valor,
                    Descricao = "Estorno de transferência falhou"
                }, ct);
                return false;
            }

            var novaTransferencia = new TransferenciaTb
            {
                IdContaCorrenteOrigem = request.IdContaCorrenteOrigem,
                IdContaCorrenteDestino = request.IdContaCorrenteDestino,
                Valor = request.Valor,
                DataMovimento = DateTime.Now
            };

            _context.Transferencias.Add(novaTransferencia);
            await _context.SaveChangesAsync(ct);

            return true;
        }
    }
}