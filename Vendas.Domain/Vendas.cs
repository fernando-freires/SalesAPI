public class Venda
{
    public Guid Id { get; set; }
    public DateTime DataVenda { get; set; }
    public Guid ClienteId { get; set; }
    public string ClienteNome { get; set; }
    public decimal ValorTotal { get; set; }
    public string Filial { get; set; }
    public List<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
    public bool Cancelada { get; set; }
}

public class ItemVenda
{
    public Guid ProdutoId { get; set; }
    public string ProdutoNome { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal Desconto { get; set; }
    public decimal ValorTotal => (ValorUnitario * Quantidade) - Desconto;
    public bool Cancelado { get; set; }
}
