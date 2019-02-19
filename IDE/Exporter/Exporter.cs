using System;
using System.IO;
using System.Text;
using M3PlusMicrocontroller;

namespace IDE.Exporter
{
    public class Exporter
    {
        private IExporterStrategy _strategy;

        public Exporter(IExporterStrategy strategy)
        {
            _strategy = strategy;
        }

        public void Export(string path, Instruction[] instructions)
        {
            var stream = new StreamWriter(path);
            var baseStream = stream.BaseStream;
            var bw = new BinaryWriter(baseStream);
            _strategy.WriteBytes(bw, instructions);
            stream.Close();
        }
    }
}