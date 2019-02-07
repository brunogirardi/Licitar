using System.ComponentModel;

namespace Licitar
{
    public interface IChaveValue : INotifyPropertyChanged
    {

        string Descricao { get; set; }

        int Id { get; set; }

        double Valor { get; set; }

    }
}