namespace Vendas.Domain
{
    public class Venda
    {
        public Guid Id { get; set; }
        public DateTime DataVenda { get; set; }
        public Guid ClienteId { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public string Filial { get; set; } = string.Empty;
        public List<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
        public bool Cancelada { get; set; }
    }

    public class ItemVenda
    {
        public Guid ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Desconto { get; set; }
        public decimal ValorTotal => (ValorUnitario * Quantidade) - Desconto;
        public bool Cancelado { get; set; }
        public Guid VendaId { get; set; }
        public Venda? Venda { get; set; }
    }
}
