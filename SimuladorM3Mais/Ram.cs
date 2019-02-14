namespace M3PlusMicrocontroller
{
    public class Ram : Direction
    {
        public override string Description => $"o endereço {Helpers.ToHex(Address)} da memória RAM";
        public override string Instruction => $"#{Helpers.ToHex(Address)}";

        public override byte Value
        {
            get => Simulador.Ram[Address];
            set => Simulador.Ram[Address] = value;
        }

        public byte Address { get; set; }

        public Ram(byte address)
        {
            Address = address;
        }
    }
}