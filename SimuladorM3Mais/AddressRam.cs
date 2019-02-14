namespace M3PlusMicrocontroller
{
        public class AddressRam : Direction
        {
            public override string Description => $"o endereço que está {Register.Description} da memória RAM";
            public override string Instruction => $"#{Register.Instruction}";
            public Register Register { get; set; }

            public override byte Value
            {
                get => Simulador.Ram[Register.Value];
                set => Simulador.Ram[Register.Value] = value;
            }

            public AddressRam(Register register)
            {
                Register = register;
            }
        }
}