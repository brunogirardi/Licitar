using System.Collections.ObjectModel;

namespace Licitar
{
    public interface ICpuGeral : IInsumoGeral
    {

        /// <summary>
        /// Relação de insumos da composição
        /// </summary>
        ObservableCollection<CpuCoefGeral> Itens { get; set; }

    }
}
