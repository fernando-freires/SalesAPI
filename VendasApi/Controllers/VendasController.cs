using Microsoft.AspNetCore.Mvc;
using Vendas.Application.Services;
using Vendas.Domain;

namespace VendasApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendasController : ControllerBase
    {
        private readonly IVendaService _vendaService;

        public VendasController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetVendas()
        {
            var vendas = await _vendaService.ObterVendasAsync();
            return Ok(vendas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVenda(Guid id)
        {
            try
            {
                var venda = await _vendaService.ObterVendaPorIdAsync(id);
                return Ok(venda);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateVenda([FromBody] Venda venda)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdVenda = await _vendaService.CriarVendaAsync(venda);
            return CreatedAtAction(nameof(GetVenda), new { id = createdVenda.Id }, createdVenda);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVenda(Guid id, [FromBody] Venda venda)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != venda.Id)
                return BadRequest("O ID da URL não corresponde ao ID da venda.");

            var updatedVenda = await _vendaService.AtualizarVendaAsync(venda);
            return Ok(updatedVenda);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelVenda(Guid id)
        {
            var success = await _vendaService.CancelarVendaAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }
    }
}
