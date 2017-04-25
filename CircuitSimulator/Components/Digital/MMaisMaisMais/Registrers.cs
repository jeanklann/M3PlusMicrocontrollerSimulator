using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class Registrers : Chip {
        byte[] reg = new byte[4];
        private float lastClock = Pin.LOW;
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
            for (int i = 8; i < 13; i++) {
                if (Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            if(Pins[9].value >= Pin.HALFCUT && lastClock <= Pin.HALFCUT) {
                for (int i = 0; i < 8; i++) {
                    if (Pins[i].simulationId != circuit.SimulationId) {
                        return false;
                    }
                }
            }
            return true;
        }

        protected internal override void Execute() {
            simulationId = circuit.SimulationId;

            if (lastClock <= Pin.HALFCUT && Pins[8].value >= Pin.HALFCUT) {
                byte val = 0;
                val += (byte)(Pins[0].value >= Pin.HALFCUT ? 1 : 0);
                val += (byte)(Pins[1].value >= Pin.HALFCUT ? 2 : 0);
                val += (byte)(Pins[2].value >= Pin.HALFCUT ? 4 : 0);
                val += (byte)(Pins[3].value >= Pin.HALFCUT ? 8 : 0);
                val += (byte)(Pins[4].value >= Pin.HALFCUT ? 16 : 0);
                val += (byte)(Pins[5].value >= Pin.HALFCUT ? 32 : 0);
                val += (byte)(Pins[6].value >= Pin.HALFCUT ? 64 : 0);
                val += (byte)(Pins[7].value >= Pin.HALFCUT ? 128 : 0);
                if (Pins[11].value < Pin.HALFCUT && Pins[12].value < Pin.HALFCUT) reg[0] = val;
                if (Pins[11].value >= Pin.HALFCUT && Pins[12].value < Pin.HALFCUT) reg[1] = val;
                if (Pins[11].value < Pin.HALFCUT && Pins[12].value >= Pin.HALFCUT) reg[2] = val;
                if (Pins[11].value >= Pin.HALFCUT && Pins[12].value >= Pin.HALFCUT) reg[3] = val;
            }

            if (Pins[10].value >= Pin.HALFCUT) {
                for (int i = 0; i < 4; i++) {
                    reg[i] = 0;
                }
            }
            if (Pins[9].value >= Pin.HALFCUT) {
                byte val = 0;
                if (Pins[11].value < Pin.HALFCUT && Pins[12].value < Pin.HALFCUT) val = reg[0];
                if (Pins[11].value >= Pin.HALFCUT && Pins[12].value < Pin.HALFCUT) val = reg[1];
                if (Pins[11].value < Pin.HALFCUT && Pins[12].value >= Pin.HALFCUT) val = reg[2];
                if (Pins[11].value >= Pin.HALFCUT && Pins[12].value >= Pin.HALFCUT) val = reg[3];
                
                for (int i = 13; i < 21; i++)
                    Pins[i].value = Pin.LOW;
                if (val >= 128) {
                    Pins[20].value = Pin.HIGH;
                    val -= 128;
                }
                if (val >= 64) {
                    Pins[19].value = Pin.HIGH;
                    val -= 64;
                }
                if (val >= 32) {
                    Pins[18].value = Pin.HIGH;
                    val -= 32;
                }
                if (val >= 16) {
                    Pins[17].value = Pin.HIGH;
                    val -= 16;
                }
                if (val >= 8) {
                    Pins[16].value = Pin.HIGH;
                    val -= 8;
                }
                if (val >= 4) {
                    Pins[15].value = Pin.HIGH;
                    val -= 4;
                }
                if (val >= 2) {
                    Pins[14].value = Pin.HIGH;
                    val -= 2;
                }
                if (val >= 1) {
                    Pins[13].value = Pin.HIGH;
                    val -= 1;
                }
                for (int i = 13; i < 21; i++) {
                    Pins[i].simulationId = simulationId;
                    Pins[i].Propagate();
                }
            }

        }
    }

}
