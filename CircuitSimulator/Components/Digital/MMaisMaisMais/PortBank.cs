﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class PortBank:Chip {
        public byte[] RegOut = new byte[4];
        private float lastClock = Pin.LOW;

        public void SetInput(int port, int pin, float value) {
            if (port < 0 || port > 3) throw new Exception("Porta inválida.");
            if (pin < 0 || pin > 8) throw new Exception("Pino inválido.");
            Pins[port * 8 + pin].Value = value;
        }
        public void SetInput(int port, byte value) {
            if (port < 0 || port > 3) throw new Exception("Porta inválida.");
            Pins[port * 8 + 0].Value = (value & 0x01) == 0x01 ? Pin.HIGH : Pin.LOW;
            Pins[port * 8 + 1].Value = (value & 0x02) == 0x02 ? Pin.HIGH : Pin.LOW;
            Pins[port * 8 + 2].Value = (value & 0x04) == 0x04 ? Pin.HIGH : Pin.LOW;
            Pins[port * 8 + 3].Value = (value & 0x08) == 0x08 ? Pin.HIGH : Pin.LOW;
            Pins[port * 8 + 4].Value = (value & 0x10) == 0x10 ? Pin.HIGH : Pin.LOW;
            Pins[port * 8 + 5].Value = (value & 0x20) == 0x20 ? Pin.HIGH : Pin.LOW;
            Pins[port * 8 + 6].Value = (value & 0x40) == 0x40 ? Pin.HIGH : Pin.LOW;
            Pins[port * 8 + 7].Value = (value & 0x80) == 0x80 ? Pin.HIGH : Pin.LOW;
        }
        public byte GetInput(int port) {
            if (port < 0 || port > 3) throw new Exception("Porta inválida.");
            byte val = 0;
            val += (byte)(Pins[port * 8 + 0].Value >= Pin.HALFCUT ? 1 : 0);
            val += (byte)(Pins[port * 8 + 1].Value >= Pin.HALFCUT ? 2 : 0);
            val += (byte)(Pins[port * 8 + 2].Value >= Pin.HALFCUT ? 4 : 0);
            val += (byte)(Pins[port * 8 + 3].Value >= Pin.HALFCUT ? 8 : 0);
            val += (byte)(Pins[port * 8 + 4].Value >= Pin.HALFCUT ? 16 : 0);
            val += (byte)(Pins[port * 8 + 5].Value >= Pin.HALFCUT ? 32 : 0);
            val += (byte)(Pins[port * 8 + 6].Value >= Pin.HALFCUT ? 64 : 0);
            val += (byte)(Pins[port * 8 + 7].Value >= Pin.HALFCUT ? 128 : 0);
            return val;
        }
        public float GetOutput(int port, int pin) {
            if (port < 0 || port > 3) throw new Exception("Porta inválida.");
            if (pin < 0 || pin > 8) throw new Exception("Pino inválido.");
            switch (pin) {
                case 0: return (RegOut[port] & 0x01) == 0x01 ? Pin.HIGH : Pin.LOW;
                case 1: return (RegOut[port] & 0x02) == 0x02 ? Pin.HIGH : Pin.LOW;
                case 2: return (RegOut[port] & 0x04) == 0x04 ? Pin.HIGH : Pin.LOW;
                case 3: return (RegOut[port] & 0x08) == 0x08 ? Pin.HIGH : Pin.LOW;
                case 4: return (RegOut[port] & 0x10) == 0x10 ? Pin.HIGH : Pin.LOW;
                case 5: return (RegOut[port] & 0x20) == 0x20 ? Pin.HIGH : Pin.LOW;
                case 6: return (RegOut[port] & 0x40) == 0x40 ? Pin.HIGH : Pin.LOW;
                case 7: return (RegOut[port] & 0x80) == 0x80 ? Pin.HIGH : Pin.LOW;
                default: return Pin.LOW;
            }
        }
        public byte GetOutput(int port) {
            if (port < 0 || port > 3) throw new Exception("Porta inválida.");
            return RegOut[port];
        }
        public PortBank(string name = "PortBank") : base(name, 85){

        }

        protected override void AllocatePins() {
            for (int i = 0; i < 32; i++) {
                Pins[i] = new Pin(this, true, false);
            }
            for (int i = 32; i < 45; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (int i = 45; i < 85; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            for (int i = 0; i < 5; i++) {
                if (Pins[8 * 5 + i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            if(Pins[8 * 5 + 3].Value >= Pin.HALFCUT) {
                for (int i = 0; i < 8; i++) {
                    if (Pins[8 * 4 + i].simulationId != circuit.SimulationId) {
                        return false;
                    }
                }
            }
            return true;
        }

        protected internal override void Execute() {
            simulationId = circuit.SimulationId;
            if (Pins[8 * 5 + 4].Value >= Pin.HALFCUT) {
                for (int i = 0; i < 4; i++) { //reseta
                    RegOut[i] = 0;
                }
            }
            if (Pins[8 * 5 + 3].Value >= Pin.HALFCUT && lastClock < Pin.HALFCUT) { //clock para output
                byte val = 0;
                val += (byte)(Pins[8 * 4 + 0].Value >= Pin.HALFCUT ? 1 : 0);
                val += (byte)(Pins[8 * 4 + 1].Value >= Pin.HALFCUT ? 2 : 0);
                val += (byte)(Pins[8 * 4 + 2].Value >= Pin.HALFCUT ? 4 : 0);
                val += (byte)(Pins[8 * 4 + 3].Value >= Pin.HALFCUT ? 8 : 0);
                val += (byte)(Pins[8 * 4 + 4].Value >= Pin.HALFCUT ? 16 : 0);
                val += (byte)(Pins[8 * 4 + 5].Value >= Pin.HALFCUT ? 32 : 0);
                val += (byte)(Pins[8 * 4 + 6].Value >= Pin.HALFCUT ? 64 : 0);
                val += (byte)(Pins[8 * 4 + 7].Value >= Pin.HALFCUT ? 128 : 0);
                if (Pins[8 * 5 + 1].Value < Pin.HALFCUT && Pins[8 * 5 + 0].Value < Pin.HALFCUT) RegOut[0] = val;
                if (Pins[8 * 5 + 1].Value >= Pin.HALFCUT && Pins[8 * 5 + 0].Value < Pin.HALFCUT) RegOut[1] = val;
                if (Pins[8 * 5 + 1].Value < Pin.HALFCUT && Pins[8 * 5 + 0].Value >= Pin.HALFCUT) RegOut[2] = val;
                if (Pins[8 * 5 + 1].Value >= Pin.HALFCUT && Pins[8 * 5 + 0].Value >= Pin.HALFCUT) RegOut[3] = val;
            }
            lastClock = Pins[8 * 5 + 3].Value;
            if (Pins[8 * 5 + 2].Value >= Pin.HALFCUT) { //joga input pro bus

                byte val = 0;
                if (Pins[8 * 5 + 1].Value < Pin.HALFCUT && Pins[8 * 5 + 0].Value < Pin.HALFCUT) val = GetInput(0);
                if (Pins[8 * 5 + 1].Value >= Pin.HALFCUT && Pins[8 * 5 + 0].Value < Pin.HALFCUT) val = GetInput(1);
                if (Pins[8 * 5 + 1].Value < Pin.HALFCUT && Pins[8 * 5 + 0].Value >= Pin.HALFCUT) val = GetInput(2);
                if (Pins[8 * 5 + 1].Value >= Pin.HALFCUT && Pins[8 * 5 + 0].Value >= Pin.HALFCUT) val = GetInput(3);

                Pins[8 * 5 + 5 + 0].Value = (val & 0x01) == 0x00 ? Pin.LOW : Pin.HIGH;
                Pins[8 * 5 + 5 + 1].Value = (val & 0x02) == 0x00 ? Pin.LOW : Pin.HIGH;
                Pins[8 * 5 + 5 + 2].Value = (val & 0x04) == 0x00 ? Pin.LOW : Pin.HIGH;
                Pins[8 * 5 + 5 + 3].Value = (val & 0x08) == 0x00 ? Pin.LOW : Pin.HIGH;
                Pins[8 * 5 + 5 + 4].Value = (val & 0x10) == 0x00 ? Pin.LOW : Pin.HIGH;
                Pins[8 * 5 + 5 + 5].Value = (val & 0x20) == 0x00 ? Pin.LOW : Pin.HIGH;
                Pins[8 * 5 + 5 + 6].Value = (val & 0x40) == 0x00 ? Pin.LOW : Pin.HIGH;
                Pins[8 * 5 + 5 + 7].Value = (val & 0x80) == 0x00 ? Pin.LOW : Pin.HIGH;

                for (int i = 0; i < 8; i++) {
                    if (Pins[8 * 5 + 1].Value < Pin.HALFCUT && Pins[8 * 5 + 0].Value < Pin.HALFCUT) {
                        Pins[8 * 5 + 5 + i].Value = Pins[8 * 0 + i].Value;
                    } else if (Pins[8 * 5 + 1].Value >= Pin.HALFCUT && Pins[8 * 5 + 0].Value < Pin.HALFCUT) {
                        Pins[8 * 5 + 5 + i].Value = Pins[8 * 1 + i].Value;
                    } else if (Pins[8 * 5 + 1].Value < Pin.HALFCUT && Pins[8 * 5 + 0].Value >= Pin.HALFCUT) {
                        Pins[8 * 5 + 5 + i].Value = Pins[8 * 2 + i].Value;
                    } else if (Pins[8 * 5 + 1].Value >= Pin.HALFCUT && Pins[8 * 5 + 0].Value >= Pin.HALFCUT) {
                        Pins[8 * 5 + 5 + i].Value = Pins[8 * 3 + i].Value;
                    }
                }


                for (int i = 0; i < 8; i++) {
                    Pins[8 * 5 + 5 + i].simulationId = circuit.SimulationId;

                    Pins[8 * 5 + 5 + i].Propagate();    //outbus
                }
            }
            for (int port = 0; port < 4; port++) {
                Pins[(8 * 6 + 5) + port * 8 + 0].Value = (RegOut[port] & 0x01) == 0x00 ? Pin.LOW : Pin.HIGH;
                Pins[(8 * 6 + 5) + port * 8 + 1].Value = (RegOut[port] & 0x02) == 0x00 ? Pin.LOW : Pin.HIGH;
                Pins[(8 * 6 + 5) + port * 8 + 2].Value = (RegOut[port] & 0x04) == 0x00 ? Pin.LOW : Pin.HIGH;
                Pins[(8 * 6 + 5) + port * 8 + 3].Value = (RegOut[port] & 0x08) == 0x00 ? Pin.LOW : Pin.HIGH;
                Pins[(8 * 6 + 5) + port * 8 + 4].Value = (RegOut[port] & 0x10) == 0x00 ? Pin.LOW : Pin.HIGH;
                Pins[(8 * 6 + 5) + port * 8 + 5].Value = (RegOut[port] & 0x20) == 0x00 ? Pin.LOW : Pin.HIGH;
                Pins[(8 * 6 + 5) + port * 8 + 6].Value = (RegOut[port] & 0x40) == 0x00 ? Pin.LOW : Pin.HIGH;
                Pins[(8 * 6 + 5) + port * 8 + 7].Value = (RegOut[port] & 0x80) == 0x00 ? Pin.LOW : Pin.HIGH;
            }
            for (int i = 0; i < 32; i++) {
                Pins[i].simulationId = circuit.SimulationId;
                Pins[8 * 6 + 5 + i].simulationId = circuit.SimulationId;

                Pins[i].Propagate();    //in
                Pins[8 * 6 + 5 + i].Propagate();        //out
            }
        }
    }
    
}