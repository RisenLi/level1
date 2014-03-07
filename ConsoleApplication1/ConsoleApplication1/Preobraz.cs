using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    static class Preobraz
    {
        public static string UdalSimv(string word)
        {
            string result = "";
            for (int i = 0; i < word.Length; i++)
            {
                if (!(" —–-.,:;!&?[]{}<>%$()'\"".Contains(word[i]) || word[i] == '\n' || word[i] == '\r'))
                    result += word[i];
            }
            return result;
        }
        public static List<string> Unicul(List<string> starSpisok)
        {
            List<string> result = new List<string>();
            foreach (var word in starSpisok)
                if (!result.Contains(word.ToLower()))
                    result.Add(word.ToLower());
            return result;
        }
    }
}
