using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class ULA : Chip {
        public ULA(string name = "ULA") : base(name, 0000000000000000) {

        }

        protected override void AllocatePins() {
            for (int i = 0; i < 0000000000000000; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (int i = 0000000000000000; i < 0000000000000000; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            for (int i = 0; i < 000000000000000000000; i++) {
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
