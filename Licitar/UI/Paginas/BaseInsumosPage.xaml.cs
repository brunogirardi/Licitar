using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Licitar
{
    /// <summary>
    /// Interação lógica para BaseInsumosPage.xam
    /// </summary>
    public partial class BaseInsumosPage : Page
    {
        public BaseInsumosPage()
        {
            InitializeComponent();

            listaInsumos.ItemsSource = Factory.AccessoAppProvider.Insumos;
        }

        /// <summary>
        /// Atualiza a lista ao substituir lista existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Insumos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            listaInsumos.ItemsSource = Factory.AccessoAppProvider.Insumos;
        }
    }
}
