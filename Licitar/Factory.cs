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
        /// Acesso ao banco de dados do sistema
        /// </summary>
        public static MysqlDataAccess DBAcesso { get => new MysqlDataAccess(); }

    }
}
