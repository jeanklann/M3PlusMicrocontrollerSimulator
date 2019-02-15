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
    public class FlipflopT : Chip {
        private float lastClk = Pin.LOW;

        public Pin T => Pins[0];
        public Pin CLK => Pins[1];
        public Pin S => Pins[2];
        public Pin R => Pins[3];
        public Pin Q => Pins[4];
        public Pin Qnot => Pins[5];

        public FlipflopT(string name = "Flipflop component") : base(name, 6) {

        }

        protected override void AllocatePins() {
            for(var i = 0; i < 4; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for(var i = 4; i < 6; i++) {
                Pins[i] = new Pin(this, true, false);
            }
            Qnot.Value = Pin.HIGH;
        }
        internal override bool CanExecute() {
            if(simulationId == circuit.SimulationId) return false;
            for(var i = 0; i < 2; i++) { //not needed to verify if S and R is connected
                if(Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }
        protected internal override void Execute() {
            base.Execute();
            if (S.GetDigital() == Pin.HIGH) { //S = 1
                Q.Value = Pin.HIGH;
                Qnot.Value = Pin.LOW;
            } else if (R.GetDigital() == Pin.HIGH) { // R = 1
                Q.Value = Pin.LOW;
                Qnot.Value = Pin.HIGH;
            } else if (CLK.Value == Pin.LOW && lastClk == Pin.HIGH) { //Clock desc
                if (T.GetDigital() == Pin.HIGH) { //T = 1
                    Q.Value = Q.Neg();
                    Qnot.Value = Qnot.Neg();
                } else { //T = 0
                    //Do nothing, because the outputs stays same
                }
            }
            lastClk = CLK.Value;
            Q.Propagate();
            Qnot.Propagate();
        }
    }
}
