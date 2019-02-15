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
        
    }
}
