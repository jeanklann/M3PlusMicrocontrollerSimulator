using System.IO;
using M3PlusMicrocontroller;

namespace IDE.Exporter
{
    public class BinaryExporterStrategy : IExporterStrategy
    {
        public void WriteBytes(BinaryWriter bw, Instruction[] instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction == null) continue;
                var bytes = instruction.Convert();
                bw.Write(bytes);
            }
        }
    }
}