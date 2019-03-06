using System.Collections.ObjectModel;

namespace Licitar
{
    public interface IOrcamentoCpu : IOrcamentoItens, IValorComLeisSociais, IOrcamentoVenda
    {
        /// <summary>
        /// Lista de insumos/serviços da cpu
        /// </summary>
        string CodigoRef { get; set; }

        /// <summary>
        /// Unidade de medida do serviço
        /// </summary>
        string Unidade { get; set; }

        /// <summary>
        /// Valor uniário do serviço
        /// </summary>
        double ValorUnitario { get; }
        
        /// <summary>
        /// Quantidade prevista no orçamento
        /// </summary>
        double Quantidade { get; set; }

        /// <summary>
        /// Valor do serviço com BDI
        /// </summary>
        double ValorComBdi { get; }

        /// <summary>
        /// BDI definido a ser aplicado sobre o serviço
        /// </summary>
        IChaveValue Bdi { get; set; }

        /// <summary>
        /// Relaciona uma composição / insumo ao item do orçamento
        /// </summary>
        IInsumoGeral Item{ get; set; }


    }
}