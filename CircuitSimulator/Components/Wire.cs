using System;

namespace CircuitSimulator.Components
{
    public class Wire : Component
    {
        public Wire(string name = "Wire") : base(name, 2)
        {
        }

        internal override bool CanExecute()
        {
            if (SimulationIdInternal == Circuit.SimulationId) return false;
            /*
            for (int i = 0; i < Pins.Length; i++) {
                if (Pins[i].simulationId == circuit.SimulationId) {
                    return false;
                }
            }*/
            return true;
        }

        protected internal override void Execute()
        {
            var index = -1;
            for (var i = 0; i < Pins.Length; i++)
            {
                if (Pins[i].SimulationIdInternal != Circuit.SimulationId) continue;
                index = i;
                break;
            }

            if (index == -1) throw new Exception("Erro interno");

            base.Execute();

            for (var i = 0; i < Pins.Length; i++)
            {
                if (i == index) continue;
                Pins[i].Value = Pins[index].Value;
                Pins[i].IsOpenInternal = Pins[index].IsOpenInternal;
                Pins[i].Propagate();
            }
        }
    }
}