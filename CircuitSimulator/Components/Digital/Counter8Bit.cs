namespace CircuitSimulator.Components.Digital.MMaisMaisMais
{
    public class Counter8Bit : Chip
    {
        private float _lastClock = Pin.Low;

        public byte InternalValue;

        public Counter8Bit(string name = "Counter8Bit") : base(name, 12)
        {
        }

        protected override void AllocatePins()
        {
            for (var i = 0; i < 4; i++) Pins[i] = new Pin(this, false, false);
            for (var i = 4; i < 12; i++) Pins[i] = new Pin(this, true, false);
        }

        internal override bool CanExecute()
        {
            if (SimulationIdInternal == Circuit.SimulationId) return false;
            for (var i = 0; i < 4; i++)
            {
                if (Pins[i].SimulationIdInternal != Circuit.SimulationId)
                    return false;
            }

            return true;
        }

        protected internal override void Execute()
        {
            SimulationIdInternal = Circuit.SimulationId;

            if (_lastClock <= Pin.Halfcut && Pins[3].Value >= Pin.Halfcut)
            {
                if (Pins[1].Value <= Pin.Halfcut)
                    ++InternalValue;
                else
                    --InternalValue;
            }

            _lastClock = Pins[3].Value;
            if (Pins[2].Value >= Pin.Halfcut) InternalValue = 0;
            if (Pins[0].Value >= Pin.Halfcut)
            {
                var val = InternalValue;
                for (var i = 4; i < Pins.Length; i++)
                    Pins[i].Value = Pin.Low;
                if (val >= 128)
                {
                    Pins[11].Value = Pin.High;
                    val -= 128;
                }

                if (val >= 64)
                {
                    Pins[10].Value = Pin.High;
                    val -= 64;
                }

                if (val >= 32)
                {
                    Pins[9].Value = Pin.High;
                    val -= 32;
                }

                if (val >= 16)
                {
                    Pins[8].Value = Pin.High;
                    val -= 16;
                }

                if (val >= 8)
                {
                    Pins[7].Value = Pin.High;
                    val -= 8;
                }

                if (val >= 4)
                {
                    Pins[6].Value = Pin.High;
                    val -= 4;
                }

                if (val >= 2)
                {
                    Pins[5].Value = Pin.High;
                    val -= 2;
                }

                if (val >= 1)
                {
                    Pins[4].Value = Pin.High;
                    val -= 1;
                }

                for (var i = 4; i < Pins.Length; i++)
                {
                    Pins[i].SimulationIdInternal = SimulationIdInternal;
                    Pins[i].Propagate();
                }
            }
        }
    }
}