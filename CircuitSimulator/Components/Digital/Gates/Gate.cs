using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator {
    public abstract class Gate : Component {
        public Pin Output { get { return Pins[Pins.Length - 1]; } }

        public Gate(string name = "Generic gate component", int pinQuantity = 2) : base(name, pinQuantity) {
            //other pins = inputs
            //last pint

        }
        protected override void AllocatePins() {
            for(int i = 0; i < Pins.Length-1; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            Pins[Pins.Length - 1] = new Pin(this, true, false);
        }
        internal override bool CanExecute() {
            if(simulationId == circuit.SimulationId) return false;
            for(int i = 0; i < Pins.Length-1; i++) {
                if(Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }
        protected internal override void Execute() {
            base.Execute();
            Output.SetDigital(Pins[0].value);
            for (int i = 1; i < Pins.Length - 1; i++) {
                Output.SetDigital(Operation(Pins[i]));
            }
            Output.Propagate();
        }

        protected internal abstract float Operation(Pin pin);

    }
}
