namespace M3PlusMicrocontroller
{
    public static class Helpers
    {
        public static int CountLines(string program, int index)
        {
            var line = 1;
            for (var i = 0; i < program.Length; i++)
            {
                if (program[i] == '\n') line++;
                if (i >= index) break;
            }

            return line;
        }

        public static string ToHex(byte value)
        {
            var res = value.ToString("X");
            if (res.Length == 1)
                return "0" + res;
            return res;
        }
    }
}