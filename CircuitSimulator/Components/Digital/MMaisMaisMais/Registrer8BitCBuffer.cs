using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class Registrer8BitCBuffer : Chip {
        public Registrer8BitCBuffer(string name = "Registrer8BitCBuffer") : base(name, 27) {

        }

        protected override void AllocatePins() {
            for (int i = 0; i < 11; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (int i = 11; i < 27; i++) {
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
