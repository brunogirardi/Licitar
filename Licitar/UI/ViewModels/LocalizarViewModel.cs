using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Licitar
{
    class LocalizarViewModel : INotifyPropertyChanged
    {

        #region Propriedades

        /// <summary>
        /// Listagem de todos os insumos disponiveis no orçamento
        /// </summary>
        public ICollectionView Colecao { get; set; }

        public IInsumoGeral InsumoSelecionado { get; set; }

        public string TextoPesquisa { get; set; } = "";

        public ICommand SelecionarItem { get; set; }

        public ICommand PesquisarInsumos { get; set; }

        #endregion

        #region Construtor

        /// <summary>
        /// Construtor da classe
        /// </summary>
        public LocalizarViewModel()
        {

            // Carrega a lista de insumos da base
            Colecao = CollectionViewSource.GetDefaultView(Factory.AccessoAppProvider.BaseReferencia);
            Colecao.Filter = FiltrarInsumosDescrição;

            // Commands
            SelecionarItem = new RelayCommand(new Action<object>(ExecuteSelecionarItem), new Func<object, bool>(CanSelecionarItem));
            PesquisarInsumos = new RelayCommand(new Action<object>(ExecutePesquisarInsumos), new Func<object, bool>(CanPesquisarInsumos));

        }

        #endregion

        #region Eventos

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Commandos

        /// <summary>
        /// Executa apartir do clique do botão de Selecionar
        /// </summary>
        /// <param name="obj"></param>
        public void ExecuteSelecionarItem(object obj)
        {
            Localizar localizar = obj as Localizar;
            localizar.Close();
        }

        /// <summary>
        /// Verifica se o botão selecionar está apto a ser executado
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CanSelecionarItem(object obj)
        {
            if (InsumoSelecionado != null) return true;

            return false;
        }

        /// <summary>
        /// Executa apartir do clique do botão pesquisar
        /// </summary>
        /// <param name="obj"></param>
        public void ExecutePesquisarInsumos(object obj)
        {
            Colecao.Refresh();
        }

        /// <summary>
        /// Verifica se o campo de texto foi preenchido
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CanPesquisarInsumos(object obj)
        {
            if (TextoPesquisa.Length > 0) return true;

            return false;
        }


        #endregion

        #region Helpers

        /// <summary>
        /// Filtra por descrição
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool FiltrarInsumosDescrição(object obj)
        {
            IInsumoGeral insumo = obj as IInsumoGeral;

            bool resultado = true;

            string busca = TextoPesquisa;
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

        #endregion

    }
}
