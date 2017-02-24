namespace CircuitSimulator {
    public class OrGate : Gate {
        public OrGate(int inputQuantity = 2, string name = "Or Gate") : base(name, inputQuantity + 1) {

        }
        protected internal override float Operation(Pin pin) {
            return Output.Or(pin);
        }
    }
}
