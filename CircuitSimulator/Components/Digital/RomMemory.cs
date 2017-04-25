using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class RomMemory : Chip {
        public byte[] InternalValue = new byte[65536];
        public RomMemory(string name = "RomMemory") : base(name, 25) {
            InternalValue[0] = 0xff;
        }

        protected override void AllocatePins() {
            for (int i = 0; i < 17; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (int i = 17; i < 25; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            if (Pins[16].simulationId != circuit.SimulationId) {
                return false;
            }

            if (Pins[16].value >= Pin.HALFCUT) {
                for (int i = 0; i < 16; i++) {
                    if (Pins[i].simulationId != circuit.SimulationId) {
                        return false;
                    }
                }
            }
            return true;
        }

        protected internal override void Execute() {
            simulationId = circuit.SimulationId;
            if (Pins[16].value >= Pin.HALFCUT) {
                int address = 0;
                address += (byte)(Pins[0].value >= Pin.HALFCUT ? 1 : 0);
                address += (byte)(Pins[1].value >= Pin.HALFCUT ? 2 : 0);
                address += (byte)(Pins[2].value >= Pin.HALFCUT ? 4 : 0);
                address += (byte)(Pins[3].value >= Pin.HALFCUT ? 8 : 0);
                address += (byte)(Pins[4].value >= Pin.HALFCUT ? 16 : 0);
                address += (byte)(Pins[5].value >= Pin.HALFCUT ? 32 : 0);
                address += (byte)(Pins[6].value >= Pin.HALFCUT ? 64 : 0);
                address += (byte)(Pins[7].value >= Pin.HALFCUT ? 128 : 0);
                address += (byte)(Pins[8].value >= Pin.HALFCUT ? 256 : 0);
                address += (byte)(Pins[9].value >= Pin.HALFCUT ? 512 : 0);
                address += (byte)(Pins[10].value >= Pin.HALFCUT ? 1024 : 0);
                address += (byte)(Pins[11].value >= Pin.HALFCUT ? 2048 : 0);
                address += (byte)(Pins[12].value >= Pin.HALFCUT ? 4096 : 0);
                address += (byte)(Pins[13].value >= Pin.HALFCUT ? 8192 : 0);
                address += (byte)(Pins[14].value >= Pin.HALFCUT ? 16384 : 0);
                address += (byte)(Pins[15].value >= Pin.HALFCUT ? 32768 : 0);

                byte val = InternalValue[address];
                for (int i = 17; i < 25; i++)
                    Pins[i].value = Pin.LOW;
                if (val >= 128) {
                    Pins[24].value = Pin.HIGH;
                    val -= 128;
                }
                if (val >= 64) {
                    Pins[23].value = Pin.HIGH;
                    val -= 64;
                }
                if (val >= 32) {
                    Pins[22].value = Pin.HIGH;
                    val -= 32;
                }
                if (val >= 16) {
                    Pins[21].value = Pin.HIGH;
                    val -= 16;
                }
                if (val >= 8) {
                    Pins[20].value = Pin.HIGH;
                    val -= 8;
                }
                if (val >= 4) {
                    Pins[19].value = Pin.HIGH;
                    val -= 4;
                }
                if (val >= 2) {
                    Pins[18].value = Pin.HIGH;
                    val -= 2;
                }
                if (val >= 1) {
                    Pins[17].value = Pin.HIGH;
                    val -= 1;
                }
                for (int i = 17; i < 25; i++) {
                    Pins[i].simulationId = simulationId;
                    Pins[i].Propagate();
                }

            }
        }
    }

}
