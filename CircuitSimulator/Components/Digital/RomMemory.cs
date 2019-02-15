namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class RomMemory : Chip {
        public byte[] InternalValue = new byte[65536];
        public RomMemory(string name = "RomMemory") : base(name, 25) {
        }

        protected override void AllocatePins() {
            for (var i = 0; i < 17; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (var i = 17; i < 25; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            if (Pins[16].simulationId != circuit.SimulationId) {
                return false;
            }

            if (Pins[16].Value >= Pin.HALFCUT) {
                for (var i = 0; i < 16; i++) {
                    if (Pins[i].simulationId != circuit.SimulationId) {
                        return false;
                    }
                }
            }
            return true;
        }

        protected internal override void Execute() {
            simulationId = circuit.SimulationId;
            if (Pins[16].Value >= Pin.HALFCUT) {
                var address = 0;
                address += (byte)(Pins[0].Value >= Pin.HALFCUT ? 1 : 0);
                address += (byte)(Pins[1].Value >= Pin.HALFCUT ? 2 : 0);
                address += (byte)(Pins[2].Value >= Pin.HALFCUT ? 4 : 0);
                address += (byte)(Pins[3].Value >= Pin.HALFCUT ? 8 : 0);
                address += (byte)(Pins[4].Value >= Pin.HALFCUT ? 16 : 0);
                address += (byte)(Pins[5].Value >= Pin.HALFCUT ? 32 : 0);
                address += (byte)(Pins[6].Value >= Pin.HALFCUT ? 64 : 0);
                address += (byte)(Pins[7].Value >= Pin.HALFCUT ? 128 : 0);
                address += (byte)(Pins[8].Value >= Pin.HALFCUT ? 256 : 0);
                address += (byte)(Pins[9].Value >= Pin.HALFCUT ? 512 : 0);
                address += (byte)(Pins[10].Value >= Pin.HALFCUT ? 1024 : 0);
                address += (byte)(Pins[11].Value >= Pin.HALFCUT ? 2048 : 0);
                address += (byte)(Pins[12].Value >= Pin.HALFCUT ? 4096 : 0);
                address += (byte)(Pins[13].Value >= Pin.HALFCUT ? 8192 : 0);
                address += (byte)(Pins[14].Value >= Pin.HALFCUT ? 16384 : 0);
                address += (byte)(Pins[15].Value >= Pin.HALFCUT ? 32768 : 0);

                var val = InternalValue[address];
                for (var i = 17; i < 25; i++)
                    Pins[i].Value = Pin.LOW;
                if (val >= 128) {
                    Pins[24].Value = Pin.HIGH;
                    val -= 128;
                }
                if (val >= 64) {
                    Pins[23].Value = Pin.HIGH;
                    val -= 64;
                }
                if (val >= 32) {
                    Pins[22].Value = Pin.HIGH;
                    val -= 32;
                }
                if (val >= 16) {
                    Pins[21].Value = Pin.HIGH;
                    val -= 16;
                }
                if (val >= 8) {
                    Pins[20].Value = Pin.HIGH;
                    val -= 8;
                }
                if (val >= 4) {
                    Pins[19].Value = Pin.HIGH;
                    val -= 4;
                }
                if (val >= 2) {
                    Pins[18].Value = Pin.HIGH;
                    val -= 2;
                }
                if (val >= 1) {
                    Pins[17].Value = Pin.HIGH;
                    val -= 1;
                }
                for (var i = 17; i < 25; i++) {
                    Pins[i].simulationId = simulationId;
                    Pins[i].Propagate();
                }

            }
        }
    }

}
