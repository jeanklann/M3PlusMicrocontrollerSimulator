using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class PortBank:Chip {
        public PortBank(string name = "PortBank") : base(name, 85){

        }

        protected override void AllocatePins() {
            for (int i = 0; i < 45; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (int i = 45; i < 85; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            for (int i = 0; i < 45; i++) {
                if (Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }

        protected internal override void Execute() {
            base.Execute();
            throw new NotImplementedException();
        }
    }
    
}
