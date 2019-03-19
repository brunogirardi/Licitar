using PropertyChanged;
using System.Linq;
using System;
using System.ComponentModel;

namespace Licitar
{
    public class InsumoGeral : IInsumoGeral
    {

        /// <summary>
        /// Codigo de referencia ao banco de dados
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Código de referencia a bases publicas
        /// </summary>
        public string CodigoRef { get; set; }

        /// <summary>
        /// Descrição do insumo / serviço
        /// </summary>
        public string Descrição { get; set; }

        /// <summary>
        /// Tipo do insumo
        /// </summary>
        public tipoInsumo Tipo { get; set; }

        /// <summary>
        /// Unidade de medição do serviço
        /// </summary>
        public string Unidade { get; set; }

        public double ValorUnitario { get; set; }

        /// <summary>
        /// Retorna o valor unitário com o acréscimo de leis sociais
        /// </summary>
        public double ValorUnitarioComLS => CalcularLeisSociais.Calcular(ValorUnitario, Unidade, Tipo);

        /// <summary>
        /// Id do banco de dado de onde o insumo/cpu foi copiado
        /// </summary>
        public int IdBaseReferencia { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
