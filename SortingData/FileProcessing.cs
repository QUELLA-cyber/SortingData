using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingData
{
    /// <summary>
    /// Обработка файла
    /// </summary>
    public static class FileProcessing
    {
        /// <summary>
        /// Сохраняет корректные данные в файлы.
        /// </summary>
        public static void SaveValidData(List<ExtracteData> validObjects, int numberOfParts, string outputDirectory)
        {
            int totalValidObjects = validObjects.Count;
            int objectsPerFile = totalValidObjects / numberOfParts;
            int remainingObjects = totalValidObjects % numberOfParts;

            List<List<ExtracteData>> fileData = new List<List<ExtracteData>>();

            for (int i = 0; i < numberOfParts; i++)
            {
                fileData.Add(new List<ExtracteData>());
            }

            for (int i = 0; i < totalValidObjects; i++)
            {
                fileData[i % numberOfParts].Add(validObjects[i]);
            }

            for (int i = 0; i < numberOfParts; i++)
            {
                string fileName = Path.Combine(outputDirectory, $"base_{i + 1}.txt");
                if (fileData[i].Count == 0)
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                    continue;
                }

                using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
                {
                    foreach (var obj in fileData[i])
                    {
                        foreach (var line in obj.OriginalLines)
                        {
                            writer.WriteLine(line);
                        }
                        writer.WriteLine();
                    }
                }
            }

            for (int i = numberOfParts; i < validObjects.Count; i++)
            {
                string fileName = Path.Combine(outputDirectory, $"base_{i + 1}.txt");
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
        }
    }
}

