using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Interação lógica para BaseInsumosPage.xam
    /// </summary>
    public partial class BaseInsumosPage : Page
    {

        private ObservableCollection<IInsumoGeral> Insumos;

        private ObservableCollection<ICpuGeral> Composicoes;

        private ObservableCollection<BaseReferencia> Bases;

        private ObservableCollection<BaseReferenciaDataBase> Datas;

        public BaseInsumosPage()
        {
            InitializeComponent();

            Bases = Factory.DBAcesso.BaseRefLista();

            cbBaseReferencia.ItemsSource = Bases;


            listaInsumos.ItemsSource = Insumos;

        }

        /// <summary>
        /// Atualiza a lista ao substituir lista existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Insumos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // listaInsumos.ItemsSource = Factory.AccessoAppProvider.Insumos;
        }

        private void ListaComposicoes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ICpuGeral selecao = e.AddedItems[0] as ICpuGeral;

            // Verifica se a composição já foi preenchida com os itens
            if (e.AddedItems.Count > 0)
            {
                foreach (var item in Factory.DBAcesso.ComposiçãoListarItens(selecao.Id))
                {
                    selecao.Itens.Add(item);
                }
            }

        }

        private void CbBaseReferencia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BaseReferencia item = e.AddedItems[0] as BaseReferencia;

            Datas = Factory.DBAcesso.DataBaseDePrecoLista(item.idBaseReferencia);

            cbDataBase.ItemsSource = Datas;
        }

        private void CmdCarregarBase_Click(object sender, RoutedEventArgs e)
        {
            if (cbDataBase.SelectedItem != null)
            {
                BaseReferenciaDataBase item = cbDataBase.SelectedItem as BaseReferenciaDataBase;

                // Carrega os insumos
                Insumos = Factory.DBAcesso.InsumosListar(item.idRefPrecoBase);

                listaInsumos.ItemsSource = Insumos;

                // Carrega as composições

                listaComposicoes.ItemsSource = Factory.DBAcesso.ComposiçãoListar(item.idRefPrecoBase);


            }
        }
    }
}
