﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator.Components.Digital.MMaisMaisMais {
    public class Disable8Bit : Chip {
        public Disable8Bit(string name = "Disable8Bit") : base(name, 17) {

        }

        protected override void AllocatePins() {
            for (int i = 0; i < 9; i++) {
                Pins[i] = new Pin(this, false, false);
            }
            for (int i = 9; i < 17; i++) {
                Pins[i] = new Pin(this, true, false);
            }
        }
        internal override bool CanExecute() {
            if (simulationId == circuit.SimulationId) return false;
            if (Pins[8].simulationId != circuit.SimulationId) {
                return false;
            }
            if (Pins[8].value >= Pin.HALFCUT) {
                for (int i = 0; i < 8; i++) {
                    if (Pins[i].simulationId != circuit.SimulationId) {
                        return false;
                    }
                }
            }
            return true;
        }

        protected internal override void Execute() {
            base.Execute();
            if (Pins[8].value >= Pin.HALFCUT) {
                for (int i = 0; i < 8; i++) {
                    Pins[i + 9].value = Pins[i].value;
                    Pins[i + 9].Propagate();
                }
            }
        }
    }

}
