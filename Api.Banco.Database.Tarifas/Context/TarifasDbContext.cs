using Api.Banco.Database.Tarifas.Domain;
using Api.Banco.Database.Tarifas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.Tarifas.Context
{
    public class TarifasDbContext : DbContext
    {
        public TarifasDbContext(DbContextOptions<TarifasDbContext> options) : base(options) { }

        public DbSet<TarifaTb> Tarifas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TarifaTb>(entity =>
            {
                entity.ToTable("tarifa");
                entity.HasKey(e => e.IdTarifa);

                entity.Property(e => e.Valor).HasPrecision(18, 2);
                entity.Property(e => e.DataMovimento).HasColumnType("datetime");

                entity.HasQueryFilter(x => !x.IsDeleted);
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(ct);
        }
    }
}
