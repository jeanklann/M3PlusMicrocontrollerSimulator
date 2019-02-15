namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class Counter8Bit : Chip {

        public byte InternalValue;
        private float lastClock = Pin.LOW;
        public Counter8Bit(string name = "Counter8Bit") : base(name, 12) {

        }

        protected override void AllocatePins() {
            for (var i = 0; i < 4; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (var i = 4; i < 12; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            for (var i = 0; i < 4; i++) {
                if (Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }

        protected internal override void Execute() {
            simulationId = circuit.SimulationId;

            if(lastClock <= Pin.HALFCUT && Pins[3].Value >= Pin.HALFCUT) {
                if (Pins[1].Value <= Pin.HALFCUT)
                    ++InternalValue;
                else
                    --InternalValue;
            }
            lastClock = Pins[3].Value;
            if (Pins[2].Value >= Pin.HALFCUT) {
                InternalValue = 0;
            }
            if (Pins[0].Value >= Pin.HALFCUT) {
                var val = InternalValue;
                for (var i = 4; i < Pins.Length; i++)
                    Pins[i].Value = Pin.LOW;
                if (val >= 128) {
                    Pins[11].Value = Pin.HIGH;
                    val -= 128;
                }
                if (val >= 64) {
                    Pins[10].Value = Pin.HIGH;
                    val -= 64;
                }
                if (val >= 32) {
                    Pins[9].Value = Pin.HIGH;
                    val -= 32;
                }
                if (val >= 16) {
                    Pins[8].Value = Pin.HIGH;
                    val -= 16;
                }
                if (val >= 8) {
                    Pins[7].Value = Pin.HIGH;
                    val -= 8;
                }
                if (val >= 4) {
                    Pins[6].Value = Pin.HIGH;
                    val -= 4;
                }
                if (val >= 2) {
                    Pins[5].Value = Pin.HIGH;
                    val -= 2;
                }
                if (val >= 1) {
                    Pins[4].Value = Pin.HIGH;
                    val -= 1;
                }
                for (var i = 4; i < Pins.Length; i++) {
                    Pins[i].simulationId = simulationId;
                    Pins[i].Propagate();
                }
            }
        }
    }

}
