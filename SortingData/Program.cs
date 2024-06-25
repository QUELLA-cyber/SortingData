using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var inputFilePath = GetInputFilePath(args);
                var inputText = ReadInputFile(inputFilePath);
                if (string.IsNullOrWhiteSpace(inputText))
                {
                    Console.WriteLine("Этот файл пустой.");
                    CleanupOutputFiles(5, Directory.GetCurrentDirectory());
                    Environment.Exit(0);
                }

                var records = ParseInputTextData(inputText);
                SaveData(records);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        /// <summary>
        /// Получение пути к входному файлу
        /// </summary>
        private static string GetInputFilePath(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: SortData <testdata.txt> [numberOfParts] [outputDirectory]");
                Environment.Exit(-1);
            }
            return args[0];
        }

        /// <summary>
        /// Чтение входного файла
        /// </summary>
        private static string ReadInputFile(string path)
        {
            return File.ReadAllText(path, Encoding.UTF8);
        }

        /// <summary>
        /// Парсинг входных данных
        /// </summary>
        private static List<ExtracteData> ParseInputTextData(string text)
        {
            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var records = new List<ExtracteData>();
            ExtracteData currentObject = null;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (currentObject != null)
                    {
                        records.Add(currentObject);
                        currentObject = null;
                    }
                }
                else if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    if (currentObject != null)
                    {
                        records.Add(currentObject);
                    }
                    currentObject = new ExtracteData
                    {
                        Title = line.Trim('[', ']')
                    };
                    currentObject.OriginalLines.Add(line);
                }
                else if (line.StartsWith("Connect="))
                {
                    if (currentObject != null)
                    {
                        currentObject.Connect = line.Substring(8).Trim('"', ';');
                        currentObject.OriginalLines.Add(line);
                    }
                }
                else if (line.StartsWith("ID="))
                {
                    if (currentObject != null)
                    {
                        currentObject.ID = line.Substring(3).Trim('"', ';');
                        currentObject.OriginalLines.Add(line);
                    }
                }
                else
                {
                    if (currentObject != null)
                    {
                        currentObject.OriginalLines.Add(line);
                    }
                }
            }

            if (currentObject != null)
            {
                records.Add(currentObject);
            }

            return records;
        }

        /// <summary>
        /// Сохранение данных
        /// </summary>
        private static void SaveData(List<ExtracteData> records)
        {
            int numberOfParts = 5; // Можно изменить, если нужно
            string outputDirectory = Directory.GetCurrentDirectory();

            var validObjects = new List<ExtracteData>();
            var invalidObjects = new List<ExtracteData>();

            foreach (var record in records)
            {
                if (Validator.IsValid(record))
                {
                    validObjects.Add(record);
                }
                else
                {
                    invalidObjects.Add(record);
                }
            }

            Console.WriteLine($"Обнаружено {validObjects.Count} корректных и {invalidObjects.Count} некорректных записей.");

            if (invalidObjects.Count > 0)
            {
                SaveInvalidData(invalidObjects, outputDirectory);
                Console.WriteLine($"Неправильные данные: {invalidObjects.Count}. Посмотрите в '{outputDirectory}/bad_data.txt'.");
            }
            else if (File.Exists(Path.Combine(outputDirectory, "bad_data.txt")))
            {
                File.Delete(Path.Combine(outputDirectory, "bad_data.txt"));
            }

            if (validObjects.Count > 0)
            {
                FileProcessing.SaveValidData(validObjects, numberOfParts, outputDirectory);
                Console.WriteLine($"Правильные данные: {validObjects.Count}. Посмотрите в '{outputDirectory}/base_*.txt' файлах.");
            }
            else
            {
                CleanupOutputFiles(numberOfParts, outputDirectory);
            }
        }

        /// <summary>
        /// Очистка выходных файлов
        /// </summary>
        private static void CleanupOutputFiles(int numberOfParts, string outputDirectory)
        {
            for (int i = 1; i <= numberOfParts; i++)
            {
                string fileName = Path.Combine(outputDirectory, $"base_{i}.txt");
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
            string badDataFileName = Path.Combine(outputDirectory, "bad_data.txt");
            if (File.Exists(badDataFileName))
            {
                File.Delete(badDataFileName);
            }
        }

        /// <summary>
        /// Сохранение невалидных данных
        /// </summary>
        private static void SaveInvalidData(List<ExtracteData> invalidObjects, string outputDirectory)
        {
            string badDataPath = Path.Combine(outputDirectory, "bad_data.txt");
            Console.WriteLine($"Сохранение некорректных данных в {badDataPath}");

            using (StreamWriter writer = new StreamWriter(badDataPath, false, Encoding.UTF8))
            {
                foreach (var obj in invalidObjects)
                {
                    foreach (var line in obj.OriginalLines)
                    {
                        writer.WriteLine(line);
                    }
                    writer.WriteLine();
                }
            }

            if (File.Exists(badDataPath))
            {
                Console.WriteLine("Файл bad_data.txt был успешно создан.");
            }
            else
            {
                Console.WriteLine("Ошибка: файл bad_data.txt не был создан.");
            }
        }

        /// <summary>
        /// Обработка ошибок
        /// </summary>
        private static void HandleError(Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }
}
