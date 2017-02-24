using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator {
    public class ClockLogicInput : Component {
        public bool IsOn = true;
        private float timeElapsed = 0f;
        private float time { get { return 1f / Frequency; } }
        public float Frequency = 1f;
        public float Value {
            get {
                return Pins[0].value;
            }
            set {
                Pins[0].SetDigital(value);
            }
        }

        public ClockLogicInput(string name = "Clock Logic Input") : base(name) {
            Pins[0].isOutput = true;
            canStart = true;
        }

        private float Switch() {
            if(Pins[0].value == Pin.HIGH) Pins[0].value = Pin.LOW;
            else Pins[0].value = Pin.HIGH;
            return Pins[0].value;
        }

        protected internal override void Execute() {
            base.Execute();
            if(IsOn) {
                timeElapsed += circuit.TimePerTick;
                if(timeElapsed >= time) {
                    timeElapsed = 0f;
                    Switch();
                }
            }
            Pins[0].Propagate();
        }
    }
}
