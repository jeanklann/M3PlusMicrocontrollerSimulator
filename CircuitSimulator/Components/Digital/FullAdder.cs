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
    public class FullAdder : Chip {
        public Pin A => Pins[0];
        public Pin B => Pins[1];
        public Pin Cin => Pins[2];
        public Pin S => Pins[3];
        public Pin Cout => Pins[4];

        public FullAdder(string name = "Full Adder") : base(name, 5) {

        }

        protected override void AllocatePins() {
            for(var i = 0; i < 3; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for(var i = 3; i < 5; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if(SimulationIdInternal == Circuit.SimulationId) return false;
            for(var i = 0; i < 3; i++) {
                if(Pins[i].SimulationIdInternal != Circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }

        protected internal override void Execute() {
            base.Execute();
            if(A.GetDigital() == Pin.High) {
                if(B.GetDigital() == Pin.High) {
                    if(Cin.GetDigital() == Pin.High) { //111
                        S.Value = Pin.High;
                        Cout.Value = Pin.High;
                    } else { //110
                        S.Value = Pin.Low;
                        Cout.Value = Pin.High;
                    }
                } else {
                    if(Cin.GetDigital() == Pin.High) { //101
                        S.Value = Pin.Low;
                        Cout.Value = Pin.High;
                    } else { //100
                        S.Value = Pin.High;
                        Cout.Value = Pin.Low;
                    }
                }
            } else {
                if(B.GetDigital() == Pin.High) {
                    if(Cin.GetDigital() == Pin.High) { // 011
                        S.Value = Pin.Low;
                        Cout.Value = Pin.High;
                    } else { //010
                        S.Value = Pin.High;
                        Cout.Value = Pin.Low;
                    }
                } else {
                    if(Cin.GetDigital() == Pin.High) { //001
                        S.Value = Pin.High;
                        Cout.Value = Pin.Low;
                    } else { //000
                        S.Value = Pin.Low;
                        Cout.Value = Pin.Low;
                    }
                }
            }
            for(var i = 0; i < 3; i++) {
                Pins[i].Propagate();
            }
        }
    }
}
