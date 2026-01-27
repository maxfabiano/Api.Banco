using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Usuarios.Dtos
{
    public record RegisterUserRequest(string Nome, int Numero, string CPF, string Senha);
}
