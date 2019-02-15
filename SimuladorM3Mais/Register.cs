using System;

namespace M3PlusMicrocontroller
{
    public class Register : Direction
    {
        public byte WitchOne { get; set; }
        private static readonly char[] registers = {new char() , 'B', 'C', 'D', 'E'};

        public override byte Value
        {
            get => Simulador.Reg[WitchOne];
            set => Simulador.Reg[WitchOne] = value;
        }

        public override string Description => $"o acumulador {registers[WitchOne]}";
        public override string Instruction => registers[WitchOne].ToString();

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
            var index = Array.IndexOf(registers, register);
            if (index >= registers.Length || index < 0)
                throw new CompilerError($"{register} is not a valid register.");
            WitchOne = (byte) index;
        }
    }
}