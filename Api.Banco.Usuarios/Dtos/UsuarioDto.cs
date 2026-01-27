using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Usuarios.Dtos
{
    namespace Api.Banco.Usuario.Api.DTOs
    {
        
        public class UsuarioDto
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string CPF { get; set; }
            public int NumeroConta { get; set; }
            public int IdContaCorrente { get; set; } 
            public bool Ativo { get; set; }
        }

        
        public record LoginRequest(string Login, string Senha);

        
        public record LoginResponse(string Token, DateTime Expiration, UsuarioDto Usuario);
    }
}
