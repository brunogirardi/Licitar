using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licitar
{
    static class CalcularLeisSociais
    {

        /// <summary>
        /// Acrescenta as leis sociais a todos os itens do tipo Mão de Obra
        /// </summary>
        /// <param name="item">Item a ser verificado e calculado</param>
        /// <returns></returns>
        public static double Calcular(IInsumoGeral item)
        {

            if (item.Tipo == tipoInsumo.MaoDeObra)
            {
                if (item.Unidade == "MÊS")
                {
                    return Math.Round(item.ValorTotal * (1 + (Factory.LeisSociais[0].Valor / 100)), 2);
                }
                else
                {
                    return Math.Round(item.ValorTotal * (1 + (Factory.LeisSociais[1].Valor / 100)), 2);
                }
            }

            return item.ValorTotal;

        }


        public static double Calcular(double valor, string unidade, tipoInsumo tipo)
        {
            if (tipo == tipoInsumo.MaoDeObra)
            {
                if (unidade == "MÊS")
                {
                    return Math.Round(valor * (1 + (Factory.LeisSociais[0].Valor / 100)), 2);
                }
                else
                {
                    return Math.Round(valor * (1 + (Factory.LeisSociais[1].Valor / 100)), 2);
                }
            }

            return valor;
        }
    }
}
