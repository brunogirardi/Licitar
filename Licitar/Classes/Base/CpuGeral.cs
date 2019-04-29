using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Licitar
{
    public class CpuGeral : ICpuGeral
    {

        public CpuGeral()
        {
            itens = new ObservableCollection<CpuCoefGeral>();

            itens.CollectionChanged += Itens_CollectionChanged; ;
        }

        private void Itens_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                ((CpuCoefGeral)e.NewItems[0]).PropertyChanged += NotificarMudancaNaCpu;
            } else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
                ((CpuCoefGeral)e.OldItems[0]).PropertyChanged -= NotificarMudancaNaCpu;
            }
        }

        #region Propriedades

        /// <summary>
        /// Código referencia ao banco de dados
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Código referencia a base original da CPU
        /// </summary>
        public string CodigoRef { get; set; }

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
        /// Valor unitário do serviço
        /// </summary>
        private double valorUnitario;
        public double ValorUnitario
        {
            get {
                if (itens.Count() > 0)
                {
                    return Itens.Sum(x => x.ValorUnitario);
                } else
                {
                    return valorUnitario;
                }
            }
            set => valorUnitario = value;
        }

        /// <summary>
        /// Campo de armazenamento dos itens que compoem a cpu
        /// </summary>
        private ObservableCollection<CpuCoefGeral> itens;

        /// <summary>
        /// Relação de insumos que compõem a cpu
        /// </summary>
        [AlsoNotifyFor(nameof(ValorUnitario))]
        public ObservableCollection<CpuCoefGeral> Itens
        {
            get => itens;
            set
            {
                itens = value;
                // Operação de atribuição de evento para monitorar qualquer alteração
                foreach (CpuCoefGeral item in value)
                {
                    item.PropertyChanged += NotificarMudancaNaCpu;
                }
            }
        }

        /// <summary>
        /// Retorna o valor unitário com o acréscimo de leis sociais
        /// </summary>
        public double ValorUnitarioComLS => Itens.Sum(x => x.ValorUnitario);


        /// <summary>
        /// Id do banco de dado de onde o insumo/cpu foi copiado
        /// </summary>
        public int IdBaseReferencia { get; set; }

        #endregion

        #region Metodos auxiliares

        /// <summary>
        /// Função responsável por disparar o gatilho para atualizar o valor total da cpu  
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Propriedade do evento</param>
        private void NotificarMudancaNaCpu(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(e.PropertyName));
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
