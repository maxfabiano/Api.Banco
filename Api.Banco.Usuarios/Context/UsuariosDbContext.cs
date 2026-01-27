using Api.Banco.Database.Usuarios.Domain;
using Api.Banco.Database.Usuarios.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Api.Banco.Usuarios.Context
{
    public class UsuariosDbContext : DbContext
    {
        public UsuariosDbContext(DbContextOptions<UsuariosDbContext> options) : base(options) { }

        public DbSet<UsuarioTb> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioTb>(entity =>
            {
                entity.ToTable("usuarios");
                entity.HasKey(e => e.IdUsuario);

                entity.HasIndex(e => e.CPF).IsUnique();
                entity.HasIndex(e => e.Numero).IsUnique();

                entity.Property(e => e.Nome).IsRequired().HasMaxLength(150);
                entity.Property(e => e.CPF).IsRequired().HasMaxLength(11);

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
