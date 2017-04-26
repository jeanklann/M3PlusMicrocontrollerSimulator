using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class RamMemory : Chip {
        public byte[] InternalValue = new byte[256];
        public RamMemory(string name = "RamMemory") : base(name, 19) {

        }

        protected override void AllocatePins() {
            for (int i = 0; i < 11; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (int i = 11; i < 19; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            for (int i = 8; i <= 10; i++) {
                if (Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            if(Pins[8].value >= Pin.HALFCUT) {
                if (Pins[9].value >= Pin.HALFCUT) {
                    for (int i = 0; i < 8; i++) {
                        if (Pins[i].simulationId != circuit.SimulationId) {
                            return false;
                        }
                    }
                } else {
                    for (int i = 11; i < 19; i++) {
                        if (Pins[i].simulationId != circuit.SimulationId) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        protected internal override void Execute() {
            simulationId = circuit.SimulationId;
            if (Pins[10].value >= Pin.HALFCUT) {
                for (int i = 0; i < 256; i++) {
                    InternalValue[i] = 0;
                }
            }
            if(Pins[8].value >= Pin.HALFCUT) {
                int address = 0;
                address += (byte)(Pins[0].value >= Pin.HALFCUT ? 1 : 0);
                address += (byte)(Pins[1].value >= Pin.HALFCUT ? 2 : 0);
                address += (byte)(Pins[2].value >= Pin.HALFCUT ? 4 : 0);
                address += (byte)(Pins[3].value >= Pin.HALFCUT ? 8 : 0);
                address += (byte)(Pins[4].value >= Pin.HALFCUT ? 16 : 0);
                address += (byte)(Pins[5].value >= Pin.HALFCUT ? 32 : 0);
                address += (byte)(Pins[6].value >= Pin.HALFCUT ? 64 : 0);
                address += (byte)(Pins[7].value >= Pin.HALFCUT ? 128 : 0);

                if(Pins[9].value >= Pin.HALFCUT) {
                    byte val = InternalValue[address];
                    for (int i = 11; i <= 18; i++)
                        Pins[i].value = Pin.LOW;
                    if (val >= 128) {
                        Pins[18].value = Pin.HIGH;
                        val -= 128;
                    }
                    if (val >= 64) {
                        Pins[17].value = Pin.HIGH;
                        val -= 64;
                    }
                    if (val >= 32) {
                        Pins[16].value = Pin.HIGH;
                        val -= 32;
                    }
                    if (val >= 16) {
                        Pins[15].value = Pin.HIGH;
                        val -= 16;
                    }
                    if (val >= 8) {
                        Pins[14].value = Pin.HIGH;
                        val -= 8;
                    }
                    if (val >= 4) {
                        Pins[13].value = Pin.HIGH;
                        val -= 4;
                    }
                    if (val >= 2) {
                        Pins[12].value = Pin.HIGH;
                        val -= 2;
                    }
                    if (val >= 1) {
                        Pins[11].value = Pin.HIGH;
                        val -= 1;
                    }
                    for (int i = 11; i <= 18; i++) {
                        Pins[i].simulationId = simulationId;
                        Pins[i].Propagate();
                    }
                } else {
                    byte value = 0;
                    value += (byte)(Pins[11].value >= Pin.HALFCUT ? 1 : 0);
                    value += (byte)(Pins[12].value >= Pin.HALFCUT ? 2 : 0);
                    value += (byte)(Pins[13].value >= Pin.HALFCUT ? 4 : 0);
                    value += (byte)(Pins[14].value >= Pin.HALFCUT ? 8 : 0);
                    value += (byte)(Pins[15].value >= Pin.HALFCUT ? 16 : 0);
                    value += (byte)(Pins[16].value >= Pin.HALFCUT ? 32 : 0);
                    value += (byte)(Pins[17].value >= Pin.HALFCUT ? 64 : 0);
                    value += (byte)(Pins[18].value >= Pin.HALFCUT ? 128 : 0);
                }

            }
            
        }
    }

}
