namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class Disable8Bit : Chip {
        public Disable8Bit(string name = "Disable8Bit") : base(name, 17) {

        }

        protected override void AllocatePins() {
            for (var i = 0; i < 9; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (var i = 9; i < 17; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (SimulationIdInternal == circuit.SimulationId) return false;
            if (Pins[8].SimulationIdInternal != circuit.SimulationId) {
                return false;
            }
            if (Pins[8].Value >= Pin.HALFCUT) {
                for (var i = 0; i < 8; i++) {
                    if (Pins[i].SimulationIdInternal != circuit.SimulationId) {
                        return false;
                    }
                }
            }
            return true;
        }

        protected internal override void Execute() {
            base.Execute();
            if (Pins[8].Value >= Pin.HALFCUT) {
                for (var i = 0; i < 8; i++) {
                    Pins[i + 9].Value = Pins[i].Value;
                    Pins[i + 9].Propagate();
                }
            }
        }
    }

}
