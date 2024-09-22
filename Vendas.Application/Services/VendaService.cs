using Vendas.Domain;
using Microsoft.EntityFrameworkCore;
using Vendas.Data.Context;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<VendaService> _logger;

        public VendaService(VendasContext context, ILogger<VendaService> logger)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            
            _context = context;
            _logger = logger;
        }

        public async Task<Venda> CriarVendaAsync(Venda venda)
        {
            venda.Id = Guid.NewGuid();
            
            foreach (var item in venda.Itens)
            {
                item.ProdutoId = Guid.NewGuid();  
                item.VendaId = venda.Id;
            }

            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Venda {VendaId} registrada com sucesso.", venda.Id);  
            return venda;
        }

        public async Task<Venda> ObterVendaPorIdAsync(Guid id)
        {
            var venda = await _context.Vendas
                                    .Include(v => v.Itens)
                                    .FirstOrDefaultAsync(v => v.Id == id);
            
            if (venda == null)
            {
                _logger.LogWarning("Venda com ID {VendaId} não foi encontrada.", id);  
                throw new KeyNotFoundException($"Venda com ID {id} não foi encontrada.");
            }

            if (venda.Cancelada)
            {
                _logger.LogWarning("Venda com ID {VendaId} foi cancelada.", id);
                throw new InvalidOperationException($"Venda com ID {id} foi cancelada.");
            }

            _logger.LogInformation("Venda {VendaId} obtida com sucesso.", id);
            return venda;
        }


        public async Task<IEnumerable<Venda>> ObterVendasAsync()
        {
            _logger.LogInformation("Obtendo todas as vendas.");  
        
            return await _context.Vendas
                .Include(v => v.Itens)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Venda> AtualizarVendaAsync(Venda venda)
        {
            _logger.LogInformation("Atualizando venda com ID {VendaId}.", venda.Id);  
            _context.Vendas.Update(venda);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Venda {VendaId} atualizada com sucesso.", venda.Id);  
            return venda;
        }

        public async Task<bool> CancelarVendaAsync(Guid id)
        {
            _logger.LogInformation("Cancelando venda com ID {VendaId}.", id);  
            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
            {
                _logger.LogWarning("Venda com ID {VendaId} não foi encontrada para cancelamento.", id);  
                return false;
            }

            _context.Vendas.Remove(venda);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Venda {VendaId} cancelada com sucesso.", id);  
            return true;
        }
    }
}
