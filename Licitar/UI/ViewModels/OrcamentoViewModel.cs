using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public IOrcamentoItens ItemOrcamentoSelecionado { get; set; }

        // Viculador com a base do orçamento
        public ICollectionView BaseReferencia { get; set; }

        public string PesquisarBaseReferencia { get; set; } = "";

        public ICommand FiltrarBaseReferencia { get; set; }

        public ICollectionView BaseOrcamento { get; set; }

        public string PesquisarBaseOrcamento { get; set; } = "";

        public ICommand FiltrarBaseOrcamento { get; set; }

        public ObservableCollection<Bdi> Bdi { get; set; }

        public ICommand VincularBaseOrcamento { get; set; }

        public ICommand DesvincularBaseOrcamento { get; set; }

        private bool _InsertItemMode;
        public bool InsertItemMode1
        {
            get { return _InsertItemMode; }
            set { _InsertItemMode = value; }
        }

        private tipoInsumo _TipoItem;
        public tipoInsumo TipoItem
        {
            get { return _TipoItem; }
            set { _TipoItem = value; }
        }

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

            BaseReferencia = CollectionViewSource.GetDefaultView(Provider.BaseReferencia);
            BaseReferencia.Filter = FiltrarInsumosDescrição;

            // Comandos
            FiltrarBaseReferencia = new RelayCommand(new Action<object>(ExecuteFiltroBaseReferencia));
            FiltrarBaseOrcamento = new RelayCommand(new Action<object>(ExecuteFiltroBaseOrcamento));

            #endregion

        }

        #region Funções Auxiliares

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

        #endregion

    }
}
