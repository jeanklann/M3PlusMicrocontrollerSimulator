using System;
using System.Collections.Generic;
using System.Text;

namespace CircuitSimulator {
    public abstract class Component {
        protected internal int simulationId = 0;

        protected internal bool canStart = false;
        protected internal Circuit circuit;

        public string Name;
        public Pin[] Pins;
        

        /// <summary>
        /// Electronic component
        /// </summary>
        /// <param name="name">The nome of the component</param>
        /// <param name="pinQuantity">The quantity of pins that this component need to have</param>
        public Component(string name = "Generic component", int pinQuantity = 1) {
            if(name != null)
                Name = name;
            Pins = new Pin[pinQuantity];
            AllocatePins();
        }
        
        /// <summary>
        /// Allocate the pins of the component
        /// </summary>
        protected virtual void AllocatePins() {
            for(int i = 0; i < Pins.Length; i++) {
                Pins[i] = new Pin(this);
            }
        }

        /// <summary>
        /// Verifies if this componente is ready to be executed
        /// </summary>
        /// <returns>true if is ready</returns>
        internal virtual bool CanExecute() {
            if(simulationId == circuit.SimulationId) return false;
            for(int i = 0; i < Pins.Length; i++) {
                if(Pins[i].simulationId != circuit.SimulationId) {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Simulate this component
        /// </summary>
        protected internal virtual void Execute() {
            simulationId = circuit.SimulationId;
            for(int i = 0; i < Pins.Length; i++) {
                Pins[i].simulationId = simulationId;
            }
        }
        
        public override string ToString() {
            StringBuilder res = new StringBuilder();
            res.Append(Name);
            res.Append("[");
            res.Append(Pins[0]);
            for(int i = 1; i < Pins.Length; i++) {
                res.Append(", ");
                res.Append(Pins[i]);
            }
            res.Append("]");
            return res.ToString();
        }

    }


    public class Pin {
        public const float HIGH = 5;
        public const float LOW = 0;

        internal int simulationId = 0;
        internal float value;
        internal bool isOpen;
        internal bool isOutput;
        internal Component component;
        internal Circuit Circuit { get { return component.circuit; } }

        public float Value { get { return value; } }
        public bool IsOpen { get { return isOpen; } }
        public bool IsOutput { get { return isOutput; } }

        public List<Pin> connectedPins;
        
        /// <summary>
        /// Pin to every component
        /// </summary>
        /// <param name="component">The attached component</param>
        /// <param name="isOutput">If this pin is an output</param>
        /// <param name="isOpen">If this pin is on third state</param>
        /// <param name="value">The default value of the pin</param>
        internal Pin(Component component, bool isOutput = false, bool isOpen = false, float value = 0f) {
            if(component == null) throw new Exception("Component cannot be null");
            this.component = component;
            this.isOutput = isOutput;
            this.isOpen = isOpen;
            this.value = value;
            connectedPins = new List<Pin>();
        }

        public bool CanExecute(bool cannotBeOpen = true) {
            if (cannotBeOpen) {
                if (isOpen) return false;
            }
            if (component.simulationId == simulationId) return false;
            return true;
        }

        /// <summary>
        /// Set a digital value to the pin.
        /// If the value it's not the correct digital value, it verifies if it's close to set high or low.
        /// </summary>
        /// <param name="value">The voltage value</param>
        public void SetDigital(float value) {
            float med = (HIGH + LOW) / 2f;
            if(value > med)
                this.value = HIGH;
            else
                this.value = LOW;
        }

        /// <summary>
        /// Gets the digital value
        /// </summary>
        /// <returns>HIGH value or LOW value</returns>
        public float GetDigital() {
            float med = (HIGH + LOW) / 2f;
            if(value > med)
                return HIGH;
            return LOW;
        }

        /// <summary>
        /// Propagate to other connected pins.
        /// It verifies if the component is ready to be executed, and then put on the queue to me added
        /// And recursively the valid connected pins propagate to others.
        /// </summary>
        internal void Propagate() {
            foreach(Pin pin in connectedPins) {
                if(pin.value != value || pin.isOpen != isOpen) {
                    pin.simulationId = simulationId;
                    pin.value = value;
                    pin.isOpen = isOpen;
                    if (pin.component.CanExecute()) {
                        Circuit.AddToExecution(pin.component);
                    }
                    pin.Propagate();
                }
            }
        }

        /// <summary>
        /// Connect this pin to other pin
        /// </summary>
        /// <param name="pin">The pin to be connected</param>
        public void Connect(Pin pin) {
            if(connectedPins.Contains(pin)) return;
            connectedPins.Add(pin);
            pin.connectedPins.Add(this);
        }

        /// <summary>
        /// Disconnect this pin to other pin
        /// </summary>
        /// <param name="pin">The pin to be disconnected</param>
        /// <param name="recursive">If need to be recursive. Nomally need to be true, to be disconnected in the other pin too</param>
        public void Disconnect(Pin pin, bool recursive = true) {
            connectedPins.Remove(pin);
            if(recursive)
                pin.Disconnect(pin, false);
        }
        /// <summary>
        /// Converts to string the value of this pin
        /// </summary>
        /// <returns>the string value</returns>
        public override string ToString() {
            return Converters.ToString(value, Greatness.Volt);
        }
        /// <summary>
        /// Compare if the other pin is open either, ...
        /// </summary>
        /// <param name="pin">the pin to be compared</param>
        /// <returns>if both are equal</returns>
        protected internal bool IsEqualOthers(Pin pin) {
            if(pin.isOpen != isOpen) return false;
            return true;
        }
        /// <summary>
        /// Compare if pin value is equal to this
        /// </summary>
        /// <param name="pin">the pin to be compared</param>
        /// <returns>if both are equal</returns>
        protected internal bool IsEqualValue(Pin pin) {
            if(pin.value != value) return false;
            return true;
        }
        /// <summary>
        /// Compare if pin digital value is equal to this
        /// </summary>
        /// <param name="pin">the pin to be compared</param>
        /// <returns>if both are equal</returns>
        protected internal bool IsDigitalEqualValue(Pin pin) {
            if(pin.GetDigital() != GetDigital()) return false;
            return true;
        }
        /// <summary>
        /// Compares if this pin is equal to the other
        /// </summary>
        /// <param name="pin">the pin to be compared</param>
        /// <returns>if both are equal</returns>
        public bool IsEqual(Pin pin) {
            if(IsEqualOthers(pin) && IsEqualValue(pin)) return true;
            return false;
        }
        /// <summary>
        /// Compares if both pins are digitally equals
        /// </summary>
        /// <param name="pin">the pin to be compared</param>
        /// <returns>if both pins are digitally equals</returns>
        public bool IsDigitalEqual(Pin pin) {
            if(IsEqualOthers(pin) && IsDigitalEqualValue(pin)) return true;
            return false;
        }
        /// <summary>
        /// Verifies if the pin is digital or analog
        /// </summary>
        /// <returns>if the pin is digital</returns>
        public bool IsDigital() {
            if(value == HIGH || value == LOW) return true;
            return false;
        }
        #region operators
        public static float operator !(Pin l) {
            return (l.GetDigital() == HIGH) ? LOW : HIGH;
        }
        public static float operator ==(Pin l, Pin r) {
            return l.IsDigitalEqual(r) ? HIGH : LOW;
        }
        public static float operator !=(Pin l, Pin r) {
            return (!l.IsDigitalEqual(r)) ? HIGH : LOW;
        }
        public static float operator +(Pin l, Pin r) {
            if(!l.IsEqualOthers(r)) return LOW;
            if(l.GetDigital() == HIGH || r.GetDigital() == HIGH) return HIGH;
            return LOW;
        }
        public static float operator -(Pin l, Pin r) {
            if(!l.IsEqualOthers(r)) return LOW;
            if(l.GetDigital() == HIGH && r.GetDigital() == LOW) return HIGH;
            return LOW;
        }
        public static float operator *(Pin l, Pin r) {
            if(!l.IsEqualOthers(r)) return LOW;
            if(l.GetDigital() == HIGH && r.GetDigital() == HIGH) return HIGH;
            return LOW;
        }
        public static float operator /(Pin l, Pin r) {
            if(!l.IsEqualOthers(r)) return LOW;
            if(l.GetDigital() == HIGH) {
                if(r.GetDigital() == HIGH) return HIGH;
                else if(r.GetDigital() == LOW) throw new Exception("Cannot divide 1 to 0");
            }
            return LOW;
        }
        public static float operator ^(Pin l, Pin r) {
            return (!l.IsDigitalEqual(r)) ? HIGH : LOW;
        }
        public static float operator |(Pin l, Pin r) {
            if(!l.IsEqualOthers(r)) return LOW;
            if(l.GetDigital() == HIGH || r.GetDigital() == HIGH) return HIGH;
            return LOW;
        }
        public static float operator &(Pin l, Pin r) {
            if(!l.IsEqualOthers(r)) return LOW;
            if(l.GetDigital() == HIGH && r.GetDigital() == HIGH) return HIGH;
            return LOW;
        }
        public static float operator <(Pin l, Pin r) {
            if(!l.IsEqualOthers(r)) return LOW;
            return l.value < r.value ? HIGH : LOW;
        }
        public static float operator >(Pin l, Pin r) {
            if(!l.IsEqualOthers(r)) return LOW;
            return l.value > r.value ? HIGH : LOW;
        }
        public static float operator <=(Pin l, Pin r) {
            if(!l.IsEqualOthers(r)) return LOW;
            return l.value <= r.value ? HIGH : LOW;
        }
        public static float operator >=(Pin l, Pin r) {
            if(!l.IsEqualOthers(r)) return LOW;
            return l.value >= r.value ? HIGH : LOW;
        }
        public float And(Pin other) {
            return this & other;
        }
        public float Nand(Pin other) {
            return (this & other) == HIGH ? LOW : HIGH;
        }
        public float Or(Pin other) {
            return this | other;
        }
        public float Nor(Pin other) {
            return (this | other) == HIGH ? LOW : HIGH;
        }
        public float Xor(Pin other) {
            return this ^ other;
        }
        public float Xnor(Pin other) {
            return (this ^ other) == HIGH ? LOW : HIGH;
        }
        public float Neg() {
            return !this;
        }
        public float Equ(Pin other) {
            return this == other;
        }
        public float Nequ(Pin other) {
            return this != other;
        }
        public float Less(Pin other) {
            return this < other;
        }
        public float More(Pin other) {
            return this > other;
        }
        public float LessOrEqu(Pin other) {
            return this <= other;
        }
        public float MoreOrEqu(Pin other) {
            return this >= other;
        }
        public float Plus(Pin other) {
            return this + other;
        }
        public float Minus(Pin other) {
            return this - other;
        }
        public float Mul(Pin other) {
            return this * other;
        }
        public float Div(Pin other) {
            return this / other;
        }
        #endregion
    }


}
