namespace CircuitSimulator {
    public class NotGate : Gate {
        public NotGate(string name = "Not Gate") : base(name, 1) {

        }
        protected internal override void Execute() {
            base.Execute();
            Output.Neg();
        }
        protected internal override float Operation(Pin pin) {
            return Output.Neg();
        }
    }
}
