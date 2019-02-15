namespace CircuitSimulator {
    public class LogicOutput : Component {
        public float Value {
            get => Pins[0].Value;
            set => Pins[0].SetDigital(value);
        }

        public LogicOutput(string name = "Logic Output") : base(name) {
            //canStart = true;
        }


        protected internal override void Execute() {
            base.Execute();
        }
    }
}
