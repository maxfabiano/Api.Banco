using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Usuarios.Dtos
{
    public record LoginResponse(string Token, DateTime Expiration, string Nome);
}
