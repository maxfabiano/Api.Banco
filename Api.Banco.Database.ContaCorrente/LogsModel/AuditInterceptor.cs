using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.ContaCorrente.Logs
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var entries = eventData.Context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                
                var entityName = entry.Entity.GetType().Name;
                var state = entry.State.ToString();

                Console.WriteLine($"[AUDIT LOG] {DateTime.Now}: Entidade {entityName} alterada para {state}");

                foreach (var prop in entry.Properties)
                {
                    if (prop.IsModified)
                    {
                        Console.WriteLine($"  - Propriedade {prop.Metadata.Name}: '{prop.OriginalValue}' -> '{prop.CurrentValue}'");
                    }
                }
            }
            return base.SavingChanges(eventData, result);
        }
    }
}
