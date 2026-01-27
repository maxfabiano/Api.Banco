using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Domain.Entities
{
    public record SaldoResponse(
        int IdConta,
        decimal SaldoAtual,
        DateTime DataConsulta
    );
}
