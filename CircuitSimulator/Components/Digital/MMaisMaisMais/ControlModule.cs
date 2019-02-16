using System.Collections.Generic;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class ControlModule : Chip {
        public Pin In0 => Pins[0];
        public Pin In1 => Pins[1];
        public Pin In2 => Pins[2];
        public Pin In3 => Pins[3];
        public Pin In4 => Pins[4];
        public Pin In5 => Pins[5];
        public Pin In6 => Pins[6];
        public Pin In7 => Pins[7];
        public Pin Clock => Pins[8];
        public Pin FlagInCarry => Pins[9];
        public Pin FlagInZero => Pins[10];
        public Pin Eoi => Pins[11];
        public Pin RoMrd => Pins[12];
        public Pin RoMcs => Pins[13];
        public Pin PcHbus => Pins[14];
        public Pin PcLbus => Pins[15];
        public Pin PcHclock => Pins[16];
        public Pin PcLclock => Pins[17];
        public Pin DataPCsel => Pins[18];
        public Pin DiRclock => Pins[19];
        public Pin SPclock => Pins[20];
        public Pin SpIncDec => Pins[21];
        public Pin SPsel => Pins[22];
        public Pin SPen => Pins[23];
        public Pin Reset => Pins[24];
        public Pin UlAbus => Pins[25];
        public Pin BuFclock => Pins[26];
        public Pin ACbus => Pins[27];
        public Pin ACclock => Pins[28];
        public Pin RGbus => Pins[29];
        public Pin RgpCclock => Pins[30];
        public Pin RaMrd => Pins[31];
        public Pin RaMwr => Pins[32];
        public Pin RaMcs => Pins[33];
        public Pin Nbus => Pins[34];
        public Pin OuTclock => Pins[35];
        public Pin UlaoPsel0 => Pins[36];
        public Pin UlaoPsel1 => Pins[37];
        public Pin UlaoPsel2 => Pins[38];
        public Pin RgpBsel0 => Pins[39];
        public Pin RgpBsel1 => Pins[40];

        public bool NeedSet = true;
        private float _lastEoi = Pin.Low;
        private int _hdCounter;
        private byte _regAddress;
        private byte RegAddress  { get => _regAddress;
            set {
                _regAddress = value;
                _regAddressInc = (byte)(_regAddress + 1);
            } }
        private byte _regAddressInc;
        private byte _ri;
        private bool _currentClock;
        private bool _lastClock;

        public int LowFrequencyIteraction = 0;

        public ControlModule(string name = "ControlModule") : base(name, 41) {
            CanStart = true;
            _internalMicroInstructions = new Dictionary<string, bool>();
        }
        protected override void AllocatePins() {
            for (var i = 0; i < Pins.Length; i++) {
                //Pins[i] = new Pin(this, true, false); //outputs
                
                if (i < 11)
                    Pins[i] = new Pin(this, false, false); //inputs
                else
                    Pins[i] = new Pin(this, true, false); //outputs
            }
        }
        internal override bool CanExecute() {
            if (SimulationIdInternal == Circuit.SimulationId) return false;
            return true;
        }
        protected internal override void Execute() {
            base.Execute();

            if (Reset.Value >= Pin.Halfcut) RegAddress = 0;
            _currentClock = Clock.Value >= Pin.Halfcut;
            if (_currentClock == true && _lastClock == false) {
                if (_internalMicroInstructions.ContainsKey("SelRI")) {
                    if (_internalMicroInstructions["SelRI"]) {
                        RegAddress = DecodificadorDeIntrucao();
                    } else {
                        RegAddress = _regAddressInc;
                    }
                } else {
                    RegAddress = 0;
                }
            }
            ResetBools();
            ProcessBools();
            _lastClock = _currentClock;
            Eoi.SetDigital(RegAddress == 0 ? Pin.High : Pin.Low);
            if(Eoi.Value >= Pin.Halfcut && _lastEoi <= Pin.Halfcut) {
                NeedSet = true;
            }
            _lastEoi = Eoi.Value;
            /*
            for (int i = 11; i < Pins.Length; i++) {
                Pins[i].value = Pin.LOW;
            }
            ULAOPsel0.value = Pin.HIGH;
            ULAOPsel1.value = Pin.HIGH;
            ULAOPsel2.value = Pin.HIGH;
            ULAbus.value = Pin.HIGH;
            */
            for (var i = 11; i < Pins.Length; i++) {
                Pins[i].Propagate();
            }
            Clock.Propagate();

        }




        private void ResetBools() {
            for (var i = 0; i < 32; i++) {
                ProcessBool(i, false, false);
            }
        }
        private void ProcessBools() {
            int address = RegAddress;
            for (var i = 0; i < 32; i++) {
                ProcessBool(i, MicroInstructions[address, i], false);
            }
            for (var i = 0; i < 32; i++) {
                ProcessBool(i, MicroInstructions[address, i], true);
            }
        }
        private void ProcessBool(int index, bool value, bool doAction) {
            string key;
            var clockSubida = _currentClock == true && _lastClock == false;
            switch (index) {
                case 0:
                    key = "SPIncDec";
                    if (doAction) {
                        SpIncDec.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 1:
                    key = "SP clock";
                    if (doAction) {
                        SPclock.SetDigital(value ? Pin.High : Pin.Low);
                        SPen.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 2:
                    key = "DIRClock";
                    if (doAction) {
                        DiRclock.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 3:
                    key = "SelDataPC";
                    if (doAction) {
                        DataPCsel.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 4:
                    key = "Clock PCL";
                    if (doAction) {
                        if (!value) {
                            PcLclock.SetDigital(Pin.Low);
                            break;
                        }
                        if (!_internalMicroInstructions["CresC"] && !_internalMicroInstructions["CresZ"]) {
                            PcLclock.SetDigital(Pin.High);
                            break;
                        }

                        if (FlagInCarry.Value >= Pin.Halfcut && _internalMicroInstructions["CresC"]) {
                            PcLclock.SetDigital(Pin.High);
                            break;
                        }
                        if (FlagInZero.Value >= Pin.Halfcut && _internalMicroInstructions["CresZ"]) {
                            PcLclock.SetDigital(Pin.High);
                            break;
                        }
                        PcLclock.SetDigital(Pin.Low);
                    }
                    break;
                case 5:
                    key = "Clock PCH";
                    if (doAction) {
                        if (!value) {
                            PcHclock.SetDigital(Pin.Low);
                            break;
                        }
                        if (!_internalMicroInstructions["CresC"] && !_internalMicroInstructions["CresZ"]) {
                            PcHclock.SetDigital(Pin.High);
                            break;
                        }

                        if (FlagInCarry.Value >= Pin.Halfcut && _internalMicroInstructions["CresC"]) {
                            PcHclock.SetDigital(Pin.High);
                            break;
                        }
                        if (FlagInZero.Value >= Pin.Halfcut && _internalMicroInstructions["CresZ"]) {
                            PcHclock.SetDigital(Pin.High);
                            break;
                        }
                        PcHclock.SetDigital(Pin.Low);
                    }
                    break;
                case 6:
                    key = "PCL Bus";
                    if (doAction) {
                        PcLbus.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 7:
                    key = "PCH Bus";
                    if (doAction) {
                        PcHbus.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 8:
                    key = "ROM cs";
                    if (doAction) {
                        RoMcs.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 9:
                    key = "ROM rd";
                    if (doAction) {
                        RoMrd.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 10:
                    key = "HD clock";
                    if (doAction) {
                        if (clockSubida) {
                            if (value)
                                ++_hdCounter;
                        }
                    }
                    break;
                case 11:
                    key = "SelRI";
                    if (doAction) {
                        if (value) {
                            if (clockSubida) {
                                ClockRi();
                            }
                            
                        }
                    }
                    break;
                case 12:
                    key = "RIClock";
                    if (doAction) {
                        if (value) {
                            if (clockSubida) {
                                ClockRi();
                            }
                        }
                    }
                    break;
                case 13:
                    key = "CresZ";
                    break;
                case 14:
                    key = "CresC";
                    break;
                case 15:
                    key = "Cres";
                    if (doAction) {
                        if(value)
                            RegAddress = 0;
                    }
                    break;
                case 18:
                    key = "HD res";
                    if (doAction) {
                        if (value)
                            _hdCounter = 0;
                    }
                    break;
                case 19:
                    key = "HD";
                    break;
                case 20:
                    key = "Out clock";
                    if (doAction) {
                        OuTclock.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 21:
                    key = "IN Bus";
                    if (doAction) {
                        Nbus.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 22:
                    key = "RAM cs";
                    if (doAction) {
                        RaMcs.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 23:
                    key = "Ram wr";
                    if (doAction) {
                        RaMwr.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 24:
                    key = "RAM rd";
                    if (doAction) {
                        RaMrd.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 25:
                    key = "Reg Clock";
                    if (doAction) {
                        RgpCclock.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 26:
                    key = "Reg Bus";
                    if (doAction) {
                        RGbus.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 27:
                    key = "AC Clock";
                    if (doAction) {
                        ACclock.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 28:
                    key = "AC Bus";
                    if (doAction) {
                        ACbus.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 29:
                    key = "BUF Clock";
                    if (doAction) {
                        BuFclock.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 30:
                    key = "ULA Bus";
                    if (doAction) {
                        UlAbus.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                case 31:
                    key = "Sel SP";
                    if (doAction) {
                        SPsel.SetDigital(value ? Pin.High : Pin.Low);
                    }
                    break;
                default:
                    key = "null";
                    break;
            }
            if (_internalMicroInstructions.ContainsKey(key))
                _internalMicroInstructions.Remove(key);
            _internalMicroInstructions.Add(key, value);
        }
        private byte DecodificadorDeIntrucao() {
            var address = 0;
            address += (_ri & 1);
            address += (_ri & 2);
            address += (_ri & 4);
            address += _hdCounter*8;
            return Addresses[address];
        }
        private void ClockRi() {
            _ri = 0;
            _ri += (byte)(Pins[0].Value >= Pin.Halfcut ? 1 : 0);
            _ri += (byte)(Pins[1].Value >= Pin.Halfcut ? 2 : 0);
            _ri += (byte)(Pins[2].Value >= Pin.Halfcut ? 4 : 0);
            _ri += (byte)(Pins[3].Value >= Pin.Halfcut ? 8 : 0);
            _ri += (byte)(Pins[4].Value >= Pin.Halfcut ? 16 : 0);
            _ri += (byte)(Pins[5].Value >= Pin.Halfcut ? 32 : 0);
            _ri += (byte)(Pins[6].Value >= Pin.Halfcut ? 64 : 0);
            _ri += (byte)(Pins[7].Value >= Pin.Halfcut ? 128 : 0);

            UlaoPsel2.SetDigital((_ri & 128) == 128 ? Pin.High : Pin.Low);
            UlaoPsel1.SetDigital((_ri & 64) == 64 ? Pin.High : Pin.Low);
            UlaoPsel0.SetDigital((_ri & 32) == 32 ? Pin.High : Pin.Low);
            RgpBsel1.SetDigital((_ri & 16) == 16 ? Pin.High : Pin.Low);
            RgpBsel0.SetDigital((_ri & 8) == 8 ? Pin.High : Pin.Low);
        }

        private Dictionary<string, bool> _internalMicroInstructions;
        public static readonly byte[] Addresses = new byte[] { //The pointers to the MicroInstructions
            0x03, 0x08, 0x0d, 0x14, 0x19, 0x1e, 0x25, 0x2a, 0x2c, 0x31, 0x36, 0x3d, 0x49, 0x56, 0x63, 0x2a,
            0x7f, 0x84, 0x8b, 0x91, 0x94, 0x98, 0x9b, 0x2a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a,
        };
        public static readonly bool[,] MicroInstructions = new bool[,] { //The result of analyzing the internal M+++
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, true, true, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, true, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, true, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, true, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, true, true, false, false, false, false, false, true, true, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, true, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true},
            {true, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, true},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, true},
            {false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true},
            {true, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, true},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, true},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, true, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, false, true, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true},
            {true, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, true},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, true},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, true, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, false, true, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true},
            {true, true, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, true},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, true},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true},
            {true, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, true},
            {true, true, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true},
            {false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, true},
            {true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false},
            {false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false},
            {false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, true, true, false, false, false, false, false, false, true, false},
            {true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, true, true, false, false, true, false, false, false, false, true},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, true, false, false, false, false, false, true},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, true, true, false, false, false, false, true, false, false, true},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, true, false, false, false, true},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
        };
    }
}
