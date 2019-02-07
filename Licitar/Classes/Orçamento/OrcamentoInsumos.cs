using System;
using System.ComponentModel;
using PropertyChanged;

namespace Licitar
{
    public class OrcamentoInsumos : IOrcamentoInsumo
    {
        public string Item { get; set; }

        public int Sequencia { get; set; }

        public int CodigoRef { get; set; }

        public string Descrição { get; set; }

        public string Unidade { get; set; }

        private double quantidade = 0;

        private double valorUnitario = 0;

        private IChaveValue bdi;

        public IChaveValue Bdi {
            get => bdi;
            set
            {
                bdi = value;
                bdi.PropertyChanged += Bdi_PropertyChanged;
            }
        }

        [AlsoNotifyFor("ValorTotal")]
        public double ValorUnitario {
            get => valorUnitario;
            set
            {
                valorUnitario = value;
                ValorTotal = Math.Round(ValorComBdi * Quantidade, 2);
            }
        }

        /// <summary>
        /// Quantitativo necessário
        /// </summary>
        [AlsoNotifyFor("ValorTotal")]
        public double Quantidade
        {
            get => quantidade;
            set
            {
                quantidade = value;
                ValorTotal = Math.Round(ValorComBdi * value, 2);
            }
        }

        /// <summary>
        /// Custo total do item para o insumo/serviço
        /// </summary>
        public double ValorTotal { get; set; }

        public tipoInsumo Tipo { get; set; }

        public int? ObjetoPai { get; set; }

        public double ValorComBdi => Math.Round(ValorUnitario * (1 + (Bdi.Valor / 100)), 2);

        private void Bdi_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Atualiza o valor total
            ValorTotal = Math.Round(ValorComBdi * Quantidade, 2);

            // Invoca os eventos para atualização do layout
            OnPropertyChanged(new PropertyChangedEventArgs("ValorComBdi"));
            OnPropertyChanged(new PropertyChangedEventArgs("ValorTotal"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs propriedade)
        {
            PropertyChanged?.Invoke(this, propriedade);
        }

    }
}
