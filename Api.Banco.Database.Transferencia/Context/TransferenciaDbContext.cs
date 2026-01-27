using Api.Banco.Database.Transferencia.Domain;
using Api.Banco.Database.Transferencia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Database.Transferencia.Context
{
    public class TransferenciaDbContext : DbContext
    {
        public TransferenciaDbContext(DbContextOptions<TransferenciaDbContext> options) : base(options) { }

        public DbSet<TransferenciaTb> Transferencias { get; set; }
        public DbSet<TransferenciaTb> Idempotencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransferenciaTb>(entity => {
                entity.ToTable("transferencia");
                entity.HasKey(e => e.IdTransferencia);

                entity.Property(e => e.Valor).HasPrecision(18, 2);
                entity.Property(e => e.DataMovimento).HasColumnType("datetime");

                entity.HasQueryFilter(x => !x.IsDeleted); 
            });

            modelBuilder.Entity<Idempotencia>(entity => {
                entity.ToTable("idempotencia");
                entity.HasKey(e => e.ChaveIdempotencia);
                entity.Property(e => e.Requisicao).HasColumnType("json");
                entity.Property(e => e.Resultado).HasColumnType("json");
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
