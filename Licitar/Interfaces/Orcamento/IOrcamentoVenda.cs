namespace Licitar
{

    /// <summary>
    /// Implementa o registro dos preços de venda
    /// </summary>
    public interface IOrcamentoVenda
    {
        /// <summary>
        /// Preço unitário da venda do item
        /// </summary>
        double ValorUnitarioVenda { get; set; }

    }
}
