using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.Transferencia.Domain.Entities
{
    public class Idempotencia : BaseEntity
    {
        public string ChaveIdempotencia { get; set; } 
        public string Requisicao { get; set; } 
        public string Resultado { get; set; } 
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
