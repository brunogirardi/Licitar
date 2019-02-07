using System.ComponentModel;

namespace Licitar
{
    public class OrcamentoTitulo : IOrcamentoItens
    {
        public string Item { get; set; }

        public int Sequencia { get; set; }

        public int? ObjetoPai { get; set; }

        public double ValorTotal { get; set; }

        public string Descrição { get; set; }

        public tipoInsumo Tipo { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}