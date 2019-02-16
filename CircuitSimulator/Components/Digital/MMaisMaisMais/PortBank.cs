using System;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais
{
    public class PortBank : Chip
    {
        private float _lastClock = Pin.Low;
        public byte[] RegOut = new byte[4];

        public PortBank(string name = "PortBank") : base(name, 85)
        {
        }

        public void SetInput(int port, int pin, float value)
        {
            if (port < 0 || port > 3) throw new Exception("Porta inválida.");
            if (pin < 0 || pin > 8) throw new Exception("Pino inválido.");
            Pins[port * 8 + pin].Value = value;
        }

        public void SetInput(int port, byte value)
        {
            if (port < 0 || port > 3) throw new Exception("Porta inválida.");
            Pins[port * 8 + 0].Value = (value & 0x01) == 0x01 ? Pin.High : Pin.Low;
            Pins[port * 8 + 1].Value = (value & 0x02) == 0x02 ? Pin.High : Pin.Low;
            Pins[port * 8 + 2].Value = (value & 0x04) == 0x04 ? Pin.High : Pin.Low;
            Pins[port * 8 + 3].Value = (value & 0x08) == 0x08 ? Pin.High : Pin.Low;
            Pins[port * 8 + 4].Value = (value & 0x10) == 0x10 ? Pin.High : Pin.Low;
            Pins[port * 8 + 5].Value = (value & 0x20) == 0x20 ? Pin.High : Pin.Low;
            Pins[port * 8 + 6].Value = (value & 0x40) == 0x40 ? Pin.High : Pin.Low;
            Pins[port * 8 + 7].Value = (value & 0x80) == 0x80 ? Pin.High : Pin.Low;
        }

        public byte GetInput(int port)
        {
            if (port < 0 || port > 3) throw new Exception("Porta inválida.");
            byte val = 0;
            val += (byte) (Pins[port * 8 + 0].Value >= Pin.Halfcut ? 1 : 0);
            val += (byte) (Pins[port * 8 + 1].Value >= Pin.Halfcut ? 2 : 0);
            val += (byte) (Pins[port * 8 + 2].Value >= Pin.Halfcut ? 4 : 0);
            val += (byte) (Pins[port * 8 + 3].Value >= Pin.Halfcut ? 8 : 0);
            val += (byte) (Pins[port * 8 + 4].Value >= Pin.Halfcut ? 16 : 0);
            val += (byte) (Pins[port * 8 + 5].Value >= Pin.Halfcut ? 32 : 0);
            val += (byte) (Pins[port * 8 + 6].Value >= Pin.Halfcut ? 64 : 0);
            val += (byte) (Pins[port * 8 + 7].Value >= Pin.Halfcut ? 128 : 0);
            return val;
        }

        public float GetOutput(int port, int pin)
        {
            if (port < 0 || port > 3) throw new Exception("Porta inválida.");
            if (pin < 0 || pin > 8) throw new Exception("Pino inválido.");
            switch (pin)
            {
                case 0: return (RegOut[port] & 0x01) == 0x01 ? Pin.High : Pin.Low;
                case 1: return (RegOut[port] & 0x02) == 0x02 ? Pin.High : Pin.Low;
                case 2: return (RegOut[port] & 0x04) == 0x04 ? Pin.High : Pin.Low;
                case 3: return (RegOut[port] & 0x08) == 0x08 ? Pin.High : Pin.Low;
                case 4: return (RegOut[port] & 0x10) == 0x10 ? Pin.High : Pin.Low;
                case 5: return (RegOut[port] & 0x20) == 0x20 ? Pin.High : Pin.Low;
                case 6: return (RegOut[port] & 0x40) == 0x40 ? Pin.High : Pin.Low;
                case 7: return (RegOut[port] & 0x80) == 0x80 ? Pin.High : Pin.Low;
                default: return Pin.Low;
            }
        }

        public byte GetOutput(int port)
        {
            if (port < 0 || port > 3) throw new Exception("Porta inválida.");
            return RegOut[port];
        }

        protected override void AllocatePins()
        {
            for (var i = 0; i < 32; i++) Pins[i] = new Pin(this, true, false);
            for (var i = 32; i < 45; i++) Pins[i] = new Pin(this, false, false);
            for (var i = 45; i < 85; i++) Pins[i] = new Pin(this, true, false);
        }

        internal override bool CanExecute()
        {
            if (SimulationIdInternal == Circuit.SimulationId) return false;
            for (var i = 0; i < 5; i++)
                if (Pins[8 * 5 + i].SimulationIdInternal != Circuit.SimulationId)
                    return false;
            if (Pins[8 * 5 + 3].Value >= Pin.Halfcut)
                for (var i = 0; i < 8; i++)
                    if (Pins[8 * 4 + i].SimulationIdInternal != Circuit.SimulationId)
                        return false;
            return true;
        }

        protected internal override void Execute()
        {
            SimulationIdInternal = Circuit.SimulationId;
            if (Pins[8 * 5 + 4].Value >= Pin.Halfcut)
                for (var i = 0; i < 4; i++) //reseta
                    RegOut[i] = 0;
            if (Pins[8 * 5 + 3].Value >= Pin.Halfcut && _lastClock < Pin.Halfcut)
            {
                //clock para output
                byte val = 0;
                val += (byte) (Pins[8 * 4 + 0].Value >= Pin.Halfcut ? 1 : 0);
                val += (byte) (Pins[8 * 4 + 1].Value >= Pin.Halfcut ? 2 : 0);
                val += (byte) (Pins[8 * 4 + 2].Value >= Pin.Halfcut ? 4 : 0);
                val += (byte) (Pins[8 * 4 + 3].Value >= Pin.Halfcut ? 8 : 0);
                val += (byte) (Pins[8 * 4 + 4].Value >= Pin.Halfcut ? 16 : 0);
                val += (byte) (Pins[8 * 4 + 5].Value >= Pin.Halfcut ? 32 : 0);
                val += (byte) (Pins[8 * 4 + 6].Value >= Pin.Halfcut ? 64 : 0);
                val += (byte) (Pins[8 * 4 + 7].Value >= Pin.Halfcut ? 128 : 0);
                if (Pins[8 * 5 + 1].Value < Pin.Halfcut && Pins[8 * 5 + 0].Value < Pin.Halfcut) RegOut[0] = val;
                if (Pins[8 * 5 + 1].Value >= Pin.Halfcut && Pins[8 * 5 + 0].Value < Pin.Halfcut) RegOut[1] = val;
                if (Pins[8 * 5 + 1].Value < Pin.Halfcut && Pins[8 * 5 + 0].Value >= Pin.Halfcut) RegOut[2] = val;
                if (Pins[8 * 5 + 1].Value >= Pin.Halfcut && Pins[8 * 5 + 0].Value >= Pin.Halfcut) RegOut[3] = val;
            }

            _lastClock = Pins[8 * 5 + 3].Value;
            if (Pins[8 * 5 + 2].Value >= Pin.Halfcut)
            {
                //joga input pro bus

                byte val = 0;
                if (Pins[8 * 5 + 1].Value < Pin.Halfcut && Pins[8 * 5 + 0].Value < Pin.Halfcut) val = GetInput(0);
                if (Pins[8 * 5 + 1].Value >= Pin.Halfcut && Pins[8 * 5 + 0].Value < Pin.Halfcut) val = GetInput(1);
                if (Pins[8 * 5 + 1].Value < Pin.Halfcut && Pins[8 * 5 + 0].Value >= Pin.Halfcut) val = GetInput(2);
                if (Pins[8 * 5 + 1].Value >= Pin.Halfcut && Pins[8 * 5 + 0].Value >= Pin.Halfcut) val = GetInput(3);

                Pins[8 * 5 + 5 + 0].Value = (val & 0x01) == 0x00 ? Pin.Low : Pin.High;
                Pins[8 * 5 + 5 + 1].Value = (val & 0x02) == 0x00 ? Pin.Low : Pin.High;
                Pins[8 * 5 + 5 + 2].Value = (val & 0x04) == 0x00 ? Pin.Low : Pin.High;
                Pins[8 * 5 + 5 + 3].Value = (val & 0x08) == 0x00 ? Pin.Low : Pin.High;
                Pins[8 * 5 + 5 + 4].Value = (val & 0x10) == 0x00 ? Pin.Low : Pin.High;
                Pins[8 * 5 + 5 + 5].Value = (val & 0x20) == 0x00 ? Pin.Low : Pin.High;
                Pins[8 * 5 + 5 + 6].Value = (val & 0x40) == 0x00 ? Pin.Low : Pin.High;
                Pins[8 * 5 + 5 + 7].Value = (val & 0x80) == 0x00 ? Pin.Low : Pin.High;

                for (var i = 0; i < 8; i++)
                    if (Pins[8 * 5 + 1].Value < Pin.Halfcut && Pins[8 * 5 + 0].Value < Pin.Halfcut)
                        Pins[8 * 5 + 5 + i].Value = Pins[8 * 0 + i].Value;
                    else if (Pins[8 * 5 + 1].Value >= Pin.Halfcut && Pins[8 * 5 + 0].Value < Pin.Halfcut)
                        Pins[8 * 5 + 5 + i].Value = Pins[8 * 1 + i].Value;
                    else if (Pins[8 * 5 + 1].Value < Pin.Halfcut && Pins[8 * 5 + 0].Value >= Pin.Halfcut)
                        Pins[8 * 5 + 5 + i].Value = Pins[8 * 2 + i].Value;
                    else if (Pins[8 * 5 + 1].Value >= Pin.Halfcut && Pins[8 * 5 + 0].Value >= Pin.Halfcut)
                        Pins[8 * 5 + 5 + i].Value = Pins[8 * 3 + i].Value;


                for (var i = 0; i < 8; i++)
                {
                    Pins[8 * 5 + 5 + i].SimulationIdInternal = Circuit.SimulationId;

                    Pins[8 * 5 + 5 + i].Propagate(); //outbus
                }
            }

            for (var port = 0; port < 4; port++)
            {
                Pins[8 * 6 + 5 + port * 8 + 0].Value = (RegOut[port] & 0x01) == 0x00 ? Pin.Low : Pin.High;
                Pins[8 * 6 + 5 + port * 8 + 1].Value = (RegOut[port] & 0x02) == 0x00 ? Pin.Low : Pin.High;
                Pins[8 * 6 + 5 + port * 8 + 2].Value = (RegOut[port] & 0x04) == 0x00 ? Pin.Low : Pin.High;
                Pins[8 * 6 + 5 + port * 8 + 3].Value = (RegOut[port] & 0x08) == 0x00 ? Pin.Low : Pin.High;
                Pins[8 * 6 + 5 + port * 8 + 4].Value = (RegOut[port] & 0x10) == 0x00 ? Pin.Low : Pin.High;
                Pins[8 * 6 + 5 + port * 8 + 5].Value = (RegOut[port] & 0x20) == 0x00 ? Pin.Low : Pin.High;
                Pins[8 * 6 + 5 + port * 8 + 6].Value = (RegOut[port] & 0x40) == 0x00 ? Pin.Low : Pin.High;
                Pins[8 * 6 + 5 + port * 8 + 7].Value = (RegOut[port] & 0x80) == 0x00 ? Pin.Low : Pin.High;
            }

            for (var i = 0; i < 32; i++)
            {
                Pins[i].SimulationIdInternal = Circuit.SimulationId;
                Pins[8 * 6 + 5 + i].SimulationIdInternal = Circuit.SimulationId;

                Pins[i].Propagate(); //in
                Pins[8 * 6 + 5 + i].Propagate(); //out
            }
        }
    }
}