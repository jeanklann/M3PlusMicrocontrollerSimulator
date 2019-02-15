﻿namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class InternalClock : Chip {
        public InternalClock(string name = "InternalClock") : base(name, 1) {

        }

        protected override void AllocatePins() {
            Pins[0] = new Pin(this, true, false);
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            if (Pins[0].simulationId != circuit.SimulationId) return false;
            return true;
        }

        protected internal override void Execute() {
            base.Execute();
        }
    }

}
