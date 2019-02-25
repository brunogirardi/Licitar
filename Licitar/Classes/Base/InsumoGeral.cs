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

        [AlsoNotifyFor("ValorTotal")]
        public double Quantidade { get; set; } = 0;

        public double ValorTotal
        {
            get => Math.Round(Quantidade * ValorUnitario, 2);
        } 
            

        [AlsoNotifyFor("ValorTotal")]
        public double ValorUnitario { get; set; }
        /// <summary>
        /// Retorna o valor unitário com o acréscimo de leis sociais
        /// </summary>
        public double ValorUnitarioComLS => CalcularLeisSociais.Calcular(ValorUnitario, Unidade, Tipo);

        /// <summary>
        /// Retorna o valor total com o acréscimo de leis sociais
        /// </summary>
        public double ValorTotalComLS => CalcularLeisSociais.Calcular(ValorTotal, Unidade, Tipo);


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
