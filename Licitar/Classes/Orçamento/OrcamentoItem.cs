using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Licitar
{
    public class OrcamentoItem : IOrcamentoItem
    {


        #region Prodriedades

        /// <summary>
        /// Insumo/Composição relacionado ao orçamento
        /// </summary>
        IInsumoGeral item;
        [AlsoNotifyFor(nameof(ValorUnitario), nameof(Vinculado))]
        public IInsumoGeral Item {
            get => item;
            set
            {
                item = value;
                CodigoRef = item.CodigoRef;
                if (value != null) item.PropertyChanged += MonitorarMudançaItem;
            }
        }

        /// <summary>
        /// Código do item dentro do banco de dados
        /// </summary>
        public int idOrcOrcamento { get; set; }

        /// <summary>
        /// Código de referencia de bases auxiliares SINAPI / SEDOP / SEINFRA e etc...
        /// </summary>
        public string CodigoRef { get; set; }

        /// <summary>
        /// Unidade de medida do serviço
        /// </summary>
        public string Unidade { get; set; }

        /// <summary>
        /// Valor uniário do serviço
        /// </summary>
        [AlsoNotifyFor(nameof(ValorComBdi))]
        public double ValorUnitario {
            get
            {
                if (Item != null)
                {
                    return Item.ValorUnitario;
                }
                return 0;
            }
            set => value = 0;
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
        [AlsoNotifyFor(nameof(ValorTotal), nameof(ValorComBdi))]
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
        public string Itemizacao { get; set; }

        /// <summary>
        /// Sequencia do item dentro do orçamento
        /// </summary>
        public int Sequencia { get; set; }

        /// <summary>
        /// Define o item pai do serviço
        /// </summary>
        public int? idOrcOrcamentoPai { get; set; }

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
        public string Descricao { get; set; }

        /// <summary>
        /// Tipo de insumo
        /// </summary>
        public tipoInsumo Tipo { get; set; } = tipoInsumo.Composicao;

        public double ValorUnitarioComLs
        {
            get
            {
                if (Item != null)
                {
                    return Item.ValorUnitarioComLS;
                }
                return 0;
            }
        }

        public double ValorTotalComLs
        {
            get
            {
                if (Item != null)
                {
                    return Item.ValorUnitarioComLS;
                }
                return 0;
            }
        }

        public double ValorUnitarioVenda { get; set; }

        public double ValorTotalVenda
        {
            get => Math.Round(Quantidade * ValorUnitarioVenda, 2);
            set => value = 0;
        }

        public bool Vinculado
        {
            get
            {
                if (Item != null)
                {
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region Helper functions

        private void MonitorarMudançaItem(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(e.PropertyName));
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
