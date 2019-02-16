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
    public class FlipflopSr : Chip {
        private float _lastClk = Pin.Low;

        public Pin Set => Pins[0];
        public Pin Reset => Pins[1];
        public Pin Clk => Pins[2];
        public Pin S => Pins[3];
        public Pin R => Pins[4];
        public Pin Q => Pins[5];
        public Pin Qnot => Pins[6];

        public FlipflopSr(string name = "Flipflop component") : base(name, 7) {

        }

        protected override void AllocatePins() {
            for(var i = 0; i < 5; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for(var i = 5; i < 7; i++) {
                Pins[i] = new Pin(this, true, false);
            }
            Qnot.Value = Pin.High;
        }
        internal override bool CanExecute() {
            if(SimulationIdInternal == Circuit.SimulationId) return false;
            for(var i = 0; i < 3; i++) { //not needed to verify if S and R is connected
                if(Pins[i].SimulationIdInternal != Circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }
        protected internal override void Execute() {
            base.Execute();
            if (S.GetDigital() == Pin.High) { //Set it = 1
                Q.Value = Pin.High;
                Qnot.Value = Pin.Low;
            } else if (R.GetDigital() == Pin.High) { // Reset it = 1
                Q.Value = Pin.Low;
                Qnot.Value = Pin.High;
            } else if(Clk.Value == Pin.Low && _lastClk == Pin.High) { //Clock desc
                if (Set.GetDigital() == Pin.High) {
                    if(Reset.GetDigital() == Pin.High) { // S = 1, R = 1
                        Q.Value = Pin.Low;
                        Qnot.Value = Pin.Low;
                    } else { // S = 1, R = 0
                        Q.Value = Pin.High;
                        Qnot.Value = Pin.Low;
                    }
                } else {
                    if(Reset.GetDigital() == Pin.High) { // S = 0, R = 1
                        Q.Value = Pin.Low;
                        Qnot.Value = Pin.High;
                    } else { // J = 0, K = 0
                        Q.Value = Pin.Low;
                        Qnot.Value = Pin.Low;
                        //Error
                    }
                }
            }
            _lastClk = Clk.Value;
            Q.Propagate();
            Qnot.Propagate();
        }
    }
}
