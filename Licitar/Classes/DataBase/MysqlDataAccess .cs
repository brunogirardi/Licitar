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

        public static ObservableCollection<IInsumoGeral> InsumosListar()
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<InsumoGeral>(
                    @"SELECT ins.*, preco.Preco as ValorUnitario
                      FROM refinsumos as ins
                      INNER JOIN refprecos as preco
                      ON ins.idRefInsumos = preco.idRefInsumos
                      WHERE preco.idRefPrecoBase = 1 AND ins.Tipo > 0", 
                    new DynamicParameters());

                List<IInsumoGeral> lista = output.ToList<IInsumoGeral>();

                return new ObservableCollection<IInsumoGeral>(lista);
            }
        }

        public static IInsumoGeral InsumoBuscar(int id)
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
        public static int InsumoBuscarIdComOCodigoRef(int CodigoRef)
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
        public static void InsumoSave(IInsumoGeral insumo)
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
        public static void InsumoSaveList(ObservableCollection<IInsumoGeral> Lista)
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


        public static ObservableCollection<IInsumoGeral> ComposiçãoListar(int codigo)
        {
            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<InsumoGeral>(
                    @"SELECT ins.CodigoRef, ins.Descrição, ins.Unidade, ins.Tipo, coef.Coeficiente, p.Preco
                      FROM refinsumoscoeficientes as coef
                      INNER JOIN refinsumos as ins ON coef.idRefInsumosItem = ins.idRefInsumos
                      INNER JOIN refprecos as p ON p.idRefInsumos = coef.idRefInsumosItem
                      WHERE coef.idRefInsumos = " + codigo + ";",
                    new DynamicParameters()); 
                
                List<IInsumoGeral> lista = output.ToList<IInsumoGeral>();

                return new ObservableCollection<IInsumoGeral>(lista);
            }
        }

        public static void ComposiçãoSave(CpuGeral cpu)
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
        public static void ComposiçãoItemSave(IInsumoGeral item, int idComposicao)
        {

            using (IDbConnection cnn = new MySqlConnection(LoadConnectionString()))
            {

                cnn.Execute("INSERT INTO ComposicaoItem (ComposicaoId, InsumoId, Quantidade) VALUES (@ComposicaoId, @InsumoId, @Quantidade)", new { ComposicaoId = idComposicao, InsumoId = item.Id, item.Quantidade });

                item.Id = (int)cnn.ExecuteScalar("SELECT last_insert_rowid()");

            }

        }

        /// <summary>
        /// Salva os itens da composição no banco de dados
        /// </summary>
        /// <param name="itens"></param>
        public static void ComposiçãoItensListSave(ObservableCollection<ItensCpuDb> itens)
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

    }
}
