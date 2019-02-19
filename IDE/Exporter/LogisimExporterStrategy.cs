using System.IO;
using System.Text;
using M3PlusMicrocontroller;

namespace IDE.Exporter
{
    public class LogisimExporterStrategy : IExporterStrategy
    {
        public void WriteBytes(BinaryWriter bw, Instruction[] instructions)
        {
            bw.Write(Encoding.ASCII.GetBytes("v2.0 raw\n"));
            for (var i = 0; i < instructions.Length; i++)
            {
                if (instructions[i] == null) continue;
                var bytes = instructions[i].Convert();
                for (var j = 0; j < bytes.Length; j++)
                {
                    if (j > 0)
                        bw.Write(' ');
                    bw.Write(Encoding.ASCII.GetBytes(bytes[j].ToString("x")));
                }

                if ((i + 1) % 16 == 0)
                    bw.Write(Encoding.ASCII.GetBytes("\n"));
                else
                    bw.Write(' ');
            }
        }
    }
}