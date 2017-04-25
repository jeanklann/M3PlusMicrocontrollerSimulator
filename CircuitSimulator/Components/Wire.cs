using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components {
    public class Wire:Component {

        public Wire(string name = "Wire") : base(name, 2) {
        }

        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            /*
            for (int i = 0; i < Pins.Length; i++) {
                if (Pins[i].simulationId == circuit.SimulationId) {
                    return false;
                }
            }*/
            return true;
        }

        protected internal override void Execute() {
            int index = -1;
            for (int i = 0; i < Pins.Length; i++) {
                if (Pins[i].simulationId == circuit.SimulationId) {
                    index = i;
                    break;
                }
            }
            if (index == -1) throw new Exception("Erro interno");

            base.Execute();
            
            for(int i = 0; i < Pins.Length; i++) {
                if (i == index) continue;
                Pins[i].value = Pins[index].value;
                Pins[i].isOpen = Pins[index].isOpen;
                Pins[i].Propagate();
            }
        }
    }
}
