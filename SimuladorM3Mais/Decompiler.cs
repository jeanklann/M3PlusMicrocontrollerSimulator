using System.Collections.Generic;
using System.Text;

namespace M3PlusMicrocontroller
{
    public class Decompiler
    {
        public string Decompile(byte[] bytes)
        {
            var sb = new StringBuilder();
            var index = 0;
            var instructionList = new List<Instructions>();
            while (index < bytes.Length)
            {
                var text = Instruction.FromBytes(bytes, index, out var totalBytes, out var address);
                var ni = new Instructions
                {
                    Text = text,
                    PointAddressBytes = address,
                    TotalBytes = totalBytes
                };
                instructionList.Add(ni);

                index += totalBytes;
            }

            var pos = 0;
            foreach (var item in instructionList)
            {
                foreach (var itemLabel in instructionList)
                {
                    if (itemLabel.PointAddress != pos) continue;
                    if (item.Labels.Contains(itemLabel.PointAddressLabel)) continue;
                    sb.AppendLine($"{itemLabel.PointAddressLabel}:");
                    item.Labels.Add(itemLabel.PointAddressLabel);
                }

                sb.AppendLine(item.Text);
                pos += item.TotalBytes;
            }

            return sb.ToString();
        }
        
        private class Instructions
        {
            public readonly List<string> Labels = new List<string>();
            public string Text { get; set; }

            public int PointAddress => PointAddressBytes != null && PointAddressBytes.Length == 2
                ? PointAddressBytes[0] * 256 + PointAddressBytes[1]
                : -1;

            public byte[] PointAddressBytes { private get; set; }

            public string PointAddressLabel => PointAddressBytes != null && PointAddressBytes.Length == 2
                ? $"E_{PointAddressBytes[0]:X2}{PointAddressBytes[1]:X2}"
                : string.Empty;

            public int TotalBytes { get; set; }
        }
    }
}