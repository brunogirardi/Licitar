using System.ComponentModel;

namespace Licitar
{
    public class OrcamentoTitulo : IOrcamentoItens
    {
        public int idOrcOrcamento { get; set; }

        public string Itemizacao { get; set; }

        public int Sequencia { get; set; }

        public int? idOrcOrcamentoPai { get; set; }

        public double ValorTotal { get; set; }

        public string Descricao { get; set; }

        public tipoInsumo Tipo { get; set; }

        public double ValorTotalVenda { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}