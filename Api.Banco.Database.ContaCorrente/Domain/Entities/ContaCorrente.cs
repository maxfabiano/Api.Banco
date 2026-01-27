using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Domain.Entities
{
    public class ContaCorrenteTb : BaseEntity
    {
        public int IdContaCorrente { get; set; }
        public int IdUsuario { get; set; }

        public bool Ativo { get; set; }
        public Decimal Saldo { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Movimento> Movimentos { get; set; }
        public virtual ICollection<ContaCorrenteRole> ContaCorrenteRoles { get; set; }
    }

}
