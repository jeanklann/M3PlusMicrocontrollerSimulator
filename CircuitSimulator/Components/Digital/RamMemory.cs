namespace CircuitSimulator.Components.Digital.MMaisMaisMais
{
    public class RamMemory : Chip
    {
        public byte[] InternalValue = new byte[256];

        public RamMemory(string name = "RamMemory") : base(name, 19)
        {
        }

        protected override void AllocatePins()
        {
            for (var i = 0; i < 11; i++) Pins[i] = new Pin(this, false, false);
            for (var i = 11; i < 19; i++) Pins[i] = new Pin(this, true, false);
        }

        internal override bool CanExecute()
        {
            if (SimulationIdInternal == Circuit.SimulationId) return false;
            for (var i = 8; i <= 10; i++)
                if (Pins[i].SimulationIdInternal != Circuit.SimulationId)
                    return false;
            if (Pins[8].Value >= Pin.Halfcut)
            {
                if (Pins[9].Value >= Pin.Halfcut)
                    for (var j = 0; j < 8; j++)
                        if (Pins[j].SimulationIdInternal != Circuit.SimulationId)
                            return false;
                else
                    for (var k = 11; k < 19; k++)
                        if (Pins[k].SimulationIdInternal != Circuit.SimulationId)
                            return false;
            }

            return true;
        }

        protected internal override void Execute()
        {
            SimulationIdInternal = Circuit.SimulationId;
            if (Pins[10].Value >= Pin.Halfcut)
                for (var i = 0; i < 256; i++)
                    InternalValue[i] = 0;
            if (Pins[8].Value >= Pin.Halfcut)
            {
                var address = 0;
                address += (byte) (Pins[0].Value >= Pin.Halfcut ? 1 : 0);
                address += (byte) (Pins[1].Value >= Pin.Halfcut ? 2 : 0);
                address += (byte) (Pins[2].Value >= Pin.Halfcut ? 4 : 0);
                address += (byte) (Pins[3].Value >= Pin.Halfcut ? 8 : 0);
                address += (byte) (Pins[4].Value >= Pin.Halfcut ? 16 : 0);
                address += (byte) (Pins[5].Value >= Pin.Halfcut ? 32 : 0);
                address += (byte) (Pins[6].Value >= Pin.Halfcut ? 64 : 0);
                address += (byte) (Pins[7].Value >= Pin.Halfcut ? 128 : 0);

                if (Pins[9].Value >= Pin.Halfcut)
                {
                    var val = InternalValue[address];
                    for (var i = 11; i <= 18; i++)
                        Pins[i].Value = Pin.Low;
                    if (val >= 128)
                    {
                        Pins[18].Value = Pin.High;
                        val -= 128;
                    }

                    if (val >= 64)
                    {
                        Pins[17].Value = Pin.High;
                        val -= 64;
                    }

                    if (val >= 32)
                    {
                        Pins[16].Value = Pin.High;
                        val -= 32;
                    }

                    if (val >= 16)
                    {
                        Pins[15].Value = Pin.High;
                        val -= 16;
                    }

                    if (val >= 8)
                    {
                        Pins[14].Value = Pin.High;
                        val -= 8;
                    }

                    if (val >= 4)
                    {
                        Pins[13].Value = Pin.High;
                        val -= 4;
                    }

                    if (val >= 2)
                    {
                        Pins[12].Value = Pin.High;
                        val -= 2;
                    }

                    if (val >= 1)
                    {
                        Pins[11].Value = Pin.High;
                        val -= 1;
                    }

                    for (var i = 11; i <= 18; i++)
                    {
                        Pins[i].SimulationIdInternal = SimulationIdInternal;
                        Pins[i].Propagate();
                    }
                }
                else
                {
                    byte value = 0;
                    value += (byte) (Pins[11].Value >= Pin.Halfcut ? 1 : 0);
                    value += (byte) (Pins[12].Value >= Pin.Halfcut ? 2 : 0);
                    value += (byte) (Pins[13].Value >= Pin.Halfcut ? 4 : 0);
                    value += (byte) (Pins[14].Value >= Pin.Halfcut ? 8 : 0);
                    value += (byte) (Pins[15].Value >= Pin.Halfcut ? 16 : 0);
                    value += (byte) (Pins[16].Value >= Pin.Halfcut ? 32 : 0);
                    value += (byte) (Pins[17].Value >= Pin.Halfcut ? 64 : 0);
                    value += (byte) (Pins[18].Value >= Pin.Halfcut ? 128 : 0);
                    InternalValue[address] = value;
                }
            }
        }
    }
}