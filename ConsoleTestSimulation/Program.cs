using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using M3PlusSimulator;


namespace ConsoleTestSimulation {
    class Program {
        const int to = 100000000;
        static void Main(string[] args) {
            Stopwatch timer = new Stopwatch();
            Console.WriteLine("Iniciar");
            Console.ReadLine();
            /*
            int[] programa = {
                0x07, 0xc0, 0x00, 0xdb, 0xe0, 0x07, 0x03, 0x00, 0x03, 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            };
            
            Helpers.GenerateFunctions(programa);
            */

            Command c1 = new Command();
            
            Console.ReadLine();
        }
        
    }

    enum asd {
        nada, alto, baixo
    }
}
