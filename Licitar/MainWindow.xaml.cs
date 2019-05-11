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

        /// <summary>
        /// Relação de lei sociais disponiveis para o orçamento
        /// </summary>
        public List<Page> Pages;

        public PageSystem CurrentPage { get; set; } = PageSystem.Orcamento;

        public MainWindow()
        {

            InitializeComponent();

            OrcamentoManager provider = Factory.AccessoAppProvider;

            Pages = new List<Page>();

            Pages.Add(new OrcamentoPage(provider));
            Pages.Add(new BaseInsumosPage());

            Conteudo.Content = Pages[0];

        }

        private async void ImportarInsumos_Click(object sender, RoutedEventArgs e)
        {

            await ImportarCsv.CarregarInsumosAsync();

            MessageBox.Show("Carregamento dos Insumos Terminado!!!");

            // ((BaseInsumosPage)Pages[1]).listaComposicoes.ItemsSource = Factory.AccessoAppProvider.Composicoes;
        }

        private async void ImportarComposicoes_Click(object sender, RoutedEventArgs e)
        {
            await ImportarCsv.CarregarComposicoesAsync();

            MessageBox.Show("Carregamento das Composições Terminado!!!");

            // ((BaseInsumosPage)Pages[1]).listaComposicoes.ItemsSource = Factory.AccessoAppProvider.Composicoes;
        }

        private void BaseInsumos_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = Pages[1];
            // CurrentPage = PageSystem.BaseDados;
        }

        private void Orcamento_Click(object sender, RoutedEventArgs e)
        {
            Conteudo.Content = Pages[0];
            // CurrentPage = PageSystem.Orcamento;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Localizar localizar = new Localizar();
            IInsumoGeral item;
            localizar.ShowDialog();
            LocalizarViewModel model = (LocalizarViewModel)localizar.DataContext;
            if (model.InsumoSelecionado != null) item = model.InsumoSelecionado;
        }
    }
}
