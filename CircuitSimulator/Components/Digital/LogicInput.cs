﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator {
    public class LogicInput : Component {
        public float Value {
            get {
                return Pins[0].value;
            }
            set {
                Pins[0].SetDigital(value);
            }
        }
        
        public LogicInput(string name = "Logic Input"):base(name) {
            Pins[0].isOutput = true;
            Pins[0].isOpen = false;
            Pins[0].value = Pin.LOW;
            canStart = true;
        }

        public float Switch() {
            if(Pins[0].value == Pin.HIGH) Pins[0].value = Pin.LOW;
            else Pins[0].value = Pin.HIGH;
            return Pins[0].value;
        }


        protected internal override void Execute() {
            base.Execute();
            Pins[0].Propagate();
        }
    }
}