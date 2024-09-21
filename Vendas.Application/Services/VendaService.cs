using Microsoft.EntityFrameworkCore;
using Vendas.Data.Context;
namespace Vendas.Application.Services
{
    public interface IVendaService
    {
        Task<Venda> CriarVendaAsync(Venda venda);
        Task<Venda> ObterVendaPorIdAsync(Guid id);
        Task<IEnumerable<Venda>> ObterVendasAsync();
        Task<Venda> AtualizarVendaAsync(Venda venda);
        Task<bool> CancelarVendaAsync(Guid id);
    }

    public class VendaService : IVendaService
    {
        private readonly VendasContext _context;

        public VendaService(VendasContext context)
        {
            _context = context;
        }

        public async Task<Venda> CriarVendaAsync(Venda venda)
        {
            venda.Id = Guid.NewGuid(); 
            foreach (var item in venda.Itens)
            {
                item.ProdutoId = Guid.NewGuid();  
            }
            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();
            return venda;
        }

        public async Task<Venda?> ObterVendaPorIdAsync(Guid id)
        {
            return await _context.Vendas.Include(v => v.Itens).FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Venda>> ObterVendasAsync()
        {
            return await _context.Vendas.Include(v => v.Itens).ToListAsync();
        }

        public async Task<Venda> AtualizarVendaAsync(Venda venda)
        {
            _context.Vendas.Update(venda);
            await _context.SaveChangesAsync();
            return venda;
        }

        public async Task<bool> CancelarVendaAsync(Guid id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null) return false;
            venda.Cancelada = true;
            _context.Vendas.Update(venda);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
