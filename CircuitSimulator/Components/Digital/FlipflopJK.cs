using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator {
    /// <summary>
    /// Flipflop JK: <para />
    /// J: pin 0,<para />
    /// K: pin 1,<para />
    /// CLK (desc): pin 2,<para />
    /// S: pin 3,<para />
    /// R: pin 4,<para />
    /// Q: pin 5,<para />
    /// </summary>
    public class FlipflopJK : Chip {
        private float lastClk = Pin.LOW;

        public Pin J { get { return Pins[0]; } }
        public Pin K { get { return Pins[1]; } }
        public Pin CLK { get { return Pins[2]; } }
        public Pin S { get { return Pins[3]; } }
        public Pin R { get { return Pins[4]; } }
        public Pin Q { get { return Pins[5]; } }
        public Pin Qnot { get { return Pins[6]; } }

        public FlipflopJK(string name = "Flipflop component") : base(name, 7) {
            
        }

        protected override void AllocatePins() {
            for(int i = 0; i < 5; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for(int i = 5; i < 7; i++) {
                Pins[i] = new Pin(this, true, false);
            }
            Qnot.Value = Pin.HIGH;
        }
        internal override bool CanExecute() {
            if(simulationId == circuit.SimulationId) return false;
            for(int i = 0; i < 3; i++) { //not needed to verify if S and R is connected
                if(Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// <para>Execute the JK Flipflop.</para>
        /// </summary>
        /// <example>
        /// JK flipflop truth table:
        /// <code>
        ///     Clock     J   K   S   R   Q  !Q   
        ///       -       -   -   1   0   1   0   
        ///       -       -   -   0   1   0   1   
        ///      desc     0   0   0   0   Q  !Q   
        ///      desc     1   0   0   0   1   0   
        ///      desc     0   1   0   0   0   1   
        ///      desc     1   1   0   0  !Q   Q   
        /// </code>
        /// </example>
        protected internal override void Execute() {
            base.Execute();
            if (S.GetDigital() == Pin.HIGH) { //S = 1
                Q.Value = Pin.HIGH;
                Qnot.Value = Pin.LOW;
            } else if (R.GetDigital() == Pin.HIGH) { // R = 1
                Q.Value = Pin.LOW;
                Qnot.Value = Pin.HIGH;
            } else if(CLK.Value == Pin.LOW && lastClk == Pin.HIGH) { //Clock desc
                 if (J.GetDigital() == Pin.HIGH) {
                    if(K.GetDigital() == Pin.HIGH) { // J = 1, K = 1
                        Q.Value = Q.Neg();
                        Qnot.Value = Qnot.Neg();
                    } else { // J = 1, K = 0
                        Q.Value = Pin.HIGH;
                        Qnot.Value = Pin.LOW;
                    }
                } else {
                    if(K.GetDigital() == Pin.HIGH) { // J = 0, K = 1
                        Q.Value = Pin.LOW;
                        Qnot.Value = Pin.HIGH;
                    } else { // J = 0, K = 0
                        //Do nothing, because the outputs stays same
                    }
                }
            }
            lastClk = CLK.Value;
            Q.Propagate();
            Qnot.Propagate();
        }
    }
}
