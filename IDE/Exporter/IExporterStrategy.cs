using System.IO;
using M3PlusMicrocontroller;

namespace IDE.Exporter
{
    public interface IExporterStrategy
    {
        void WriteBytes(BinaryWriter bw, Instruction[] instructions);
    }
}