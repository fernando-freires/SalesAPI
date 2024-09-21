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
        }
    }
}
