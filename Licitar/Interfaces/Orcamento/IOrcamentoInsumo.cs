using System.ComponentModel;

namespace Licitar
{
    public interface IOrcamentoInsumo : IOrcamentoItens, INotifyPropertyChanged, IValorComLeisSociais
    {

        int CodigoRef { get; set; }

        string Unidade { get; set; }

        double ValorUnitario { get; set; }

        double Quantidade { get; set; }

        IChaveValue Bdi { get; set; }

        double ValorComBdi { get; }

    }
}