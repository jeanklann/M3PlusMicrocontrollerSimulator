namespace CircuitSimulator
{
    /// <summary>
    ///     Flipflop JK:
    ///     <para />
    ///     J: pin 0,
    ///     <para />
    ///     K: pin 1,
    ///     <para />
    ///     CLK (desc): pin 2,
    ///     <para />
    ///     S: pin 3,
    ///     <para />
    ///     R: pin 4,
    ///     <para />
    ///     Q: pin 5,
    ///     <para />
    /// </summary>
    public class FlipflopT : Chip
    {
        private float _lastClk = Pin.Low;

        public FlipflopT(string name = "Flipflop component") : base(name, 6)
        {
        }

        public Pin T => Pins[0];
        public Pin Clk => Pins[1];
        public Pin S => Pins[2];
        public Pin R => Pins[3];
        public Pin Q => Pins[4];
        public Pin Qnot => Pins[5];

        protected override void AllocatePins()
        {
            for (var i = 0; i < 4; i++) Pins[i] = new Pin(this, false, false);
            for (var i = 4; i < 6; i++) Pins[i] = new Pin(this, true, false);
            Qnot.Value = Pin.High;
        }

        internal override bool CanExecute()
        {
            if (SimulationIdInternal == Circuit.SimulationId) return false;
            for (var i = 0; i < 2; i++) //not needed to verify if S and R is connected
            {
                if (Pins[i].SimulationIdInternal != Circuit.SimulationId)
                    return false;
            }

            return true;
        }

        protected internal override void Execute()
        {
            base.Execute();
            if (S.GetDigital() == Pin.High)
            {
                //S = 1
                Q.Value = Pin.High;
                Qnot.Value = Pin.Low;
            }
            else if (R.GetDigital() == Pin.High)
            {
                // R = 1
                Q.Value = Pin.Low;
                Qnot.Value = Pin.High;
            }
            else if (Clk.Value == Pin.Low && _lastClk == Pin.High)
            {
                //Clock desc
                if (T.GetDigital() == Pin.High)
                {
                    //T = 1
                    Q.Value = Q.Neg();
                    Qnot.Value = Qnot.Neg();
                }
            }

            _lastClk = Clk.Value;
            Q.Propagate();
            Qnot.Propagate();
        }
    }
}