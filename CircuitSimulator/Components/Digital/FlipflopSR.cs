namespace CircuitSimulator {
    /// <summary>
    /// Flipflop JK: <para />
    /// J: pin 0,<para />
    /// K: pin 1,<para />
    /// CLK (desc): pin 2,<para />
    /// S: pin 3,<para />
    /// R: pin 4,<para />
    /// Q: pin 5,<para />
    /// </summary>
    public class FlipflopSR : Chip {
        private float lastClk = Pin.LOW;

        public Pin Set => Pins[0];
        public Pin Reset => Pins[1];
        public Pin CLK => Pins[2];
        public Pin S => Pins[3];
        public Pin R => Pins[4];
        public Pin Q => Pins[5];
        public Pin Qnot => Pins[6];

        public FlipflopSR(string name = "Flipflop component") : base(name, 7) {

        }

        protected override void AllocatePins() {
            for(var i = 0; i < 5; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for(var i = 5; i < 7; i++) {
                Pins[i] = new Pin(this, true, false);
            }
            Qnot.Value = Pin.HIGH;
        }
        internal override bool CanExecute() {
            if(simulationId == circuit.SimulationId) return false;
            for(var i = 0; i < 3; i++) { //not needed to verify if S and R is connected
                if(Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }
        protected internal override void Execute() {
            base.Execute();
            if (S.GetDigital() == Pin.HIGH) { //Set it = 1
                Q.Value = Pin.HIGH;
                Qnot.Value = Pin.LOW;
            } else if (R.GetDigital() == Pin.HIGH) { // Reset it = 1
                Q.Value = Pin.LOW;
                Qnot.Value = Pin.HIGH;
            } else if(CLK.Value == Pin.LOW && lastClk == Pin.HIGH) { //Clock desc
                if (Set.GetDigital() == Pin.HIGH) {
                    if(Reset.GetDigital() == Pin.HIGH) { // S = 1, R = 1
                        Q.Value = Pin.LOW;
                        Qnot.Value = Pin.LOW;
                    } else { // S = 1, R = 0
                        Q.Value = Pin.HIGH;
                        Qnot.Value = Pin.LOW;
                    }
                } else {
                    if(Reset.GetDigital() == Pin.HIGH) { // S = 0, R = 1
                        Q.Value = Pin.LOW;
                        Qnot.Value = Pin.HIGH;
                    } else { // J = 0, K = 0
                        Q.Value = Pin.LOW;
                        Qnot.Value = Pin.LOW;
                        //Error
                    }
                }
            }
            lastClk = CLK.Value;
            Q.Propagate();
            Qnot.Propagate();
        }
    }
}
