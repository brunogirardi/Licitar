using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Licitar
{
    class OrcamentoViewModel
    {

        #region Propriedades

        public OrcamentoManager Provider { get; set; }

        // Gerenciador do orçamento
        public ICollectionView Orcamento { get; set; }

        /// <summary>
        /// Armazena o item selecionado no orçamento
        /// </summary>
        public IOrcamentoItens ItemOrcamentoSelecionado { get; set; } = null;

        /// <summary>
        /// Armazena o item selecionado na lista de insumos da cpu
        /// </summary>
        public CpuCoefGeral ItemCpuSelecionados { get; set; } = null;

        public ICommand EdiçãoCpuAdicionarInsumo { get; set; }

        public ICommand EdiçãoCpuRemoverInsumo { get; set; }

        // Viculador com a base do orçamento
        public ObservableCollection<IInsumoGeral> BaseListaMostradaParaVinculor { get; set; }

        public ICollectionView BaseReferencia { get; set; }

        public IInsumoGeral BaseReferenciaSelecionado { get; set; }
        
        public string PesquisarBaseReferencia { get; set; } = "";

        public ICommand FiltrarBaseReferencia { get; set; }

        public ICollectionView BaseOrcamento { get; set; }

        public IOrcamentoItem BaseOrcamentoSelecionado { get; set; }

        private int baseReferenciaMostrada = 0;
        public int BaseReferenciaMostrada {
            get => baseReferenciaMostrada;
            set
            {
                baseReferenciaMostrada = value;
                
                // Atualiza a lista para a base selecionada
                AtualizarBaseParaVincular();
            }
        }

        public string PesquisarBaseOrcamento { get; set; } = "";

        public ICommand FiltrarBaseOrcamento { get; set; }

        public ObservableCollection<Bdi> Bdi { get; set; }

        public ICommand VincularBaseOrcamento { get; set; }

        public ICommand DesvincularBaseOrcamento { get; set; }


        #endregion

        /// <summary>
        /// Contrutor padrão do View Model
        /// </summary>
        /// <param name="provider"></param>
        public OrcamentoViewModel(OrcamentoManager provider)
        {
            Provider = provider;

            #region Vincular Orçamento
            Orcamento = CollectionViewSource.GetDefaultView(Provider.Orcamento.Colecao);

            BaseOrcamento = CollectionViewSource.GetDefaultView(Provider.Orcamento.Colecao);
            BaseOrcamento.Filter = FiltrarOrcamentoDescrição;

            BaseListaMostradaParaVinculor = new ObservableCollection<IInsumoGeral>();
            BaseReferencia = CollectionViewSource.GetDefaultView(BaseListaMostradaParaVinculor);
            BaseReferencia.Filter = FiltrarInsumosDescrição;
            BaseReferenciaMostrada = 0;

            // Comandos
            FiltrarBaseReferencia = new RelayCommand(new Action<object>(ExecuteFiltroBaseReferencia));
            FiltrarBaseOrcamento = new RelayCommand(new Action<object>(ExecuteFiltroBaseOrcamento));

            EdiçãoCpuAdicionarInsumo = new RelayCommand(new Action<object>(ExecuteEditarCpuAdicionarItem), new Func<object, bool>(CanExecuteEditarCpuAdicionarItem));
            EdiçãoCpuRemoverInsumo = new RelayCommand(new Action<object>(ExecuteEditarCpuRemoverItem), new Func<object, bool>(CanExecuteEditarCpuRemoverItem));

            VincularBaseOrcamento = new RelayCommand(new Action<object>(ExecuteVincularOrcamento), new Func<object, bool>(CanExecuteVincularOrcamento));
            DesvincularBaseOrcamento = new RelayCommand(new Action<object>(ExecuteDesvincularOrcamento), new Func<object, bool>(CanExecuteDesvincularOrcamento));

            #endregion

        }

        #region Comandos para Editar cpu

        private void ExecuteEditarCpuRemoverItem(object obj)
        {

            OrcamentoItem cpu = ItemOrcamentoSelecionado as OrcamentoItem;

            ((CpuGeral)cpu.Item).RemoverItem(ItemCpuSelecionados);

        }

        private bool CanExecuteEditarCpuRemoverItem(object obj)
        {
            if (ItemCpuSelecionados != null) return true;

            return false;
        }

        private void ExecuteEditarCpuAdicionarItem(object obj)
        {

            OrcamentoItem selecionado = ItemOrcamentoSelecionado as OrcamentoItem;

            if (selecionado.Item.GetType() == typeof(CpuGeral))
            {
                Localizar LocalizarInsumo = new Localizar();

                LocalizarInsumo.ShowDialog();

                IInsumoGeral ItemSelecionado = ((LocalizarViewModel)LocalizarInsumo.DataContext).InsumoSelecionado;

                if (ItemSelecionado != null)
                {
                    CpuCoefGeral NovoItem = new CpuCoefGeral(ItemSelecionado);

                    ((CpuGeral)selecionado.Item).AdicionarItem(NovoItem);
                }

            }

        }

        private bool CanExecuteEditarCpuAdicionarItem(object obj)
        {
            OrcamentoItem selecionado = ItemOrcamentoSelecionado as OrcamentoItem;

            if ((ItemOrcamentoSelecionado != null) && 
                (ItemOrcamentoSelecionado.GetType() == typeof(OrcamentoItem)) &&
                (selecionado.Item != null) &&
                (selecionado.Item.GetType() == typeof(CpuGeral)))
                    return true;

            return false;

        }

        #endregion

        #region Comandos para Vincular com a Base

        /// <summary>
        /// Executa o filtro com base no termo <see cref="PesquisarBaseReferencia"/> na lista da referencia
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteFiltroBaseReferencia(object obj)
        {
            BaseReferencia.Refresh();
        }

        /// <summary>
        /// Executa o filtro com base no termo <see cref="PesquisarBaseOrcamento"/> na lista do orçamento
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteFiltroBaseOrcamento(object obj)
        {
            BaseOrcamento.Refresh();
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

            string busca = PesquisarBaseReferencia;
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

            string busca = PesquisarBaseOrcamento;
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
        /// Metodo responsável por vincular os itens da base de referencia aos itens do orçamento
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteVincularOrcamento(object obj)
        {

            IInsumoGeral source;
            int id = 0;

            // Verifica se o item está vindo de alguma base externa
            if (BaseReferenciaMostrada == 1)
            {

                IInsumoGeral Insumo = BaseReferenciaSelecionado;
                List<IInsumoGeral> Lista;


                ObservableCollection<IInsumoGeral> baseOrcamento = Factory.AccessoAppProvider.BaseOrcamento;
                Lista = baseOrcamento.Where(x => x.CodigoRef == Insumo.Id.ToString()).ToList();

                // Verifica se o item faz parte da base, caso negativo adiciona na base
                if (Lista.Count() == 0)
                {
                    // Adiciona o item a base do orçamento
                    id = Factory.DBAcesso.InsumosOrcamentoAdicionarDaBase(Insumo.Id, 1);

                    // Atualiza a base do orçamento com os itens importados
                    Factory.DBAcesso.InsumosOrcamentoListaAtualizar(Factory.AccessoAppProvider.BaseOrcamento, 1);
                }
                else
                {
                    id = Lista[0].Id;
                }

                // Localiza na lista da base do orçamento o item a ser linkado no orçamento
                source = Factory.AccessoAppProvider.BaseOrcamento.Where(x => x.Id == id).First();

            }
            else
            {
                source = BaseReferenciaSelecionado;
                id = source.Id;
            }

            Factory.DBAcesso.OrcamentoVincularItem(id, BaseOrcamentoSelecionado.idOrcOrcamento);

            BaseOrcamentoSelecionado.Item = source;
        }

        /// <summary>
        /// Metodo que libera para utilização do comando de vincular se as premissas básicas foram atendidas
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanExecuteVincularOrcamento(object obj)
        {
            // Verifica se há itens selecionados na lista da base de referencia e no orçamento
            if((BaseReferenciaSelecionado != null) && (BaseOrcamentoSelecionado != null) && (BaseReferenciaMostrada >= 0))
                return true;

            return false;
        }

        /// <summary>
        /// Metodo ressponsável por desvincular um item do orçamento
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteDesvincularOrcamento(object obj)
        {
            // Verifica se o objeto é do tipo Cpu
            if (BaseOrcamentoSelecionado.GetType() == typeof(OrcamentoItem))
            {
                // Retira o vinculo no banco de dados
                Factory.DBAcesso.OrcamentoDesvincularItem(BaseOrcamentoSelecionado.idOrcOrcamento);
                // Limpa o vinculo na aplicação
                BaseOrcamentoSelecionado.Item = null;
            }
        }

        /// <summary>
        /// Metodo que libera o botão do comando desvincular
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanExecuteDesvincularOrcamento(object obj)
        {
            // Verifica se há itens selecionados na lista da base de referencia e no orçamento
            if (BaseOrcamentoSelecionado != null)
                return true;

            return false;
        }

        #endregion

        #region Helper Functions para Vincular com a Base

        /// <summary>
        /// Atualiza a lista de itens que será mostrada na base de referencia
        /// </summary>
        private void AtualizarBaseParaVincular()
        {

            BaseListaMostradaParaVinculor.Clear();

            ObservableCollection<IInsumoGeral> ListaAMostrar;

            if (baseReferenciaMostrada == 0)
            {
                ListaAMostrar = Provider.BaseOrcamento;
            }
            else
            {
                ListaAMostrar = Provider.BaseReferencia;
            }

            foreach (IInsumoGeral item in ListaAMostrar)
            {
                BaseListaMostradaParaVinculor.Add(item);
            }

            BaseReferencia.Refresh();

        }

        #endregion

    }
}
