using System.Collections.ObjectModel;

namespace Licitar
{
    interface ICpuGeral : IInsumoGeral
    {

        /// <summary>
        /// Relação de insumos da composição
        /// </summary>
        ObservableCollection<IInsumoGeral> Itens { get; set; }

    }
}
