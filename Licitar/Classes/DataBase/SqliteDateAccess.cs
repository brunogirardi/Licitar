using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Licitar
{
    public class SqliteDateAccess
    {
        #region Helper Functions

        private static string LoadConnectionString(string id = "SQLite")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        #endregion

        #region Insumos

        public static ObservableCollection<IInsumoGeral> InsumosListar()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<InsumoGeral>("SELECT * FROM BaseInsumo", new DynamicParameters());

                List<IInsumoGeral> lista = output.ToList<IInsumoGeral>();

                return new ObservableCollection<IInsumoGeral>(lista);
            }
        }

        public static IInsumoGeral InsumoBuscar(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.QuerySingle<InsumoGeral>("SELECT * FROM BaseInsumo WHERE Id=@Id", new { Id = id });

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
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                return cnn.ExecuteScalar<int>("SELECT id FROM BaseInsumo WHERE CodigoRef=@CodigoRef", CodigoRef);
            }
        }

        /// <summary>
        /// Insere um insumo ao banco de dados
        /// </summary>
        /// <param name="insumo">Insumo a ser inserido</param>
        public static void InsumoSave(IInsumoGeral insumo)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {

                cnn.Execute("INSERT INTO BaseInsumo (Descrição, Unidade, ValorUnitario, Tipo, CodigoRef) VALUES (@Descrição, @Unidade, @ValorUnitario, @Tipo, @CodigoRef)", insumo);

                insumo.Id = Convert.ToInt32(cnn.ExecuteScalar("SELECT last_insert_rowid()"));

            }
        }

        public static void InsumoSaveList(ObservableCollection<IInsumoGeral> Lista)
        {

            StringBuilder sb = new StringBuilder();

            sb.Append(@"BEGIN TRANSACTION;");

            foreach (var item in Lista)
            {
                sb.Append($"INSERT INTO BaseInsumo (Descrição, Unidade, ValorUnitario, Tipo, CodigoRef) VALUES ('{item.Descrição.Replace("\"", "\\\"").Replace("'", "''")}', '{item.Unidade}', {item.ValorUnitario.ToString().Replace(",", ".")}, {(int)item.Tipo}, '{item.CodigoRef}');");
            }

            sb.Append("COMMIT;");

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute(sb.ToString(), new { });
            }

        }


        #endregion

        #region Composições

        public static void ComposiçãoSave(CpuGeral cpu)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
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

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {

                cnn.Execute("INSERT INTO ComposicaoItem (ComposicaoId, InsumoId, Quantidade) VALUES (@ComposicaoId, @InsumoId, @Quantidade)", new { ComposicaoId = idComposicao, InsumoId = item.Id, item.Quantidade });

                item.Id = (int)cnn.ExecuteScalar("SELECT last_insert_rowid()");

            }

        }


        public static void ComposiçãoItensListSave(ObservableCollection<ItensCpuDb> itens)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"BEGIN TRANSACTION;");

            foreach (var item in itens)
            {
                sb.Append($"INSERT INTO BaseComposicaoItem (ComposicaoId, InsumoId, Quantidade) VALUES ({item.ComposicaoId}, {item.InsumoId}, {item.Quantidade.ToString().Replace(",", ".")});");
            }

            sb.Append("COMMIT;");

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute(sb.ToString(), new { });
            }
        }

        #endregion

    }
}
