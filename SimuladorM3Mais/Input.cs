using System;

namespace M3PlusMicrocontroller
{
    public class Input : Register
    {
        private static readonly string[] Registers = {"IN0", "IN1", "IN2", "IN3"};
        public override string Description => $"a entrada {Registers[WitchOne]}";
        public override string Instruction => Registers[WitchOne];

        public override byte Value
        {
            get => Simulador.In[WitchOne];
            set => Simulador.In[WitchOne] = value;
        }

        public Input(string register)
        {
            register = register.ToUpper();
            var index = Array.IndexOf(Registers, register);
            if (index >= Registers.Length || index < 0)
                throw new Exception($"{register} is not a valid input.");
            WitchOne = (byte) index;
        }
    }
}