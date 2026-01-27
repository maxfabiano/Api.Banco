using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Domain.Entities
{
    public class Role : BaseEntity
    {
        [Key]
        public int IdRole { get; set; }
        public string Nome { get; set; }

        public virtual ICollection<ContaCorrenteRole> ContaCorrenteRoles { get; set; }
    }
}
