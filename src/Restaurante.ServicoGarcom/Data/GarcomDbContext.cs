using Microsoft.EntityFrameworkCore;
using Restaurante.ServicoGarcom.Data.Modelos;

namespace Restaurante.ServicoGarcom.Data
{
    public class GarcomDbContext : DbContext
    {
        public GarcomDbContext(DbContextOptions<GarcomDbContext> options) : base(options)
        {
        }

        // Mapeia nossa classe Pedido para uma tabela chamada "Pedidos"
        public DbSet<Pedido> Pedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Converte o Enum StatusPedido para string no banco
            modelBuilder.Entity<Pedido>()
                .Property(p => p.Status)
                .HasConversion<string>();
        }
    }
}