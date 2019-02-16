namespace CircuitSimulator
{
    public class ClockLogicInput : Component
    {
        private float _timeElapsed;
        public float Frequency = 1f;
        public bool IsOn = true;

        public ClockLogicInput(string name = "Clock Logic Input") : base(name)
        {
            Pins[0].IsOutputInternal = true;
            CanStart = true;
        }

        private float Time => 1f / Frequency;

        public float Value
        {
            get => Pins[0].Value;
            set => Pins[0].SetDigital(value);
        }

        private float Switch()
        {
            if (Pins[0].Value == Pin.High) Pins[0].Value = Pin.Low;
            else Pins[0].Value = Pin.High;
            return Pins[0].Value;
        }

        protected internal override void Execute()
        {
            base.Execute();
            if (IsOn)
            {
                _timeElapsed += Circuit.TimePerTick;
                if (_timeElapsed >= Time)
                {
                    _timeElapsed = 0f;
                    Switch();
                }
            }

            Pins[0].Propagate();
        }
    }
}