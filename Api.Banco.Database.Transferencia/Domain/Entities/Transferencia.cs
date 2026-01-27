using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.Transferencia.Domain.Entities
{
    public class TransferenciaTb : BaseEntity
    {
        public int IdTransferencia { get; set; } //
        public int IdContaCorrenteOrigem { get; set; } 
        public int IdContaCorrenteDestino { get; set; } 
        public DateTime DataMovimento { get; set; } 
        public decimal Valor { get; set; } 
    }
}
