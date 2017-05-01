using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class Registrer8BitCBuffer : Chip {
        public byte InternalValue = 0;
        private float lastClock = Pin.LOW;

        public Registrer8BitCBuffer(string name = "Registrer8BitCBuffer") : base(name, 27) {

        }

        protected override void AllocatePins() {
            for (int i = 0; i < 11; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (int i = 11; i < 27; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            for (int i = 8; i <= 10; i++) {
                if (Pins[i].simulationId != circuit.SimulationId) return false;
            }
            if(Pins[8].Value >= Pin.HALFCUT && lastClock <= Pin.HALFCUT) {
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

            if (lastClock <= Pin.HALFCUT && Pins[8].Value >= Pin.HALFCUT) {
                InternalValue = 0;
                InternalValue += (byte)(Pins[0].Value >= Pin.HALFCUT ? 1 : 0);
                InternalValue += (byte)(Pins[1].Value >= Pin.HALFCUT ? 2 : 0);
                InternalValue += (byte)(Pins[2].Value >= Pin.HALFCUT ? 4 : 0);
                InternalValue += (byte)(Pins[3].Value >= Pin.HALFCUT ? 8 : 0);
                InternalValue += (byte)(Pins[4].Value >= Pin.HALFCUT ? 16 : 0);
                InternalValue += (byte)(Pins[5].Value >= Pin.HALFCUT ? 32 : 0);
                InternalValue += (byte)(Pins[6].Value >= Pin.HALFCUT ? 64 : 0);
                InternalValue += (byte)(Pins[7].Value >= Pin.HALFCUT ? 128 : 0);
            }
            lastClock = Pins[8].Value;

            if (Pins[10].Value >= Pin.HALFCUT) {
                InternalValue = 0;
            }

            if (Pins[9].Value >= Pin.HALFCUT) {
                byte val = InternalValue;
                for (int i = 19; i < 27; i++)
                    Pins[i].Value = Pin.LOW;
                if (val >= 128) {
                    Pins[26].Value = Pin.HIGH;
                    val -= 128;
                }
                if (val >= 64) {
                    Pins[25].Value = Pin.HIGH;
                    val -= 64;
                }
                if (val >= 32) {
                    Pins[24].Value = Pin.HIGH;
                    val -= 32;
                }
                if (val >= 16) {
                    Pins[23].Value = Pin.HIGH;
                    val -= 16;
                }
                if (val >= 8) {
                    Pins[22].Value = Pin.HIGH;
                    val -= 8;
                }
                if (val >= 4) {
                    Pins[21].Value = Pin.HIGH;
                    val -= 4;
                }
                if (val >= 2) {
                    Pins[20].Value = Pin.HIGH;
                    val -= 2;
                }
                if (val >= 1) {
                    Pins[19].Value = Pin.HIGH;
                    val -= 1;
                }
                for (int i = 19; i < 27; i++) {
                    Pins[i].simulationId = simulationId;
                    Pins[i].Propagate();
                }
            }
            byte val2 = InternalValue;
            for (int i = 11; i < 19; i++)
                Pins[i].Value = Pin.LOW;
            if (val2 >= 128) {
                Pins[18].Value = Pin.HIGH;
                val2 -= 128;
            }
            if (val2 >= 64) {
                Pins[17].Value = Pin.HIGH;
                val2 -= 64;
            }
            if (val2 >= 32) {
                Pins[16].Value = Pin.HIGH;
                val2 -= 32;
            }
            if (val2 >= 16) {
                Pins[15].Value = Pin.HIGH;
                val2 -= 16;
            }
            if (val2 >= 8) {
                Pins[14].Value = Pin.HIGH;
                val2 -= 8;
            }
            if (val2 >= 4) {
                Pins[13].Value = Pin.HIGH;
                val2 -= 4;
            }
            if (val2 >= 2) {
                Pins[12].Value = Pin.HIGH;
                val2 -= 2;
            }
            if (val2 >= 1) {
                Pins[11].Value = Pin.HIGH;
                val2 -= 1;
            }
            for (int i = 11; i < 19; i++) {
                Pins[i].simulationId = simulationId;
                Pins[i].Propagate();
            }

        }
    }

}
