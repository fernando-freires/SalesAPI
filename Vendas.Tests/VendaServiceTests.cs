using Vendas.Application.Services;
using Vendas.Data.Context;
using Vendas.Domain;
using FluentAssertions;
using NSubstitute;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Vendas.Tests
{
    public class VendaServiceTests
    {
        private readonly IVendaService _vendaService;
        private readonly VendasContext _context;
        private readonly ILogger<VendaService> _logger;

        public VendaServiceTests()
        {
            var options = new DbContextOptionsBuilder<VendasContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new VendasContext(options);

            _logger = Substitute.For<ILogger<VendaService>>();
            _vendaService = new VendaService(_context, _logger);
        }

        [Fact]
public async Task CriarVenda_DeveRegistrarVendaComSucesso()
{
    var faker = new Faker("pt_BR");
    var venda = new Venda
    {
        ClienteId = Guid.NewGuid(),
        ClienteNome = faker.Person.FullName,
        DataVenda = DateTime.UtcNow,
        Filial = faker.Company.CompanyName(),
        Itens = new List<ItemVenda>
        {
            new ItemVenda
            {
                ProdutoNome = faker.Commerce.ProductName(),
                Quantidade = faker.Random.Int(1, 10),
                ValorUnitario = faker.Random.Decimal(10, 100),
                Desconto = faker.Random.Decimal(0, 10)
            }
        },
        ValorTotal = faker.Random.Decimal(100, 1000),
        Cancelada = false
    };

    var result = await _vendaService.CriarVendaAsync(venda);

    result.Should().NotBeNull();
    result.Id.Should().NotBeEmpty();
    result.ClienteNome.Should().Be(venda.ClienteNome);
    result.Itens.Should().HaveCount(1);
    result.Itens.First().ProdutoNome.Should().Be(venda.Itens.First().ProdutoNome);

    _logger.Received(1).Log(
        LogLevel.Information,
        Arg.Any<EventId>(),
        Arg.Is<object>(v => v.ToString().Contains($"Venda {result.Id} registrada com sucesso")),
        null,
        Arg.Any<Func<object, Exception?, string>>()
    );
}

        [Fact]
        public async Task ObterVendaPorId_DeveRetornarVendaQuandoExistente()
        {
            var venda = new Venda
            {
                Id = Guid.NewGuid(),
                ClienteId = Guid.NewGuid(),
                ClienteNome = "Teste Cliente",
                DataVenda = DateTime.UtcNow,
                Filial = "Filial Teste",
                Itens = new List<ItemVenda>
                {
                    new ItemVenda
                    {
                        ProdutoId = Guid.NewGuid(),
                        ProdutoNome = "Produto Teste",
                        Quantidade = 2,
                        ValorUnitario = 100,
                        Desconto = 10
                    }
                },
                ValorTotal = 190,
                Cancelada = false
            };
            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();

            var result = await _vendaService.ObterVendaPorIdAsync(venda.Id);

            result.Should().NotBeNull();
            result.Id.Should().Be(venda.Id);
            result.ClienteNome.Should().Be("Teste Cliente");
        }

        [Fact]
        public async Task CancelarVenda_DeveRemoverVendaQuandoExistente()
        {
            var venda = new Venda
            {
                Id = Guid.NewGuid(),
                ClienteId = Guid.NewGuid(),
                ClienteNome = "Cliente Cancelado",
                DataVenda = DateTime.UtcNow,
                Filial = "Filial Teste",
                Itens = new List<ItemVenda>
                {
                    new ItemVenda
                    {
                        ProdutoId = Guid.NewGuid(),
                        ProdutoNome = "Produto Teste",
                        Quantidade = 2,
                        ValorUnitario = 100,
                        Desconto = 10
                    }
                },
                ValorTotal = 190,
                Cancelada = false
            };
            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();

            var result = await _vendaService.CancelarVendaAsync(venda.Id);

            var vendaCancelada = await _context.Vendas.FindAsync(venda.Id);
            result.Should().BeTrue();
            vendaCancelada.Should().BeNull();
        }
    }
}
