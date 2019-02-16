namespace CircuitSimulator
{
    public class NandGate : Gate
    {
        public NandGate(int inputQuantity = 2, string name = "Nand Gate") : base(name, inputQuantity + 1)
        {
        }

        protected internal override float Operation(Pin pin)
        {
            return Output.Nand(pin);
        }
    }
}