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

        public OrcamentoLista Orcamento { get; set; }

        public Provider()
        {

            Bdis = LoadBdis();

            LeisSociais = LoadLeisSociais();

            Orcamento = LoadOrcamento();

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
                new LeisSociais() { Id=1, Descricao="Mensalista", Valor=87.56D },
                new LeisSociais() { Id=2, Descricao="Horista", Valor=122.56D }
            };
            return itens;
        }

        /// <summary>
        /// Carrega os insumos disponiveis para o orçamento
        /// </summary>
        /// <returns></returns>
        private OrcamentoLista LoadOrcamento()
        {
            ObservableCollection<IOrcamentoItens> itens = new ObservableCollection<IOrcamentoItens>()
            {
                new OrcamentoTitulo() { Item ="01", Descrição ="PRIMEIRO NIVEL", Tipo =tipoInsumo.Titulo, Sequencia = 1 },
                new OrcamentoTitulo() { Item ="01.01", Descrição ="PRIMEIRO NIVEL", Tipo =tipoInsumo.Titulo, Sequencia = 1, ObjetoPai=0 },
                new OrcamentoTitulo() { Item ="01.01.01", Descrição ="SEGUNDO NIVEL", Tipo =tipoInsumo.Titulo, Sequencia = 1, ObjetoPai=1 },
                new OrcamentoInsumos() { Item="01.01.01.01", Bdi=Bdis[0], CodigoRef= 154, Descrição="CIMENTO PORTLAND CP-II 32 MPA", Tipo = tipoInsumo.Material, Unidade="SC", ValorUnitario=1D , Sequencia=2, Quantidade=1, ObjetoPai=2 },
                new OrcamentoInsumos() { Item="01.01.01.02", Bdi=Bdis[0], CodigoRef= 154, Descrição="AREIA MÉDIA", Tipo = tipoInsumo.Material, Unidade="SC", ValorUnitario=1D , Sequencia=2, Quantidade=1, ObjetoPai=2 },
                new OrcamentoInsumos() { Item="01.01.01.03", Bdi=Bdis[0], CodigoRef= 154, Descrição="SERVENTE", Tipo = tipoInsumo.Material, Unidade="SC", ValorUnitario=1D , Sequencia=2, Quantidade=1, ObjetoPai=2 },
                new OrcamentoInsumos() { Item="01.01.01.04", Bdi=Bdis[0], CodigoRef= 154, Descrição="PEDREIRO", Tipo = tipoInsumo.Material, Unidade="SC", ValorUnitario=1D , Sequencia=2, Quantidade=1, ObjetoPai=2 },
                new OrcamentoTitulo() { Item ="01.02", Descrição ="PRIMEIRO NIVEL", Tipo =tipoInsumo.Titulo, Sequencia = 1, ObjetoPai=0 },
                new OrcamentoTitulo() { Item ="01.02.01", Descrição ="SEGUNDO NIVEL", Tipo =tipoInsumo.Titulo, Sequencia = 1, ObjetoPai=7 },
                new OrcamentoInsumos() { Item="01.02.01.01", Bdi=Bdis[0], CodigoRef= 154, Descrição="CIMENTO PORTLAND CP-II 32 MPA", Tipo = tipoInsumo.Material, Unidade="SC", ValorUnitario=1D , Sequencia=2, Quantidade=1, ObjetoPai=8 },
                new OrcamentoInsumos() { Item="01.02.01.02", Bdi=Bdis[0], CodigoRef= 154, Descrição="AREIA MÉDIA", Tipo = tipoInsumo.Material, Unidade="SC", ValorUnitario=1D , Sequencia=2, Quantidade=1, ObjetoPai=8 },
                new OrcamentoInsumos() { Item="01.02.01.03", Bdi=Bdis[0], CodigoRef= 154, Descrição="SERVENTE", Tipo = tipoInsumo.Material, Unidade="SC", ValorUnitario=1D , Sequencia=2, Quantidade=1, ObjetoPai=8 },
                new OrcamentoInsumos() { Item="01.02.01.04", Bdi=Bdis[0], CodigoRef= 154, Descrição="PEDREIRO", Tipo = tipoInsumo.Material, Unidade="SC", ValorUnitario=1D , Sequencia=2, Quantidade=1, ObjetoPai=8 },
                new OrcamentoCpu() {
                    Item = "01.02.01.04",
                    Bdi = Bdis[0],
                    CodigoRef = 154,
                    Descrição = "PEDREIRO",
                    Tipo = tipoInsumo.Material,
                    Unidade= "SC",
                    Sequencia = 2,
                    Quantidade = 1,
                    ObjetoPai = 8,
                    //Itens = new ObservableCollection<IOrcamentoInsumo>()
                    //{
                    //    new OrcamentoInsumos() { CodigoRef= 154, Descrição="CIMENTO PORTLAND CP-II 32 MPA", Tipo = tipoInsumo.Material, Unidade="SC", ValorUnitario=27.45D , Quantidade=7 },
                    //    new OrcamentoInsumos() { CodigoRef= 154, Descrição="AREIA MÉDIA", Tipo = tipoInsumo.Material, Unidade="M3", ValorUnitario=32D , Quantidade=.8D },
                    //    new OrcamentoInsumos() { CodigoRef= 154, Descrição="BRITA", Tipo = tipoInsumo.Material, Unidade="M3", ValorUnitario=32D , Quantidade=.6D },
                    //    new OrcamentoInsumos() { CodigoRef= 154, Descrição="SERVENTE", Tipo = tipoInsumo.MaoDeObra, Unidade="H", ValorUnitario=1D , Quantidade=3D },
                    //    new OrcamentoInsumos() { CodigoRef= 154, Descrição="PEDREIRO", Tipo = tipoInsumo.MaoDeObra, Unidade="H", ValorUnitario=1D , Quantidade=1.5D },
                    //}
                },

            };

            return new OrcamentoLista(itens);
        }

    }
}
