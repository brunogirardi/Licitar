﻿using System.ComponentModel;

namespace Licitar
{
    interface IInsumoGeral : INotifyPropertyChanged
    {
        /// <summary>
        /// Código de referencia de bases auxiliares SINAPI / SEDOP / SEINFRA e etc...
        /// </summary>
        int CodigoRef { get; set; }

        /// <summary>
        /// Descrição do serviço
        /// </summary>
        string Descrição { get; set; }

        /// <summary>
        /// Tipo de insumo
        /// </summary>
        tipoInsumo Tipo { get; set; }

        /// <summary>
        /// Unidade de medida do serviço
        /// </summary>
        string Unidade { get; set; }

        /// <summary>
        /// Quantidade prevista no orçamento
        /// </summary>
        double Quantidade { get; set; }

        /// <summary>
        /// Valor uniário do serviço
        /// </summary>
        double ValorUnitario { get; }

        /// <summary>
        /// Valor total do item
        /// </summary>
        double ValorTotal { get; }

    }
}