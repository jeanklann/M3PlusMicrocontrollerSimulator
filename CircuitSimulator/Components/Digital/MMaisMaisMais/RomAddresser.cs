using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class RomAddresser : Chip {
        private float lastClockL = Pin.LOW;
        private float lastClockH = Pin.LOW;
        private byte RegL = 0;
        private byte RegH = 0;

        public RomAddresser(string name = "RomAddresser") : base(name, 38) {

        }

        protected override void AllocatePins() {
            for (int i = 0; i < 14; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (int i = 14; i < 38; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            for (int i = 8; i < 14; i++) {
                if (Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            if(Pins[8].value >= Pin.HALFCUT) {
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

            if ((Pins[9].value >= Pin.HALFCUT && lastClockH < Pin.HALFCUT) ||
                (Pins[10].value >= Pin.HALFCUT && lastClockL < Pin.HALFCUT)) {
                byte val = 0;
                val += (byte)(Pins[0].value >= Pin.HALFCUT ? 1 : 0);
                val += (byte)(Pins[1].value >= Pin.HALFCUT ? 2 : 0);
                val += (byte)(Pins[2].value >= Pin.HALFCUT ? 4 : 0);
                val += (byte)(Pins[3].value >= Pin.HALFCUT ? 8 : 0);
                val += (byte)(Pins[4].value >= Pin.HALFCUT ? 16 : 0);
                val += (byte)(Pins[5].value >= Pin.HALFCUT ? 32 : 0);
                val += (byte)(Pins[6].value >= Pin.HALFCUT ? 64 : 0);
                val += (byte)(Pins[7].value >= Pin.HALFCUT ? 128 : 0);
                if (Pins[9].value >= Pin.HALFCUT && lastClockH < Pin.HALFCUT) {
                    RegH = val;
                } else {
                    RegL = val;
                }
            }

            if (Pins[13].value >= Pin.HALFCUT) {
                RegH = 0;
                RegL = 0;
            }
            
            lastClockH = Pins[9].value;
            lastClockL = Pins[10].value;
            ToOutput();

        }

        private void ToOutput() {
            byte val = RegL;
            for (int i = 14; i < 22; i++)
                Pins[i].value = Pin.LOW;
            if (val >= 128) {
                Pins[21].value = Pin.HIGH;
                val -= 128;
                if(Pins[12].value >= Pin.HALFCUT) {
                    Pins[37].value = Pin.HIGH;
                }
            }
            if (val >= 64) {
                Pins[20].value = Pin.HIGH;
                val -= 64;
                if (Pins[12].value >= Pin.HALFCUT) {
                    Pins[36].value = Pin.HIGH;
                }
            }
            if (val >= 32) {
                Pins[19].value = Pin.HIGH;
                val -= 32;
                if (Pins[12].value >= Pin.HALFCUT) {
                    Pins[35].value = Pin.HIGH;
                }
            }
            if (val >= 16) {
                Pins[18].value = Pin.HIGH;
                val -= 16;
                if (Pins[12].value >= Pin.HALFCUT) {
                    Pins[34].value = Pin.HIGH;
                }
            }
            if (val >= 8) {
                Pins[17].value = Pin.HIGH;
                val -= 8;
                if (Pins[12].value >= Pin.HALFCUT) {
                    Pins[33].value = Pin.HIGH;
                }
            }
            if (val >= 4) {
                Pins[16].value = Pin.HIGH;
                val -= 4;
                if (Pins[12].value >= Pin.HALFCUT) {
                    Pins[32].value = Pin.HIGH;
                }
            }
            if (val >= 2) {
                Pins[15].value = Pin.HIGH;
                val -= 2;
                if (Pins[12].value >= Pin.HALFCUT) {
                    Pins[31].value = Pin.HIGH;
                }
            }
            if (val >= 1) {
                Pins[14].value = Pin.HIGH;
                val -= 1;
                if (Pins[12].value >= Pin.HALFCUT) {
                    Pins[30].value = Pin.HIGH;
                }
            }
            for (int i = 14; i < 22; i++) {
                Pins[i].simulationId = simulationId;
                Pins[i].Propagate();
            }

            val = RegH;
            for (int i = 22; i < 30; i++)
                Pins[i].value = Pin.LOW;
            if (val >= 128) {
                Pins[29].value = Pin.HIGH;
                val -= 128;
                if (Pins[11].value >= Pin.HALFCUT) {
                    Pins[37].value = Pin.HIGH;
                }
            }
            if (val >= 64) {
                Pins[28].value = Pin.HIGH;
                val -= 64;
                if (Pins[11].value >= Pin.HALFCUT) {
                    Pins[36].value = Pin.HIGH;
                }
            }
            if (val >= 32) {
                Pins[27].value = Pin.HIGH;
                val -= 32;
                if (Pins[11].value >= Pin.HALFCUT) {
                    Pins[35].value = Pin.HIGH;
                }
            }
            if (val >= 16) {
                Pins[26].value = Pin.HIGH;
                val -= 16;
                if (Pins[11].value >= Pin.HALFCUT) {
                    Pins[34].value = Pin.HIGH;
                }
            }
            if (val >= 8) {
                Pins[25].value = Pin.HIGH;
                val -= 8;
                if (Pins[11].value >= Pin.HALFCUT) {
                    Pins[33].value = Pin.HIGH;
                }
            }
            if (val >= 4) {
                Pins[24].value = Pin.HIGH;
                val -= 4;
                if (Pins[11].value >= Pin.HALFCUT) {
                    Pins[32].value = Pin.HIGH;
                }
            }
            if (val >= 2) {
                Pins[23].value = Pin.HIGH;
                val -= 2;
                if (Pins[11].value >= Pin.HALFCUT) {
                    Pins[31].value = Pin.HIGH;
                }
            }
            if (val >= 1) {
                Pins[22].value = Pin.HIGH;
                val -= 1;
                if (Pins[11].value >= Pin.HALFCUT) {
                    Pins[30].value = Pin.HIGH;
                }
            }
            for (int i = 22; i < 30; i++) {
                Pins[i].simulationId = simulationId;
                Pins[i].Propagate();
            }
            if(Pins[11].value >= Pin.HALFCUT || Pins[12].value >= Pin.HALFCUT) {
                for (int i = 30; i < 38; i++) {
                    Pins[i].simulationId = simulationId;
                    Pins[i].Propagate();
                }
            }
        }
    }

}
