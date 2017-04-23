using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class Registrers : Chip {
        public Registrers(string name = "Registrers") : base(name, 21) {

        }

        protected override void AllocatePins() {
            for (int i = 0; i < 13; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (int i = 13; i < 21; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            for (int i = 0; i < 13; i++) {
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
