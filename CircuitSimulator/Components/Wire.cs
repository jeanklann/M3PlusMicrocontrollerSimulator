using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components {
    public class Wire:Component {

        public Wire(string name = "Wire") : base(name, 2) {
        }


        protected internal override void Execute() {
            base.Execute();
            for(int i = 0; i < Pins.Length; i++) {
                Pins[i].Propagate();
            }
        }
    }
}
