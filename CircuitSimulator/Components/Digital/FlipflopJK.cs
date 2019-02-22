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
    public class FlipflopJk : Chip
    {
        private float _lastClk = Pin.Low;

        public FlipflopJk(string name = "Flipflop component") : base(name, 7)
        {
        }

        public Pin J => Pins[0];
        public Pin K => Pins[1];
        public Pin Clk => Pins[2];
        public Pin S => Pins[3];
        public Pin R => Pins[4];
        public Pin Q => Pins[5];
        public Pin Qnot => Pins[6];

        protected override void AllocatePins()
        {
            for (var i = 0; i < 5; i++) Pins[i] = new Pin(this, false, false);
            for (var i = 5; i < 7; i++) Pins[i] = new Pin(this, true, false);
            Qnot.Value = Pin.High;
        }

        internal override bool CanExecute()
        {
            if (SimulationIdInternal == Circuit.SimulationId) return false;
            for (var i = 0; i < 3; i++) //not needed to verify if S and R is connected
            {
                if (Pins[i].SimulationIdInternal != Circuit.SimulationId)
                    return false;
            }

            return true;
        }

        /// <summary>
        ///     <para>Execute the JK Flipflop.</para>
        /// </summary>
        /// <example>
        ///     JK flipflop truth table:
        ///     <code>
        ///     Clock     J   K   S   R   Q  !Q   
        ///       -       -   -   1   0   1   0   
        ///       -       -   -   0   1   0   1   
        ///      desc     0   0   0   0   Q  !Q   
        ///      desc     1   0   0   0   1   0   
        ///      desc     0   1   0   0   0   1   
        ///      desc     1   1   0   0  !Q   Q   
        /// </code>
        /// </example>
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
                if (J.GetDigital() == Pin.High)
                {
                    if (K.GetDigital() == Pin.High)
                    {
                        // J = 1, K = 1
                        Q.Value = Q.Neg();
                        Qnot.Value = Qnot.Neg();
                    }
                    else
                    {
                        // J = 1, K = 0
                        Q.Value = Pin.High;
                        Qnot.Value = Pin.Low;
                    }
                }
                else
                {
                    if (K.GetDigital() == Pin.High)
                    {
                        // J = 0, K = 1
                        Q.Value = Pin.Low;
                        Qnot.Value = Pin.High;
                    }
                }
            }

            _lastClk = Clk.Value;
            Q.Propagate();
            Qnot.Propagate();
        }
    }
}