using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator {
    public class LogicOutput : Component {
        public float Value {
            get {
                return Pins[0].value;
            }
            set {
                Pins[0].SetDigital(value);
            }
        }

        public LogicOutput(string name = "Logic Output") : base(name) {
            //canStart = true;
        }


        protected internal override void Execute() {
            base.Execute();
        }
    }
}
