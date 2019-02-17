using System.Collections.ObjectModel;
using System.Windows;

namespace Licitar
{
    public class Factory
    {

        /// <summary>
        /// Expõe o provider principal
        /// </summary>
        public static Provider AccessoAppProvider { get => ((App)Application.Current).provider; }

        /// <summary>
        /// Acesso a lista de Leis Sociais
        /// </summary>
        public static ObservableCollection<IChaveValue> LeisSociais { get => ((App)Application.Current).provider.LeisSociais; }

        /// <summary>
        /// Acesso a lista de bonificações
        /// </summary>
        public static ObservableCollection<IChaveValue> Bdis { get => ((App)Application.Current).provider.Bdis; }

    }
}
