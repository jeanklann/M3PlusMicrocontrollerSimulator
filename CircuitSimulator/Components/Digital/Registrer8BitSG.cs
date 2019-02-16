namespace CircuitSimulator.Components.Digital.MMaisMaisMais
{
    public class Registrer8BitSg : Chip
    {
        private byte _internalValue;
        private float _lastClock = Pin.Low;

        public Registrer8BitSg(string name = "Registrer8BitSG") : base(name, 18)
        {
        }

        public Pin Clock => Pins[8];
        public Pin Reset => Pins[9];

        protected override void AllocatePins()
        {
            for (var i = 0; i < 10; i++) Pins[i] = new Pin(this, false, false);
            for (var i = 10; i < 18; i++) Pins[i] = new Pin(this, true, false);
        }

        internal override bool CanExecute()
        {
            if (SimulationIdInternal == Circuit.SimulationId) return false;
            for (var i = 8; i <= 9; i++)
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
            base.Execute();
            var halfCut = (Pin.High + Pin.Low) / 2f;
            if (Reset.Value >= halfCut)
            {
                _internalValue = 0;
            }
            else
            {
                if (Clock.Value >= halfCut && _lastClock < halfCut)
                {
                    _internalValue = 0;
                    if (Pins[0].Value >= halfCut) _internalValue += 1;
                    if (Pins[1].Value >= halfCut) _internalValue += 2;
                    if (Pins[2].Value >= halfCut) _internalValue += 4;
                    if (Pins[3].Value >= halfCut) _internalValue += 8;
                    if (Pins[4].Value >= halfCut) _internalValue += 16;
                    if (Pins[5].Value >= halfCut) _internalValue += 32;
                    if (Pins[6].Value >= halfCut) _internalValue += 64;
                    if (Pins[7].Value >= halfCut) _internalValue += 128;
                }
            }

            _lastClock = Clock.Value;

            var tempVal = _internalValue;
            for (var i = 10; i < 18; i++) Pins[i].SetDigital(Pin.Low);
            if ((tempVal & 0x01) == 0x01) Pins[10].SetDigital(Pin.High);
            if ((tempVal & 0x02) == 0x02) Pins[11].SetDigital(Pin.High);
            if ((tempVal & 0x04) == 0x04) Pins[12].SetDigital(Pin.High);
            if ((tempVal & 0x08) == 0x08) Pins[13].SetDigital(Pin.High);
            if ((tempVal & 0x10) == 0x10) Pins[14].SetDigital(Pin.High);
            if ((tempVal & 0x20) == 0x20) Pins[15].SetDigital(Pin.High);
            if ((tempVal & 0x40) == 0x40) Pins[16].SetDigital(Pin.High);
            if ((tempVal & 0x80) == 0x80) Pins[17].SetDigital(Pin.High);

            for (var i = 10; i < 18; i++) Pins[i].Propagate();
        }
    }
}