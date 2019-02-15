using System.Collections.Generic;
using M3PlusMicrocontroller;

namespace M3PlusMicrocontroller
{
    public static class Functions
    {
        
        public static readonly Function Add = (simulator, from, to, i) => from.Value + simulator.Reg[0];
        public static readonly Function Sub = (simulator, from, to, i) => from.Value - simulator.Reg[0];
        public static readonly Function Mov = (simulator, from, to, i) => from.Value;
        public static readonly Function Inc = (simulator, from, to, i) => from.Value+1;
        public static readonly Function And = (simulator, from, to, i) => from.Value & simulator.Reg[0];
        public static readonly Function Or = (simulator, from, to, i) => from.Value | simulator.Reg[0];
        public static readonly Function Xor = (simulator, from, to, i) => from.Value ^ simulator.Reg[0];
        public static readonly Function Not = (simulator, from, to, i) => (from.Value*-1) & byte.MaxValue;
        public static readonly Function Push = (simulator, from, to, i) =>
        {
            --simulator.PointerStack;
            simulator.Stack[simulator.PointerStack] = to.Value;
            return 0;
        };
        public static readonly Function Pop = (simulator, from, to, i) =>
        {
            to.Value = simulator.Stack[simulator.PointerStack];
            ++simulator.PointerStack;
            return 0;
        };

        public static readonly Function Popa = Pop;
        public static readonly Function Pusha = Push;
        public static readonly Function Jmp = (simulator, from, to, i) =>
        {
            JmpInternal(simulator, to);
            return 0;
        };
        public static readonly Function Jmpz = (simulator, from, to, i) =>
        {
            if (simulator.FlagZ)
                JmpInternal(simulator, to);
            return 0;
        };
        public static readonly Function Jmpc = (simulator, from, to, i) =>
        {
            if (simulator.FlagC)
                JmpInternal(simulator, to);
            return 0;
        };
        
        public static readonly Function Call = (simulator, from, to, i) =>
        {
            if (!(to is Address address))
                throw new CompilerError("Erro na execução do comando.");
            var next = simulator.NextInstruction;

            --simulator.PointerStack;
            simulator.Stack[simulator.PointerStack] = (byte)(next / 256);
            --simulator.PointerStack;
            simulator.Stack[simulator.PointerStack] = (byte)(next % 256);

            --simulator.PointerStack;
            simulator.Stack[simulator.PointerStack] = (byte)(address.ValueAddress / 256);
            --simulator.PointerStack;
            simulator.Stack[simulator.PointerStack] = (byte)(address.ValueAddress % 256);

            simulator.NextInstruction = address.ValueAddress;

            simulator.PointerStack += 2;
                        
            return 0;
        };
        
        public static readonly Function Ret = (simulator, from, to, i) =>
        {
            int next;

            next = simulator.Stack[simulator.PointerStack] % 256;
            ++simulator.PointerStack;
            next += simulator.Stack[simulator.PointerStack] * 256;
            ++simulator.PointerStack;

            simulator.NextInstruction = next;
                        
            return 0;
        };
        
        
        
        private static void JmpInternal(Simulator simulator, Direction to)
        {
            if (!(to is Address address))
                throw new CompilerError("Erro na execução do comando.");
            
            --simulator.PointerStack;
            simulator.Stack[simulator.PointerStack] = (byte) (address.ValueAddress/ 256);
            --simulator.PointerStack;
            simulator.Stack[simulator.PointerStack] = (byte) (address.ValueAddress % 256);
            simulator.NextInstruction = address.ValueAddress;
            simulator.PointerStack += 2;
        }
        public static readonly IEnumerable<FunctionBytes> FunctionBytes = new List<FunctionBytes>
        {
            new FunctionBytes("ADD", Add, FunctionByteType.Operation, 0b000_00_000),
            new FunctionBytes("SUB", Sub, FunctionByteType.Operation, 0b001_00_000),
            new FunctionBytes("AND", And, FunctionByteType.Operation, 0b010_00_000),
            new FunctionBytes("OR", Or, FunctionByteType.Operation, 0b011_00_000),
            new FunctionBytes("XOR", Xor, FunctionByteType.Operation, 0b100_00_000),
            new FunctionBytes("NOT", Not, FunctionByteType.Operation, 0b101_00_000),
            new FunctionBytes("MOV", Mov, FunctionByteType.Operation, 0b110_00_000),
            new FunctionBytes("INC", Inc, FunctionByteType.Operation, 0b111_00_000),
            new FunctionBytes("JMP", Jmp, FunctionByteType.Control, 0b000_00_011, 1),
            new FunctionBytes("JMPC", Jmpc, FunctionByteType.Control, 0b000_00_100, 1),
            new FunctionBytes("JMPZ", Jmpz, FunctionByteType.Control, 0b000_00_101, 1),
            new FunctionBytes("CALL", Call, FunctionByteType.Control, 0b000_00_110, 1),
            new FunctionBytes("RET", Ret, FunctionByteType.Control, 0b000_00_000, 2),
            new FunctionBytes("PUSH", Push, FunctionByteType.Control, 0b000_00_011, 2),
            new FunctionBytes("POP", Pop, FunctionByteType.Control, 0b000_00_100, 2),
            new FunctionBytes("PUSHA", Pusha, FunctionByteType.Control, 0b000_00_101, 2),
            new FunctionBytes("POPA", Popa, FunctionByteType.Control, 0b000_00_110, 2)
        };
        public static readonly IEnumerable<ControlBytes> ControlBytes = new List<ControlBytes>
        {
            
        };
        
    }

    public class ControlBytes
    {
        public byte Bytes { get; set; }
        
    }

    public class FunctionBytes
    {
        public FunctionBytes(string name, Function function, FunctionByteType type, byte bytes, int pageIndex = 0)
        {
            Function = function;
            Bytes = bytes;
            PageIndex = pageIndex;
            Name = name;
            Type = type;
        }

        public Function Function { get; set; }
        public byte Bytes { get; set; }
        public int PageIndex { get; set; }
        public string Name { get; set; }
        public FunctionByteType Type { get; set; }
    }
    
    public enum FunctionByteType
    {
        Operation = 0b111_00_000, 
        Register = 0b000_11_000, 
        Control  = 0b000_00_111
    }
}
