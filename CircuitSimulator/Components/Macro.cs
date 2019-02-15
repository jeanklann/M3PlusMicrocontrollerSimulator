using System;

namespace CircuitSimulator {
    public class Macro:Component {
        public Pin Input(int pin) {
            if(pin >= InputInternal.Length || pin < 0) throw new Exception("The pin "+ pin +" is not a valid input pin");
            return InputInternal[pin].Pins[0];
        }
        public Pin Output(int pin) {
            if(pin >= OutputInternal.Length || pin < 0) throw new Exception("The pin " + pin + " is not a valid input pin");
            return OutputInternal[pin].Pins[0];
        }
        public int InputQuantity => InputInternal.Length;
        public int OutputQuantity => OutputInternal.Length;

        public bool Enabled = true;
        public Circuit Circuit;

        protected internal LogicInput[] InputInternal;
        protected internal LogicOutput[] OutputInternal;


        public Macro(int inputQuantity, int outputQuantity, string name = "Macro", Circuit circuit = null):base(name, inputQuantity+outputQuantity) {
            if(circuit == null) {
                Circuit = new Circuit();
            } else {
                Circuit = circuit;
            }
            InputInternal = new LogicInput[inputQuantity];
            OutputInternal = new LogicOutput[outputQuantity];

            for(var i = 0; i < inputQuantity; i++) {
                InputInternal[i] = new LogicInput("Input " + i);
                Circuit.AddComponent(InputInternal[i]);
            }
            for(var i = 0; i < outputQuantity; i++) {
                OutputInternal[i] = new LogicOutput("Output " + i);
                Circuit.AddComponent(OutputInternal[i]);
            }
        }

        protected internal override void Execute() {
            base.Execute();
            if(Enabled) {
                for(var i1 = 0; i1 < InputInternal.Length; i1++) {
                    InputInternal[i1].Value = Pins[i1].Value;
                }
                Circuit.Tick();
                for(var i2 = 0; i2 < OutputInternal.Length; i2++) {
                    Pins[i2 + InputInternal.Length].SetDigital(OutputInternal[i2].Value);
                }
            }
            for(var i = 0; i < OutputInternal.Length; i++) {
                Pins[i+InputInternal.Length].Propagate();
            }
        }
        internal override bool CanExecute() {
            if(SimulationIdInternal == circuit.SimulationId) return false;
            for(var i = 0; i < InputInternal.Length; i++) {
                if(Pins[i].SimulationIdInternal != circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }
    }
}
