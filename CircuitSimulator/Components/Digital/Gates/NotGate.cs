namespace CircuitSimulator {
    public class NotGate : Gate {
        public NotGate(string name = "Not Gate") : base(name, 2) {

        }
        protected internal override void Execute() {
            simulationId = circuit.SimulationId;
            for (int i = 0; i < Pins.Length; i++) {
                Pins[i].simulationId = simulationId;
            }
            Output.SetDigital(Operation(Pins[0]));
            Output.Propagate();
            //base.Execute();
        }
        protected internal override float Operation(Pin pin) {
            return Pins[0].Neg();
        }
    }
}
