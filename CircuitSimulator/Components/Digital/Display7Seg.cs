using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital {
    public class Display7Seg:Component {
        public Pin a { get { return Pins[0]; } }
        public Pin b { get { return Pins[1]; } }
        public Pin c { get { return Pins[2]; } }
        public Pin d { get { return Pins[3]; } }
        public Pin e { get { return Pins[4]; } }
        public Pin f { get { return Pins[5]; } }
        public Pin g { get { return Pins[6]; } }
        public Pin Dot { get { return Pins[7]; } }
        
        public Display7Seg(string name = "7 Segment Display"):base(name,8) {

        }
        protected internal override void Execute() {
            base.Execute();
        }
    }
}
