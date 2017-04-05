using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator {
    /// <summary>
    /// Flipflop JK: <para />
    /// J: pin 0,<para />
    /// K: pin 1,<para />
    /// CLK (desc): pin 2,<para />
    /// S: pin 3,<para />
    /// R: pin 4,<para />
    /// Q: pin 5,<para />
    /// </summary>
    public class FlipflopD : Chip {
        private float lastClk = Pin.LOW;

        public Pin D { get { return Pins[0]; } }
        public Pin CLK { get { return Pins[1]; } }
        public Pin S { get { return Pins[2]; } }
        public Pin R { get { return Pins[3]; } }
        public Pin Q { get { return Pins[4]; } }
        public Pin Qnot { get { return Pins[5]; } }

        public FlipflopD(string name = "Flipflop component") : base(name, 6) {

        }

        protected override void AllocatePins() {
            for(int i = 0; i < 4; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for(int i = 4; i < 6; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if(simulationId == circuit.SimulationId) return false;
            for(int i = 0; i < 2; i++) { //not needed to verify if S and R is connected
                if(Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }
        protected internal override void Execute() {
            base.Execute();
            if (S.GetDigital() == Pin.HIGH) { //S = 1
                Q.value = Pin.HIGH;
                Qnot.value = Pin.LOW;
            } else if (R.GetDigital() == Pin.HIGH) { // R = 1
                Q.value = Pin.LOW;
                Qnot.value = Pin.HIGH;
            } else if (CLK.value == Pin.LOW && lastClk == Pin.HIGH) { //Clock desc
                if (D.GetDigital() == Pin.HIGH) { //D = 1
                    Q.value = Pin.HIGH;
                    Qnot.value = Pin.LOW;
                } else { //D = 0
                    Q.value = Pin.LOW;
                    Qnot.value = Pin.HIGH;
                }
            }
            lastClk = CLK.value;
            Q.Propagate();
            Qnot.Propagate();
        }
    }
}
