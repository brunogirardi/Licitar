using System.ComponentModel;

namespace Licitar
{
    public interface IInsumoGeral : INotifyPropertyChanged
    {

        /// <summary>
        /// Recebe a id proveniente do banco de dados
        /// </summary>
        int Id { get; set; }
        
        /// <summary>
        /// Código de referencia de bases auxiliares SINAPI / SEDOP / SEINFRA e etc...
        /// </summary>
        string CodigoRef { get; set; }

        /// <summary>
        /// Descrição do serviço
        /// </summary>
        string Descrição { get; set; }

        /// <summary>
        /// Tipo de insumo
        /// </summary>
        tipoInsumo Tipo { get; set; }

        /// <summary>
        /// Unidade de medida do serviço
        /// </summary>
        string Unidade { get; set; }

        /// <summary>
        /// Quantidade prevista no orçamento
        /// </summary>
        double Quantidade { get; set; }

        /// <summary>
        /// Valor uniário do serviço
        /// </summary>
        double ValorUnitario { get; }

        /// <summary>
        /// Valor total do item
        /// </summary>
        double ValorTotal { get; }

        /// <summary>
        /// Retorna o valor unitário com leis sociais
        /// </summary>
        double ValorUnitarioComLS { get; }

        /// <summary>
        /// Retorna o valor total com leis sociais
        /// </summary>
        double ValorTotalComLS { get; }

    }
}
