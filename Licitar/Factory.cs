using System.Collections.ObjectModel;
using System.Windows;

namespace Licitar
{
    public class Factory
    {

        /// <summary>
        /// Expõe o provider principal
        /// </summary>
        public static OrcamentoManager AccessoAppProvider { get => ((App)Application.Current).provider; }

        /// <summary>
        /// Acesso a lista de Leis Sociais
        /// </summary>
        public static ObservableCollection<LeisSociais> LeisSociais { get => ((App)Application.Current).provider.LeisSociais; }

        /// <summary>
        /// Acesso a lista de bonificações
        /// </summary>
        public static ObservableCollection<Bdi> Bdis { get => ((App)Application.Current).provider.Bdis; }

        /// <summary>
        /// Acesso a lista de bases de referencia do orçamento
        /// </summary>
        public static OrcamentoBasesReferenciaLista BasesReferencia { get => ((App)Application.Current).provider.ListaReferencias; }

        /// <summary>
        /// Acesso a lista de insumos da base do orçamento
        /// </summary>
        public static ObservableCollection<IInsumoGeral> BaseOrcamento { get => ((App)Application.Current).provider.ListaReferencias.Lista[0].Insumos; }

        /// <summary>
        /// Acesso ao banco de dados do sistema
        /// </summary>
        public static MysqlDataAccess DBAcesso { get => new MysqlDataAccess(); }

    }
}
