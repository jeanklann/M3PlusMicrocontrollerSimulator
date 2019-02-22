namespace CircuitSimulator.Components.Digital.MMaisMaisMais
{
    public class Registrers : Chip
    {
        private float _lastClock = Pin.Low;
        public byte[] Reg = new byte[4];

        public Registrers(string name = "Registrers") : base(name, 21)
        {
        }

        protected override void AllocatePins()
        {
            for (var i = 0; i < 13; i++) Pins[i] = new Pin(this, false, false);
            for (var i = 13; i < 21; i++) Pins[i] = new Pin(this, true, false);
        }

        internal override bool CanExecute()
        {
            if (SimulationIdInternal == Circuit.SimulationId) return false;
            for (var i = 8; i < 13; i++)
            {
                if (Pins[i].SimulationIdInternal != Circuit.SimulationId)
                    return false;
            }

            if (Pins[9].Value >= Pin.Halfcut && _lastClock <= Pin.Halfcut)
            {
                for (var i = 0; i < 8; i++)
                {
                    if (Pins[i].SimulationIdInternal != Circuit.SimulationId)
                        return false;
                }
            }

            return true;
        }

        protected internal override void Execute()
        {
            SimulationIdInternal = Circuit.SimulationId;

            if (_lastClock <= Pin.Halfcut && Pins[9].Value >= Pin.Halfcut)
            {
                byte val = 0;
                val += (byte) (Pins[0].Value >= Pin.Halfcut ? 1 : 0);
                val += (byte) (Pins[1].Value >= Pin.Halfcut ? 2 : 0);
                val += (byte) (Pins[2].Value >= Pin.Halfcut ? 4 : 0);
                val += (byte) (Pins[3].Value >= Pin.Halfcut ? 8 : 0);
                val += (byte) (Pins[4].Value >= Pin.Halfcut ? 16 : 0);
                val += (byte) (Pins[5].Value >= Pin.Halfcut ? 32 : 0);
                val += (byte) (Pins[6].Value >= Pin.Halfcut ? 64 : 0);
                val += (byte) (Pins[7].Value >= Pin.Halfcut ? 128 : 0);
                if (Pins[11].Value < Pin.Halfcut && Pins[12].Value < Pin.Halfcut) Reg[0] = val;
                if (Pins[11].Value >= Pin.Halfcut && Pins[12].Value < Pin.Halfcut) Reg[1] = val;
                if (Pins[11].Value < Pin.Halfcut && Pins[12].Value >= Pin.Halfcut) Reg[2] = val;
                if (Pins[11].Value >= Pin.Halfcut && Pins[12].Value >= Pin.Halfcut) Reg[3] = val;
            }

            _lastClock = Pins[9].Value;

            if (Pins[10].Value >= Pin.Halfcut)
            {
                for (var i = 0; i < 4; i++)
                    Reg[i] = 0;
            }

            if (Pins[8].Value >= Pin.Halfcut)
            {
                byte val = 0;
                if (Pins[11].Value < Pin.Halfcut && Pins[12].Value < Pin.Halfcut) val = Reg[0];
                if (Pins[11].Value >= Pin.Halfcut && Pins[12].Value < Pin.Halfcut) val = Reg[1];
                if (Pins[11].Value < Pin.Halfcut && Pins[12].Value >= Pin.Halfcut) val = Reg[2];
                if (Pins[11].Value >= Pin.Halfcut && Pins[12].Value >= Pin.Halfcut) val = Reg[3];

                for (var i = 13; i < 21; i++)
                    Pins[i].Value = Pin.Low;
                if (val >= 128)
                {
                    Pins[20].Value = Pin.High;
                    val -= 128;
                }

                if (val >= 64)
                {
                    Pins[19].Value = Pin.High;
                    val -= 64;
                }

                if (val >= 32)
                {
                    Pins[18].Value = Pin.High;
                    val -= 32;
                }

                if (val >= 16)
                {
                    Pins[17].Value = Pin.High;
                    val -= 16;
                }

                if (val >= 8)
                {
                    Pins[16].Value = Pin.High;
                    val -= 8;
                }

                if (val >= 4)
                {
                    Pins[15].Value = Pin.High;
                    val -= 4;
                }

                if (val >= 2)
                {
                    Pins[14].Value = Pin.High;
                    val -= 2;
                }

                if (val >= 1)
                {
                    Pins[13].Value = Pin.High;
                    val -= 1;
                }

                for (var i = 13; i < 21; i++)
                {
                    Pins[i].SimulationIdInternal = SimulationIdInternal;
                    Pins[i].Propagate();
                }
            }
        }
    }
}