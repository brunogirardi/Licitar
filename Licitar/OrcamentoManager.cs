using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licitar
{
    public class OrcamentoManager
    {

        public int idOrcPrincipal { get; set; }

        public string ObjetoLicitacao { get; set; }

        public string LocalLicitacao { get; set; }

        public string NumeroLicitacao { get; set; }

        public DateTime DataAbertura { get; set; }

        public DateTime DataPublicacao { get; set; }

        public string EmpresaNome { get; set; }

        public string RepresentanteLegalNome { get; set; }

        public string RepresentanteLegalCrea { get; set; }

        public string RepresentanteLegalCargo { get; set; }

        public string EmpresaCnpj { get; set; }

        public ObservableCollection<Bdi> Bdis { get; set; }

        public ObservableCollection<LeisSociais> LeisSociais { get; set; }

        public ObservableCollection<InsumoGeral> BaseReferencia { get; set; }

        public ObservableCollection<InsumoGeral> BaseOrcamento { get; set; }

        public OrcamentoLista Orcamento { get; set; }

        /// <summary>
        /// Construtor do Orçamento
        /// </summary>
        public OrcamentoManager()
        {
            Bdis = Factory.DBAcesso.BdiLista(1);

            LeisSociais = Factory.DBAcesso.LeisSociaisLista(1);
        }

    }
}
