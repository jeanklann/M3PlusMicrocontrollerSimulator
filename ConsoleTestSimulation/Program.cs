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
            //Stopwatch timer = new Stopwatch();
            //Console.WriteLine("Iniciar");
            //Console.ReadLine();
            /*
            int[] programa = {
                0x07, 0xc0, 0x00, 0xdb, 0xe0, 0x07, 0x03, 0x00, 0x03,
            };
            
            Command[] Commands = Helpers.GenerateFunctions(programa);
            */

            /*
            Command c1 = new Command();
            Simulator s = new Simulator();
            s.NextInstruction = 3;
            
            c1.Execute = delegate (Simulator ss) {
                
            };
            */
            string Program = 
                "MOV 37,A\n"+
                "loop:\n"+
                "SUB 1,A\n"+
                "JMPZ fora\n"+
                "JMP loop\n"+
                "fora:\n";

            TokenAnalyser tka = new TokenAnalyser();
            tka.Program = Program;

            while(true) {
                Token token = tka.NextToken();
                Console.WriteLine("Type: \"" + token.Type + "\", Value: \"" + token.Value + "\".");
                Console.ReadLine();
            }
            
        }
        
    }

    enum asd {
        nada, alto, baixo
    }
}
