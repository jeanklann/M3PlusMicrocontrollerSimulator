namespace CircuitSimulator.Components.Digital {
    public class Display7Seg:Component {
        public Pin A => Pins[0];
        public Pin B => Pins[1];
        public Pin C => Pins[2];
        public Pin D => Pins[3];
        public Pin E => Pins[4];
        public Pin F => Pins[5];
        public Pin G => Pins[6];
        public Pin Dot => Pins[7];

        public Display7Seg(string name = "7 Segment Display"):base(name,8) {

        }
    }
}
