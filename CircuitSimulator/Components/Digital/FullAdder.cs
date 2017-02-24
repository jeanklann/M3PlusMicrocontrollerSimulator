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
    public class FullAdder : Chip {
        public Pin A { get { return Pins[0]; } }
        public Pin B { get { return Pins[1]; } }
        public Pin Cin { get { return Pins[2]; } }
        public Pin S { get { return Pins[3]; } }
        public Pin Cout { get { return Pins[4]; } }

        public FullAdder(string name = "Full Adder") : base(name, 5) {

        }

        protected override void AllocatePins() {
            for(int i = 0; i < 3; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for(int i = 3; i < 5; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if(simulationId == circuit.SimulationId) return false;
            for(int i = 0; i < 3; i++) {
                if(Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }

        protected internal override void Execute() {
            base.Execute();
            if(A.GetDigital() == Pin.HIGH) {
                if(B.GetDigital() == Pin.HIGH) {
                    if(Cin.GetDigital() == Pin.HIGH) { //111
                        S.value = Pin.HIGH;
                        Cout.value = Pin.HIGH;
                    } else { //110
                        S.value = Pin.LOW;
                        Cout.value = Pin.HIGH;
                    }
                } else {
                    if(Cin.GetDigital() == Pin.HIGH) { //101
                        S.value = Pin.LOW;
                        Cout.value = Pin.HIGH;
                    } else { //100
                        S.value = Pin.HIGH;
                        Cout.value = Pin.LOW;
                    }
                }
            } else {
                if(B.GetDigital() == Pin.HIGH) {
                    if(Cin.GetDigital() == Pin.HIGH) { // 011
                        S.value = Pin.LOW;
                        Cout.value = Pin.HIGH;
                    } else { //010
                        S.value = Pin.HIGH;
                        Cout.value = Pin.LOW;
                    }
                } else {
                    if(Cin.GetDigital() == Pin.HIGH) { //001
                        S.value = Pin.HIGH;
                        Cout.value = Pin.LOW;
                    } else { //000
                        S.value = Pin.LOW;
                        Cout.value = Pin.LOW;
                    }
                }
            }
            for(int i = 0; i < 3; i++) {
                Pins[i].Propagate();
            }
        }
    }
}
