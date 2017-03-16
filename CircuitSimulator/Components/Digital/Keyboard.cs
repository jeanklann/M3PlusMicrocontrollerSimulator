using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital {
    public class Keyboard : Component {
        public byte Value = 0;

        public Keyboard(string name = "Keyboard Input"):base(name, 8) {
            for (int i = 0; i < Pins.Length; i++) {
                Pins[i].isOutput = true;
                Pins[i].isOpen = false;
                Pins[i].value = Pin.LOW;
            }
            canStart = true;
        }
        


        protected internal override void Execute() {
            base.Execute();
            
            byte valueTemp = Value;
            if (valueTemp >= 128) {
                Pins[0].SetDigital(Pin.HIGH);
                valueTemp -= 128;
            } else {
                Pins[0].SetDigital(Pin.LOW);
            }
            if (valueTemp >= 64) {
                Pins[1].SetDigital(Pin.HIGH);
                valueTemp -= 64;
            } else {
                Pins[1].SetDigital(Pin.LOW);
            }
            if (valueTemp >= 32) {
                Pins[2].SetDigital(Pin.HIGH);
                valueTemp -= 32;
            } else {
                Pins[2].SetDigital(Pin.LOW);
            }
            if (valueTemp >= 16) {
                Pins[3].SetDigital(Pin.HIGH);
                valueTemp -= 16;
            } else {
                Pins[3].SetDigital(Pin.LOW);
            }
            if (valueTemp >= 8) {
                Pins[4].SetDigital(Pin.HIGH);
                valueTemp -= 8;
            } else {
                Pins[4].SetDigital(Pin.LOW);
            }
            if (valueTemp >= 4) {
                Pins[5].SetDigital(Pin.HIGH);
                valueTemp -= 4;
            } else {
                Pins[5].SetDigital(Pin.LOW);
            }
            if (valueTemp >= 2) {
                Pins[6].SetDigital(Pin.HIGH);
                valueTemp -= 2;
            } else {
                Pins[6].SetDigital(Pin.LOW);
            }
            if (valueTemp >= 1) {
                Pins[7].SetDigital(Pin.HIGH);
            } else {
                Pins[7].SetDigital(Pin.LOW);
            }

            for (int i = 0; i < Pins.Length; i++) {
                Pins[i].Propagate();
            }
        }
    }
}
