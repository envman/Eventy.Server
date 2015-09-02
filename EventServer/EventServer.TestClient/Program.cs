using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventServer.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        private void Run()
        {
            DrawSplash();

            Console.ReadLine();
        }

        private static void DrawSplash()
        {
            Console.WriteLine("Test Console");
            Console.WriteLine("------------");

            Console.WriteLine(@"                     ..-^ ~~~^ -..");
            Console.WriteLine(@"                   .~             ~.");
            Console.WriteLine(@"                  (;:             :;)");
            Console.WriteLine(@"                   (:             :)");
            Console.WriteLine(@"                     ':._     _.:'");
            Console.WriteLine(@"                          | |");
            Console.WriteLine(@"                        (=====)");
            Console.WriteLine(@"                          | |");
            Console.WriteLine(@"                          | |");
            Console.WriteLine(@"                          | |");
            Console.WriteLine(@"                       ((/   \))");
        }
    }
}
