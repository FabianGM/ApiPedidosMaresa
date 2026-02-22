using Microsoft.EntityFrameworkCore;
using PedidosAPI.Models;

namespace PedidosAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<PedidoCabecera> PedidoCabeceras { get; set; }
        public DbSet<PedidoDetalle> PedidoDetalles { get; set; }
        public DbSet<LogAuditoria> LogAuditorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PedidoDetalle>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<PedidoDetalle>()
                .Property(p => p.PedidoId)
                .HasColumnName("PedidoId");
        }
    }
}