using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Licitar
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {

        public OrcamentoManager provider { get; set; }

        public App()
        {
            provider = Factory.DBAcesso.OrcamentoDados(1);

            ObservableCollection<IOrcamentoItens> itens = new ObservableCollection<IOrcamentoItens>()
            {
                new OrcamentoTitulo() { Item ="01", Descrição ="PRIMEIRO NIVEL", Tipo =tipoInsumo.Titulo, Sequencia = 1 },
                new OrcamentoTitulo() { Item ="01.01", Descrição ="PRIMEIRO NIVEL", Tipo =tipoInsumo.Titulo, Sequencia = 1, ObjetoPai=0 },
                new OrcamentoTitulo() { Item ="01.01.01", Descrição ="SEGUNDO NIVEL", Tipo =tipoInsumo.Titulo, Sequencia = 1, ObjetoPai=1 },
                new OrcamentoInsumos() { Item="01.01.01.01", Bdi=provider.Bdis[0], CodigoRef= 154, Descrição="CIMENTO PORTLAND CP-II 32 MPA", Tipo = tipoInsumo.Material, Unidade="SC", ValorUnitario=1D , Sequencia=2, Quantidade=1, ObjetoPai=2 },
                new OrcamentoInsumos() { Item="01.01.01.02", Bdi=provider.Bdis[0], CodigoRef= 154, Descrição="AREIA MÉDIA", Tipo = tipoInsumo.Material, Unidade="SC", ValorUnitario=1D , Sequencia=2, Quantidade=1, ObjetoPai=2 },
                new OrcamentoInsumos() { Item="01.01.01.03", Bdi=provider.Bdis[0], CodigoRef= 154, Descrição="SERVENTE", Tipo = tipoInsumo.Material, Unidade="SC", ValorUnitario=1D , Sequencia=2, Quantidade=1, ObjetoPai=2 },
                new OrcamentoInsumos() { Item="01.01.01.04", Bdi=provider.Bdis[0], CodigoRef= 154, Descrição="PEDREIRO", Tipo = tipoInsumo.Material, Unidade="SC", ValorUnitario=1D , Sequencia=2, Quantidade=1, ObjetoPai=2 },
                new OrcamentoTitulo() { Item ="01.02", Descrição ="PRIMEIRO NIVEL", Tipo =tipoInsumo.Titulo, Sequencia = 1, ObjetoPai=0 },
                new OrcamentoTitulo() { Item ="01.02.01", Descrição ="SEGUNDO NIVEL", Tipo =tipoInsumo.Titulo, Sequencia = 1, ObjetoPai=7 },
                new OrcamentoInsumos() { Item="01.02.01.01", Bdi=provider.Bdis[0], CodigoRef= 154, Descrição="CIMENTO PORTLAND CP-II 32 MPA", Tipo = tipoInsumo.Material, Unidade="SC", ValorUnitario=1D , Sequencia=2, Quantidade=1, ObjetoPai=8 },
                new OrcamentoInsumos() { Item="01.02.01.02", Bdi=provider.Bdis[0], CodigoRef= 154, Descrição="AREIA MÉDIA", Tipo = tipoInsumo.Material, Unidade="SC", ValorUnitario=1D , Sequencia=2, Quantidade=1, ObjetoPai=8 },
                new OrcamentoInsumos() { Item="01.02.01.03", Bdi=provider.Bdis[0], CodigoRef= 154, Descrição="SERVENTE", Tipo = tipoInsumo.MaoDeObra, Unidade="SC", ValorUnitario=1D , Sequencia=2, Quantidade=1, ObjetoPai=8 },
                new OrcamentoInsumos() { Item="01.02.01.04", Bdi=provider.Bdis[0], CodigoRef= 154, Descrição="PEDREIRO", Tipo = tipoInsumo.MaoDeObra, Unidade="SC", ValorUnitario=1D , Sequencia=2, Quantidade=1, ObjetoPai=8 },
                new OrcamentoCpu() {
                    Item = "01.02.01.05",
                    Bdi = provider.Bdis[0],
                    CodigoRef = 157,
                    Descrição = "CONCRETO USINADO FCK=35MPA",
                    Unidade= "M3",
                    Sequencia = 2,
                    Quantidade = 1,
                    ObjetoPai = 8,
                    Itens = new ObservableCollection<IInsumoGeral>()
                    {
                        new InsumoGeral() { CodigoRef="154", Descrição="CIMENTO PORTLAND CP-II 32 MPA", Tipo = tipoInsumo.Material, Unidade="SC", ValorUnitario=5D , Quantidade=7 },
                        new InsumoGeral() { CodigoRef="154", Descrição="AREIA MÉDIA", Tipo = tipoInsumo.Material, Unidade="M3", ValorUnitario=10D , Quantidade=1D },
                        new InsumoGeral() { CodigoRef="154", Descrição="BRITA", Tipo = tipoInsumo.Material, Unidade="M3", ValorUnitario=5D , Quantidade=1D },
                        new InsumoGeral() { CodigoRef="154", Descrição="SERVENTE", Tipo = tipoInsumo.MaoDeObra, Unidade="H", ValorUnitario=1D , Quantidade=1D },
                        new InsumoGeral() { CodigoRef="154", Descrição="PEDREIRO", Tipo = tipoInsumo.MaoDeObra, Unidade="H", ValorUnitario=1D , Quantidade=1D }
                    }
                },


            };

            provider.Orcamento = new OrcamentoLista(itens);
        }
    }
}
