using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licitar
{
    class OrcamentoCpu : IOrcamentoCpu
    {
        /// <summary>
        /// Lista de insumos/serviços da cpu
        /// </summary>
        public ObservableCollection<IInsumoGeral> Itens { get; set; }

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
        public double ValorUnitario { get; }

        /// <summary>
        /// Quantidade prevista no orçamento
        /// </summary>
        public double Quantidade { get; set; }

        /// <summary>
        /// Valor do serviço com BDI
        /// </summary>
        public double ValorComBdi => Math.Round(ValorUnitario * Bdi.Valor,2);

        /// <summary>
        /// BDI definido para o insumo
        /// </summary>
        public IChaveValue Bdi { get; set; }

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
        public double ValorTotal { get; set; }

        /// <summary>
        /// Descrição do serviço
        /// </summary>
        public string Descrição { get; set; }

        /// <summary>
        /// Tipo de insumo
        /// </summary>
        public tipoInsumo Tipo { get; set; } = tipoInsumo.Composicao;

        /// <summary>
        /// Evento responsável por atualizar o wpf
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public double ValorTotalLeisSociais()
        {
            throw new NotImplementedException();
        }

        public double ValorTotalTipo(tipoInsumo tipo)
        {
            throw new NotImplementedException();
        }
    }
}
