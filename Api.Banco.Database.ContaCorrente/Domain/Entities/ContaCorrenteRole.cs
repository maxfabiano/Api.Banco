using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Domain.Entities
{
    public class ContaCorrenteRole
    {
        public int IdContaCorrente { get; set; }
        public int IdRole { get; set; }

        public virtual ContaCorrenteTb ContaCorrente { get; set; }
        public virtual Role Role { get; set; }
    }
}
