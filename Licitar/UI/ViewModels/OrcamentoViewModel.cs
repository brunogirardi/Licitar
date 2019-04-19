using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licitar
{
    class OrcamentoViewModel
    {

        public OrcamentoManager Orcamento { get; set; }

        private ICollectionView BaseReferencia { get; set; }

        private ICollectionView BaseOrcamento { get; set; }

        private bool _InsertItemMode;
        public bool InsertItemMode
        {
            get { return _InsertItemMode; }
            set { _InsertItemMode = value; }
        }

        private tipoInsumo _TipoItem;
        public tipoInsumo TipoItem
        {
            get { return _TipoItem; }
            set { _TipoItem = value; }
        }


    }
}
