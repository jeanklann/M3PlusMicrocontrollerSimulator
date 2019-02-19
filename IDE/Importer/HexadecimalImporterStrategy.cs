using System.Globalization;
using System.IO;

namespace IDE.Importer
{
    public class HexadecimalImporterStrategy : IImporterStrategy
    {
        public byte[] GetBytes(StreamReader stream)
        {
            
            var baseStream = stream.BaseStream;
            var br = new BinaryReader(baseStream);

            var bytes = new byte[baseStream.Length / 2];
            while (baseStream.Position < baseStream.Length)
                bytes[baseStream.Position / 2] = byte.Parse((char) br.ReadByte() + "" + (char) br.ReadByte(),
                    NumberStyles.HexNumber);
            return bytes;
        }
    }
}