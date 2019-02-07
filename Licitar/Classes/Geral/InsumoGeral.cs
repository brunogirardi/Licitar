using PropertyChanged;
using System.Linq;
using System;
using System.ComponentModel;

namespace Licitar
{
    class InsumoGeral : IInsumoGeral
    {

        public int CodigoRef { get; set; }

        public string Descrição { get; set; }

        public tipoInsumo Tipo { get; set; }

        public string Unidade { get; set; }

        [AlsoNotifyFor("ValorTotal")]
        public double Quantidade { get; set; } = 0;

        public double ValorTotal
        {
            get => Math.Round(Quantidade * ValorUnitario, 2);
        } 
            

        [AlsoNotifyFor("ValorTotal")]
        public double ValorUnitario { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
