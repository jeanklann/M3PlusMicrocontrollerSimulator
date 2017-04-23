using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class Counter8Bit : Chip {
        public Counter8Bit(string name = "Counter8Bit") : base(name, 12) {

        }

        protected override void AllocatePins() {
            for (int i = 0; i < 4; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (int i = 4; i < 12; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            for (int i = 0; i < 4; i++) {
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
