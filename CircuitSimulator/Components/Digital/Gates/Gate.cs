namespace CircuitSimulator {
    public abstract class Gate : Component {
        public Pin Output => Pins[Pins.Length - 1];

        public Gate(string name = "Generic gate component", int pinQuantity = 2) : base(name, pinQuantity) {
            //other pins = inputs
            //last pint

        }
        protected override void AllocatePins() {
            for(var i = 0; i < Pins.Length-1; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            Pins[Pins.Length - 1] = new Pin(this, true, false);
        }
        internal override bool CanExecute() {
            if(SimulationIdInternal == Circuit.SimulationId) return false;
            for(var i = 0; i < Pins.Length-1; i++) {
                if(Pins[i].SimulationIdInternal != Circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }
        protected internal override void Execute() {
            base.Execute();
            Output.SetDigital(Pins[0].Value);
            for (var i = 1; i < Pins.Length - 1; i++) {
                Output.SetDigital(Operation(Pins[i]));
            }
            Output.Propagate();
        }

        protected internal abstract float Operation(Pin pin);

    }
}
