using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator {
    public class Macro:Component {
        public Pin Input(int pin) {
            if(pin >= input.Length || pin < 0) throw new Exception("The pin "+ pin +" is not a valid input pin");
            return input[pin].Pins[0];
        }
        public Pin Output(int pin) {
            if(pin >= output.Length || pin < 0) throw new Exception("The pin " + pin + " is not a valid input pin");
            return output[pin].Pins[0];
        }
        public int InputQuantity { get { return input.Length; } }
        public int OutputQuantity { get { return output.Length; } }

        public bool Enabled = true;
        public Circuit Circuit;

        protected internal LogicInput[] input;
        protected internal LogicOutput[] output;


        public Macro(int inputQuantity, int outputQuantity, string name = "Macro", Circuit circuit = null):base(name, inputQuantity+outputQuantity) {
            if(circuit == null) {
                Circuit = new Circuit();
            } else {
                Circuit = circuit;
            }
            input = new LogicInput[inputQuantity];
            output = new LogicOutput[outputQuantity];

            for(int i = 0; i < inputQuantity; i++) {
                input[i] = new LogicInput("Input " + i);
                Circuit.AddComponent(input[i]);
            }
            for(int i = 0; i < outputQuantity; i++) {
                output[i] = new LogicOutput("Output " + i);
                Circuit.AddComponent(output[i]);
            }
        }

        protected internal override void Execute() {
            base.Execute();
            if(Enabled) {
                for(int i1 = 0; i1 < input.Length; i1++) {
                    input[i1].Value = Pins[i1].Value;
                }
                Circuit.Tick();
                for(int i2 = 0; i2 < output.Length; i2++) {
                    Pins[i2 + input.Length].SetDigital(output[i2].Value);
                }
            }
            for(int i = 0; i < output.Length; i++) {
                Pins[i+input.Length].Propagate();
            }
        }
        internal override bool CanExecute() {
            if(simulationId == circuit.SimulationId) return false;
            for(int i = 0; i < input.Length; i++) {
                if(Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }
    }
}
