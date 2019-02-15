namespace M3PlusMicrocontroller
{
    public class Acumulator : Direction
    {
        public override byte Value
        {
            get => Simulador.Reg[0];
            set => Simulador.Reg[0] = value;
        }

        public override string Description => "o registrador acumulador";
        public override string Instruction => "A";
    }
}