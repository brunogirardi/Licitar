using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Licitar
{
    public class OrcamentoLista : INotifyPropertyChanged
    {
        public ObservableCollection<IOrcamentoItens> Colecao { get; set; }

        public OrcamentoLista(ObservableCollection<IOrcamentoItens> itens)
        {
            this.Colecao = itens;

            this.RecalcularTotal();

            this.MonitorarTotalizadores();
        }

        #region Helper Methods

        /// <summary>
        /// Registra os eventos necessários para a atualização dos totais do orçamento
        /// </summary>
        private void MonitorarTotalizadores()
        {
            int indexer = 0;

            foreach (IOrcamentoItens item in Colecao)
            {
                if (item.ObjetoPai != null && (item.GetType() != typeof(OrcamentoTitulo)))
                {
                    item.PropertyChanged += CalcularValorTotal;
                }

                indexer++;
            }
        }

        /// <summary>
        /// Atualiza os totais do orçamento
        /// </summary>
        private void RecalcularTotal()
        {
            IOrcamentoItens objetoPai;
            IOrcamentoItens objetoFilho;

            // Reinicia o valor total de todos os titulos
            foreach (var item in Colecao)
            {
                if (item.GetType() == typeof(OrcamentoTitulo))
                {
                    item.ValorTotal = 0;
                }
            }

            for (int i = Colecao.Count - 1; i >= 0; i--)
            {
                if (Colecao[i].ObjetoPai != null)
                {
                    // Verifica se o item tem um objeto pai
                    int ObjetoPai = Colecao[i].ObjetoPai.GetValueOrDefault();
                    
                    // Link os objetos a serem manipulados
                    objetoPai = Colecao[ObjetoPai];
                    objetoFilho = Colecao[i];

                    objetoPai.ValorTotal += objetoFilho.ValorTotal;
                }

            }
        }

        #endregion

        private void CalcularValorTotal(object sender, PropertyChangedEventArgs e)
        {
              RecalcularTotal();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}