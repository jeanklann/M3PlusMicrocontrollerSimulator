using System.IO;
using System.Text;
using M3PlusMicrocontroller;

namespace IDE.Exporter
{
    public class HexadecimalExporterStrategy : IExporterStrategy
    {
        public void WriteBytes(BinaryWriter bw, Instruction[] instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction == null) continue;
                var bytes = instruction.Convert();
                foreach (var value in bytes)
                    bw.Write(Encoding.ASCII.GetBytes(value.ToString("X2")));
            }
        }
    }
}