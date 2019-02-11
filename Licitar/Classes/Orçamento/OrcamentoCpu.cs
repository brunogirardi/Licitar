using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Licitar
{
    class OrcamentoCpu : IOrcamentoCpu
    {
        
        #region Prodriedades

        private ObservableCollection<IInsumoGeral> insumos;
        
        /// <summary>
        /// Relação de insumos da cpu
        /// </summary>
        [AlsoNotifyFor(nameof(ValorUnitario))]
        public ObservableCollection<IInsumoGeral> Itens {
            get => insumos;
            set
            {
                insumos = value;
                RegistrarMonitoramentoItenCpu(value);
            }

        }

        /// <summary>
        /// Código de referencia de bases auxiliares SINAPI / SEDOP / SEINFRA e etc...
        /// </summary>
        public int CodigoRef { get; set; }

        /// <summary>
        /// Unidade de medida do serviço
        /// </summary>
        public string Unidade { get; set; }

        /// <summary>
        /// Valor uniário do serviço
        /// </summary>
        [AlsoNotifyFor(nameof(ValorComBdi))]
        public double ValorUnitario {
            get => CalcularValorUnitario();
            set => value = 1;
        }

        /// <summary>
        /// Quantidade prevista no orçamento
        /// </summary>
        [AlsoNotifyFor(nameof(ValorTotal))]
        public double Quantidade { get; set; }

        /// <summary>
        /// Valor do serviço com BDI
        /// </summary>
        [AlsoNotifyFor(nameof(ValorTotal))]
        public double ValorComBdi => Math.Round(ValorUnitario * (1 + (Bdi.Valor / 100)), 2);

        /// <summary>
        /// BDI definido para o insumo
        /// </summary>
        private IChaveValue bdi;
        public IChaveValue Bdi
        {
            get => bdi;
            set
            {
                bdi = value;
                bdi.PropertyChanged += Bdi_PropertyChanged;
            }
        }

        /// <summary>
        /// Código da itemização do orçamento
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// Sequencia do item dentro do orçamento
        /// </summary>
        public int Sequencia { get; set; }

        /// <summary>
        /// Define o item pai do serviço
        /// </summary>
        public int? ObjetoPai { get; set; }

        /// <summary>
        /// Valor total do item
        /// </summary>
        public double ValorTotal {
            get => Math.Round(Quantidade * ValorComBdi, 2);
            set => value = 1;
        }

        /// <summary>
        /// Descrição do serviço
        /// </summary>
        public string Descrição { get; set; }

        /// <summary>
        /// Tipo de insumo
        /// </summary>
        public tipoInsumo Tipo { get; set; } = tipoInsumo.Composicao;

        public double ValorUnitarioComLs => ValorUnitario;

        public double ValorTotalComLs => ValorTotal;

        #endregion

        #region Helper functions

        private void RegistrarMonitoramentoItenCpu(ObservableCollection<IInsumoGeral> itens)
        {
            foreach (IInsumoGeral item in itens)
            {
                item.PropertyChanged += MonitorarMudançaItem;
            }
        }

        private void MonitorarMudançaItem(object sender, PropertyChangedEventArgs e)
        {
            // Verifica se a mudança vai gerar alteração de valor na CPU
            if (e.PropertyName == "ValorTotal")
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ValorTotal)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ValorUnitario)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ValorComBdi)));
            }
        }

        /// <summary>
        /// Recalcula o valor do serviço com BDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bdi_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Atualiza o valor total
            ValorTotal = Math.Round(ValorComBdi * Quantidade, 2);

            // Invoca os eventos para atualização do layout
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(ValorComBdi)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(ValorTotal)));
        }

        private double CalcularValorUnitario()
        {
            return Itens.Sum(x => CalcularLeisSociais.Calcular(x));
        }

        public double ValorTotalTipo(tipoInsumo tipo)
        {
            return Itens.Where(item => item.Tipo == tipo).Sum(x => x.ValorTotal);
        }
        #endregion

        #region Eventos

        /// <summary>
        /// Evento responsável por atualizar o wpf
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs propriedade)
        {
            PropertyChanged?.Invoke(this, propriedade);
        }

        #endregion

    }
}
