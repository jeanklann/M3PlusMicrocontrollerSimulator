using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace M3PlusMicrocontroller {
    public class Program {
        public static void Main(string[] args) {
            string prog = "apagado:\nmov IN4,a\nand 32,a\njmpz apagado\npisca:\nmov 01,a\nmov a,out1\nmov 00,a\nmov a,out1\njmp pisca";
            Compiler compiler = new Compiler();
            Simulator simulator = new Simulator();
            simulator.Program =  compiler.Compile(prog); //gera os tokens e instancia todas as funções das instruções.
            simulator.Run_v1();




            Console.ReadLine();
        }
    }
}
