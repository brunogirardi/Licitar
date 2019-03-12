using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Licitar
{
    /// <summary>
    /// Interação lógica para Orcamento.xam
    /// </summary>
    public partial class OrcamentoPage : Page
    {

        private OrcamentoManager _provider;

        public OrcamentoPage(OrcamentoManager provider)
        {
            InitializeComponent();

            _provider = provider;

            DataContext = _provider;

            listBdi.ItemsSource = _provider.Bdis;

            listInsumos.ItemsSource = _provider.Orcamento.Colecao;

            dgListaLinkOrcamento.ItemsSource = _provider.Orcamento.Colecao;

            dgListaLinkBase.ItemsSource = _provider.BaseOrcamento;
        }

        private void CmdAdicionaItem_Click(object sender, RoutedEventArgs e)
        {
            _provider.Bdis[1].Valor = 30;
        }

        private void CbSelecionarBase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ((ComboBox)sender).SelectedIndex;

            switch (index)
            {
                case 0:
                    dgListaLinkBase.ItemsSource = _provider.BaseOrcamento;
                    break;
                case 1:
                    dgListaLinkBase.ItemsSource = _provider.BaseReferencia;
                    break;
            }
        }

        /// <summary>
        /// Botão responsavel por vincular o orçamento com a base de insumos do orçamento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrcVincular_Click(object sender, RoutedEventArgs e)
        {
            // Verifica se as duas listas tem itens selecionados
            if ((dgListaLinkOrcamento.SelectedItems.Count > 0) && (dgListaLinkBase.SelectedItems.Count > 0))
            {

            }
        }
    }
}
