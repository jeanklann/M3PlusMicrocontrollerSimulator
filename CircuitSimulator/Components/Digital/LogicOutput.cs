namespace CircuitSimulator
{
    public class LogicOutput : Component
    {
        public LogicOutput(string name = "Logic Output") : base(name)
        {
            //canStart = true;
        }

        public float Value
        {
            get => Pins[0].Value;
            set => Pins[0].SetDigital(value);
        }


        protected internal override void Execute()
        {
            base.Execute();
        }
    }
}