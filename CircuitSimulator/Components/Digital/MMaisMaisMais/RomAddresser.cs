namespace CircuitSimulator.Components.Digital.MMaisMaisMais
{
    public class RomAddresser : Chip
    {
        private float _lastClockH = Pin.Low;
        private float _lastClockL = Pin.Low;
        public byte RegH;
        public byte RegL;

        public RomAddresser(string name = "RomAddresser") : base(name, 38)
        {
        }

        protected override void AllocatePins()
        {
            for (var i = 0; i < 14; i++) Pins[i] = new Pin(this, false, false);
            for (var i = 14; i < 38; i++) Pins[i] = new Pin(this, true, false);
        }

        internal override bool CanExecute()
        {
            if (SimulationIdInternal == Circuit.SimulationId) return false;
            for (var i = 8; i < 14; i++)
            {
                if (Pins[i].SimulationIdInternal != Circuit.SimulationId)
                    return false;
            }

            if (Pins[8].Value >= Pin.Halfcut)
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

            if (Pins[9].Value >= Pin.Halfcut && _lastClockH < Pin.Halfcut ||
                Pins[10].Value >= Pin.Halfcut && _lastClockL < Pin.Halfcut)
            {
                if (Pins[8].Value < Pin.Halfcut)
                {
                    var address = RegH * 256 + RegL;
                    ++address;
                    RegH = (byte) (address / 256);
                    RegL = (byte) (address % 256);
                }
                else
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
                    if (Pins[9].Value >= Pin.Halfcut && _lastClockH < Pin.Halfcut) RegH = val;
                    if (Pins[10].Value >= Pin.Halfcut && _lastClockH < Pin.Halfcut) RegL = val;
                }
            }

            if (Pins[13].Value >= Pin.Halfcut)
            {
                RegH = 0;
                RegL = 0;
            }

            _lastClockH = Pins[9].Value;
            _lastClockL = Pins[10].Value;
            ToOutput();
        }

        private void ToOutput()
        {
            var val = RegL;
            for (var i = 14; i < 22; i++)
                Pins[i].Value = Pin.Low;
            if (Pins[12].Value >= Pin.Halfcut || Pins[11].Value >= Pin.Halfcut)
            {
                for (var i = 30; i < 38; i++)
                    Pins[i].Value = Pin.Low;
            }

            if (val >= 128)
            {
                Pins[21].Value = Pin.High;
                val -= 128;
                if (Pins[12].Value >= Pin.Halfcut) Pins[37].Value = Pin.High;
            }

            if (val >= 64)
            {
                Pins[20].Value = Pin.High;
                val -= 64;
                if (Pins[12].Value >= Pin.Halfcut) Pins[36].Value = Pin.High;
            }

            if (val >= 32)
            {
                Pins[19].Value = Pin.High;
                val -= 32;
                if (Pins[12].Value >= Pin.Halfcut) Pins[35].Value = Pin.High;
            }

            if (val >= 16)
            {
                Pins[18].Value = Pin.High;
                val -= 16;
                if (Pins[12].Value >= Pin.Halfcut) Pins[34].Value = Pin.High;
            }

            if (val >= 8)
            {
                Pins[17].Value = Pin.High;
                val -= 8;
                if (Pins[12].Value >= Pin.Halfcut) Pins[33].Value = Pin.High;
            }

            if (val >= 4)
            {
                Pins[16].Value = Pin.High;
                val -= 4;
                if (Pins[12].Value >= Pin.Halfcut) Pins[32].Value = Pin.High;
            }

            if (val >= 2)
            {
                Pins[15].Value = Pin.High;
                val -= 2;
                if (Pins[12].Value >= Pin.Halfcut) Pins[31].Value = Pin.High;
            }

            if (val >= 1)
            {
                Pins[14].Value = Pin.High;
                val -= 1;
                if (Pins[12].Value >= Pin.Halfcut) Pins[30].Value = Pin.High;
            }

            for (var i = 14; i < 22; i++)
            {
                Pins[i].SimulationIdInternal = SimulationIdInternal;
                Pins[i].Propagate();
            }

            val = RegH;
            for (var i = 22; i < 30; i++)
                Pins[i].Value = Pin.Low;
            if (val >= 128)
            {
                Pins[29].Value = Pin.High;
                val -= 128;
                if (Pins[11].Value >= Pin.Halfcut) Pins[37].Value = Pin.High;
            }

            if (val >= 64)
            {
                Pins[28].Value = Pin.High;
                val -= 64;
                if (Pins[11].Value >= Pin.Halfcut) Pins[36].Value = Pin.High;
            }

            if (val >= 32)
            {
                Pins[27].Value = Pin.High;
                val -= 32;
                if (Pins[11].Value >= Pin.Halfcut) Pins[35].Value = Pin.High;
            }

            if (val >= 16)
            {
                Pins[26].Value = Pin.High;
                val -= 16;
                if (Pins[11].Value >= Pin.Halfcut) Pins[34].Value = Pin.High;
            }

            if (val >= 8)
            {
                Pins[25].Value = Pin.High;
                val -= 8;
                if (Pins[11].Value >= Pin.Halfcut) Pins[33].Value = Pin.High;
            }

            if (val >= 4)
            {
                Pins[24].Value = Pin.High;
                val -= 4;
                if (Pins[11].Value >= Pin.Halfcut) Pins[32].Value = Pin.High;
            }

            if (val >= 2)
            {
                Pins[23].Value = Pin.High;
                val -= 2;
                if (Pins[11].Value >= Pin.Halfcut) Pins[31].Value = Pin.High;
            }

            if (val >= 1)
            {
                Pins[22].Value = Pin.High;
                val -= 1;
                if (Pins[11].Value >= Pin.Halfcut) Pins[30].Value = Pin.High;
            }

            for (var i = 22; i < 30; i++)
            {
                Pins[i].SimulationIdInternal = SimulationIdInternal;
                Pins[i].Propagate();
            }

            if (Pins[11].Value >= Pin.Halfcut || Pins[12].Value >= Pin.Halfcut &&
                !(Pins[11].Value >= Pin.Halfcut && Pins[12].Value >= Pin.Halfcut))
                for (var i = 30; i < 38; i++)
                {
                    Pins[i].SimulationIdInternal = SimulationIdInternal;
                    Pins[i].Propagate();
                }
        }
    }
}