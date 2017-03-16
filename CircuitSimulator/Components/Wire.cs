using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components {
    public class Wire:Component {

        public Wire(string name = "Wire") : base(name, 2) {
        }

        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            for (int i = 0; i < Pins.Length; i++) {
                if (Pins[i].simulationId != circuit.SimulationId) {
                    return true;
                }
            }
            return true;
        }

        protected internal override void Execute() {
            float value = Pin.LOW;
            for (int i = 0; i < Pins.Length; i++) {
                if (Pins[i].simulationId == circuit.SimulationId) {
                    value = Pins[i].Value;
                }
            }

            base.Execute();
            
            for(int i = 0; i < Pins.Length; i++) {
                Pins[i].value = value;
                Pins[i].Propagate();
            }
        }
    }
}
