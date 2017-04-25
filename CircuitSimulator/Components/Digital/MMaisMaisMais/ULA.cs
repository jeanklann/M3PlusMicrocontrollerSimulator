using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class ULA : Chip {
        public ULA(string name = "ULA") : base(name, 30) {

        }

        protected override void AllocatePins() {
            for (int i = 0; i < 20; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (int i = 20; i < 30; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            for (int i = 0; i < 20; i++) {
                if (Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }

        protected internal override void Execute() {
            simulationId = circuit.SimulationId;
            
            byte res_byte = 0;
            int res = 0;
            bool fz = false;
            bool fc = false;

            byte a = 0;
            a += (byte)(Pins[0].value >= Pin.HALFCUT ? 1 : 0);
            a += (byte)(Pins[1].value >= Pin.HALFCUT ? 2 : 0);
            a += (byte)(Pins[2].value >= Pin.HALFCUT ? 4 : 0);
            a += (byte)(Pins[3].value >= Pin.HALFCUT ? 8 : 0);
            a += (byte)(Pins[4].value >= Pin.HALFCUT ? 16 : 0);
            a += (byte)(Pins[5].value >= Pin.HALFCUT ? 32 : 0);
            a += (byte)(Pins[6].value >= Pin.HALFCUT ? 64 : 0);
            a += (byte)(Pins[7].value >= Pin.HALFCUT ? 128 : 0);

            byte b = 0;
            b += (byte)(Pins[8].value >= Pin.HALFCUT ? 1 : 0);
            b += (byte)(Pins[9].value >= Pin.HALFCUT ? 2 : 0);
            b += (byte)(Pins[10].value >= Pin.HALFCUT ? 4 : 0);
            b += (byte)(Pins[11].value >= Pin.HALFCUT ? 8 : 0);
            b += (byte)(Pins[12].value >= Pin.HALFCUT ? 16 : 0);
            b += (byte)(Pins[13].value >= Pin.HALFCUT ? 32 : 0);
            b += (byte)(Pins[14].value >= Pin.HALFCUT ? 64 : 0);
            b += (byte)(Pins[15].value >= Pin.HALFCUT ? 128 : 0);

            bool s0 = Pins[16].value >= Pin.HALFCUT;
            bool s1 = Pins[17].value >= Pin.HALFCUT;
            bool s2 = Pins[18].value >= Pin.HALFCUT;
            int s = 0;

            s += s0 ? 1 : 0;
            s += s1 ? 2 : 0;
            s += s2 ? 4 : 0;

            switch (s) {
                case 0:
                    res = a + b;
                    break;
                case 1:
                    res = a - b;
                    break;
                case 2:
                    res = a & b;
                    break;
                case 3:
                    res = a | b;
                    break;
                case 4:
                    res = a ^ b;
                    break;
                case 5:
                    res = 255 ^ b;
                    break;
                case 6:
                    res = b;
                    break;
                case 7:
                    res = ++b;
                    break;
            }

            res_byte = (byte)res;
            if (res >= 256) {
                fc = true;
            }
            if(res == 0) {
                fz = true;
            }
                
            for (int i = 20; i < 30; i++)
                Pins[i].value = Pin.LOW;
            if(fz)
                Pins[28].value = Pin.HIGH;
            if(fc)
                Pins[29].value = Pin.HIGH;
            for (int i = 28; i <= 29; i++) {
                Pins[i].simulationId = simulationId;
                Pins[i].Propagate();
            }
            if (Pins[19].value >= Pin.HALFCUT) {
                if (res_byte >= 128) {
                    Pins[27].value = Pin.HIGH;
                    res_byte -= 128;
                }
                if (res_byte >= 64) {
                    Pins[26].value = Pin.HIGH;
                    res_byte -= 64;
                }
                if (res_byte >= 32) {
                    Pins[25].value = Pin.HIGH;
                    res_byte -= 32;
                }
                if (res_byte >= 16) {
                    Pins[24].value = Pin.HIGH;
                    res_byte -= 16;
                }
                if (res_byte >= 8) {
                    Pins[23].value = Pin.HIGH;
                    res_byte -= 8;
                }
                if (res_byte >= 4) {
                    Pins[22].value = Pin.HIGH;
                    res_byte -= 4;
                }
                if (res_byte >= 2) {
                    Pins[21].value = Pin.HIGH;
                    res_byte -= 2;
                }
                if (res_byte >= 1) {
                    Pins[20].value = Pin.HIGH;
                    res_byte -= 1;
                }
                for (int i = 20; i < 28; i++) {
                    Pins[i].simulationId = simulationId;
                    Pins[i].Propagate();
                }
            }




        }
    }

}
