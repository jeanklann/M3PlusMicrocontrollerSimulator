﻿using System.Text;

namespace CircuitSimulator {
    public class Converters {
        private static readonly float[] values = new float[] {1e-12f, 1e-9f, 1e-6f, 1e-3f, 1f, 1e3f, 1e6f, 1e9f, 1e12f};
        private static readonly float[] multiplyValues = new float[] { 1e12f, 1e9f, 1e6f, 1e3f, 1f, 1e-3f, 1e-6f, 1e-9f, 1e-12f };
        private static readonly string[,] prefixes = new string[,] { { "p", "n", "u", "m", "", "k", "M", "G", "T" }, { " pico", " nano", " micro", " mili", "", " kilo", " mega", " giga", " tera" } };
        private static readonly string[] volt = new string[] {"V", " volt", " volts"};
        private static readonly string[] resistance = new string[] { "ohm", " ohm", " ohms" };
        private static readonly string[] current = new string[] { "A", " ampere", " amperes" };
        private static readonly string[] frequency = new string[] { "Hz", " hertz", " hertz" };
        private static readonly string[] time = new string[] { "s", " segundo", " segundos" };

        /// <summary>
        /// Convert the greatness value in an easies string to be readable to humans
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="greatness">The gratness</param>
        /// <param name="type">If its normal, like, 1V, or 144V or extended, like, 1 volt, or 144 volts.</param>
        /// <returns>The converted value</returns>
        public static string ToString(float value, Greatness greatness, ConvertionType type = ConvertionType.Normal) {
            StringBuilder res = new StringBuilder();
            if(value < 0) {
                res.Append("-");
                value = -value;
            }
            int greatnessIndex = 0;
            if(value > 0f) {
                int index = getIndex(value);
                float finalValue = value * multiplyValues[index];
                res.Append(finalValue);
                if(type == ConvertionType.Extended) {
                    greatnessIndex = finalValue == 1f ? 1 : 2;
                    res.Append(prefixes[1, index]);
                } else {
                    res.Append(prefixes[0, index]);
                }
            } else {
                res.Append(value);
                if(type == ConvertionType.Extended) {
                    greatnessIndex = 2;
                }
            }
            switch(greatness) {
                case Greatness.Current: res.Append(current[greatnessIndex]); break;
                case Greatness.Frequency: res.Append(frequency[greatnessIndex]); break;
                case Greatness.Resistance: res.Append(resistance[greatnessIndex]); break;
                case Greatness.Time: res.Append(time[greatnessIndex]); break;
                case Greatness.Volt: res.Append(volt[greatnessIndex]); break;
                default: break;
            }
            return res.ToString();
        }
        private static int getIndex(float value) {
            if(value < values[0]) return 0;
            for(int i = 1; i < values.Length; i++) {
                if(value < values[i]) {
                    return value < 0f ? i - 2 : i - 1;
                }
            }
            return values.Length - 1;
        }
    }
    public enum Greatness {
        Volt, Resistance, Current, Frequency, Time
    }
    public enum ConvertionType {
        Normal, Extended
    }
}