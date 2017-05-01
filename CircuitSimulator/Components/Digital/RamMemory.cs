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
            if(Pins[8].Value >= Pin.HALFCUT) {
                if (Pins[9].Value >= Pin.HALFCUT) {
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
            if (Pins[10].Value >= Pin.HALFCUT) {
                for (int i = 0; i < 256; i++) {
                    InternalValue[i] = 0;
                }
            }
            if(Pins[8].Value >= Pin.HALFCUT) {
                int address = 0;
                address += (byte)(Pins[0].Value >= Pin.HALFCUT ? 1 : 0);
                address += (byte)(Pins[1].Value >= Pin.HALFCUT ? 2 : 0);
                address += (byte)(Pins[2].Value >= Pin.HALFCUT ? 4 : 0);
                address += (byte)(Pins[3].Value >= Pin.HALFCUT ? 8 : 0);
                address += (byte)(Pins[4].Value >= Pin.HALFCUT ? 16 : 0);
                address += (byte)(Pins[5].Value >= Pin.HALFCUT ? 32 : 0);
                address += (byte)(Pins[6].Value >= Pin.HALFCUT ? 64 : 0);
                address += (byte)(Pins[7].Value >= Pin.HALFCUT ? 128 : 0);

                if(Pins[9].Value >= Pin.HALFCUT) {
                    byte val = InternalValue[address];
                    for (int i = 11; i <= 18; i++)
                        Pins[i].Value = Pin.LOW;
                    if (val >= 128) {
                        Pins[18].Value = Pin.HIGH;
                        val -= 128;
                    }
                    if (val >= 64) {
                        Pins[17].Value = Pin.HIGH;
                        val -= 64;
                    }
                    if (val >= 32) {
                        Pins[16].Value = Pin.HIGH;
                        val -= 32;
                    }
                    if (val >= 16) {
                        Pins[15].Value = Pin.HIGH;
                        val -= 16;
                    }
                    if (val >= 8) {
                        Pins[14].Value = Pin.HIGH;
                        val -= 8;
                    }
                    if (val >= 4) {
                        Pins[13].Value = Pin.HIGH;
                        val -= 4;
                    }
                    if (val >= 2) {
                        Pins[12].Value = Pin.HIGH;
                        val -= 2;
                    }
                    if (val >= 1) {
                        Pins[11].Value = Pin.HIGH;
                        val -= 1;
                    }
                    for (int i = 11; i <= 18; i++) {
                        Pins[i].simulationId = simulationId;
                        Pins[i].Propagate();
                    }
                } else {
                    byte value = 0;
                    value += (byte)(Pins[11].Value >= Pin.HALFCUT ? 1 : 0);
                    value += (byte)(Pins[12].Value >= Pin.HALFCUT ? 2 : 0);
                    value += (byte)(Pins[13].Value >= Pin.HALFCUT ? 4 : 0);
                    value += (byte)(Pins[14].Value >= Pin.HALFCUT ? 8 : 0);
                    value += (byte)(Pins[15].Value >= Pin.HALFCUT ? 16 : 0);
                    value += (byte)(Pins[16].Value >= Pin.HALFCUT ? 32 : 0);
                    value += (byte)(Pins[17].Value >= Pin.HALFCUT ? 64 : 0);
                    value += (byte)(Pins[18].Value >= Pin.HALFCUT ? 128 : 0);
                    InternalValue[address] = value;
                }

            }
            
        }
    }

}
