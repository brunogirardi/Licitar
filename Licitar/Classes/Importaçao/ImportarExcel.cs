using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using ExcelDataReader;
using System.Linq;

namespace Licitar
{
    public class ImportarExcel
    {
        /// <summary>
        /// Importa os insumos SINAPI a partir do excel divulgado pela Caixa Econômica
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<IInsumoGeral> CarregarInsumos()
        {

            ObservableCollection<IInsumoGeral> insumos = new ObservableCollection<IInsumoGeral>();

            // Mostra o dialog para seleção do arquivo
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Selecione o arquivo para importação";
            dialog.Filter = "Excel file (*.xls; *.xlsx)|*.xls;*.xlsx";

            // Abre a janela de seleção do arquivo e verifica se a seleção foi válida
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                FileStream fs = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read);

                using (IExcelDataReader excelReader = ExcelReaderFactory.CreateReader(fs))
                {
                    int i = 1;

                    while (excelReader.Read())
                    {

                        if (i > 7)
                        {
                            InsumoGeral novo = new InsumoGeral() { 
                                CodigoRef = excelReader.GetDouble(0).ToString(),
                                Descrição = excelReader.GetString(1).Trim(),
                                Unidade =  excelReader.GetString(2).Trim(),
                                ValorUnitario = double.Parse(excelReader.GetString(4).Trim()),
                                Tipo = tipoInsumo.Indefinido
                            };

                            insumos.Add(novo);

                        }

                        i += 1;
                    }
                }
            }

            MysqlDataAccess.InsumoSaveList(insumos);

            return insumos;

        }


        /// <summary>
        /// Importa as composições SINAPI a partir do excel divulgado pela Caixa Econômica
        /// </summary>
        /// <returns></returns>
        public static void CarregarComposicoes()
        {

            ObservableCollection<IInsumoGeral> cpus = new ObservableCollection<IInsumoGeral>();

            // Mostra o dialog para seleção do arquivo
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Selecione o arquivo para importação";
            dialog.Filter = "Excel file (*.xls; *.xlsx)|*.xls;*.xlsx";

            // Abre a janela de seleção do arquivo e verifica se a seleção foi válida
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                FileStream fs = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read);

                using (IExcelDataReader excelReader = ExcelReaderFactory.CreateReader(fs))
                {

                    int i = 1;

                    // 1a Etapa - Relaciona todos os cabeçalhos das composições
                    while (excelReader.Read())
                    {
                        // Começa a processar apartir da 7a linha
                        if (i > 7)
                        {
                            // Escreve os dados principais da CPU
                            if (excelReader.GetString(11) == "")
                            {
                                InsumoGeral cpuGeral = new InsumoGeral()
                                {
                                    CodigoRef = excelReader.GetString(6).Trim(),
                                    Descrição = excelReader.GetString(7).Trim(),
                                    Unidade = excelReader.GetString(8).Trim(),
                                    Tipo = tipoInsumo.Composicao
                                };
                                cpus.Add(cpuGeral);
                            }
                        }
                        i += 1;
                    }
                }

                // Salva no banco de dados os headers das composições
                MysqlDataAccess.InsumoSaveList(cpus);

                Factory.AccessoAppProvider.Composicoes = cpus;

                //    // Recebe a lista de insumos cadastradas no banco de dados
                //    cpus = SqliteDateAccess.InsumosListar();

                //    // Inicia o contador de linhas
                //    i = 1;

                //    // Resetar o ponteiro
                //    excelReader.Reset();

                //    // Variavel com a lista de itens
                //    ObservableCollection<ItensCpuDb> itens = new ObservableCollection<ItensCpuDb>();

                //    // Processa as informações dos insumos das composições
                //    while (excelReader.Read())
                //    {

                //        if (i > 7)
                //        {
                //            // Processa apenas insumos de composição
                //            if (excelReader.GetString(11) != "")
                //            {
                //                // Cria uma nova instancia para registrar o item
                //                ItensCpuDb novo = new ItensCpuDb()
                //                {
                //                    ComposicaoId = cpus.Where(x => x.CodigoRef == excelReader.GetString(6).Trim()).Select(a => a.Id).First(),
                //                    InsumoId = cpus.Where(x => x.CodigoRef == excelReader.GetString(12).Trim()).Select(a => a.Id).First(),
                //                    Quantidade = double.Parse(excelReader.GetString(16).Trim())
                //                };

                //                // Adiciona o insumo na composição
                //                itens.Add(novo);
                //            }

                //        }
                //        i += 1;
                //    }

                //    SqliteDateAccess.ComposiçãoItensListSave(itens);
                //}
            }
        }
    }
}
