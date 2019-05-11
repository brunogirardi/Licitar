using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Licitar
{
    public class OrcamentoLista : INotifyPropertyChanged
    {

        #region Propriedades

        /// <summary>
        /// Propriedade que segura a lista de itens do orçamento
        /// </summary>
        public ObservableCollection<IOrcamentoItens> Colecao { get; private set; }

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor responsável por montar o orçamento a partir do resultado do banco de dados
        /// Transforma uma <see cref="ItensOrcamentoDb" /> para o formato do orçamento
        /// </summary>
        /// <param name="lista">Lista de itens vindo do bando de dados</param>
        public OrcamentoLista(List<ItensOrcamentoDb> lista)
        {
            ObservableCollection<IOrcamentoItens> NovaLista = new ObservableCollection<IOrcamentoItens>();

            foreach (var item in lista)
            {
                /// Converte o registro em uma instacia de <see cref="OrcamentoTitulo"/>
                if (item.Titulo == true)
                {
                    NovaLista.Add(new OrcamentoTitulo()
                    {
                        idOrcOrcamento = item.idOrcOrcamento,
                        Itemizacao = item.Itemizacao,
                        Sequencia = item.Sequencia,
                        idOrcOrcamentoPai = item.idOrcOrcamentoPai,
                        Descricao = item.Descricao,
                        Tipo = tipoInsumo.Titulo,
                    });
                }

                /// Converte o registro em uma instacia de <see cref="OrcamentoItem"/>
                else
                {
                    OrcamentoItem novoItem = new OrcamentoItem()
                    {
                        idOrcOrcamento = item.idOrcOrcamento,
                        Itemizacao = item.Itemizacao,
                        Sequencia = item.Sequencia,
                        idOrcOrcamentoPai = item.idOrcOrcamentoPai,
                        Descricao = item.Descricao,
                        Unidade = item.Unidade,
                        CodigoRef = item.CodBaseRef,
                        Quantidade = item.Quantidade,
                        ValorUnitarioVenda = item.ValorUnitarioVenda,
                        Bdi = Factory.Bdis.Where(x => x.Id == item.idOrcBdis).First(),
                    };

                    if (item.idOrcInsumos != 0)
                        novoItem.Item = Factory.AccessoAppProvider.BaseOrcamento.Where(x => x.Id == item.idOrcInsumos).First();

                    NovaLista.Add(novoItem);

                }

            }

            this.Colecao = NovaLista;

            Reordenar();

            Itemizar();

            this.RecalcularTotal();

            this.MonitorarTotalizadores();

        }

        /// <summary>
        /// Construtor responsável por iniciar a instancia apartir de uma coleção já formatada
        /// </summary>
        /// <param name="itens"></param>
        public OrcamentoLista(ObservableCollection<IOrcamentoItens> itens)
        {
            this.Colecao = itens;

            this.RecalcularTotal();

            this.MonitorarTotalizadores();
        }

        #endregion

        #region Public Methods

        public void AdicionarItem(IOrcamentoItens Item, int Posicao)
        {
            Colecao.Insert(Posicao, Item);

            Reordenar();

            Itemizar();
        }

        public void AdicionarItem(IOrcamentoItens Item)
        {
            Colecao.Add(Item);

            Reordenar();

            Itemizar();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Registra os eventos necessários para a atualização dos totais do orçamento
        /// </summary>
        private void MonitorarTotalizadores()
        {
            int indexer = 0;

            foreach (IOrcamentoItens item in Colecao)
            {
                if (item.idOrcOrcamentoPai != null && (item.GetType() != typeof(OrcamentoTitulo)))
                {
                    item.PropertyChanged += CalcularTotais;
                }

                indexer++;
            }
        }

        /// <summary>
        /// Atualiza os totais do orçamento
        /// </summary>
        private void RecalcularTotal()
        {
            IOrcamentoItens objetoPai;
            IOrcamentoItens objetoFilho;

            // Reinicia o valor total de todos os titulos
            foreach (var item in Colecao)
            {
                if (item.GetType() == typeof(OrcamentoTitulo))
                {
                    item.ValorTotal = 0;
                    item.ValorTotalVenda = 0;
                }
            }

            for (int i = Colecao.Count - 1; i >= 0; i--)

            {
                if (Colecao[i].idOrcOrcamentoPai != null)
                {

                    // Verifica se o item tem um objeto pai
                    int ObjetoPai = IndexDoTituloPai(Colecao[i].idOrcOrcamentoPai.GetValueOrDefault());

                    if (i != ObjetoPai)
                    {
                        // Link os objetos a serem manipulados
                        objetoPai = Colecao[ObjetoPai];
                        objetoFilho = Colecao[i];

                        objetoPai.ValorTotal += objetoFilho.ValorTotal;
                        objetoPai.ValorTotalVenda += objetoFilho.ValorTotalVenda;
                    }
                }
            }
        }

        /// <summary>
        /// Gera a itemização dos itens do orçamento
        /// </summary>
        /// <returns></returns>
        private void Itemizar(IOrcamentoItens Pai = null)
        {

            IEnumerable<IOrcamentoItens> itens;

            // Verifica se o pai e nulo e inicia o processo
            if (Pai == null) { itens = Colecao.Where(x => x.idOrcOrcamentoPai == null); }
            else { itens = Colecao.Where(x => x.idOrcOrcamentoPai == Pai.idOrcOrcamento); }

            // Monta itemizacao dos subitens
            foreach (var item in itens)
            {
                item.Itemizacao = Pai.Itemizacao + "." + item.Sequencia.ToString();
                Itemizar(item);
            }

        }

        /// <summary>
        /// Reordena os itens da coleção
        /// </summary>
        private void Reordenar()
        {

            List<IOrcamentoItens> ListaReordenada = new List<IOrcamentoItens>();
            IEnumerable<IOrcamentoItens> Itens;

            Itens = Colecao.Where(x => x.idOrcOrcamentoPai == 0);

            // Monta itemizacao dos subitens
            foreach (var item in Itens)
            {
                ListaReordenada.Add(item);
                OrdenarItens(item, ListaReordenada);
            }

            Colecao = new ObservableCollection<IOrcamentoItens>(ListaReordenada);

        }

        /// <summary>
        /// Reordena os subitens da coleção, função trabalha em conjunto com a fução <see cref="Reordenar"/>
        /// </summary>
        /// <param name="Pai"></param>
        /// <param name="Lista"></param>
        private void OrdenarItens(IOrcamentoItens Pai, List<IOrcamentoItens> Lista)
        {
            IEnumerable<IOrcamentoItens> Itens;

            Itens = Colecao.Where(x => x.idOrcOrcamentoPai == Pai.idOrcOrcamento).OrderBy(x => x.Sequencia);

            // Monta itemizacao dos subitens
            foreach (var item in Itens)
            {
                Lista.Add(item);
                OrdenarItens(item, Lista);
            }
        }

        private int IndexDoTituloPai(int valor)
        {
            if (valor == 0) return 0;
             
            int i = 0;

            while (Colecao[i].idOrcOrcamento != valor)
            {
                i += 1;
            }

            return i;
        }

        private void CalcularTotais(object sender, PropertyChangedEventArgs e)
        {
              RecalcularTotal();
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}