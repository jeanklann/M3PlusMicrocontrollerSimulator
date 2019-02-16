using System;

namespace M3PlusMicrocontroller
{
    public class Output : Register
    {
        private static readonly string[] registers = {"OUT0", "OUT1", "OUT2", "OUT3"};

        public Output(string register)
        {
            register = register.ToUpper();
            var index = Array.IndexOf(registers, register);
            if (index >= registers.Length || index < 0)
                throw new Exception($"{register} is not a valid output.");
            WitchOne = (byte) index;
        }

        public override string Description => $"a saída {registers[WitchOne]}";
        public override string Instruction => registers[WitchOne];

        public override byte Value
        {
            get => Simulador.Out[WitchOne];
            set => Simulador.Out[WitchOne] = value;
        }
    }
}