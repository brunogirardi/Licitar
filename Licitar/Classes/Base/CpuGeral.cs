using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Licitar
{
    class CpuGeral : ICpuGeral
    {
        #region Propriedades

        /// <summary>
        /// Código referencia a base original da CPU
        /// </summary>
        public int CodigoRef { get; set; }

        /// <summary>
        /// Descrição do serviço
        /// </summary>
        public string Descrição { get; set; }

        /// <summary>
        /// Tipo de insumo
        /// </summary>
        public tipoInsumo Tipo { get; set; } = tipoInsumo.Composicao;

        /// <summary>
        /// Unidade de medida da cpu
        /// </summary>
        public string Unidade { get; set; }

        /// <summary>
        /// Quantidade utilizada do serviço
        /// </summary>
        [AlsoNotifyFor(nameof(ValorTotal))]
        public double Quantidade { get; set; }

        /// <summary>
        /// Valor unitário do serviço
        /// </summary>
        [AlsoNotifyFor(nameof(ValorTotal))]
        public double ValorUnitario => Itens.Sum(x => CalcularLeisSociais.Calcular(x));

        /// <summary>
        /// Valor total do serviço
        /// </summary>
        public double ValorTotal {
            get => Math.Round(Quantidade * ValorUnitario, 2);
        }

        /// <summary>
        /// Campo de armazenamento dos itens que compoem a cpu
        /// </summary>
        private ObservableCollection<IInsumoGeral> itens;

        /// <summary>
        /// Relação de insumos que compõem a cpu
        /// </summary>
        [AlsoNotifyFor(nameof(ValorUnitario), new string[] { nameof(ValorTotal)})]
        public ObservableCollection<IInsumoGeral> Itens
        {
            get => itens;
            set
            {
                // Operação de atribuição de evento para monitorar qqualquer alteração
                foreach (IInsumoGeral item in value)
                {
                    item.PropertyChanged += NotificarMudancaNaCpu;
                }
            }
        }

        /// <summary>
        /// Retorna o valor unitário com o acréscimo de leis sociais
        /// </summary>
        public double ValorUnitarioComLS => CalcularLeisSociais.Calcular(ValorUnitario, Unidade, Tipo);

        /// <summary>
        /// Retorna o valor total com o acréscimo de leis sociais
        /// </summary>
        public double ValorTotalComLS => CalcularLeisSociais.Calcular(ValorTotal, Unidade, Tipo);

        #endregion

        #region Metodos auxiliares

        /// <summary>
        /// Função responsável por disparar o gatilho para atualizar o valor total da cpu  
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Propriedade do evento</param>
        private void NotificarMudancaNaCpu(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ValorTotal")
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ValorTotal)));
            }
        }

        #endregion

        #region Evento PropertyChanged

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
