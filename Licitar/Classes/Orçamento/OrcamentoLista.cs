using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Licitar
{
    public class OrcamentoLista : INotifyPropertyChanged
    {
        /// <summary>
        /// Construtor responsável por montar o orçamento
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

            this.RecalcularTotal();

            this.MonitorarTotalizadores();
        }
    
        /// <summary>
        /// Propriedade que segura a lista de itens do orçamento
        /// </summary>
        public ObservableCollection<IOrcamentoItens> Colecao { get; set; }


        public OrcamentoLista(ObservableCollection<IOrcamentoItens> itens)
        {
            this.Colecao = itens;

            this.RecalcularTotal();

            this.MonitorarTotalizadores();
        }

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

        #endregion

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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}