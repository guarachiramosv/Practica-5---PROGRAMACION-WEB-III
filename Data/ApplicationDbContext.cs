using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using practica5PR.Models;

namespace practica5PR.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Estante> Estantes { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Categoria>()
                .HasMany(c => c.Medicamentos)
                .WithOne(m => m.Categoria)
                .HasForeignKey(m => m.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Estante>()
                .HasMany(e => e.Medicamentos)
                .WithOne(m => m.Estante)
                .HasForeignKey(m => m.EstanteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Categoria>()
                .Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Entity<Estante>()
                .Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Entity<Medicamento>()
                .Property(m => m.Nombre)
                .IsRequired()
                .HasMaxLength(150);

            builder.Entity<Medicamento>()
                .Property(m => m.Precio)
                .HasColumnType("decimal(10,2)");
        }
    }
}