using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Domain.Entities
{

    public class Movimento : BaseEntity
    {
        public int IdMovimento { get; set; }

        public int IdUsuario { get; set; }

        public int IdTipoMovimento { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public DateTime DataMovimento { get; set; }

        [ForeignKey("IdTipoMovimento")] 
        public virtual TipoMovimento TipoMovimento { get; set; }
        public virtual ContaCorrenteTb ContaCorrente { get; set; }
    }

}
