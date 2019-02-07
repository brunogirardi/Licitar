using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Licitar
{
    class CpuGeral : ICpuGeral
    {

        public int CodigoRef { get; set; }

        public string Descrição { get; set; }

        public tipoInsumo Tipo { get; set; }

        public string Unidade { get; set; }

        public double Quantidade { get; set; }

        public double ValorUnitario => Itens.Sum(x => x.ValorTotal);

        public double ValorTotal { get; set; }

        public ObservableCollection<IInsumoGeral> Itens { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
