using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;

namespace Licitar
{
    public class MysqlDataAccess
    {
        #region Helper Functions

        private static string LoadConnectionString(string id = "MySql")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        #endregion

        #region Insumos

        public ObservableCollection<IInsumoGeral> InsumosListar(int id)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<InsumoGeral>(
                    @"SELECT ins.idRefInsumos as id, ins.CodigoRef, ins.Descrição, ins.Unidade, ins.Tipo, preco.Preco as ValorUnitario
                      FROM refinsumos as ins
                      INNER JOIN refprecos as preco
                      ON ins.idRefInsumos = preco.idRefInsumos
                      WHERE preco.idRefPrecoBase = @Id AND ins.Tipo > 0", 
                    new { Id = id });

                List<IInsumoGeral> lista = output.ToList<IInsumoGeral>();

                return new ObservableCollection<IInsumoGeral>(lista);
            }
        }

        public IInsumoGeral InsumoBuscar(int id)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                var output = cnn.QuerySingle<InsumoGeral>("SELECT * FROM refinsumos WHERE Id=@Id", new { Id = id });

                return output;
            }
        }

        /// <summary>
        /// Pesquisa a ID do item no banco de dados através do código de Referencia
        /// </summary>
        /// <param name="CodigoRef"></param>
        /// <returns>Retorna um inteiro com a ID do banco de dados</returns>
        public int InsumoBuscarIdComOCodigoRef(int CodigoRef)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                return cnn.ExecuteScalar<int>("SELECT id FROM refinsumos WHERE CodigoRef=@CodigoRef", CodigoRef);
            }
        }

        /// <summary>
        /// Insere um insumo ao banco de dados
        /// </summary>
        /// <param name="insumo">Insumo a ser inserido</param>
        public void InsumoSave(IInsumoGeral insumo)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {

                cnn.Execute("INSERT INTO refinsumos (Descrição, Unidade, Tipo, CodigoRef) VALUES (@Descrição, @Unidade, @Tipo, @CodigoRef)", insumo);

                insumo.Id = Convert.ToInt32(cnn.ExecuteScalar("SELECT last_insert_rowid()"));

            }
        }

        /// <summary>
        /// Salva a lista de insumos no banco de dados
        /// </summary>
        /// <param name="Lista"></param>
        public void InsumoSaveList(ObservableCollection<IInsumoGeral> Lista)
        {

            MySqlTransaction transaction;
            MySqlCommand command = new MySqlCommand();

            // Inicia a conexão com o banco de dados
            using (MySqlConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                cnn.Open();

                transaction = cnn.BeginTransaction();

                command.Connection = cnn;
                command.Transaction = transaction;

                foreach (var item in Lista)
                {
                    // Limpa os parametros
                    command.Parameters.Clear();

                    // Adiciona o insumo a tabela RefInsumos
                    command.CommandText = @"INSERT INTO refinsumos (Descrição, Unidade, Tipo, CodigoRef) 
                                            SELECT * FROM (SELECT @Descrição, @Unidade, @Tipo, @CodigoRef) AS tmp
                                            WHERE NOT EXISTS (
                                                SELECT CodigoRef FROM refinsumos WHERE CodigoRef=@CodigoRef
                                            );";

                    command.Parameters.AddWithValue("@Descrição", item.Descrição.Replace("\"", "\\\"").Replace("'", "''"));
                    command.Parameters.AddWithValue("@Unidade", item.Unidade);
                    command.Parameters.AddWithValue("@Tipo", (int)item.Tipo);
                    command.Parameters.AddWithValue("@CodigoRef", item.CodigoRef);
                    command.ExecuteNonQuery();
                    
                    // Limpa os parametros
                    command.Parameters.Clear();

                    // Registra o preço para o insumo na tabela RefPrecos
                    command.CommandText = @"INSERT INTO refprecos (idRefPrecoBase, idRefInsumos, Preco) 
                                            VALUE(1, (SELECT idRefInsumos FROM RefInsumos WHERE CodigoRef = @CodigoRef), @preco);";

                    command.Parameters.AddWithValue("@Preco", item.ValorUnitario.ToString().Replace(",", "."));
                    command.Parameters.AddWithValue("@CodigoRef", item.CodigoRef);
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
        }


        #endregion

        #region Composições

        public ObservableCollection<IInsumoGeral> ComposiçãoListar(int id)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CpuGeral>(
                    @"SELECT ins.idRefInsumos as id, ins.CodigoRef, ins.Descrição, ins.Unidade, ins.Tipo, preco.Preco as ValorUnitario
                      FROM refinsumos as ins
                      INNER JOIN refprecos as preco
                      ON ins.idRefInsumos = preco.idRefInsumos
                      WHERE preco.idRefPrecoBase = @Id AND ins.Tipo = 0",
                    new { Id = id });

                List<CpuGeral> lista = output.ToList();

                return new ObservableCollection<IInsumoGeral>(lista);
            }
        }

        public ObservableCollection<CpuCoefGeral> ComposiçãoListarItens(int codigo)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                var output = cnn.ExecuteReader(
                    @"SELECT coef.idRefInsumosCoeficientes , ins.idRefInsumos as id, ins.CodigoRef, ins.Descrição, ins.Unidade, ins.Tipo,  p.Preco as ValorUnitario, coef.Coeficiente
                      FROM refinsumoscoeficientes as coef
                      INNER JOIN refinsumos as ins ON coef.idRefInsumosItem = ins.idRefInsumos
                      INNER JOIN refprecos as p ON p.idRefInsumos = coef.idRefInsumosItem
                      WHERE coef.idRefInsumos = " + codigo + ";",
                    new DynamicParameters());

                List<CpuCoefGeral> lista = new List<CpuCoefGeral>();

                while (output.Read())
                {
                    lista.Add(
                            new CpuCoefGeral()
                            {
                                IdCoeficiente = output.GetInt32(0),
                                Coeficiente = output.GetDouble(7),
                                Insumo = new InsumoGeral()
                                {
                                    Id = output.GetInt32(1),
                                    CodigoRef = output.GetString(2),
                                    Descrição = output.GetString(3),
                                    Unidade = output.GetString(4),
                                    Tipo = tipoInsumo.Indefinido,
                                    ValorUnitario = output.GetDouble(6),
                                }
                            }
                        );
                }

                return new ObservableCollection<CpuCoefGeral>(lista);
            }
        }

        public void ComposiçãoSave(CpuGeral cpu)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {

                cnn.Execute("INSERT INTO BaseInsumo (Descrição, Unidade, ValorUnitario, Tipo, CodigoRef) VALUES (@Descrição, @Unidade, @ValorUnitario, @Tipo, @CodigoRef)", cpu);

                cpu.Id = (int)cnn.ExecuteScalar("SELECT last_insert_rowid()");

            }

            foreach (var item in cpu.Itens)
            {
                ComposiçãoItemSave(item, cpu.Id);
            }
        }

        /// <summary>
        /// Acrescenta um item a uma composição
        /// </summary>
        /// <param name="item"></param>
        /// <param name="idComposicao"></param>
        public void ComposiçãoItemSave(CpuCoefGeral item, int idComposicao)
        {

            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {

                cnn.Execute("INSERT INTO ComposicaoItem (ComposicaoId, InsumoId, Quantidade) VALUES (@ComposicaoId, @InsumoId, @Quantidade)", new { ComposicaoId = idComposicao, InsumoId = item.Insumo.Id, item.Coeficiente });

                item.IdCoeficiente = (int)cnn.ExecuteScalar("SELECT last_insert_rowid()");

            }

        }

        /// <summary>
        /// Salva os itens da composição no banco de dados
        /// </summary>
        /// <param name="itens"></param>
        public void ComposiçãoItensListSave(ObservableCollection<ItensCpuDb> itens)
        {

            MySqlTransaction transaction;
            MySqlCommand command = new MySqlCommand();

            // Inicia a conexão com o banco de dados
            using (MySqlConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                cnn.Open();

                transaction = cnn.BeginTransaction();

                command.Connection = cnn;
                command.Transaction = transaction;

                foreach (var item in itens)
                {
                    // Limpa os parametros
                    command.Parameters.Clear();

                    // Adiciona o insumo a tabela RefInsumos
                    command.CommandText = @"INSERT INTO refinsumoscoeficientes (idRefInsumos, idRefInsumosItem, Coeficiente) 
                                            VALUES (
                                                (SELECT idRefInsumos FROM RefInsumos WHERE CodigoRef=@InsumosCodigoRef AND Tipo=0),
                                                (SELECT idRefInsumos FROM RefInsumos WHERE CodigoRef=@ItemCodigoRef AND Tipo=@Tipo),
                                                @Coeficiente
                                            );";

                    // Seta as variaveis
                    command.Parameters.AddWithValue("@InsumosCodigoRef", item.ComposicaoCodigoRef);
                    command.Parameters.AddWithValue("@ItemCodigoRef", item.InsumoCodigoRef);
                    command.Parameters.AddWithValue("@Tipo", item.InsumoTipo == "COMPOSICAO" ? 0 : 5);
                    command.Parameters.AddWithValue("@Coeficiente", item.Quantidade);
                    command.ExecuteNonQuery();

                }

                transaction.Commit();
            }

        }

        #endregion

        #region Bases de Referência

        /// <summary>
        /// Relaciona as bases de referencia cadastradas no sistema
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<BaseReferencia> BaseRefLista()
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<BaseReferencia>(
                    "SELECT * FROM refbases;",
                    new DynamicParameters());

                List<BaseReferencia> lista = output.ToList();

                return new ObservableCollection<BaseReferencia>(lista);
            }
        }

        /// <summary>
        /// Relaciona as datas base para os preços disponíveis para a bases de referencia selecionada
        /// </summary>
        /// <param name="id">Código da base de referencia</param>
        /// <returns></returns>
        public ObservableCollection<BaseReferenciaDataBase> DataBaseDePrecoLista(int id)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<BaseReferenciaDataBase>("SELECT * FROM engenharia.refprecobase WHERE idRefBases=@Id;", new { Id = id });

                List<BaseReferenciaDataBase> lista = output.ToList();

                return new ObservableCollection<BaseReferenciaDataBase>(lista);
            }
        }

        #endregion

        #region Orçamentos

        #region Geral

        public OrcamentoManager OrcamentoDados(int id)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<OrcamentoManager>("SELECT * FROM OrcPrincipal WHERE idOrcPrincipal=@Id;", new { Id = id }).First();

                return output;
            }
        }

        public ObservableCollection<Bdi> BdiLista(int id)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Bdi>("SELECT Descricao, Valor, idOrcBdis as Id FROM orcBdis WHERE idOrcRevisao=@Id;", new { Id = id });

                return new ObservableCollection<Bdi>(output.ToList());
            }
        }

        public ObservableCollection<LeisSociais> LeisSociaisLista(int id)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<LeisSociais>("SELECT Descricao, Valor, idOrcLeisSociais as Id FROM orcLeisSociais WHERE idOrcRevisao=@Id;", new { Id = id });

                return new ObservableCollection<LeisSociais>(output.ToList());
            }
        }


        public List<ItensOrcamentoDb> OrcamentoLista(int id)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<ItensOrcamentoDb>("SELECT * FROM orcOrcamento WHERE idOrcRevisao=@Id;", new { Id = id });

                return new List<ItensOrcamentoDb>(output.ToList());
            }
        }

        #endregion

        #region Base do Orçamento

        /// <summary>
        /// Carrega os insumos/composições salvos no orçamento
        /// </summary>
        /// <param name="revisao"></param>
        /// <returns></returns>
        public ObservableCollection<IInsumoGeral> InsumosOrcamentoLista(int revisao)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                // Localiza todos os insumos da base do orçamento
                IEnumerable<InsumoGeral> insumos = cnn.Query<InsumoGeral>(@"SELECT ins.idOrcInsumos as id, ins.idRefInsumos as IdBaseReferencia, ins.Descricao as Descrição, ins.Unidade, ins.idTipoInsumos as Tipo, preco.Preco as ValorUnitario 
                                                                            FROM orcinsumos as ins
                                                                            INNER JOIN refprecos as preco ON ins.idRefInsumos = preco.idRefInsumos
                                                                            WHERE ins.idOrcRevisa = @Id AND ins.idTipoInsumos>0;", new { Id = revisao });

                // Localiza todos os insumos da base do orçamento
                IEnumerable<CpuGeral> cpus = cnn.Query<CpuGeral>(@"SELECT ins.idOrcInsumos as id, ins.idRefInsumos as IdBaseReferencia, ins.Descricao as Descrição, ins.Unidade, ins.idTipoInsumos as Tipo, preco.Preco as ValorUnitario 
                                                                   FROM orcinsumos as ins
                                                                   INNER JOIN refprecos as preco ON ins.idRefInsumos = preco.idRefInsumos
                                                                   WHERE ins.idOrcRevisa = @Id AND ins.idTipoInsumos=0;", new { Id = revisao });

                // Localiza os coeficientes das cpus
                IEnumerable<ItensCpuOrc> coefs = cnn.Query<ItensCpuOrc>(@"SELECT coef.* FROM orcinsumoscoeficientes as coef
                                                                          INNER JOIN orcinsumos as ins ON coef.idOrcInsumos = ins.idOrcInsumos
                                                                          WHERE idOrcRevisa = @Id;", new { Id = revisao });

                List<IInsumoGeral> listaGeral = new List<IInsumoGeral>();

                //listaGeral.;
                listaGeral.AddRange(insumos.ToList());
                listaGeral.AddRange(cpus.ToList());

                // Processa as cpus
                foreach (var item in listaGeral)
                {
                    // Verifica se o item é do tipo cpu
                    if (item.Tipo == 0)
                    {
                        List<ItensCpuOrc> coefsCpu = coefs.Where(x => x.idOrcInsumos == item.Id).ToList();

                        foreach (ItensCpuOrc coefCpu in coefsCpu)
                        {
                            CpuCoefGeral cpuCoefTemp = new CpuCoefGeral()
                            {
                                Coeficiente = coefCpu.Coeficiente,
                                IdCoeficiente = coefCpu.idOrcInsumosCoeficientes,
                                Insumo = listaGeral.Where(x => x.Id == coefCpu.idOrcInsumosItem).First()
                            };

                            ((CpuGeral)item).Itens.Add(cpuCoefTemp);
                        }

                    }
                }

                return new ObservableCollection<IInsumoGeral>(listaGeral);
            }
        }

        public int InsumosOrcamentoAdicionarDaBase(int IdInsumo, int IdRevisao)
        {
            int output;

            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {

                output = cnn.ExecuteScalar<int>("SET @@SESSION.max_sp_recursion_depth=25;CALL SP_IMPORTAR_INS_BASE_REF(@Id, @Revisao);", new { Id = IdInsumo, Revisao = IdRevisao });
                
            }

            return output;
        }

        public ObservableCollection<IInsumoGeral> InsumosOrcamentoListaAtualizar(ObservableCollection<IInsumoGeral> ItensBase, int Revisao)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                // Localiza todos os insumos da base do orçamento
                IEnumerable<InsumoGeral> insumos = cnn.Query<InsumoGeral>(@"SELECT ins.idOrcInsumos as id, ins.idRefInsumos as IdBaseReferencia, ins.Descricao as Descrição, ins.Unidade, ins.idTipoInsumos as Tipo, preco.Preco as ValorUnitario 
                                                                            FROM orcinsumos as ins
                                                                            INNER JOIN refprecos as preco ON ins.idRefInsumos = preco.idRefInsumos
                                                                            WHERE ins.idOrcRevisa = @Id AND ins.idTipoInsumos>0;", new { Id = Revisao });

                // Localiza todos os insumos da base do orçamento
                IEnumerable<CpuGeral> cpus = cnn.Query<CpuGeral>(@"SELECT ins.idOrcInsumos as id, ins.idRefInsumos as IdBaseReferencia, ins.Descricao as Descrição, ins.Unidade, ins.idTipoInsumos as Tipo, preco.Preco as ValorUnitario 
                                                                   FROM orcinsumos as ins
                                                                   INNER JOIN refprecos as preco ON ins.idRefInsumos = preco.idRefInsumos
                                                                   WHERE ins.idOrcRevisa = @Id AND ins.idTipoInsumos=0;", new { Id = Revisao });

                // Localiza os coeficientes das cpus
                IEnumerable<ItensCpuOrc> coefs = cnn.Query<ItensCpuOrc>(@"SELECT coef.* FROM orcinsumoscoeficientes as coef
                                                                          INNER JOIN orcinsumos as ins ON coef.idOrcInsumos = ins.idOrcInsumos
                                                                          WHERE idOrcRevisa = @Id;", new { Id = Revisao });

                List<IInsumoGeral> listaGeral = new List<IInsumoGeral>();

                // Gera uma lista com todos os insumos do banco de daos
                listaGeral.AddRange(insumos.ToList());
                listaGeral.AddRange(cpus.ToList());

                // Verifica se todos os insumos estão na lista base
                foreach (var item in listaGeral)
                {
                    if (ItensBase.Where(x => x.Id == item.Id).Count() == 0)
                    {
                        ItensBase.Add(item);
                    }
                }

                // Processa as cpus dos itens acrescidos a lista
                foreach (var item in listaGeral)
                {

                    // Verifica se o item é do tipo cpu
                    if (item.Tipo == 0)
                    {
                        // Verifica se a composição está vazia e se estiver adiciona os insumos e coeficientes
                        if (((CpuGeral)item).Itens.Count() == 0)
                        {
                            List<ItensCpuOrc> coefsCpu = coefs.Where(x => x.idOrcInsumos == item.Id).ToList();

                            foreach (ItensCpuOrc coefCpu in coefsCpu)
                            {
                                CpuCoefGeral cpuCoefTemp = new CpuCoefGeral()
                                {
                                    Coeficiente = coefCpu.Coeficiente,
                                    IdCoeficiente = coefCpu.idOrcInsumosCoeficientes,
                                    Insumo = listaGeral.Where(x => x.Id == coefCpu.idOrcInsumosItem).First()
                                };

                                ((CpuGeral)item).Itens.Add(cpuCoefTemp);
                            }
                        }
                    }
                }

                return new ObservableCollection<IInsumoGeral>(listaGeral);
            }
        }

        #endregion

        #region Manipular Orçamento

        /// <summary>
        /// Vincula o item do orçamento ao item da base de insumos do orçamento
        /// </summary>
        /// <param name="idItemBase"></param>
        /// <param name="idItemOrcamento"></param>
        /// <returns></returns>
        public void OrcamentoVincularItem(int idItemBase, int idItemOrcamento)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                var output = cnn.Execute("UPDATE orcorcamento SET idOrcInsumos = @idInsumo WHERE idOrcOrcamento=@idInsumoOrc;", new { idInsumo = idItemBase, idInsumoOrc = idItemOrcamento });
            }
        }

        /// <summary>
        /// Vincula o item do orçamento ao item da base de insumos do orçamento
        /// </summary>
        /// <param name="idItemBase"></param>
        /// <param name="idItemOrcamento"></param>
        /// <returns></returns>
        public void OrcamentoDesvincularItem(int idItemOrcamento)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                var output = cnn.Execute("UPDATE orcorcamento SET idOrcInsumos = null WHERE idOrcOrcamento=@idInsumoOrc;", new { idInsumoOrc = idItemOrcamento });
            }
        }

        #endregion

        #endregion

    }
}
