using System;

namespace CircuitSimulator
{
    public class ShiftRegister : Component
    {
        private readonly byte _bits = 8;
        private float _lastClock = Pin.Low;
        private ulong _storedValue;

        public ShiftRegister(string name = "Shift Register", byte bits = 8) : base(name, bits + 2)
        {
            if (bits >= 1 && bits <= 64)
                _bits = bits;
            else throw new Exception("Incorrect number of bits: " + bits);
        }

        /// <summary>
        ///     Input value
        /// </summary>
        public Pin Input => Pins[0];

        /// <summary>
        ///     Clock (desc)
        /// </summary>
        public Pin Clock => Pins[1];

        public Pin Output(int index)
        {
            return Pins[index + 2];
        }

        protected override void AllocatePins()
        {
            for (var i = 0; i < 2; i++) Pins[i] = new Pin(this, false, false);
            for (var i = 2; i < _bits + 2; i++) Pins[i] = new Pin(this, true, false);
        }

        internal override bool CanExecute()
        {
            if (SimulationIdInternal == Circuit.SimulationId) return false;
            for (var i = 0; i < 2; i++)
            {
                if (Pins[i].SimulationIdInternal != Circuit.SimulationId)
                    return false;
            }

            return true;
        }

        protected internal override void Execute()
        {
            base.Execute();
            if (Clock.Value == Pin.Low && _lastClock == Pin.High)
            {
                _storedValue *= 2;
                _storedValue += Input.GetDigital() == Pin.High ? 1u : 0u;
                ulong temp = 1;
                for (var i = 0; i < _bits; i++)
                {
                    var val = _storedValue & temp;
                    if (val > 0)
                        Pins[i + 2].Value = Pin.High;
                    else
                        Pins[i + 2].Value = Pin.Low;
                    temp *= 2;
                }
            }

            _lastClock = Clock.Value;
            for (var i = 2; i < _bits + 2; i++) Pins[i].Propagate();
        }
    }
}