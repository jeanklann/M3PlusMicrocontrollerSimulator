using System;

namespace M3PlusMicrocontroller
{
    public class Register : Direction
    {
        public byte WitchOne { get; set; }
        private static readonly char[] Registers = new[] {'A', 'B', 'C', 'D', 'E'};

        public override byte Value
        {
            get => Simulador.Reg[WitchOne];
            set => Simulador.Reg[WitchOne] = value;
        }

        public override string Description => $"o registrador {Registers[WitchOne]}";
        public override string Instruction => Registers[WitchOne].ToString();

        protected Register()
        {
        }

        public Register(byte witchOne)
        {
            WitchOne = witchOne;
        }

        public Register(char register)
        {
            register = char.ToUpper(register);
            var index = Array.IndexOf(Registers, register);
            if (index >= Registers.Length || index < 0)
                throw new Exception($"{register} is not a valid register.");
            WitchOne = (byte) index;
        }
    }
}