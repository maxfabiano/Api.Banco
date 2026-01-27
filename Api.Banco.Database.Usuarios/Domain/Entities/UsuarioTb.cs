using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.Usuarios.Domain.Entities
{
    public class UsuarioTb :BaseEntity
    {
        [Key]
        public int IdUsuario { get; set; }

        public string CPF { get; set; }
        public int Numero { get; set; }

        public string Nome { get; set; }
        public string SenhaHash { get; set; }
        public string Salt { get; set; }

        public bool Ativo { get; set; }
    }
}
