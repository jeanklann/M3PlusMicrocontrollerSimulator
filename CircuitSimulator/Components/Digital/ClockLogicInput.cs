namespace CircuitSimulator {
    public class ClockLogicInput : Component {
        public bool IsOn = true;
        private float timeElapsed;
        private float time => 1f / Frequency;
        public float Frequency = 1f;
        public float Value {
            get => Pins[0].Value;
            set => Pins[0].SetDigital(value);
        }

        public ClockLogicInput(string name = "Clock Logic Input") : base(name) {
            Pins[0].IsOutputInternal = true;
            CanStart = true;
        }

        private float Switch() {
            if(Pins[0].Value == Pin.HIGH) Pins[0].Value = Pin.LOW;
            else Pins[0].Value = Pin.HIGH;
            return Pins[0].Value;
        }

        protected internal override void Execute() {
            base.Execute();
            if(IsOn) {
                timeElapsed += circuit.TimePerTick;
                if(timeElapsed >= time) {
                    timeElapsed = 0f;
                    Switch();
                }
            }
            Pins[0].Propagate();
        }
    }
}
