using Api.Banco.Database.Usuarios.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.Usuarios.Application.Command
{
    public record GetUsuarioBySenhaQuery(string SenhaHash) : IRequest<UsuarioTb?>;
}
