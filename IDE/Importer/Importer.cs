using System.IO;
using M3PlusMicrocontroller;

namespace IDE.Importer
{
    public class Importer
    {
        private IImporterStrategy _strategy;

        public Importer(IImporterStrategy strategy)
        {
            _strategy = strategy;
        }

        public string Import(string path)
        {
            var stream = new StreamReader(path);
            var bytes = _strategy.GetBytes(stream);
            stream.Close();
            var decompiler = new Decompiler();
            return decompiler.Decompile(bytes);
        }
    }
}