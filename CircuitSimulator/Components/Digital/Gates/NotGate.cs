namespace CircuitSimulator {
    public class NotGate : Gate {
        public NotGate(string name = "Not Gate") : base(name, 2) {

        }
        protected internal override void Execute() {
            SimulationIdInternal = circuit.SimulationId;
            for (var i = 0; i < Pins.Length; i++) {
                Pins[i].SimulationIdInternal = SimulationIdInternal;
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
