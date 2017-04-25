using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class Counter8Bit : Chip {

        private byte internalValue = 0;
        private float lastClock = Pin.LOW;
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
            simulationId = circuit.SimulationId;

            if(lastClock <= Pin.HALFCUT && Pins[3].value >= Pin.HALFCUT) {
                if (Pins[1].value <= Pin.HALFCUT)
                    ++internalValue;
                else
                    --internalValue;
            }
            lastClock = Pins[3].value;
            if (Pins[2].value >= Pin.HALFCUT) {
                internalValue = 0;
            }
            if (Pins[0].value >= Pin.HALFCUT) {
                byte val = internalValue;
                for (int i = 4; i < Pins.Length; i++)
                    Pins[i].value = Pin.LOW;
                if (val >= 128) {
                    Pins[11].value = Pin.HIGH;
                    val -= 128;
                }
                if (val >= 64) {
                    Pins[10].value = Pin.HIGH;
                    val -= 64;
                }
                if (val >= 32) {
                    Pins[9].value = Pin.HIGH;
                    val -= 32;
                }
                if (val >= 16) {
                    Pins[8].value = Pin.HIGH;
                    val -= 16;
                }
                if (val >= 8) {
                    Pins[7].value = Pin.HIGH;
                    val -= 8;
                }
                if (val >= 4) {
                    Pins[6].value = Pin.HIGH;
                    val -= 4;
                }
                if (val >= 2) {
                    Pins[5].value = Pin.HIGH;
                    val -= 2;
                }
                if (val >= 1) {
                    Pins[4].value = Pin.HIGH;
                    val -= 1;
                }
                for (int i = 4; i < Pins.Length; i++) {
                    Pins[i].simulationId = simulationId;
                    Pins[i].Propagate();
                }
            }
        }
    }

}
