using System;

namespace M3PlusMicrocontroller
{
    public class Register : Direction
    {
        public byte WitchOne { get; set; }
        private static readonly char[] Registers = new[] {new char() , 'B', 'C', 'D', 'E'};

        public override byte Value
        {
            get => Simulador.Reg[WitchOne];
            set => Simulador.Reg[WitchOne] = value;
        }

        public override string Description => $"o acumulador {Registers[WitchOne]}";
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
                throw new CompilerError($"{register} is not a valid register.");
            WitchOne = (byte) index;
        }
    }
    public class Acumulator : Direction
    {
        public override byte Value
        {
            get => Simulador.Reg[0];
            set => Simulador.Reg[0] = value;
        }

        public override string Description => "o registrador acumulador";
        public override string Instruction => "A";

        public Acumulator()
        {
        }
    }
}