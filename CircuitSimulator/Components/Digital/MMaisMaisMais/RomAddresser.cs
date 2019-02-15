namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class RomAddresser : Chip {
        private float _lastClockL = Pin.LOW;
        private float _lastClockH = Pin.LOW;
        public byte RegL;
        public byte RegH;

        public RomAddresser(string name = "RomAddresser") : base(name, 38) {

        }

        protected override void AllocatePins() {
            for (var i = 0; i < 14; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (var i = 14; i < 38; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (SimulationIdInternal == circuit.SimulationId) return false;
            for (var i = 8; i < 14; i++) {
                if (Pins[i].SimulationIdInternal != circuit.SimulationId) {
                    return false;
                }
            }
            if(Pins[8].Value >= Pin.HALFCUT) {
                for (var i = 0; i < 8; i++) {
                    if (Pins[i].SimulationIdInternal != circuit.SimulationId) {
                        return false;
                    }
                }
            }
            return true;
        }

        protected internal override void Execute() {
            SimulationIdInternal = circuit.SimulationId;

            if ((Pins[9].Value >= Pin.HALFCUT && _lastClockH < Pin.HALFCUT) ||
                (Pins[10].Value >= Pin.HALFCUT && _lastClockL < Pin.HALFCUT)) {
                if (Pins[8].Value < Pin.HALFCUT) {
                    var address = RegH * 256 + RegL;
                    ++address;
                    RegH = (byte) (address / 256);
                    RegL = (byte) (address % 256);
                } else {
                    byte val = 0;
                    val += (byte)(Pins[0].Value >= Pin.HALFCUT ? 1 : 0);
                    val += (byte)(Pins[1].Value >= Pin.HALFCUT ? 2 : 0);
                    val += (byte)(Pins[2].Value >= Pin.HALFCUT ? 4 : 0);
                    val += (byte)(Pins[3].Value >= Pin.HALFCUT ? 8 : 0);
                    val += (byte)(Pins[4].Value >= Pin.HALFCUT ? 16 : 0);
                    val += (byte)(Pins[5].Value >= Pin.HALFCUT ? 32 : 0);
                    val += (byte)(Pins[6].Value >= Pin.HALFCUT ? 64 : 0);
                    val += (byte)(Pins[7].Value >= Pin.HALFCUT ? 128 : 0);
                    if (Pins[9].Value >= Pin.HALFCUT && _lastClockH < Pin.HALFCUT) {
                        RegH = val;
                    }
                    if (Pins[10].Value >= Pin.HALFCUT && _lastClockH < Pin.HALFCUT) {
                        RegL = val;
                    }
                }
            }

            if (Pins[13].Value >= Pin.HALFCUT) {
                RegH = 0;
                RegL = 0;
            }
            
            _lastClockH = Pins[9].Value;
            _lastClockL = Pins[10].Value;
            ToOutput();

        }

        private void ToOutput() {
            var val = RegL;
            for (var i = 14; i < 22; i++) 
                Pins[i].Value = Pin.LOW;
            if (Pins[12].Value >= Pin.HALFCUT || Pins[11].Value >= Pin.HALFCUT) {
                for (var i = 30; i < 38; i++) {
                    Pins[i].Value = Pin.LOW;
                }
            }
            if (val >= 128) {
                Pins[21].Value = Pin.HIGH;
                val -= 128;
                if(Pins[12].Value >= Pin.HALFCUT) {
                    Pins[37].Value = Pin.HIGH;
                }
            }
            if (val >= 64) {
                Pins[20].Value = Pin.HIGH;
                val -= 64;
                if (Pins[12].Value >= Pin.HALFCUT) {
                    Pins[36].Value = Pin.HIGH;
                }
            }
            if (val >= 32) {
                Pins[19].Value = Pin.HIGH;
                val -= 32;
                if (Pins[12].Value >= Pin.HALFCUT) {
                    Pins[35].Value = Pin.HIGH;
                }
            }
            if (val >= 16) {
                Pins[18].Value = Pin.HIGH;
                val -= 16;
                if (Pins[12].Value >= Pin.HALFCUT) {
                    Pins[34].Value = Pin.HIGH;
                }
            }
            if (val >= 8) {
                Pins[17].Value = Pin.HIGH;
                val -= 8;
                if (Pins[12].Value >= Pin.HALFCUT) {
                    Pins[33].Value = Pin.HIGH;
                }
            }
            if (val >= 4) {
                Pins[16].Value = Pin.HIGH;
                val -= 4;
                if (Pins[12].Value >= Pin.HALFCUT) {
                    Pins[32].Value = Pin.HIGH;
                }
            }
            if (val >= 2) {
                Pins[15].Value = Pin.HIGH;
                val -= 2;
                if (Pins[12].Value >= Pin.HALFCUT) {
                    Pins[31].Value = Pin.HIGH;
                }
            }
            if (val >= 1) {
                Pins[14].Value = Pin.HIGH;
                val -= 1;
                if (Pins[12].Value >= Pin.HALFCUT) {
                    Pins[30].Value = Pin.HIGH;
                }
            }
            for (var i = 14; i < 22; i++) {
                Pins[i].SimulationIdInternal = SimulationIdInternal;
                Pins[i].Propagate();
            }

            val = RegH;
            for (var i = 22; i < 30; i++)
                Pins[i].Value = Pin.LOW;
            if (val >= 128) {
                Pins[29].Value = Pin.HIGH;
                val -= 128;
                if (Pins[11].Value >= Pin.HALFCUT) {
                    Pins[37].Value = Pin.HIGH;
                }
            }
            if (val >= 64) {
                Pins[28].Value = Pin.HIGH;
                val -= 64;
                if (Pins[11].Value >= Pin.HALFCUT) {
                    Pins[36].Value = Pin.HIGH;
                }
            }
            if (val >= 32) {
                Pins[27].Value = Pin.HIGH;
                val -= 32;
                if (Pins[11].Value >= Pin.HALFCUT) {
                    Pins[35].Value = Pin.HIGH;
                }
            }
            if (val >= 16) {
                Pins[26].Value = Pin.HIGH;
                val -= 16;
                if (Pins[11].Value >= Pin.HALFCUT) {
                    Pins[34].Value = Pin.HIGH;
                }
            }
            if (val >= 8) {
                Pins[25].Value = Pin.HIGH;
                val -= 8;
                if (Pins[11].Value >= Pin.HALFCUT) {
                    Pins[33].Value = Pin.HIGH;
                }
            }
            if (val >= 4) {
                Pins[24].Value = Pin.HIGH;
                val -= 4;
                if (Pins[11].Value >= Pin.HALFCUT) {
                    Pins[32].Value = Pin.HIGH;
                }
            }
            if (val >= 2) {
                Pins[23].Value = Pin.HIGH;
                val -= 2;
                if (Pins[11].Value >= Pin.HALFCUT) {
                    Pins[31].Value = Pin.HIGH;
                }
            }
            if (val >= 1) {
                Pins[22].Value = Pin.HIGH;
                val -= 1;
                if (Pins[11].Value >= Pin.HALFCUT) {
                    Pins[30].Value = Pin.HIGH;
                }
            }
            for (var i = 22; i < 30; i++) {
                Pins[i].SimulationIdInternal = SimulationIdInternal;
                Pins[i].Propagate();
            }
            if(Pins[11].Value >= Pin.HALFCUT || Pins[12].Value >= Pin.HALFCUT &&
                !(Pins[11].Value >= Pin.HALFCUT && Pins[12].Value >= Pin.HALFCUT)) {
                for (var i = 30; i < 38; i++) {
                    Pins[i].SimulationIdInternal = SimulationIdInternal;
                    Pins[i].Propagate();
                }
            }
        }
    }

}
