namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class Registrer8BitSG : Chip {
        public Pin Clock => Pins[8];
        public Pin Reset => Pins[9];


        private byte internalValue;
        private float lastClock = Pin.LOW;

        public Registrer8BitSG(string name = "Registrer8BitSG") : base(name, 18) {

        }

        protected override void AllocatePins() {
            for (var i = 0; i < 10; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (var i = 10; i < 18; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            for (var i = 8; i <= 9; i++) {
                if (Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            if (Pins[8].Value >= Pin.HALFCUT && lastClock <= Pin.HALFCUT) {
                for (var i = 0; i < 8; i++) {
                    if (Pins[i].simulationId != circuit.SimulationId) {
                        return false;
                    }
                }
            }
            return true;
        }

        protected internal override void Execute() {
            base.Execute();
            var halfCut = (Pin.HIGH + Pin.LOW) / 2f;
            if(Reset.Value >= halfCut) {
                internalValue = 0;
            } else {
                if(Clock.Value >= halfCut && lastClock < halfCut) {
                    internalValue = 0;
                    if (Pins[0].Value >= halfCut) internalValue += 1;
                    if (Pins[1].Value >= halfCut) internalValue += 2;
                    if (Pins[2].Value >= halfCut) internalValue += 4;
                    if (Pins[3].Value >= halfCut) internalValue += 8;
                    if (Pins[4].Value >= halfCut) internalValue += 16;
                    if (Pins[5].Value >= halfCut) internalValue += 32;
                    if (Pins[6].Value >= halfCut) internalValue += 64;
                    if (Pins[7].Value >= halfCut) internalValue += 128;
                }
            }
            lastClock = Clock.Value;

            var tempVal = internalValue;
            for (var i = 10; i < 18; i++) {
                Pins[i].SetDigital(Pin.LOW);
            }
            if ((tempVal & 0x01) == 0x01) Pins[10].SetDigital(Pin.HIGH);
            if ((tempVal & 0x02) == 0x02) Pins[11].SetDigital(Pin.HIGH);
            if ((tempVal & 0x04) == 0x04) Pins[12].SetDigital(Pin.HIGH);
            if ((tempVal & 0x08) == 0x08) Pins[13].SetDigital(Pin.HIGH);
            if ((tempVal & 0x10) == 0x10) Pins[14].SetDigital(Pin.HIGH);
            if ((tempVal & 0x20) == 0x20) Pins[15].SetDigital(Pin.HIGH);
            if ((tempVal & 0x40) == 0x40) Pins[16].SetDigital(Pin.HIGH);
            if ((tempVal & 0x80) == 0x80) Pins[17].SetDigital(Pin.HIGH);
            
            for (var i = 10; i < 18; i++) {
                Pins[i].Propagate();
            }
        }
    }

}
