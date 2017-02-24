using System;

namespace CircuitSimulator {
    public class AndGate : Gate {
        public AndGate(int inputQuantity = 2, string name = "And Gate") : base(name, inputQuantity+1) {
            
        }
        protected internal override float Operation(Pin pin) {
            return Output.And(pin);
        }
    }
}
