using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Application.Command
{
    public record CriarMovimentoCommand(int IdUsuario, decimal Valor, int IdTipoMovimento, string Descricao, int? IdConta) : IRequest<bool>;
}
