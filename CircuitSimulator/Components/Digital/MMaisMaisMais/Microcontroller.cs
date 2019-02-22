namespace CircuitSimulator.Components.Digital.MMaisMaisMais
{
    public class Microcontroller : Component
    {
        public PortBank PortBank;
        public bool PreventUpdateInput = false;

        public Microcontroller(string name = "Logic Input") : base(name, 64)
        {
            for (var i = 0; i < 32; i++)
            {
                Pins[i].IsOutputInternal = false;
                Pins[i].IsOpenInternal = false;
            }

            for (var i = 32; i < 64; i++)
            {
                Pins[i].IsOutputInternal = true;
                Pins[i].IsOpenInternal = false;
            }

            CanStart = true;
        }

        internal override bool CanExecute()
        {
            if (SimulationIdInternal == Circuit.SimulationId) return false;
            /*
            for (int i = 0; i < 32; i++) {
                if (Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            */
            return true;
        }

        public void SetInput(byte value, int indexInput)
        {
            SetPin(value, indexInput * 8);
        }

        public void SetOutput(byte value, int indexOutput)
        {
            SetPin(value, 32 + indexOutput * 8);
        }

        public byte PinValuesToByteValue(int index)
        {
            var pins = new float[8];
            for (var i = 7; i >= 0; i--) pins[i] = Pins[8 * index + i].Value;
            byte value = 0;
            var valSup = (Pin.High + Pin.Low) / 2;
            if (pins[0] >= valSup) value += 1;
            if (pins[1] >= valSup) value += 2;
            if (pins[2] >= valSup) value += 4;
            if (pins[3] >= valSup) value += 8;
            if (pins[4] >= valSup) value += 16;
            if (pins[5] >= valSup) value += 32;
            if (pins[6] >= valSup) value += 64;
            if (pins[7] >= valSup) value += 128;

            return value;
        }

        private void SetPin(byte value, int index)
        {
            index += 7;
            if (value >= 128)
            {
                value -= 128;
                Pins[index].SetDigital(Pin.High);
                index--;
            }
            else
            {
                Pins[index].SetDigital(Pin.Low);
                index--;
            }

            if (value >= 64)
            {
                value -= 64;
                Pins[index].SetDigital(Pin.High);
                index--;
            }
            else
            {
                Pins[index].SetDigital(Pin.Low);
                index--;
            }

            if (value >= 32)
            {
                value -= 32;
                Pins[index].SetDigital(Pin.High);
                index--;
            }
            else
            {
                Pins[index].SetDigital(Pin.Low);
                index--;
            }

            if (value >= 16)
            {
                value -= 16;
                Pins[index].SetDigital(Pin.High);
                index--;
            }
            else
            {
                Pins[index].SetDigital(Pin.Low);
                index--;
            }

            if (value >= 8)
            {
                value -= 8;
                Pins[index].SetDigital(Pin.High);
                index--;
            }
            else
            {
                Pins[index].SetDigital(Pin.Low);
                index--;
            }

            if (value >= 4)
            {
                value -= 4;
                Pins[index].SetDigital(Pin.High);
                index--;
            }
            else
            {
                Pins[index].SetDigital(Pin.Low);
                index--;
            }

            if (value >= 2)
            {
                value -= 2;
                Pins[index].SetDigital(Pin.High);
                index--;
            }
            else
            {
                Pins[index].SetDigital(Pin.Low);
                index--;
            }

            if (value >= 1)
                Pins[index].SetDigital(Pin.High);
            else
                Pins[index].SetDigital(Pin.Low);
        }

        protected internal override void Execute()
        {
            base.Execute();
            if (PortBank != null)
            {
                for (var i = 0; i < 4; i++)
                {
                    SetOutput(PortBank.GetOutput(i), i);
                    if (PreventUpdateInput)
                        SetInput(PortBank.GetInput(i), i);
                    else
                        for (var j = 0; j < 8; j++)
                            PortBank.SetInput(i, j, Pins[i * 8 + j].Value);
                }
            }

            for (var i = 32; i < 64; i++) Pins[i].Propagate();
        }
    }
}