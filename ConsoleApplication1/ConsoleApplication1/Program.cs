using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApplication1
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            int count = 0;
            List<String> text = new List<string>();
            text.AddRange(Clipboard.GetText().Split(' ', ',', '.', ':', ';', '\n', '\r', '?', '!', '–', '-', '/', '—', '='));
            text.Sort();
            foreach (var word in Preobraz.Unicul(text))
                if (word != "")
                {
                    if (count == 1000)
                        break;
                    Console.WriteLine(Preobraz.UdalSimv(word));
                    count++;
                }
        }
    }
}
