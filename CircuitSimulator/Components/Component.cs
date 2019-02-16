using System.Text;

namespace CircuitSimulator {
    public abstract class Component {
        protected internal int SimulationIdInternal;

        protected internal bool CanStart = false;
        protected internal Circuit Circuit;

        public long Id = -1;
        public string Name;
        public Pin[] Pins;
        public int SimulationId => SimulationIdInternal;


        /// <summary>
        /// Electronic component
        /// </summary>
        /// <param name="name">The nome of the component</param>
        /// <param name="pinQuantity">The quantity of pins that this component need to have</param>
        public Component(string name = "Generic component", int pinQuantity = 1) {
            if (name != null)
                Name = name;
            Pins = new Pin[pinQuantity];
            AllocatePins();
        }
        
        /// <summary>
        /// Allocate the pins of the component
        /// </summary>
        protected virtual void AllocatePins() {
            for(var i = 0; i < Pins.Length; i++) {
                Pins[i] = new Pin(this);
            }
        }

        /// <summary>
        /// Verifies if this componente is ready to be executed
        /// </summary>
        /// <returns>true if is ready</returns>
        internal virtual bool CanExecute() {
            if(SimulationIdInternal == Circuit.SimulationId) return false;
            for(var i = 0; i < Pins.Length; i++) {
                if(Pins[i].SimulationIdInternal != Circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Simulate this component
        /// </summary>
        protected internal virtual void Execute() {
            SimulationIdInternal = Circuit.SimulationId;
            for(var i = 0; i < Pins.Length; i++) {
                Pins[i].SimulationIdInternal = SimulationIdInternal;
            }
        }
        
        public override string ToString() {
            var res = new StringBuilder();
            res.Append(Name);
            res.Append("[");
            res.Append(Pins[0]);
            for(var i = 1; i < Pins.Length; i++) {
                res.Append(", ");
                res.Append(Pins[i]);
            }
            res.Append("]");
            return res.ToString();
        }

    }
}
