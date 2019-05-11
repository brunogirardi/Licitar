using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Licitar
{
    /// <summary>
    /// Classe responsável por gerenciar as classes de referencia utilizadas pelo orçamento
    /// </summary>
    public class OrcamentoBasesReferenciaLista
    {

        List<OrcamentoBasesReferenciaItem> Lista { get; set; } = new List<OrcamentoBasesReferenciaItem>();

        /// <summary>
        /// Construtor responsavel por resgatar as informações da lista de bases de referencia atreladas ao orçamento
        /// </summary>
        /// <param name="id">Código de revisão do orçamento</param>
        public OrcamentoBasesReferenciaLista(int id)
        {
            // Adiciona a base de insumos e serviços do orçamento
            Lista.Add(new OrcamentoBasesReferenciaItem() {
                BaseDescricao = "ORÇAMENTO",
                PrecoDescricao = "ORÇAMENTO",
                Insumos = Factory.DBAcesso.InsumosOrcamentoLista(id)
            });

            // Obtem a base de insumos e serviços para o orçamentos de referencia
            foreach (var item in Factory.DBAcesso.OrcamentoReferencias(id))
            {
                Lista.Add(item);
                item.Insumos = Factory.DBAcesso.GeralListar(item.idRefPrecoBase);
            }


        }

    }

}
