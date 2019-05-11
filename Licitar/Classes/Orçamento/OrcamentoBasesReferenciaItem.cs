using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licitar
{

    public class OrcamentoBasesReferenciaItem
    {

        public int IdOrcBaseReferencia { get; set; }

        public string BaseDescricao { get; set; }

        public int idRefPrecoBase { get; set; }

        public string PrecoDescricao { get; set; }

        public int idOrcRevisao { get; set; }

        public ObservableCollection<IInsumoGeral> Insumos { get; set; }

    }

}
