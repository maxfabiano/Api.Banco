using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Domain.Entities
{
    public class AuditLog : BaseEntity
    {
        public long Id { get; set; }
        public string EntityName { get; set; }
        public string Action { get; set; } 
        public string OldValues { get; set; } 
        public string NewValues { get; set; } 
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
