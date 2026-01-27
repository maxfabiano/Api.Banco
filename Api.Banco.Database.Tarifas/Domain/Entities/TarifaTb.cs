using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.Tarifas.Domain.Entities
{
    public class TarifaTb : BaseEntity
    {
        public int IdTarifa { get; set; }     
        public int IdContaCorrente { get; set; } 
        public DateTime DataMovimento { get; set; }
        public decimal Valor { get; set; }      
    }
}
