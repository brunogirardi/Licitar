using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Relação de BDIs disponiveis para o orçamento
        /// </summary>
        public ObservableCollection<IChaveValue> bdis;

        /// <summary>
        /// Relação de lei sociais disponiveis para o orçamento
        /// </summary>
        public ObservableCollection<IChaveValue> leiSociais;

        public MainWindow()
        {

            InitializeComponent();

            Provider provider = new Provider();

            OrcamentoPage frame = new OrcamentoPage(provider);

            Conteudo.Content = frame;

        }
    }
}
