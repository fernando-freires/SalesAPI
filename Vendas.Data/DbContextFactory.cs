using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Vendas.Data.Context
{
    public class VendasContextFactory : IDesignTimeDbContextFactory<VendasContext>
    {
        public VendasContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VendasContext>();
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=VendasDB;User Id=sa;Password=Your_password123;Trusted_Connection=False;MultipleActiveResultSets=true");

            return new VendasContext(optionsBuilder.Options);
        }
    }
}
