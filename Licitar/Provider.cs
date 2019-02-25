using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licitar
{
    public class Provider
    {

        public string ObjetoLicitacao { get; set; } = "SAA Beija-flor";

        public string LocalLicitacao { get; set; } = "Marituba/PA";

        public string DocumentoRefLicitacao { get; set; } = "COSANPA 012/2018";

        public DateTime AberturaProposta { get; set; } = DateTime.Today;

        public string Empresa { get; set; } = "Carmona Cabrera Construtora de Obras Ltda";

        public string RepresentanteLegal { get; set; } = "Bruno Rodrigues Girardi";

        public string Cnpj { get; set; } = "00.480.154/0001-15";

        public ObservableCollection<IChaveValue> Bdis { get; set; }

        public ObservableCollection<IChaveValue> LeisSociais { get; set; }

        public ObservableCollection<IInsumoGeral> Insumos { get; set; } = new ObservableCollection<IInsumoGeral>();

        public ObservableCollection<IInsumoGeral> Composicoes { get; set; } = new ObservableCollection<IInsumoGeral>();

        public OrcamentoLista Orcamento { get; set; }

        public Provider()
        {

            Bdis = LoadBdis();

            LeisSociais = LoadLeisSociais();

        }

        /// <summary>
        /// Carrega os BDIs disponiveis para o orçamento
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<IChaveValue> LoadBdis()
        {
            ObservableCollection<IChaveValue> itens = new ObservableCollection<IChaveValue>()
            {
                new Bdi() { Id=1, Descricao="Serviços", Valor=27.56D},
                new Bdi() { Id=2, Descricao="Fornecimento", Valor=22.56D}
            };
            return itens;
        }

        /// <summary>
        /// Carrega as Leis Sociais disponiveis para o orçamento
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<IChaveValue> LoadLeisSociais()
        {
            ObservableCollection<IChaveValue> itens = new ObservableCollection<IChaveValue>()
            {
                new LeisSociais() { Id=1, Descricao="Mensalista", Valor=50D },
                new LeisSociais() { Id=2, Descricao="Horista", Valor=100D }
            };
            return itens;
        }

    }
}
