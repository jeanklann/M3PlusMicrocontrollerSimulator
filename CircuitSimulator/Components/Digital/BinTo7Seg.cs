using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital {
    public class BinTo7Seg:Chip {
        /// <summary>
        /// Less dignificant bit
        /// </summary>
        public Pin A { get { return Pins[0]; } }
        public Pin B { get { return Pins[1]; } }
        public Pin C { get { return Pins[2]; } }
        /// <summary>
        /// Most significant bit
        /// </summary>
        public Pin D { get { return Pins[3]; } }
        /// <summary>
        /// The enable pin
        /// </summary>
        public Pin Enable { get { return Pins[4]; } }
        public Pin a { get { return Pins[5]; } }
        public Pin b { get { return Pins[6]; } }
        public Pin c { get { return Pins[7]; } }
        public Pin d { get { return Pins[8]; } }
        public Pin e { get { return Pins[9]; } }
        public Pin f { get { return Pins[10]; } }
        public Pin g { get { return Pins[11]; } }
        public bool IsHexadecimalValid = true;

        private static readonly float[,] table = new float[,] {
            {Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.LOW}, //0
            {Pin.LOW, Pin.HIGH, Pin.HIGH, Pin.LOW, Pin.LOW, Pin.LOW, Pin.LOW}, //1
            {Pin.HIGH, Pin.HIGH, Pin.LOW, Pin.HIGH, Pin.HIGH, Pin.LOW, Pin.HIGH}, //2
            {Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.LOW, Pin.LOW, Pin.HIGH}, //3
            {Pin.LOW, Pin.HIGH, Pin.HIGH, Pin.LOW, Pin.LOW, Pin.HIGH, Pin.HIGH}, //4
            {Pin.HIGH, Pin.LOW, Pin.HIGH, Pin.HIGH, Pin.LOW, Pin.HIGH, Pin.HIGH}, //5
            {Pin.HIGH, Pin.LOW, Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.HIGH}, //6
            {Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.LOW, Pin.LOW, Pin.LOW, Pin.LOW}, //7
            {Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.HIGH}, //8
            {Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.LOW, Pin.HIGH, Pin.HIGH}, //9
            {Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.LOW, Pin.HIGH, Pin.HIGH, Pin.HIGH}, //A
            {Pin.LOW, Pin.LOW, Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.HIGH}, //B
            {Pin.HIGH, Pin.LOW, Pin.LOW, Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.LOW}, //C
            {Pin.LOW, Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.LOW, Pin.HIGH}, //D
            {Pin.HIGH, Pin.LOW, Pin.LOW, Pin.HIGH, Pin.HIGH, Pin.HIGH, Pin.HIGH}, //E
            {Pin.HIGH, Pin.LOW, Pin.LOW, Pin.LOW, Pin.HIGH, Pin.HIGH, Pin.HIGH}, //F
            {Pin.LOW, Pin.LOW, Pin.LOW, Pin.LOW, Pin.LOW, Pin.LOW, Pin.LOW}, //clean
        };



        public BinTo7Seg(string name = "BinTo7Seg") : base(name, 12) {

        }
        protected override void AllocatePins() {
            for(int i = 0; i < Pins.Length; i++) {
                if(i<4)
                    Pins[i] = new Pin(this, false, false); //inputs
                else if(i == 4)
                    Pins[i] = new Pin(this, false, false, Pin.HIGH); //enable pin
                else
                    Pins[i] = new Pin(this, true, false); //outputs
            }
        }
        internal override bool CanExecute() {
            if(simulationId == circuit.SimulationId) return false;
            for(int i = 0; i < 4; i++) { //not needed to verify if Enable is connected
                if(Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }
        protected internal override void Execute() {
            base.Execute();
            if(Enable.GetDigital() == Pin.LOW) {
                ToOutput(16);
            } else {
                int value = 0;
                if(D.GetDigital() == Pin.HIGH)
                    value += 8;
                if(C.GetDigital() == Pin.HIGH)
                    value += 4;
                if(B.GetDigital() == Pin.HIGH)
                    value += 2;
                if(A.GetDigital() == Pin.HIGH)
                    value += 1;
                if(!IsHexadecimalValid) {
                    if(value >= 9)
                        value = 16;
                }

                ToOutput(value);
            }
            
            for(int i = 5; i < Pins.Length; i++) {
                Pins[i].Propagate();
            }
        }

        /// <summary>
        /// Sends the content to the outputs
        /// </summary>
        /// <param name="value">The value</param>
        private void ToOutput(int value) {
            if(value > 16 || value < 0) {
                value = 16;
                a.isOpen = true;
                b.isOpen = true;
                c.isOpen = true;
                d.isOpen = true;
                e.isOpen = true;
                f.isOpen = true;
                g.isOpen = true;
            } else {
                a.isOpen = false;
                b.isOpen = false;
                c.isOpen = false;
                d.isOpen = false;
                e.isOpen = false;
                f.isOpen = false;
                g.isOpen = false;
            }
            a.Value = table[value, 0];
            b.Value = table[value, 1];
            c.Value = table[value, 2];
            d.Value = table[value, 3];
            e.Value = table[value, 4];
            f.Value = table[value, 5];
            g.Value = table[value, 6];
        }

    }
}
