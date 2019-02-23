using System;
using System.Collections.Generic;

namespace CircuitSimulator
{
    public class Pin
    {
        public const float High = 5;
        public const float Low = 0;
        public const float Halfcut = (High + Low) / 2f;
        internal Component Component;

        public List<Pin> ConnectedPins;
        internal bool IsOpenInternal;
        internal bool IsOutputInternal;
        internal int SimulationIdInternal;
        public float Value;

        /// <summary>
        ///     Pin to every component
        /// </summary>
        /// <param name="component">The attached component</param>
        /// <param name="isOutput">If this pin is an output</param>
        /// <param name="isOpen">If this pin is on third state</param>
        /// <param name="value">The default value of the pin</param>
        internal Pin(Component component, bool isOutput = false, bool isOpen = false, float value = 0f)
        {
            if (component == null) throw new Exception("Component cannot be null");
            Component = component;
            IsOutputInternal = isOutput;
            IsOpenInternal = isOpen;
            Value = value;
            ConnectedPins = new List<Pin>();
        }

        public int SimulationId => SimulationIdInternal;
        internal Circuit Circuit => Component.Circuit;

        //public float Value { get { return value; } }
        public bool IsOpen => IsOpenInternal;
        public bool IsOutput => IsOutputInternal;

        public bool CanExecute(bool cannotBeOpen = true)
        {
            if (cannotBeOpen)
            {
                if (IsOpenInternal)
                    return false;
            }

            if (Component.SimulationIdInternal == SimulationIdInternal) return false;
            return true;
        }

        /// <summary>
        ///     Set a digital value to the pin.
        ///     If the value it's not the correct digital value, it verifies if it's close to set high or low.
        /// </summary>
        /// <param name="value">The voltage value</param>
        public void SetDigital(float value)
        {
            var med = (High + Low) / 2f;
            if (value > med)
                Value = High;
            else
                Value = Low;
        }

        /// <summary>
        ///     Gets the digital value
        /// </summary>
        /// <returns>HIGH value or LOW value</returns>
        public float GetDigital()
        {
            var med = (High + Low) / 2f;
            if (Value > med)
                return High;
            return Low;
        }

        /// <summary>
        ///     Propagate to other connected pins.
        ///     It verifies if the component is ready to be executed, and then put on the queue to me added
        ///     And recursively the valid connected pins propagate to others.
        /// </summary>
        internal void Propagate()
        {
            foreach (var pin in ConnectedPins)
            {
                if (pin.SimulationIdInternal != Circuit.SimulationId || pin.Value != Value ||
                    pin.IsOpenInternal != IsOpenInternal)
                {
                    pin.SimulationIdInternal = SimulationIdInternal;
                    pin.Value = Value;
                    pin.IsOpenInternal = IsOpenInternal;
                    if (pin.Component.CanExecute()) Circuit.AddToExecution(pin.Component);
                    pin.Propagate();
                }
            }
        }

        /// <summary>
        ///     Connect this pin to other pin
        /// </summary>
        /// <param name="pin">The pin to be connected</param>
        public void Connect(Pin pin)
        {
            if (!ConnectedPins.Contains(pin))
                ConnectedPins.Add(pin);
            if (!pin.ConnectedPins.Contains(pin))
                pin.ConnectedPins.Add(this);
        }

        /// <summary>
        ///     Disconnect this pin to other pin
        /// </summary>
        /// <param name="pin">The pin to be disconnected</param>
        /// <param name="recursive">If need to be recursive. Nomally need to be true, to be disconnected in the other pin too</param>
        public void Disconnect(Pin pin, bool recursive = true)
        {
            ConnectedPins.Remove(pin);
            if (recursive)
                pin.Disconnect(pin, false);
        }

        /// <summary>
        ///     Converts to string the value of this pin
        /// </summary>
        /// <returns>the string value</returns>
        public override string ToString()
        {
            return Converters.ToString(Value, Greatness.Volt);
        }

        /// <summary>
        ///     Compare if the other pin is open either, ...
        /// </summary>
        /// <param name="pin">the pin to be compared</param>
        /// <returns>if both are equal</returns>
        protected internal bool IsEqualOthers(Pin pin)
        {
            if (pin.IsOpenInternal != IsOpenInternal) return false;
            return true;
        }

        /// <summary>
        ///     Compare if pin value is equal to this
        /// </summary>
        /// <param name="pin">the pin to be compared</param>
        /// <returns>if both are equal</returns>
        protected internal bool IsEqualValue(Pin pin)
        {
            if (pin.Value != Value) return false;
            return true;
        }

        /// <summary>
        ///     Compare if pin digital value is equal to this
        /// </summary>
        /// <param name="pin">the pin to be compared</param>
        /// <returns>if both are equal</returns>
        protected internal bool IsDigitalEqualValue(Pin pin)
        {
            if (pin.GetDigital() != GetDigital()) return false;
            return true;
        }

        /// <summary>
        ///     Compares if this pin is equal to the other
        /// </summary>
        /// <param name="pin">the pin to be compared</param>
        /// <returns>if both are equal</returns>
        public bool IsEqual(Pin pin)
        {
            if (IsEqualOthers(pin) && IsEqualValue(pin)) return true;
            return false;
        }

        /// <summary>
        ///     Compares if both pins are digitally equals
        /// </summary>
        /// <param name="pin">the pin to be compared</param>
        /// <returns>if both pins are digitally equals</returns>
        public bool IsDigitalEqual(Pin pin)
        {
            if (IsEqualOthers(pin) && IsDigitalEqualValue(pin)) return true;
            return false;
        }

        /// <summary>
        ///     Verifies if the pin is digital or analog
        /// </summary>
        /// <returns>if the pin is digital</returns>
        public bool IsDigital()
        {
            if (Value == High || Value == Low) return true;
            return false;
        }

        #region operators

        public static float operator !(Pin l)
        {
            return l.GetDigital() == High ? Low : High;
        }

        public static float operator ==(Pin l, Pin r)
        {
            return l.IsDigitalEqual(r) ? High : Low;
        }

        public static float operator !=(Pin l, Pin r)
        {
            return !l.IsDigitalEqual(r) ? High : Low;
        }

        public static float operator +(Pin l, Pin r)
        {
            if (!l.IsEqualOthers(r)) return Low;
            if (l.GetDigital() == High || r.GetDigital() == High) return High;
            return Low;
        }

        public static float operator -(Pin l, Pin r)
        {
            if (!l.IsEqualOthers(r)) return Low;
            if (l.GetDigital() == High && r.GetDigital() == Low) return High;
            return Low;
        }

        public static float operator *(Pin l, Pin r)
        {
            if (!l.IsEqualOthers(r)) return Low;
            if (l.GetDigital() == High && r.GetDigital() == High) return High;
            return Low;
        }

        public static float operator /(Pin l, Pin r)
        {
            if (!l.IsEqualOthers(r)) return Low;
            if (l.GetDigital() == High)
            {
                if (r.GetDigital() == High) return High;
                if (r.GetDigital() == Low) throw new Exception("Cannot divide 1 to 0");
            }

            return Low;
        }

        public static float operator ^(Pin l, Pin r)
        {
            return !l.IsDigitalEqual(r) ? High : Low;
        }

        public static float operator |(Pin l, Pin r)
        {
            if (!l.IsEqualOthers(r)) return Low;
            if (l.GetDigital() == High || r.GetDigital() == High) return High;
            return Low;
        }

        public static float operator &(Pin l, Pin r)
        {
            if (!l.IsEqualOthers(r)) return Low;
            if (l.GetDigital() == High && r.GetDigital() == High) return High;
            return Low;
        }

        public static float operator <(Pin l, Pin r)
        {
            if (!l.IsEqualOthers(r)) return Low;
            return l.Value < r.Value ? High : Low;
        }

        public static float operator >(Pin l, Pin r)
        {
            if (!l.IsEqualOthers(r)) return Low;
            return l.Value > r.Value ? High : Low;
        }

        public static float operator <=(Pin l, Pin r)
        {
            if (!l.IsEqualOthers(r)) return Low;
            return l.Value <= r.Value ? High : Low;
        }

        public static float operator >=(Pin l, Pin r)
        {
            if (!l.IsEqualOthers(r)) return Low;
            return l.Value >= r.Value ? High : Low;
        }

        public float And(Pin other)
        {
            return this & other;
        }

        public float Nand(Pin other)
        {
            return (this & other) == High ? Low : High;
        }

        public float Or(Pin other)
        {
            return this | other;
        }

        public float Nor(Pin other)
        {
            return (this | other) == High ? Low : High;
        }

        public float Xor(Pin other)
        {
            return this ^ other;
        }

        public float Xnor(Pin other)
        {
            return (this ^ other) == High ? Low : High;
        }

        public float Neg()
        {
            return !this;
        }

        public float Equ(Pin other)
        {
            return this == other;
        }

        public float Nequ(Pin other)
        {
            return this != other;
        }

        public float Less(Pin other)
        {
            return this < other;
        }

        public float More(Pin other)
        {
            return this > other;
        }

        public float LessOrEqu(Pin other)
        {
            return this <= other;
        }

        public float MoreOrEqu(Pin other)
        {
            return this >= other;
        }

        public float Plus(Pin other)
        {
            return this + other;
        }

        public float Minus(Pin other)
        {
            return this - other;
        }

        public float Mul(Pin other)
        {
            return this * other;
        }

        public float Div(Pin other)
        {
            return this / other;
        }

        #endregion
    }
}