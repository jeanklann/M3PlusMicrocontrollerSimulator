namespace CircuitSimulator
{
    public class XnorGate : Gate
    {
        public XnorGate(int inputQuantity = 2, string name = "Xnor Gate") : base(name, inputQuantity + 1)
        {
        }

        protected internal override float Operation(Pin pin)
        {
            return Output.Xnor(pin);
        }
    }
}