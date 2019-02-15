namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class Registrers : Chip {
        public byte[] Reg = new byte[4];
        private float _lastClock = Pin.LOW;
        public Registrers(string name = "Registrers") : base(name, 21) {

        }

        protected override void AllocatePins() {
            for (var i = 0; i < 13; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (var i = 13; i < 21; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            for (var i = 8; i < 13; i++) {
                if (Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            if(Pins[9].Value >= Pin.HALFCUT && _lastClock <= Pin.HALFCUT) {
                for (var i = 0; i < 8; i++) {
                    if (Pins[i].simulationId != circuit.SimulationId) {
                        return false;
                    }
                }
            }
            return true;
        }

        protected internal override void Execute() {
            simulationId = circuit.SimulationId;

            if (_lastClock <= Pin.HALFCUT && Pins[9].Value >= Pin.HALFCUT) {
                byte val = 0;
                val += (byte)(Pins[0].Value >= Pin.HALFCUT ? 1 : 0);
                val += (byte)(Pins[1].Value >= Pin.HALFCUT ? 2 : 0);
                val += (byte)(Pins[2].Value >= Pin.HALFCUT ? 4 : 0);
                val += (byte)(Pins[3].Value >= Pin.HALFCUT ? 8 : 0);
                val += (byte)(Pins[4].Value >= Pin.HALFCUT ? 16 : 0);
                val += (byte)(Pins[5].Value >= Pin.HALFCUT ? 32 : 0);
                val += (byte)(Pins[6].Value >= Pin.HALFCUT ? 64 : 0);
                val += (byte)(Pins[7].Value >= Pin.HALFCUT ? 128 : 0);
                if (Pins[11].Value < Pin.HALFCUT && Pins[12].Value < Pin.HALFCUT) Reg[0] = val;
                if (Pins[11].Value >= Pin.HALFCUT && Pins[12].Value < Pin.HALFCUT) Reg[1] = val;
                if (Pins[11].Value < Pin.HALFCUT && Pins[12].Value >= Pin.HALFCUT) Reg[2] = val;
                if (Pins[11].Value >= Pin.HALFCUT && Pins[12].Value >= Pin.HALFCUT) Reg[3] = val;
            }
            _lastClock = Pins[9].Value;

            if (Pins[10].Value >= Pin.HALFCUT) {
                for (var i = 0; i < 4; i++) {
                    Reg[i] = 0;
                }
            }
            if (Pins[8].Value >= Pin.HALFCUT) {
                byte val = 0;
                if (Pins[11].Value < Pin.HALFCUT && Pins[12].Value < Pin.HALFCUT) val = Reg[0];
                if (Pins[11].Value >= Pin.HALFCUT && Pins[12].Value < Pin.HALFCUT) val = Reg[1];
                if (Pins[11].Value < Pin.HALFCUT && Pins[12].Value >= Pin.HALFCUT) val = Reg[2];
                if (Pins[11].Value >= Pin.HALFCUT && Pins[12].Value >= Pin.HALFCUT) val = Reg[3];
                
                for (var i = 13; i < 21; i++)
                    Pins[i].Value = Pin.LOW;
                if (val >= 128) {
                    Pins[20].Value = Pin.HIGH;
                    val -= 128;
                }
                if (val >= 64) {
                    Pins[19].Value = Pin.HIGH;
                    val -= 64;
                }
                if (val >= 32) {
                    Pins[18].Value = Pin.HIGH;
                    val -= 32;
                }
                if (val >= 16) {
                    Pins[17].Value = Pin.HIGH;
                    val -= 16;
                }
                if (val >= 8) {
                    Pins[16].Value = Pin.HIGH;
                    val -= 8;
                }
                if (val >= 4) {
                    Pins[15].Value = Pin.HIGH;
                    val -= 4;
                }
                if (val >= 2) {
                    Pins[14].Value = Pin.HIGH;
                    val -= 2;
                }
                if (val >= 1) {
                    Pins[13].Value = Pin.HIGH;
                    val -= 1;
                }
                for (var i = 13; i < 21; i++) {
                    Pins[i].simulationId = simulationId;
                    Pins[i].Propagate();
                }
            }

        }
    }

}
