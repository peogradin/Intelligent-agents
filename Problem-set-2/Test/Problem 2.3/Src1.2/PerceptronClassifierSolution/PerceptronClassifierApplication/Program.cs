using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
//using NLP.Tokenization;
//using System.Text;

namespace PerceptronClassifierApplication
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            //Console.WriteLine("Running Tokenizer Tests...\n");
            //TokenizerTests.RunTests();

            //Console.WriteLine("\nAll tests completed.");
            //Console.ReadLine(); // Keep console open

        }
    }
}
