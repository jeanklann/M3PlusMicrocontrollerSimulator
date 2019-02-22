namespace CircuitSimulator.Components.Digital.MMaisMaisMais
{
    public class RomMemory : Chip
    {
        public byte[] InternalValue = new byte[65536];

        public RomMemory(string name = "RomMemory") : base(name, 25)
        {
        }

        protected override void AllocatePins()
        {
            for (var i = 0; i < 17; i++) Pins[i] = new Pin(this, false, false);
            for (var i = 17; i < 25; i++) Pins[i] = new Pin(this, true, false);
        }

        internal override bool CanExecute()
        {
            if (SimulationIdInternal == Circuit.SimulationId) return false;
            if (Pins[16].SimulationIdInternal != Circuit.SimulationId) return false;

            if (Pins[16].Value >= Pin.Halfcut)
            {
                for (var i = 0; i < 16; i++)
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
            if (Pins[16].Value >= Pin.Halfcut)
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
                address += (byte) (Pins[8].Value >= Pin.Halfcut ? 256 : 0);
                address += (byte) (Pins[9].Value >= Pin.Halfcut ? 512 : 0);
                address += (byte) (Pins[10].Value >= Pin.Halfcut ? 1024 : 0);
                address += (byte) (Pins[11].Value >= Pin.Halfcut ? 2048 : 0);
                address += (byte) (Pins[12].Value >= Pin.Halfcut ? 4096 : 0);
                address += (byte) (Pins[13].Value >= Pin.Halfcut ? 8192 : 0);
                address += (byte) (Pins[14].Value >= Pin.Halfcut ? 16384 : 0);
                address += (byte) (Pins[15].Value >= Pin.Halfcut ? 32768 : 0);

                var val = InternalValue[address];
                for (var i = 17; i < 25; i++)
                    Pins[i].Value = Pin.Low;
                if (val >= 128)
                {
                    Pins[24].Value = Pin.High;
                    val -= 128;
                }

                if (val >= 64)
                {
                    Pins[23].Value = Pin.High;
                    val -= 64;
                }

                if (val >= 32)
                {
                    Pins[22].Value = Pin.High;
                    val -= 32;
                }

                if (val >= 16)
                {
                    Pins[21].Value = Pin.High;
                    val -= 16;
                }

                if (val >= 8)
                {
                    Pins[20].Value = Pin.High;
                    val -= 8;
                }

                if (val >= 4)
                {
                    Pins[19].Value = Pin.High;
                    val -= 4;
                }

                if (val >= 2)
                {
                    Pins[18].Value = Pin.High;
                    val -= 2;
                }

                if (val >= 1)
                {
                    Pins[17].Value = Pin.High;
                    val -= 1;
                }

                for (var i = 17; i < 25; i++)
                {
                    Pins[i].SimulationIdInternal = SimulationIdInternal;
                    Pins[i].Propagate();
                }
            }
        }
    }
}