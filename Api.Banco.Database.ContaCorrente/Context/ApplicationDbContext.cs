using Api.Banco.Database.ContaCorrente.Domain;
using Api.Banco.Database.ContaCorrente.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Api.Banco.Database.ContaCorrente.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ContaCorrenteTb> ContasCorrentes { get; set; }
        public DbSet<Movimento> Movimentos { get; set; }
        public DbSet<Idempotencia> Idempotencias { get; set; }
        public DbSet<TipoMovimento> TipoMovimento { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoMovimento>(entity => {
                entity.ToTable("tipos_movimento");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descricao).IsRequired().HasMaxLength(10);
                entity.HasData(
                    new TipoMovimento { Id = 1, Descricao = "Credito" },
                    new TipoMovimento { Id = 2, Descricao = "Debito" }
                );
            });

            modelBuilder.Entity<ContaCorrenteTb>(entity => {
                entity.ToTable("contacorrente");
                entity.HasKey(e => e.IdContaCorrente); 
                entity.Property(e => e.IdContaCorrente).HasColumnName("id_conta_corrente");

                entity.Property(e => e.IdUsuario).HasColumnName("IdUsuario");

                entity.Property(e => e.Ativo).HasDefaultValue(0);
                entity.HasQueryFilter(x => !x.IsDeleted);
            });
            modelBuilder.Entity<Role>(entity => {
                entity.ToTable("roles");
                entity.HasKey(e => e.IdRole); 
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(50);
            });
            modelBuilder.Entity<Movimento>(entity => {
                entity.ToTable("movimento");
                entity.HasKey(e => e.IdMovimento);

                entity.Property(e => e.IdUsuario)
                      .HasColumnName("IdUsuario") 
                      .IsRequired();

                entity.HasOne(d => d.ContaCorrente)
                      .WithMany(p => p.Movimentos)
                      .HasForeignKey(d => d.IdUsuario)
                      .HasPrincipalKey(p => p.IdUsuario); 

                entity.HasOne(d => d.TipoMovimento)
                      .WithMany()
                      .HasForeignKey(d => d.IdTipoMovimento)
                      .HasConstraintName("FK_movimento_tipos_movimento_TipoMovimentoId");
            });

            modelBuilder.Entity<Idempotencia>(entity => {
                entity.ToTable("idempotencia");
                entity.HasKey(e => e.ChaveIdempotencia);
                entity.Property(e => e.ChaveIdempotencia).HasColumnName("chave_idempotencia").HasMaxLength(100);
                entity.Property(e => e.Requisicao).HasColumnType("json");
                entity.Property(e => e.Resultado).HasColumnType("json");
            });

            modelBuilder.Entity<ContaCorrenteRole>(entity => {
                entity.ToTable("contacorrente_roles");

                entity.HasKey(e => new { e.IdContaCorrente, e.IdRole });

                entity.Property(e => e.IdContaCorrente).HasColumnName("id_conta_corrente");

                entity.HasOne(d => d.ContaCorrente)
                      .WithMany(p => p.ContaCorrenteRoles) 
                      .HasForeignKey(d => d.IdContaCorrente)
                      .HasConstraintName("FK_contacorrente_roles_contacorrente");
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified; 
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}