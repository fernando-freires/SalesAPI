using Microsoft.EntityFrameworkCore;
using Vendas.Domain;

namespace Vendas.Data.Context
{
    public class VendasContext : DbContext
    {
        public VendasContext(DbContextOptions<VendasContext> options) : base(options) { }

        public DbSet<Venda> Vendas { get; set; } = null!; 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Venda>()
                .HasKey(v => v.Id);
            
            modelBuilder.Entity<ItemVenda>()
                .HasKey(i => i.ProdutoId);

            modelBuilder.Entity<ItemVenda>()
                .HasOne(i => i.Venda)
                .WithMany(v => v.Itens)
                .HasForeignKey(i => i.VendaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemVenda>()
                .Property(i => i.ValorUnitario)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ItemVenda>()
                .Property(i => i.Desconto)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Venda>()
                .Property(v => v.ValorTotal)
                .HasColumnType("decimal(18,2)");
        }
    }
}
