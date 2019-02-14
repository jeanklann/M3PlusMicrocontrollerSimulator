using System;

namespace M3PlusMicrocontroller
{
    public class Output : Register
    {
        private static readonly string[] Registers = new[] {"OUT1", "OUT2", "OUT3", "OUT4"};
        public override string Description => $"a saída {Registers[WitchOne]}";
        public override string Instruction => Registers[WitchOne];

        public override byte Value
        {
            get => Simulador.Out[WitchOne];
            set => Simulador.Out[WitchOne] = value;
        }

        public Output(byte witchOne)
        {
            WitchOne = witchOne;
        }

        public Output(string register)
        {
            register = register.ToUpper();
            var index = Array.IndexOf(Registers, register);
            if (index >= Registers.Length || index < 0)
                throw new Exception($"{register} is not a valid output.");
            WitchOne = (byte) index;
        }
    }
}