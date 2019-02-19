using System.IO;

namespace IDE.Importer
{
    public class BinaryImporterStrategy : IImporterStrategy
    {
        public byte[] GetBytes(StreamReader stream)
        {
            var baseStream = stream.BaseStream;
            var br = new BinaryReader(baseStream);
            var bytes = new byte[baseStream.Length];
            while (baseStream.Position < baseStream.Length) bytes[baseStream.Position] = br.ReadByte();
            return bytes;
        }
    }
}