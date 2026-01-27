using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.ContaCorrente.DTOs
{
    public record MovimentacaoRequest(int? IdUsuario,decimal Valor, string Descricao, int IdConta);

    public record SaldoResponse(int IdConta, decimal SaldoAtual);

    public record MovimentoDto(int IdMovimento, decimal Valor, string Tipo, string Descricao, DateTime Data);
}
