using System;
using System.Threading;
using System.IO;
using System.Drawing;

namespace didaticos.redimensionar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando redimensionador");

            Thread thread = new System.Threading.Thread(Redimensionar);

            thread.Start();


            Console.WriteLine("Tecle para fechar");

            Console.Read();
        }

        static void Redimensionar()
        {

            #region "Diretorios"
            string diretorio_entrada = "Arquivos_Entrada";
            string diretorio_finalizado = "Arquivos_Finalizados";
            string diretorio_redimensionado = "Arquivos_Redimensionados";

            if (!Directory.Exists(diretorio_entrada))
            {
                Directory.CreateDirectory(diretorio_entrada);
            }
            if (!Directory.Exists(diretorio_finalizado))
            {
                Directory.CreateDirectory(diretorio_finalizado);
            }
            if (!Directory.Exists(diretorio_redimensionado))
            {
                Directory.CreateDirectory(diretorio_redimensionado);
            }
            #endregion


            while (true)
            {
                //Meu programa vai olhar para a pasta de entrada
                var arquivosEntrada = Directory.EnumerateFiles(diretorio_entrada);
                //SE tiver aquivo, ele irá redimencionar
                int novaAltura = 200;

                FileStream fileStream;
                FileInfo fileInfo;
                    
                foreach (var arquivo in arquivosEntrada)
                {

                    var teste = arquivosEntrada;

                    fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    fileInfo = new FileInfo(arquivo);


                    string caminho = Environment.CurrentDirectory + @"\" + diretorio_redimensionado 
                        + @"\" + DateTime.Now.Millisecond.ToString() + "_" + fileInfo.Name;

                    //ler o tamanho que irá redimensionar

                    Redimensionador(Image.FromStream(fileStream), novaAltura, caminho);

                    //fecha o arquivo
                    fileStream.Close();
                    //copia os arquivos redimensionados para a pasta de redimensionados

                    string caminhoFinalizado = Environment.CurrentDirectory + @"\" + diretorio_finalizado + @"\" + fileInfo.Name;

                    fileInfo.MoveTo(caminhoFinalizado);

                    //move o arquivo de entrada para a pasta de finalizados
                }

               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagem"> Imagem a ser redimensionada</param>
        /// <param name="altura">Altura que desejamos redimensionar</param>
        /// <param name="caminho">Caminho aonde iremos gravar o arquivo redimencionado</param>
        /// <returns></returns>
        
        static void Redimensionador(Image imagem, int altura, string caminho)
        {
            double ratio = (double)altura / imagem.Height;

            int novaLargura = (int)(imagem.Width * ratio);
            int novaAltura = (int)(imagem.Height * ratio);

            Bitmap novaImagem = new Bitmap(novaLargura, novaAltura);

            using (Graphics g = Graphics.FromImage(novaImagem)) 
            {
                g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);
            }

            novaImagem.Save(caminho);

            imagem.Dispose();

        }
        
    }
}
