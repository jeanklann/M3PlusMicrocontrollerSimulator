using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator{
    public class ShiftRegister:Component {
        private ulong storedValue = 0;
        private byte bits = 8;
        private float lastClock = Pin.LOW;
        /// <summary>
        /// Input value
        /// </summary>
        public Pin Input { get { return Pins[0]; } }
        /// <summary>
        /// Clock (desc)
        /// </summary>
        public Pin Clock { get { return Pins[1]; } }
        
        public ShiftRegister(string name = "Shift Register", byte bits = 8):base(name, bits+2) {
            if(bits >= 1 && bits <= 64)
                this.bits = bits;
            else throw new Exception("Incorrect number of bits: " + bits);
        }
        public Pin Output(int index) {
            return Pins[index + 2];
        }

        protected override void AllocatePins() {
            for(int i = 0; i < 2; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for(int i = 2; i < bits+2; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }

        internal override bool CanExecute() {
            if(simulationId == circuit.SimulationId) return false;
            for(int i = 0; i < 2; i++) {
                if(Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }
        
        protected internal override void Execute() {
            base.Execute();
            if(Clock.value == Pin.LOW && lastClock == Pin.HIGH) {
                storedValue *= 2;
                storedValue += Input.GetDigital() == Pin.HIGH ? 1u : 0u;
                ulong temp = 1;
                for(int i = 0; i < bits; i++) {
                    ulong val = storedValue & temp;
                    if(val > 0) {
                        Pins[i + 2].value = Pin.HIGH;
                    } else {
                        Pins[i + 2].value = Pin.LOW;
                    }
                    temp *= 2;
                }
            }
            lastClock = Clock.value;
            for(int i = 2; i < bits + 2; i++) {
                Pins[i].Propagate();
            }
        }
    }
}
