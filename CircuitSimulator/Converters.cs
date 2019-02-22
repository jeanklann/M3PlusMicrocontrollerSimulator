using System.Text;

namespace CircuitSimulator
{
    public class Converters
    {
        private static readonly float[] values = {1e-12f, 1e-9f, 1e-6f, 1e-3f, 1f, 1e3f, 1e6f, 1e9f, 1e12f};
        private static readonly float[] multiplyValues = {1e12f, 1e9f, 1e6f, 1e3f, 1f, 1e-3f, 1e-6f, 1e-9f, 1e-12f};

        private static readonly string[,] prefixes =
        {
            {"p", "n", "u", "m", "", "k", "M", "G", "T"},
            {" pico", " nano", " micro", " mili", "", " kilo", " mega", " giga", " tera"}
        };

        private static readonly string[] volt = {"V", " volt", " volts"};
        private static readonly string[] resistance = {"ohm", " ohm", " ohms"};
        private static readonly string[] current = {"A", " ampere", " amperes"};
        private static readonly string[] frequency = {"Hz", " hertz", " hertz"};
        private static readonly string[] time = {"s", " segundo", " segundos"};

        /// <summary>
        ///     Convert the greatness value in an easies string to be readable to humans
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="greatness">The gratness</param>
        /// <param name="type">If its normal, like, 1V, or 144V or extended, like, 1 volt, or 144 volts.</param>
        /// <returns>The converted value</returns>
        public static string ToString(float value, Greatness greatness, ConvertionType type = ConvertionType.Normal)
        {
            var res = new StringBuilder();
            if (value < 0)
            {
                res.Append("-");
                value = -value;
            }

            var greatnessIndex = 0;
            if (value > 0f)
            {
                var index = GetIndex(value);
                var finalValue = value * multiplyValues[index];
                res.Append(finalValue);
                if (type == ConvertionType.Extended)
                {
                    greatnessIndex = finalValue == 1f ? 1 : 2;
                    res.Append(prefixes[1, index]);
                }
                else
                {
                    res.Append(prefixes[0, index]);
                }
            }
            else
            {
                res.Append(value);
                if (type == ConvertionType.Extended) greatnessIndex = 2;
            }

            switch (greatness)
            {
                case Greatness.Current:
                    res.Append(current[greatnessIndex]);
                    break;
                case Greatness.Frequency:
                    res.Append(frequency[greatnessIndex]);
                    break;
                case Greatness.Resistance:
                    res.Append(resistance[greatnessIndex]);
                    break;
                case Greatness.Time:
                    res.Append(time[greatnessIndex]);
                    break;
                case Greatness.Volt:
                    res.Append(volt[greatnessIndex]);
                    break;
            }

            return res.ToString();
        }

        private static int GetIndex(float value)
        {
            if (value < values[0]) return 0;
            for (var i = 1; i < values.Length; i++)
            {
                if (value < values[i])
                    return value < 0f ? i - 2 : i - 1;
            }

            return values.Length - 1;
        }
    }
}