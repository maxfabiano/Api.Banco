using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.Transferencia.Application.Command
{
    public record EfetuarTransferenciaCommand(
        int IdContaCorrenteOrigem,
        int IdContaCorrenteDestino,
        decimal Valor
    ) : IRequest<bool>;
}
