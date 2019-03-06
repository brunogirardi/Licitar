using System.ComponentModel;

namespace Licitar
{
    public interface IOrcamentoItens : INotifyPropertyChanged
    {
        int idOrcOrcamento { get; set; }

        string Itemizacao { get; set; }

        int Sequencia { get; set; }

        int? idOrcOrcamentoPai { get; set; }

        string Descricao { get; set; }

        tipoInsumo Tipo { get; set; }

        double ValorTotal { get; set; }

        double ValorTotalVenda { get; set; }

    }
}