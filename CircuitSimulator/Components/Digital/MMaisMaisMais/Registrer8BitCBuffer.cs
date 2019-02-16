namespace CircuitSimulator.Components.Digital.MMaisMaisMais
{
    public class Registrer8BitCBuffer : Chip
    {
        private float _lastClock = Pin.Low;
        public byte InternalValue;

        public Registrer8BitCBuffer(string name = "Registrer8BitCBuffer") : base(name, 27)
        {
        }

        protected override void AllocatePins()
        {
            for (var i = 0; i < 11; i++) Pins[i] = new Pin(this, false, false);
            for (var i = 11; i < 27; i++) Pins[i] = new Pin(this, true, false);
        }

        internal override bool CanExecute()
        {
            if (SimulationIdInternal == Circuit.SimulationId) return false;
            for (var i = 8; i <= 10; i++)
                if (Pins[i].SimulationIdInternal != Circuit.SimulationId)
                    return false;
            if (Pins[8].Value >= Pin.Halfcut && _lastClock <= Pin.Halfcut)
                for (var i = 0; i < 8; i++)
                    if (Pins[i].SimulationIdInternal != Circuit.SimulationId)
                        return false;
            return true;
        }

        protected internal override void Execute()
        {
            SimulationIdInternal = Circuit.SimulationId;

            if (_lastClock <= Pin.Halfcut && Pins[8].Value >= Pin.Halfcut)
            {
                InternalValue = 0;
                InternalValue += (byte) (Pins[0].Value >= Pin.Halfcut ? 1 : 0);
                InternalValue += (byte) (Pins[1].Value >= Pin.Halfcut ? 2 : 0);
                InternalValue += (byte) (Pins[2].Value >= Pin.Halfcut ? 4 : 0);
                InternalValue += (byte) (Pins[3].Value >= Pin.Halfcut ? 8 : 0);
                InternalValue += (byte) (Pins[4].Value >= Pin.Halfcut ? 16 : 0);
                InternalValue += (byte) (Pins[5].Value >= Pin.Halfcut ? 32 : 0);
                InternalValue += (byte) (Pins[6].Value >= Pin.Halfcut ? 64 : 0);
                InternalValue += (byte) (Pins[7].Value >= Pin.Halfcut ? 128 : 0);
            }

            _lastClock = Pins[8].Value;

            if (Pins[10].Value >= Pin.Halfcut) InternalValue = 0;

            if (Pins[9].Value >= Pin.Halfcut)
            {
                var val = InternalValue;
                for (var i = 19; i < 27; i++)
                    Pins[i].Value = Pin.Low;
                if (val >= 128)
                {
                    Pins[26].Value = Pin.High;
                    val -= 128;
                }

                if (val >= 64)
                {
                    Pins[25].Value = Pin.High;
                    val -= 64;
                }

                if (val >= 32)
                {
                    Pins[24].Value = Pin.High;
                    val -= 32;
                }

                if (val >= 16)
                {
                    Pins[23].Value = Pin.High;
                    val -= 16;
                }

                if (val >= 8)
                {
                    Pins[22].Value = Pin.High;
                    val -= 8;
                }

                if (val >= 4)
                {
                    Pins[21].Value = Pin.High;
                    val -= 4;
                }

                if (val >= 2)
                {
                    Pins[20].Value = Pin.High;
                    val -= 2;
                }

                if (val >= 1)
                {
                    Pins[19].Value = Pin.High;
                    val -= 1;
                }

                for (var i = 19; i < 27; i++)
                {
                    Pins[i].SimulationIdInternal = SimulationIdInternal;
                    Pins[i].Propagate();
                }
            }

            var val2 = InternalValue;
            for (var i = 11; i < 19; i++)
                Pins[i].Value = Pin.Low;
            if (val2 >= 128)
            {
                Pins[18].Value = Pin.High;
                val2 -= 128;
            }

            if (val2 >= 64)
            {
                Pins[17].Value = Pin.High;
                val2 -= 64;
            }

            if (val2 >= 32)
            {
                Pins[16].Value = Pin.High;
                val2 -= 32;
            }

            if (val2 >= 16)
            {
                Pins[15].Value = Pin.High;
                val2 -= 16;
            }

            if (val2 >= 8)
            {
                Pins[14].Value = Pin.High;
                val2 -= 8;
            }

            if (val2 >= 4)
            {
                Pins[13].Value = Pin.High;
                val2 -= 4;
            }

            if (val2 >= 2)
            {
                Pins[12].Value = Pin.High;
                val2 -= 2;
            }

            if (val2 >= 1)
            {
                Pins[11].Value = Pin.High;
                val2 -= 1;
            }

            for (var i = 11; i < 19; i++)
            {
                Pins[i].SimulationIdInternal = SimulationIdInternal;
                Pins[i].Propagate();
            }
        }
    }
}