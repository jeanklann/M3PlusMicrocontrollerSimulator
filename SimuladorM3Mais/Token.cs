namespace M3PlusMicrocontroller
{
    public class Token
    {
        public const char Instructionseparator = ',';
        public const char Identificatorseparator = ':';
        public const char Ram = '#';

        public static readonly string[] Inputs = {"IN0", "IN1", "IN2", "IN3"};
        public static readonly string[] Outputs = {"OUT0", "OUT1", "OUT2", "OUT3"};
        public static readonly string[] Registrers = {"A", "B", "C", "D", "E"};

        public static readonly string[] CpuInstructions =
        {
            "ADD", "SUB", "AND", "OR", "XOR", "NOT", "MOV", "INC", "JMP", "JMPC", "JMPZ", "CALL", "RET", "PUSH", "POP",
            "PUSHA", "POPA"
        };

        public static readonly char[] Lineseparator = {'\n', '\r'};
        public static readonly char[] Space = {' ', '\t'};
        public static readonly string[] Comment = {";", "//"};
        public readonly int Index;

        public readonly TokenType Type;
        public readonly string Value;

        public Token(TokenType type = TokenType.Error, string value = "", int index = 0)
        {
            Type = type;
            Value = value;
            Index = index;
        }
    }
}