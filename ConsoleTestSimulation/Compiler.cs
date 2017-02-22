using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestSimulation {
    public class Compiler {

    }
    public class Token {


        //public static readonly string[] RAMADDRESS = { "#" };
        //public static readonly string[] INPUTS = { "IN1", "IN2", "IN3", "IN4" };
        //public static readonly string[] OUTPUTS = { "OUT1", "OUT2", "OUT3", "OUT4" };
        //public static readonly string[] REGISTRERS = { "A", "B", "C", "D", "E" };
        //public static readonly string[] CPUINSTRUCTIONS = { "MOV", "ADD", "INC", "SUB", "JMP", "JMPZ", "JMPC", "CALL", "RET" };
        //public static readonly string[] LINESEPARATOR = { "\n", "\r\n", "\r" };
        //public static string INSTRUCTIONSEPARATOR = ",";
        //public static string SPACE = " ";
        //public static string IDENTIFICATORSEPARATOR = ":";
        //public static string COMMENT = ";";
        public static List<string> INSTRUCTIONSEPARATOR = new List<string>(new string[] { "," });
        public static List<string> SPACE = new List<string>(new string[] { " " });
        public static List<string> IDENTIFICATORSEPARATOR = new List<string>(new string[] { ":" });
        public static List<string> COMMENT = new List<string>(new string[] { ";" });
        public static List<string> RAMADDRESS = new List<string>(new string[] {"#"});
        public static List<string> INPUTS = new List<string>(new string[] { "IN1", "IN2", "IN3", "IN4" });
        public static List<string> OUTPUTS = new List<string>(new string[] { "OUT1", "OUT2", "OUT3", "OUT4" });
        public static List<string> REGISTRERS = new List<string>(new string[] { "A", "B", "C", "D", "E" });
        public static List<string> CPUINSTRUCTIONS = new List<string>(new string[] { "MOV", "ADD", "INC", "SUB", "JMP", "JMPZ", "JMPC", "CALL", "RET" });
        public static List<string> LINESEPARATOR = new List<string>(new string[] { "\n", "\r\n", "\r" });
        public static List<string> SYMBOLS = new List<string>();





        public TokenType Type = TokenType.EoF;
        public string Value = "";

        public Token(TokenType Type, string Value) {
            this.Type = Type;
            this.Value = Value;
        }
        public Token(TokenType Type) {
            this.Type = Type;
        }
        public Token() {
        }
    }
    public class TokenAnalyser {
        public List<Token> Tokens = new List<Token>();
        public string Program;
        private int Index = 0;
        public Token NextToken() {
            Token tokenTemp = new Token();
            Token token = new Token();
            bool Analyzed = false;
            do {
                
                token.Value = tokenTemp.Value;
            } while(!Analyzed);
            token.Type = tokenTemp.Type;
            return token;
        }

    }
    public enum TokenType {
        Comment, Separator, RamAddress, Number, Input, Output, Registrer, CPUInstruction, Identificator, Space, EoF
    }


}
