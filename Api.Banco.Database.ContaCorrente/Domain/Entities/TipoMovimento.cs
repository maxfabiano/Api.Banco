using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Domain.Entities
{

    public class TipoMovimento : BaseEntity
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
    }
}
