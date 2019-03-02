using System;

namespace Licitar
{

    /// <summary>
    /// Item da DB que armazena a data base de preços das bases de referência
    /// </summary>
    public class BaseReferenciaDataBase
    {

        public int idRefPrecoBase { get; set; }

        public DateTime Data { get; set; }

        public string Descricao { get; set; }

        public int idRefBases { get; set; }

    }
}
