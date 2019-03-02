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
        }

        private void CmdAdicionaItem_Click(object sender, RoutedEventArgs e)
        {
            _provider.Bdis[1].Valor = 30;
        }
    }
}
