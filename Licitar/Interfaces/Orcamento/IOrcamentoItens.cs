using System.ComponentModel;

namespace Licitar
{
    public interface IOrcamentoItens : INotifyPropertyChanged
    {
        string Item { get; set; }

        int Sequencia { get; set; }

        string Descrição { get; set; }

        tipoInsumo Tipo { get; set; }

        int? ObjetoPai { get; set; }

        double ValorTotal { get; set; }

    }
}