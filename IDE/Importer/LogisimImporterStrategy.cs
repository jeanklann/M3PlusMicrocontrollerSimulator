using System;
using System.Globalization;
using System.IO;

namespace IDE.Importer
{
    public class LogisimImporterStrategy : IImporterStrategy
    {
        public byte[] GetBytes(StreamReader stream)
        {
            var all = stream.ReadToEnd();
            all = all.Replace("v2.0 raw", "");
            all = all.Replace("\r", " ");
            all = all.Replace("\n", " ");
            all = all.Replace("  ", " ");
            while (all.Contains("*"))
            {
                var i = all.IndexOf("*", StringComparison.Ordinal);
                var bi = i;
                var ei = i;
                var chr = all[bi];
                while (chr != ' ')
                {
                    --bi;
                    chr = all[bi];
                }

                chr = all[ei];
                while (chr != ' ')
                {
                    ++ei;
                    chr = all[ei];
                }

                var quant = int.Parse(all.Substring(bi, i - bi));
                var str = all.Substring(i + 1, ei - (i + 1));
                var tmp = "";
                for (var j = 0; j < quant; j++)
                {
                    tmp += ' ';
                    tmp += str;
                }

                all = all.Replace(all.Substring(bi, ei - bi), tmp);
            }

            var hexes = all.Split(' ');
            var bytes = new byte[hexes.Length];
            for (var i = 0; i < hexes.Length; i++)
            {
                if (hexes[i] == "") continue;
                bytes[i] = byte.Parse(hexes[i], NumberStyles.HexNumber);
            }

            return bytes;
        }
    }
}