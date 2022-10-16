using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace ClassLibraryDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            string nameFile = args[0];
            string fileContent = File.ReadAllText(nameFile);
            var dt = new DictionaryTask();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var di = dt.GetType().InvokeMember("GetDictionary", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, dt, new object[] {fileContent});
            sw.Stop();
            Console.WriteLine("Время выполнения для однопоточной реализации: {0}", sw.ElapsedMilliseconds);
            WriteAllWordsInFile((Dictionary<string, int>)di, nameFile);

            sw.Restart();
            var diThread = dt.GetDictionaryWithThreading(fileContent);
            sw.Stop();
            Console.WriteLine("Время выполнения для многопоточной реализации: {0}", sw.ElapsedMilliseconds);
            WriteAllWordsInFile(diThread, nameFile+".threads");

            Console.ReadKey();
        }

        /// <summary>
        /// Записывает в файл перечисление всех уникальных слов, встречающихся в тесте, 
        /// и количеством их употреблений, отсортированный в порядке убывания количества употреблений
        /// </summary>
        /// <param name="dict"> Сортированный словарь </param>
        /// <param name="nameFile"> Название файла </param>
        private static void WriteAllWordsInFile(Dictionary<string, int> dict, string nameFile)
        {
            string[] content = new string[dict.Count];
            int c = 0;
            foreach (var d in dict)
            {
                content[c] = d.Key + " " + d.Value;
                c++;
            }
            File.WriteAllLines(nameFile + ".count.txt", content);
        }
    }
}
