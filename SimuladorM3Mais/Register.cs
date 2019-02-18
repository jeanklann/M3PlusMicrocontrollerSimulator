using System;

namespace M3PlusMicrocontroller
{
    public class Register : Direction
    {
        private static readonly char[] registers = {new char(), 'B', 'C', 'D', 'E'};

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

        public byte WitchOne { get; set; }

        public override byte Value
        {
            get => Simulador.Reg[WitchOne];
            set => Simulador.Reg[WitchOne] = value;
        }

        public override string Description => $"o regitrador {registers[WitchOne]}";
        public override string Instruction => registers[WitchOne].ToString();
    }
}