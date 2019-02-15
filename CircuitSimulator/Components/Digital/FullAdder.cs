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
            if(simulationId == circuit.SimulationId) return false;
            for(var i = 0; i < 3; i++) {
                if(Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }

        protected internal override void Execute() {
            base.Execute();
            if(A.GetDigital() == Pin.HIGH) {
                if(B.GetDigital() == Pin.HIGH) {
                    if(Cin.GetDigital() == Pin.HIGH) { //111
                        S.Value = Pin.HIGH;
                        Cout.Value = Pin.HIGH;
                    } else { //110
                        S.Value = Pin.LOW;
                        Cout.Value = Pin.HIGH;
                    }
                } else {
                    if(Cin.GetDigital() == Pin.HIGH) { //101
                        S.Value = Pin.LOW;
                        Cout.Value = Pin.HIGH;
                    } else { //100
                        S.Value = Pin.HIGH;
                        Cout.Value = Pin.LOW;
                    }
                }
            } else {
                if(B.GetDigital() == Pin.HIGH) {
                    if(Cin.GetDigital() == Pin.HIGH) { // 011
                        S.Value = Pin.LOW;
                        Cout.Value = Pin.HIGH;
                    } else { //010
                        S.Value = Pin.HIGH;
                        Cout.Value = Pin.LOW;
                    }
                } else {
                    if(Cin.GetDigital() == Pin.HIGH) { //001
                        S.Value = Pin.HIGH;
                        Cout.Value = Pin.LOW;
                    } else { //000
                        S.Value = Pin.LOW;
                        Cout.Value = Pin.LOW;
                    }
                }
            }
            for(var i = 0; i < 3; i++) {
                Pins[i].Propagate();
            }
        }
    }
}
