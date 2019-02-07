using System.Collections.ObjectModel;

namespace Licitar
{
    interface IOrcamentoCpu : IOrcamentoItens
    {
        /// <summary>
        /// Lista de insumos/serviços da cpu
        /// </summary>
        int CodigoRef { get; set; }

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
        /// Armazena os itens da composição
        /// </summary>
        ObservableCollection<IInsumoGeral> Itens { get; set; }

        /// <summary>
        /// Calcula o valor total do tipo de insumo
        /// </summary>
        /// <param name="tipo">Tipo do Insumo</param>
        /// <returns></returns>
        double ValorTotalTipo(tipoInsumo tipo);

        /// <summary>
        /// Calcula o valor total das Leis Sociais
        /// </summary>
        /// <returns></returns>
        double ValorTotalLeisSociais();

    }
}