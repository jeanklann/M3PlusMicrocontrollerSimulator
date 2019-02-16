namespace CircuitSimulator
{
    public class XorGate : Gate
    {
        public XorGate(int inputQuantity = 2, string name = "Xor Gate") : base(name, inputQuantity + 1)
        {
        }

        protected internal override float Operation(Pin pin)
        {
            return Output.Xor(pin);
        }
    }
}