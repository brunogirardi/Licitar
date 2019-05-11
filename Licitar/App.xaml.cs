using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Licitar
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {

        public OrcamentoManager provider { get; set; }

        public App()
        {
            // Recupera os dados básicos do orçamento
            provider = Factory.DBAcesso.OrcamentoDados(1);

            // Listar a base de referencia
            provider.BaseReferencia = Factory.DBAcesso.ComposiçãoListar(1);

            // Listar a base do orçamento
            provider.BaseOrcamento = Factory.DBAcesso.InsumosOrcamentoLista(1);

            // Preenche bases de referencia do orçamento
            provider.ListaReferencias = new OrcamentoBasesReferenciaLista(1);

            // Preenche o orçamento
            provider.Orcamento = new OrcamentoLista(Factory.DBAcesso.OrcamentoLista(1));

        }
    }
}
