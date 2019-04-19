using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Licitar
{
    public enum tipoInsumo
    {
        Composicao,
        Material,
        MaoDeObra,
        Equipamentos,
        Verbas,
        Indefinido,
        Titulo,
    }   

    public class TipoInsumo
    {
        public static IEnumerable<string> Lista()
        {
            return new List<string>()
            {
                "Composição",
                "Material",
                "Mão de Obra",
                "Equipamentos",
                "Verbas",
                "Indefinido",
                "Titulo"
            };
        }
    }
}