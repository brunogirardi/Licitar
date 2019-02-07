using System.ComponentModel;

namespace Licitar
{
    public interface IOrcamentoInsumo : IOrcamentoItens, INotifyPropertyChanged
    {

        int CodigoRef { get; set; }

        string Unidade { get; set; }

        double ValorUnitario { get; set; }

        double Quantidade { get; set; }

        double ValorComBdi { get; }

        IChaveValue Bdi { get; set; }

    }
}