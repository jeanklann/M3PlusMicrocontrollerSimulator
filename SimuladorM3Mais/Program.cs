using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace M3PlusMicrocontroller {
    public class Program {
        public static void Main(string[] args) {
            TokenAnalyzer tokenAnaliser = new TokenAnalyzer();
            string prog = "MOV 42,A";
            Compiler compiler = new Compiler();
            compiler.Compile(prog);
            Console.ReadLine();
        }
    }
}
