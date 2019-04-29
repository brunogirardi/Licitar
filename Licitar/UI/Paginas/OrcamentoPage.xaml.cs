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


        public OrcamentoPage(OrcamentoManager provider)
        {

            InitializeComponent();

            OrcamentoViewModel orcamentoViewModel = new OrcamentoViewModel(provider);

            OrcamentoGrid.DataContext = orcamentoViewModel;            

        }

    //    /// <summary>
    //    /// Botão responsavel por vincular o orçamento com a base de insumos do orçamento
    //    /// </summary>
    //    /// <param name="sender"></param>
    //    /// <param name="e"></param>
    //    private void OrcVincular_Click(object sender, RoutedEventArgs e)
    //    {

    //        if ((cbSelecionarBase.SelectedIndex == -1) || (dgListaLinkBase.SelectedItem == null) || (dgListaLinkOrcamento.SelectedItem == null)) return;

    //        OrcamentoItem destino;
    //        IInsumoGeral source;
    //        int id = 0;

    //        // Verifica se o item está vindo de alguma base externa
    //        if (cbSelecionarBase.SelectedIndex == 1)
    //        {

    //            IInsumoGeral Insumo = dgListaLinkBase.SelectedItem as IInsumoGeral;
    //            List<IInsumoGeral> Lista; 


    //            ObservableCollection<IInsumoGeral> baseOrcamento = _provider.BaseOrcamento;
    //            Lista = baseOrcamento.Where(x => x.CodigoRef == Insumo.Id.ToString()).ToList();

    //            // Verifica se o item faz parte da base, caso negativo adiciona na base
    //            if (Lista.Count() == 0)
    //            {
    //                // Adiciona o item a base do orçamento
    //                id = Factory.DBAcesso.InsumosOrcamentoAdicionarDaBase(Insumo.Id, 1);

    //                // Atualiza a base do orçamento com os itens importados
    //                Factory.DBAcesso.InsumosOrcamentoListaAtualizar(Factory.AccessoAppProvider.BaseOrcamento, 1);
    //            } else
    //            {
    //                id = Lista[0].Id;
    //            }

    //            // Localiza na lista da base do orçamento o item a ser linkado no orçamento
    //            source = baseOrcamento.Where(x => x.Id == id).First();

    //        } else
    //        {
    //            source = (IInsumoGeral)dgListaLinkBase.SelectedItem;
    //            id = source.Id;
    //        }

    //        foreach (var item in dgListaLinkOrcamento.SelectedItems)
    //        {

    //            destino = (OrcamentoItem)item;

    //            Factory.DBAcesso.OrcamentoVincularItem(id, destino.idOrcOrcamento);

    //            destino.Item = source;

    //        }

    //    }

    //    /// <summary>
    //    /// Botão responsavel por desvicular do orçamento o item da base do orçamento
    //    /// </summary>
    //    /// <param name="sender"></param>
    //    /// <param name="e"></param>
    //    private void OrcDesvincular_Click(object sender, RoutedEventArgs e)
    //    {

    //        var cpu = dgListaLinkOrcamento.SelectedItem;

    //        if (cpu == null)
    //        {
    //            MessageBox.Show("Favor selecionar o item do orçamento a ser desvinculado!");
    //            return;
    //        }

    //        // Verifica se o objeto é do tipo Cpu
    //        if (cpu.GetType() == typeof(OrcamentoItem))
    //        {
    //            OrcamentoItem item = dgListaLinkOrcamento.SelectedItem as OrcamentoItem;
    //            // Retira o vinculo no banco de dados
    //            Factory.DBAcesso.OrcamentoDesvincularItem(item.idOrcOrcamento);
    //            // Limpa o vinculo na aplicação
    //            item.Item = null;
    //        }

    //    }


    //    private void DgListaLinkBase_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //    {

    //    }
    }
}
