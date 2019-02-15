namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class Ula : Chip {
        private float _lastEnable = Pin.LOW;
        public Ula(string name = "ULA") : base(name, 30) {

        }

        protected override void AllocatePins() {
            for (var i = 0; i < 20; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (var i = 20; i < 30; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            for (var i = 8; i < 20; i++) {
                if (Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }

        protected internal override void Execute() {
            simulationId = circuit.SimulationId;
            
            byte resByte;
            var res = 0;
            var fz = false;
            var fc = false;

            byte a = 0;
            a += (byte)(Pins[0].Value >= Pin.HALFCUT ? 1 : 0);
            a += (byte)(Pins[1].Value >= Pin.HALFCUT ? 2 : 0);
            a += (byte)(Pins[2].Value >= Pin.HALFCUT ? 4 : 0);
            a += (byte)(Pins[3].Value >= Pin.HALFCUT ? 8 : 0);
            a += (byte)(Pins[4].Value >= Pin.HALFCUT ? 16 : 0);
            a += (byte)(Pins[5].Value >= Pin.HALFCUT ? 32 : 0);
            a += (byte)(Pins[6].Value >= Pin.HALFCUT ? 64 : 0);
            a += (byte)(Pins[7].Value >= Pin.HALFCUT ? 128 : 0);

            byte b = 0;
            b += (byte)(Pins[8].Value >= Pin.HALFCUT ? 1 : 0);
            b += (byte)(Pins[9].Value >= Pin.HALFCUT ? 2 : 0);
            b += (byte)(Pins[10].Value >= Pin.HALFCUT ? 4 : 0);
            b += (byte)(Pins[11].Value >= Pin.HALFCUT ? 8 : 0);
            b += (byte)(Pins[12].Value >= Pin.HALFCUT ? 16 : 0);
            b += (byte)(Pins[13].Value >= Pin.HALFCUT ? 32 : 0);
            b += (byte)(Pins[14].Value >= Pin.HALFCUT ? 64 : 0);
            b += (byte)(Pins[15].Value >= Pin.HALFCUT ? 128 : 0);

            var s0 = Pins[16].Value >= Pin.HALFCUT;
            var s1 = Pins[17].Value >= Pin.HALFCUT;
            var s2 = Pins[18].Value >= Pin.HALFCUT;
            var s = 0;

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
                    res = b+1;
                    break;
            }

            resByte = (byte)res;
            if (res >= 256) {
                fc = true;
            }
            if(resByte == 0) {
                fz = true;
            }
            for (var i = 20; i < 28; i++)
                Pins[i].Value = Pin.LOW;
            if (Pins[19].Value >= Pin.HALFCUT && _lastEnable < Pin.HALFCUT) {
                for (var i = 28; i < 30; i++) {
                    Pins[i].Value = Pin.LOW;
                }
                if (fz)
                    Pins[28].Value = Pin.HIGH;
                if (fc)
                    Pins[29].Value = Pin.HIGH;
            }
            for (var i = 28; i <= 29; i++) {
                Pins[i].simulationId = simulationId;
                Pins[i].Propagate();
            }
            if (Pins[19].Value >= Pin.HALFCUT) {
                if (resByte >= 128) {
                    Pins[27].Value = Pin.HIGH;
                    resByte -= 128;
                }
                if (resByte >= 64) {
                    Pins[26].Value = Pin.HIGH;
                    resByte -= 64;
                }
                if (resByte >= 32) {
                    Pins[25].Value = Pin.HIGH;
                    resByte -= 32;
                }
                if (resByte >= 16) {
                    Pins[24].Value = Pin.HIGH;
                    resByte -= 16;
                }
                if (resByte >= 8) {
                    Pins[23].Value = Pin.HIGH;
                    resByte -= 8;
                }
                if (resByte >= 4) {
                    Pins[22].Value = Pin.HIGH;
                    resByte -= 4;
                }
                if (resByte >= 2) {
                    Pins[21].Value = Pin.HIGH;
                    resByte -= 2;
                }
                if (resByte >= 1) {
                    Pins[20].Value = Pin.HIGH;
                    resByte -= 1;
                }
                for (var i = 20; i < 28; i++) {
                    Pins[i].simulationId = simulationId;
                    Pins[i].Propagate();
                }
            }
            _lastEnable = Pins[19].Value;




        }
    }

}
