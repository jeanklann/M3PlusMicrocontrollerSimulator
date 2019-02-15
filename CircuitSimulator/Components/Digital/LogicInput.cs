﻿namespace CircuitSimulator {
    public class LogicInput : Component {
        public float Value {
            get => Pins[0].Value;
            set => Pins[0].SetDigital(value);
        }
        
        public LogicInput(string name = "Logic Input"):base(name) {
            Pins[0].IsOutputInternal = true;
            Pins[0].IsOpenInternal = false;
            Pins[0].Value = Pin.LOW;
            CanStart = true;
        }

        public float Switch() {
            if(Pins[0].Value == Pin.HIGH) Pins[0].Value = Pin.LOW;
            else Pins[0].Value = Pin.HIGH;
            return Pins[0].Value;
        }


        protected internal override void Execute() {
            base.Execute();
            Pins[0].Propagate();
        }
    }
}
