using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class RamMemory : Chip {
        public RamMemory(string name = "RamMemory") : base(name, 19) {

        }

        protected override void AllocatePins() {
            for (int i = 0; i < 11; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (int i = 11; i < 19; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            for (int i = 0; i < 11; i++) {
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
