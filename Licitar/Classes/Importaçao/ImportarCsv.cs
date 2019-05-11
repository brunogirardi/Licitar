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
        /// Importa as composições SINAPI a partir de um arquivo CSV
        /// </summary>
        /// <returns></returns>
        public async static Task CarregarComposicoesAsync()
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
                    await Factory.DBAcesso.InsumoSaveListAsync(cpus);

                    MessageBox.Show("Concluído a importação dos Headers das composições");

                    // Reinicia o contador para iniciar o processamento dos itens da composição
                    Reader.BaseStream.Position = 0;

                    //Inicia o processamento das composições
                    ObservableCollection<ItensCpuDb> itens = new ObservableCollection<ItensCpuDb>();

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

                    }

                    MessageBox.Show("Iniciando a importação dos coeficientes das composições");

                    // Salva os coeficientes no banco de dados
                    await Factory.DBAcesso.ComposiçãoItensListSaveAsync(itens);

                }

            }
        }

        /// <summary>
        /// Importa os insumos SINAPI a partir de um arquivo CSV
        /// </summary>
        /// <returns></returns>
        public async static Task CarregarInsumosAsync()
        {

            ObservableCollection<IInsumoGeral> insumos = new ObservableCollection<IInsumoGeral>();

            // Mostra o dialog para seleção do arquivo
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Selecione o arquivo para importação";
            dialog.Filter = "Comma separated value (*.csv)|*.csv";

            // Abre a janela de seleção do arquivo e verifica se a seleção foi válida
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                FileStream fs = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read);

                using (StreamReader Reader = new StreamReader(fs, Encoding.UTF7))
                {

                    // 1a Etapa - Relaciona todos os cabeçalhos das composições
                    while (!Reader.EndOfStream)
                    {
                        try
                        {
                            string[] dados = Reader.ReadLine().Split(';');

                            if (dados.Length == 5)
                            {
                                InsumoGeral novo = new InsumoGeral()
                                {
                                    CodigoRef = dados[0],
                                    Descrição = dados[1],
                                    Unidade = dados[2],
                                    ValorUnitario = double.Parse(dados[4]),
                                    Tipo = tipoInsumo.Indefinido
                                };

                                insumos.Add(novo);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + " Local:" + Reader.ReadLine());
                        }
                    
                    }

                    // Salva na tabela de insumo o titulo das composições
                    await Factory.DBAcesso.InsumoSaveListAsync(insumos);
                }
            }

        }

    }
}
