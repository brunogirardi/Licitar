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

            Factory.DBAcesso.InsumoSaveList(insumos);

            return insumos;

        }
     }
}
