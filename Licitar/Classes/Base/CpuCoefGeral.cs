using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licitar
{
    public class CpuCoefGeral : INotifyPropertyChanged
    {

        #region Construtor
        /// <summary>
        /// Contrutor padrão
        /// </summary>
        public CpuCoefGeral()
        {

        }

        /// <summary>
        /// Cria um novo item da composição a partir de um <see cref="IInsumoGeral"/>
        /// </summary>
        /// <param name="item"></param>
        public CpuCoefGeral(IInsumoGeral item)
        {
            Insumo = item;
            Coeficiente = 1;
        }

        #endregion 

        /// <summary>
        /// Id do coeficiente armazenado no banco de dados
        /// </summary>
        public int IdCoeficiente { get; set; }

        /// <summary>
        /// Insumo relacionado ao coeficiente
        /// </summary>
        private IInsumoGeral insumo;
        public IInsumoGeral Insumo {
            get => insumo;
            set
            {
                insumo = value;
                insumo.PropertyChanged += NotificarMudanaNoInsumo;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ValorUnitario)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ValorUnitarioComLs)));
            }
        }

        public double ValorUnitario => Math.Round(insumo.ValorUnitario * Coeficiente, 2);

        public double ValorUnitarioComLs => Math.Round(insumo.ValorUnitarioComLS * Coeficiente, 2);

        /// <summary>
        /// Coeficiente do insumo utilizado na cpu
        /// </summary>
        [AlsoNotifyFor(nameof(ValorUnitario), nameof(ValorUnitarioComLs))]
        public double Coeficiente { get; set; }

        #region Evento PropertyChanged
        /// <summary>
        /// Transmite para a cpu eventuais mudanças no insumo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotificarMudanaNoInsumo(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(e.PropertyName));
        }

        /// <summary>
        /// Evento para disparar o PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invoca o evento de propertychanged
        /// </summary>
        /// <param name="propriedade"></param>
        public void OnPropertyChanged(PropertyChangedEventArgs propriedade)
        {
            PropertyChanged?.Invoke(this, propriedade);
        }
        #endregion

    }
}
