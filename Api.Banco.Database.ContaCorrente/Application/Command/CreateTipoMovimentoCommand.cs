using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Application.Command
{
    public record CreateTipoMovimentoCommand(int Id, string Descricao) : IRequest<int>;
}
