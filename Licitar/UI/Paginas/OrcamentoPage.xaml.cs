using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        private ICollectionView BaseReferencia;

        private ICollectionView BaseOrcamento;

        public OrcamentoPage(OrcamentoManager provider)
        {
            InitializeComponent();

            _provider = provider;

            DataContext = _provider;

            listBdi.ItemsSource = _provider.Bdis;

            listInsumos.ItemsSource = _provider.Orcamento.Colecao;

            BaseOrcamento = CollectionViewSource.GetDefaultView(_provider.Orcamento.Colecao);
            dgListaLinkOrcamento.ItemsSource = BaseOrcamento;
            BaseOrcamento.Filter = FiltrarOrcamentoDescrição;

            BaseReferencia = CollectionViewSource.GetDefaultView(_provider.BaseReferencia);
            dgListaLinkBase.ItemsSource = BaseReferencia;
            BaseReferencia.Filter = FiltrarInsumosDescrição;

        }

        /// <summary>
        /// Metodo responsável por aplicar filtros na base de referencia
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool FiltrarInsumosDescrição(object obj)
        {
            IInsumoGeral insumo = obj as IInsumoGeral;

            bool resultado = true;

            string busca = edtPesquisarBaseReferencia.Text;
            List<string> buscaArray = new List<string>(busca.Split('%'));

            if (busca.Length == 0) return true;

            if (busca[0] != '%')
            {
                if (!insumo.Descrição.ToUpper().StartsWith(buscaArray[0].ToUpper()))
                    return false;

                buscaArray.RemoveAt(0);
            }

            foreach (string trecho in buscaArray)
            {
                if (insumo.Descrição.ToUpper().Contains(trecho.ToUpper()) == false)
                    return false;
            }

            return resultado;
        }

        /// <summary>
        /// Metodo responsável por aplicar filtros no orçamento
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool FiltrarOrcamentoDescrição(object obj)
        {
            IOrcamentoItens orcamento = obj as IOrcamentoItens;

            bool resultado = true;

            string busca = edtPesquisarOrcamento.Text;
            List<string> buscaArray = new List<string>(busca.Split('%'));

            if (busca.Length == 0) return true;

            if (busca[0] != '%')
            {
                if (!orcamento.Descricao.ToUpper().StartsWith(buscaArray[0].ToUpper()))
                    return false;

                buscaArray.RemoveAt(0);
            }

            foreach (string trecho in buscaArray)
            {
                if (orcamento.Descricao.ToUpper().Contains(trecho.ToUpper()) == false)
                    return false;
            }

            return resultado;
        }

        /// <summary>
        /// Aplica o filtro ao clicar no botão de pesquisa da base de referencia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdPesquisarBaseReferencia_Click(object sender, RoutedEventArgs e)
        {
            BaseReferencia.Refresh();
        }

        /// <summary>
        /// Aplica o filtro ao clicar no botão de pesquisa do orçamento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdPesquisarOrcamento_Click(object sender, RoutedEventArgs e)
        {
            BaseOrcamento.Refresh();
        }

        /// <summary>
        /// Altera a fonte das cpus/insumos da base de referencia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbSelecionarBase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ((ComboBox)sender).SelectedIndex;

            switch (index)
            {
                case 0:
                    BaseReferencia = CollectionViewSource.GetDefaultView(_provider.BaseOrcamento);
                    break;
                case 1:
                    BaseReferencia = CollectionViewSource.GetDefaultView(_provider.BaseReferencia);
                    break;
            }
            dgListaLinkBase.ItemsSource = BaseReferencia;
            BaseReferencia.Filter = FiltrarInsumosDescrição;
            BaseReferencia.Refresh();
        }

        /// <summary>
        /// Botão responsavel por vincular o orçamento com a base de insumos do orçamento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrcVincular_Click(object sender, RoutedEventArgs e)
        {

            if ((cbSelecionarBase.SelectedIndex == -1) || (dgListaLinkBase.SelectedItem == null) || (dgListaLinkOrcamento.SelectedItem == null)) return;

            OrcamentoItem destino;
            IInsumoGeral source;
            int id = 0;

            // Verifica se o item está vindo de alguma base externa
            if (cbSelecionarBase.SelectedIndex == 1)
            {

                IInsumoGeral Insumo = dgListaLinkBase.SelectedItem as IInsumoGeral;
                List<IInsumoGeral> Lista; 


                ObservableCollection<IInsumoGeral> baseOrcamento = _provider.BaseOrcamento;
                Lista = baseOrcamento.Where(x => x.CodigoRef == Insumo.Id.ToString()).ToList();

                // Verifica se o item faz parte da base, caso negativo adiciona na base
                if (Lista.Count() == 0)
                {
                    // Adiciona o item a base do orçamento
                    id = Factory.DBAcesso.InsumosOrcamentoAdicionarDaBase(Insumo.Id, 1);

                    // Atualiza a base do orçamento com os itens importados
                    Factory.DBAcesso.InsumosOrcamentoListaAtualizar(Factory.AccessoAppProvider.BaseOrcamento, 1);
                } else
                {
                    id = Lista[0].Id;
                }

                // Localiza na lista da base do orçamento o item a ser linkado no orçamento
                source = baseOrcamento.Where(x => x.Id == id).First();

            } else
            {
                source = (IInsumoGeral)dgListaLinkBase.SelectedItem;
                id = source.Id;
            }

            foreach (var item in dgListaLinkOrcamento.SelectedItems)
            {

                destino = (OrcamentoItem)item;

                Factory.DBAcesso.OrcamentoVincularItem(id, destino.idOrcOrcamento);

                destino.Item = source;

            }

        }

        private void OrcDesvincular_Click(object sender, RoutedEventArgs e)
        {

            var cpu = dgListaLinkOrcamento.SelectedItem;

            if (cpu == null)
            {
                MessageBox.Show("Favor selecionar o item do orçamento a ser desvinculado!");
                return;
            }

            // Verifica se o objeto é do tipo Cpu
            if (cpu.GetType() == typeof(OrcamentoItem))
            {
                OrcamentoItem item = dgListaLinkOrcamento.SelectedItem as OrcamentoItem;
                // Retira o vinculo no banco de dados
                Factory.DBAcesso.OrcamentoDesvincularItem(item.idOrcOrcamento);
                // Limpa o vinculo na aplicação
                item.Item = null;
            }

        }
    }
}
