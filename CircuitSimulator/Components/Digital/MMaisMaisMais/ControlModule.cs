using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    class ControlModule : Chip {
        public Pin In0 { get { return Pins[0]; } }
        public Pin In1 { get { return Pins[1]; } }
        public Pin In2 { get { return Pins[2]; } }
        public Pin In3 { get { return Pins[3]; } }
        public Pin In4 { get { return Pins[4]; } }
        public Pin In5 { get { return Pins[5]; } }
        public Pin In6 { get { return Pins[6]; } }
        public Pin In7 { get { return Pins[7]; } }
        public Pin Clock { get { return Pins[8]; } }
        public Pin FlagInCarry { get { return Pins[9]; } }
        public Pin FlagInZero { get { return Pins[10]; } }
        public Pin EOI { get { return Pins[11]; } }
        public Pin ROMrd { get { return Pins[12]; } }
        public Pin ROMcs { get { return Pins[13]; } }
        public Pin PCHbus { get { return Pins[14]; } }
        public Pin PCLbus { get { return Pins[15]; } }
        public Pin PCHclock { get { return Pins[16]; } }
        public Pin PCLclock { get { return Pins[17]; } }
        public Pin DataPCsel { get { return Pins[18]; } }
        public Pin DIRclock { get { return Pins[19]; } }
        public Pin SPclock { get { return Pins[20]; } }
        public Pin SPIncDec { get { return Pins[21]; } }
        public Pin SPsel { get { return Pins[22]; } }
        public Pin SPen { get { return Pins[23]; } }
        public Pin Reset { get { return Pins[24]; } }
        public Pin ULAbus { get { return Pins[25]; } }
        public Pin BUFclock { get { return Pins[26]; } }
        public Pin ACbus { get { return Pins[27]; } }
        public Pin ACclock { get { return Pins[28]; } }
        public Pin RGbus { get { return Pins[29]; } }
        public Pin RGPCclock { get { return Pins[30]; } }
        public Pin RAMrd { get { return Pins[31]; } }
        public Pin RAMwr { get { return Pins[32]; } }
        public Pin RAMcs { get { return Pins[33]; } }
        public Pin INbus { get { return Pins[34]; } }
        public Pin OUTclock { get { return Pins[35]; } }
        public Pin ULAOPsel0 { get { return Pins[36]; } }
        public Pin ULAOPsel1 { get { return Pins[37]; } }
        public Pin ULAOPsel2 { get { return Pins[38]; } }
        public Pin RGPBsel0 { get { return Pins[39]; } }
        public Pin RGPBsel1 { get { return Pins[40]; } }

        public static MicrocontrollerData MicrocontrollerData;

        private int HDCounter = 0;
        private byte regAddress = 0;
        private byte RegAddress  { get{ return regAddress; } set {
                regAddress = value;
                regAddressInc = (byte)(regAddress + 1);
            } }
        private byte regAddressInc = 0;
        private byte RI = 0;
        private byte[] InputData;
        private int indexInstruction;
        private bool currentClock = false;
        private bool lastClock = false;

        public ControlModule(string name = "ControlModule") : base(name, 41) {
            canStart = true;
            internalMicroInstructions = new Dictionary<string, bool>();
            MicrocontrollerData = new MicrocontrollerData();
        }
        protected override void AllocatePins() {
            for (int i = 0; i < Pins.Length; i++) {
                if (i < 11)
                    Pins[i] = new Pin(this, false, false); //inputs
                else
                    Pins[i] = new Pin(this, true, false); //outputs
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            return true;
        }
        protected internal override void Execute() {
            base.Execute();
            float halfCut = (Pin.HIGH - Pin.LOW) / 2f;

            if (Reset.value >= halfCut) RegAddress = 0;
            currentClock = Clock.value >= halfCut;
            if (currentClock == true && lastClock == false) {
                if (internalMicroInstructions.ContainsKey("SelRI")) {
                    if (internalMicroInstructions["SelRI"]) {
                        RegAddress = DecodificadorDeIntrucao();
                    } else {
                        RegAddress = regAddressInc;
                    }
                } else {
                    RegAddress = 0;
                }
                ResetBools();
                ProcessBools();
            }
            lastClock = currentClock;
            EOI.SetDigital(RegAddress == 0 ? Pin.HIGH : Pin.LOW);
            
                
        }




        private void ResetBools() {
            for (int i = 0; i < 32; i++) {
                ProcessBool(i, false, false);
            }
        }
        private void ProcessBools() {
            int address = regAddress;
            for (int i = 0; i < 32; i++) {
                ProcessBool(i, MicroInstructions[address, i], false);
            }
            for (int i = 0; i < 32; i++) {
                ProcessBool(i, MicroInstructions[address, i], true);
            }
        }
        private void ProcessBool(int index, bool value, bool doAction) {
            string key;
            float halfCut = (Pin.HIGH - Pin.LOW) / 2f;
            switch (index) {
                case 0:
                    key = "SPIncDec";
                    if (doAction) {
                        SPIncDec.SetDigital(value ? Pin.HIGH : Pin.LOW);
                    }
                    break;
                case 1:
                    key = "SP clock";
                    if (doAction) {
                        SPclock.SetDigital(value ? Pin.HIGH : Pin.LOW);
                        SPen.SetDigital(value ? Pin.HIGH : Pin.LOW);
                    }
                    break;
                case 2:
                    key = "DIRClock";
                    if (doAction) {
                        DIRclock.SetDigital(value ? Pin.HIGH : Pin.LOW);
                    }
                    break;
                case 3:
                    key = "SelDataPC";
                    if (doAction) {
                        DataPCsel.SetDigital(value ? Pin.HIGH : Pin.LOW);
                    }
                    break;
                case 4:
                    key = "Clock PCL";
                    if (doAction) {
                        if (!value) {
                            PCLclock.SetDigital(Pin.LOW);
                            break;
                        }
                        if (!internalMicroInstructions["CresC"] || !internalMicroInstructions["CresZ"]) {
                            PCLclock.SetDigital(Pin.HIGH);
                            break;
                        }

                        if (FlagInCarry.Value >= halfCut && internalMicroInstructions["CresC"]) {
                            PCLclock.SetDigital(Pin.HIGH);
                            break;
                        }
                        if (FlagInZero.Value >= halfCut && internalMicroInstructions["CresZ"]) {
                            PCLclock.SetDigital(Pin.HIGH);
                            break;
                        }
                        PCLclock.SetDigital(Pin.LOW);
                    }
                    break;
                case 5:
                    key = "Clock PCH";
                    if (doAction) {
                        if (!value) {
                            PCHclock.SetDigital(Pin.LOW);
                            break;
                        }
                        if (!internalMicroInstructions["CresC"] || !internalMicroInstructions["CresZ"]) {
                            PCHclock.SetDigital(Pin.HIGH);
                            break;
                        }

                        if (FlagInCarry.Value >= halfCut && internalMicroInstructions["CresC"]) {
                            PCHclock.SetDigital(Pin.HIGH);
                            break;
                        }
                        if (FlagInZero.Value >= halfCut && internalMicroInstructions["CresZ"]) {
                            PCHclock.SetDigital(Pin.HIGH);
                            break;
                        }
                        PCHclock.SetDigital(Pin.LOW);
                    }
                    break;
                case 6:
                    key = "PCL Bus";
                    if (doAction) {
                        PCLbus.SetDigital(value ? Pin.HIGH : Pin.LOW);
                    }
                    break;
                case 7:
                    key = "PCH Bus";
                    if (doAction) {
                        PCHbus.SetDigital(value ? Pin.HIGH : Pin.LOW);
                    }
                    break;
                case 8:
                    key = "ROM cs";
                    if (doAction) {
                        ROMcs.SetDigital(value ? Pin.HIGH : Pin.LOW);
                    }
                    break;
                case 9:
                    key = "ROM rd";
                    if (doAction) {
                        ROMrd.SetDigital(value ? Pin.HIGH : Pin.LOW);
                    }
                    break;
                case 10:
                    key = "HD clock";
                    if (doAction) {
                        if (value)
                            ++HDCounter;
                    }
                    break;
                case 11:
                    key = "SelRI";
                    if (doAction) {
                        if (value) {
                            ClockRI();
                            RegAddress = DecodificadorDeIntrucao();
                        }
                    }
                    break;
                case 12:
                    key = "RIClock";
                    if (doAction) {
                        if (value) {
                            ClockRI();
                            RegAddress = DecodificadorDeIntrucao();
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
                        if(value)
                            HDCounter = 0;
                    }
                    break;
                case 19:
                    key = "HD";
                    break;
                case 20:
                    key = "Out clock";
                    break;
                case 21:
                    key = "IN Bus";
                    break;
                case 22:
                    key = "RAM cs";
                    break;
                case 23:
                    key = "Ram wr";
                    break;
                case 24:
                    key = "RAM rd";
                    break;
                case 25:
                    key = "Reg Clock";
                    break;
                case 26:
                    key = "Reg Bus";
                    break;
                case 27:
                    key = "AC Clock";
                    break;
                case 28:
                    key = "AC Bus";
                    break;
                case 29:
                    key = "BUF Clock";
                    break;
                case 30:
                    key = "ULA Bus";
                    break;
                case 31:
                    key = "Sel SP";
                    break;
                default:
                    key = "null";
                    break;
            }
            if (internalMicroInstructions.ContainsKey(key))
                internalMicroInstructions.Remove(key);
            internalMicroInstructions.Add(key, value);
        }
        private byte DecodificadorDeIntrucao() {
            int address = 0;
            address += (RI & 128) == 1 ? 1 : 0;
            address += (RI & 64) == 1 ? 2 : 0;
            address += (RI & 32) == 1 ? 4 : 0;
            address += HDCounter*8;
            return Addresses[address];
        }
        private void ClockRI() {
            RI = InputData[indexInstruction];
            ULAOPsel2.SetDigital((RI & 1) == 1 ? Pin.HIGH : Pin.LOW);
            ULAOPsel1.SetDigital((RI & 2) == 1 ? Pin.HIGH : Pin.LOW);
            ULAOPsel0.SetDigital((RI & 4) == 1 ? Pin.HIGH : Pin.LOW);
            RGPBsel1.SetDigital((RI & 8) == 1 ? Pin.HIGH : Pin.LOW);
            RGPBsel0.SetDigital((RI & 16) == 1 ? Pin.HIGH : Pin.LOW);
        }
        private void SetInputData(byte[] value) {
            InputData = value;
        }

        private Dictionary<string, bool> internalMicroInstructions;
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
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, true, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, true, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, true, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, true, true, false, false, false, false, false, true, true, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, true, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
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
            {false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true},
            {true, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, true},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, true},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, true, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, true, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true},
            {true, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, true},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true},
            {false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, true},
            {false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, true, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, true, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
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
            {false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, true, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false},
            {false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false},
            {false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, true, true, false, false, false, false, false, false, true, false},
            {true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, true, true, false, false, true, false, false, false, false, true},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, true, false, false, false, false, false, true},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, true, true, false, false, false, false, true, false, false, true},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, true},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, false, false, true, false, false, false, true},
            {false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
        };
    }

    public class MicrocontrollerData {
        public int NextInstruction;
        public bool Flag_C;
        public bool Flag_Z;
        public byte[] Reg; //0:A, 1:B, 2:C, 3:D, 4:E
        public byte[] In; //0: IN4, 1: IN1, 2: IN2, 3: IN3
        public byte[] Out; //0: OUT4, 1: OUT1, 2: OUT2, 3: OUT3
        public byte[] RAM;
        public byte[] Stack;
        public byte PointerStack;
    }
}
