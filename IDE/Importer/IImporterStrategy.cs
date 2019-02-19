using System.IO;

namespace IDE.Importer
{
    public interface IImporterStrategy
    {
        byte[] GetBytes(StreamReader stream);
    }
}