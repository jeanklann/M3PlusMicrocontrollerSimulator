namespace CircuitSimulator
{
    public class NorGate : Gate
    {
        public NorGate(int inputQuantity = 2, string name = "Nor Gate") : base(name, inputQuantity + 1)
        {
        }

        protected internal override float Operation(Pin pin)
        {
            return Output.Nor(pin);
        }
    }
}