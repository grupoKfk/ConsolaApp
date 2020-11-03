using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HiloCSharp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            Thread hilo = new Thread(()=>Imprimir("adios", "gente"));
            hilo.Start();

            Console.Write("Hilo finalizado");
            Console.Read();
        }

        static void Imprimir(string str1, string str2) {
            for (int i = 0; i < 1000; i++)
                Console.Write("y");
        }
    }
}
