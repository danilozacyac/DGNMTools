using ScjnUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGNMTools.UtilitiesDgnm
{
    public class FileUtilities
    {
        private string filePathOrigin = @"L:\SociosSiger.txt";
        private string filePathDestiny = @"L:\Archivitos\";


        /// <summary>
        /// Obtiene el número total de líneas de texto que contiene el archivo TXT que se quiere partir
        /// </summary>
        /// <returns></returns>
        public int GetTotalLinesOnTxt()
        {
            int totalLines = 0;

            using (var freader = new StreamReader(filePathOrigin))
            {
                try
                {
                    string line;

                    while ((line = freader.ReadLine()) != null)
                    {
                        totalLines++;
                    }
                }
                finally
                {
                    
                }
            }
            return totalLines;
        }

        /// <summary>
        /// Separa un archivo TXT en archivos más pequeños para que puedan ser abiertos por cualquier programa
        /// </summary>
        /// <param name="filePathOrigin">URL de la ruta del archivo original</param>
        /// <param name="fileDirectoryDestiny">URL del directorio donde se depositarán los archivos</param>
        /// <param name="splitIn">Número de partes en las que se dividirá el archivo</param>
        public void SplitBigTextFile(string filePathOrigin, string fileDirectoryDestiny, int splitIn)
        {
            int filePartNumber = 1;
            int maxLineNumber = 10000;
            DateTime date2 = DateTime.Now;
            int currentRowCount = 0;
            using (var freader = new StreamReader(filePathOrigin))
            {
                StreamWriter pak = null;
                try
                {
                    pak = new StreamWriter(GetPackFilename(maxLineNumber, filePartNumber, date2), false);
                    string line;

                    while ((line = freader.ReadLine()) != null)
                    {
                        if (currentRowCount < maxLineNumber)
                        {
                            pak.WriteLine(line); //writing line to small file
                            currentRowCount++; //increasing the lines of small file
                        }
                        else
                        {
                            pak.Flush();
                            pak.Close(); //closing the file
                            currentRowCount = 0;
                            filePartNumber++; //nr++ -> just for file name to be Pack-2;
                            pak = new StreamWriter(GetPackFilename(maxLineNumber, filePartNumber, date2), false);
                        }
                    }
                }
                finally
                {
                    if (pak != null)
                    {
                        pak.Dispose();
                    }
                }
            }
        }

        private string GetPackFilename(int package, int nr, DateTime date2)
        {
            return string.Format("{0}{1}Pack-{2}+_{3}.txt",filePathDestiny, package, nr, DateTimeUtilities.DateToInt( date2));
        }

    }
}
