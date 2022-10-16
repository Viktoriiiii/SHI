using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDictionary
{
    public class DictionaryTask
    {
        private Dictionary<string, int> GetDictionary(string fileContent)
        {
            string AbsentSignsInWords = "1234567890[](){}<>.,:;?!-*@#$%^&=+`~/|№'\"";            
            StringBuilder sb = new StringBuilder(fileContent);
            foreach (char c in AbsentSignsInWords)
                sb = sb.Replace(c, ' ');
            sb = sb.Replace(Environment.NewLine, " ");
            string[] words = sb.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, int> wordsCount = new Dictionary<string, int>();
            foreach (string w in words)
                if (wordsCount.TryGetValue(w, out int c))
                    wordsCount[w] = c + 1;
                else
                    wordsCount.Add(w, 1);
            wordsCount = wordsCount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return wordsCount;
        }

        public Dictionary<string, int> GetDictionaryWithThreading(string fileContent)
        {
            string AbsentSignsInWords = "1234567890[](){}<>.,:;?!-*@#$%^&=+`~/|№'\"";
            StringBuilder sb = new StringBuilder(fileContent);
            foreach (char c in AbsentSignsInWords)
                sb = sb.Replace(c, ' ');
            sb = sb.Replace(Environment.NewLine, " ");
            string[] words = sb.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            ConcurrentDictionary<string, int> cd = new ConcurrentDictionary<string, int>();
            string[] differentWords = words.Distinct().ToArray();
            Parallel.ForEach(differentWords, word =>
            {
                int c = words.Where(ex => ex.Equals(word)).Count();
                cd.TryAdd(word, c);
            });          
            Dictionary<string, int> wordsCount = new Dictionary<string, int>();
            wordsCount = cd.ToDictionary(x => x.Key, x => x.Value);
            wordsCount = wordsCount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return wordsCount;
        }        
    }
}
