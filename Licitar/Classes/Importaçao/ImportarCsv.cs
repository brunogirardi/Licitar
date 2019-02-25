using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Licitar
{
    class ImportarCsv
    {
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
            dialog.Filter = "Comma separated value (*.csv)|*.csv";

            // Abre a janela de seleção do arquivo e verifica se a seleção foi válida
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                FileStream fs = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read);

                // Inicia a leitura do arquivo
                using (StreamReader Reader = new StreamReader(fs, Encoding.UTF7))
                {

                    // 1a Etapa - Relaciona todos os cabeçalhos das composições
                    while (!Reader.EndOfStream)
                    {

                        string[] dados = Reader.ReadLine().Split(';');

                        // Escreve os dados principais da CPU
                        if (dados[11] == "")
                        {
                            InsumoGeral cpuGeral = new InsumoGeral()
                            {
                                CodigoRef = dados[6],
                                Descrição = dados[7],
                                Unidade = dados[8],
                                ValorUnitario = double.Parse(dados[10]),
                                Tipo = tipoInsumo.Composicao
                            };
                            cpus.Add(cpuGeral);
                        }
                    }

                    // Salva na tabela de insumo o titulo das composições
                    MysqlDataAccess.InsumoSaveList(cpus);

                    // Reinicia o contador para iniciar o processamento dos itens da composição
                    Reader.BaseStream.Position = 0;

                    // Inicia o processamento das composições
                    ObservableCollection<ItensCpuDb> itens = new ObservableCollection<ItensCpuDb>();

                    // Contador para submeter requests sem ter timeout
                    int contador = 1;

                    while (!Reader.EndOfStream)
                    {

                        string[] dados = Reader.ReadLine().Split(';');

                        if (dados[11] != "")
                        {
                            // Cria uma nova instancia para registrar o item
                            ItensCpuDb novo = new ItensCpuDb()
                            {
                                ComposicaoCodigoRef = dados[6],
                                InsumoCodigoRef = dados[12],
                                InsumoTipo = dados[11],
                                Quantidade = double.Parse(dados[16])
                            };

                            // Adiciona o insumo na composição
                            itens.Add(novo);
                        }

                        if (contador >= 3000)
                        {
                            // Salva os coeficientes no banco de dados
                            MysqlDataAccess.ComposiçãoItensListSave(itens);

                            // Zera o contador
                            contador = 0;

                            // Limpa a coleção
                            itens.Clear();
                        }

                        contador += 1;
                    }

                    // Atualiza a lista de Composições
                    Factory.AccessoAppProvider.Composicoes = cpus;

                }


            }
        }
    }
}
