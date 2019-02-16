namespace CircuitSimulator.Components.Digital {
    public class BinTo7Seg:Chip {
        /// <summary>
        /// Less dignificant bit
        /// </summary>
        public Pin A => Pins[0];

        public Pin B => Pins[1];
        public Pin C => Pins[2];

        /// <summary>
        /// Most significant bit
        /// </summary>
        public Pin D => Pins[3];

        /// <summary>
        /// The enable pin
        /// </summary>
        public Pin Enable => Pins[4];

        public Pin OutA => Pins[5];
        public Pin OutB => Pins[6];
        public Pin OutC => Pins[7];
        public Pin OutD => Pins[8];
        public Pin OutE => Pins[9];
        public Pin OutF => Pins[10];
        public Pin OutG => Pins[11];
        public bool IsHexadecimalValid = true;

        private static readonly float[,] table = new float[,] {
            {Pin.High, Pin.High, Pin.High, Pin.High, Pin.High, Pin.High, Pin.Low}, //0
            {Pin.Low, Pin.High, Pin.High, Pin.Low, Pin.Low, Pin.Low, Pin.Low}, //1
            {Pin.High, Pin.High, Pin.Low, Pin.High, Pin.High, Pin.Low, Pin.High}, //2
            {Pin.High, Pin.High, Pin.High, Pin.High, Pin.Low, Pin.Low, Pin.High}, //3
            {Pin.Low, Pin.High, Pin.High, Pin.Low, Pin.Low, Pin.High, Pin.High}, //4
            {Pin.High, Pin.Low, Pin.High, Pin.High, Pin.Low, Pin.High, Pin.High}, //5
            {Pin.High, Pin.Low, Pin.High, Pin.High, Pin.High, Pin.High, Pin.High}, //6
            {Pin.High, Pin.High, Pin.High, Pin.Low, Pin.Low, Pin.Low, Pin.Low}, //7
            {Pin.High, Pin.High, Pin.High, Pin.High, Pin.High, Pin.High, Pin.High}, //8
            {Pin.High, Pin.High, Pin.High, Pin.High, Pin.Low, Pin.High, Pin.High}, //9
            {Pin.High, Pin.High, Pin.High, Pin.Low, Pin.High, Pin.High, Pin.High}, //A
            {Pin.Low, Pin.Low, Pin.High, Pin.High, Pin.High, Pin.High, Pin.High}, //B
            {Pin.High, Pin.Low, Pin.Low, Pin.High, Pin.High, Pin.High, Pin.Low}, //C
            {Pin.Low, Pin.High, Pin.High, Pin.High, Pin.High, Pin.Low, Pin.High}, //D
            {Pin.High, Pin.Low, Pin.Low, Pin.High, Pin.High, Pin.High, Pin.High}, //E
            {Pin.High, Pin.Low, Pin.Low, Pin.Low, Pin.High, Pin.High, Pin.High}, //F
            {Pin.Low, Pin.Low, Pin.Low, Pin.Low, Pin.Low, Pin.Low, Pin.Low}, //clean
        };



        public BinTo7Seg(string name = "BinTo7Seg") : base(name, 12) {

        }
        protected override void AllocatePins() {
            for(var i = 0; i < Pins.Length; i++) {
                if(i<4)
                    Pins[i] = new Pin(this, false, false); //inputs
                else if(i == 4)
                    Pins[i] = new Pin(this, false, false, Pin.High); //enable pin
                else
                    Pins[i] = new Pin(this, true, false); //outputs
            }
        }
        internal override bool CanExecute() {
            if(SimulationIdInternal == Circuit.SimulationId) return false;
            for(var i = 0; i < 4; i++) { //not needed to verify if Enable is connected
                if(Pins[i].SimulationIdInternal != Circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }
        protected internal override void Execute() {
            base.Execute();
            if(Enable.GetDigital() == Pin.Low) {
                ToOutput(16);
            } else {
                var value = 0;
                if(D.GetDigital() == Pin.High)
                    value += 8;
                if(C.GetDigital() == Pin.High)
                    value += 4;
                if(B.GetDigital() == Pin.High)
                    value += 2;
                if(A.GetDigital() == Pin.High)
                    value += 1;
                if(!IsHexadecimalValid) {
                    if(value >= 9)
                        value = 16;
                }

                ToOutput(value);
            }
            
            for(var i = 5; i < Pins.Length; i++) {
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
                OutA.IsOpenInternal = true;
                OutB.IsOpenInternal = true;
                OutC.IsOpenInternal = true;
                OutD.IsOpenInternal = true;
                OutE.IsOpenInternal = true;
                OutF.IsOpenInternal = true;
                OutG.IsOpenInternal = true;
            } else {
                OutA.IsOpenInternal = false;
                OutB.IsOpenInternal = false;
                OutC.IsOpenInternal = false;
                OutD.IsOpenInternal = false;
                OutE.IsOpenInternal = false;
                OutF.IsOpenInternal = false;
                OutG.IsOpenInternal = false;
            }
            OutA.Value = table[value, 0];
            OutB.Value = table[value, 1];
            OutC.Value = table[value, 2];
            OutD.Value = table[value, 3];
            OutE.Value = table[value, 4];
            OutF.Value = table[value, 5];
            OutG.Value = table[value, 6];
        }

    }
}
