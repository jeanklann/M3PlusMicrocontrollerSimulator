using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace IDE
{
    [Serializable]
    public class FileProject
    {
        public string Code;
        public ComponentProject[] Components;
        public int Frequency = 1;
        public byte[] Instructions;
        public Project Project;

        public WireProject[] Wires;

        public static bool Save(string dir)
        {
            var project = new FileProject();
            if (UiStatics.Simulador != null) project.Frequency = UiStatics.Simulador.Frequency;
            project.Code = UiStatics.Codigo.scintilla.Text;
            project.Instructions = new byte[5];
            project.Instructions[0] = 13;
            project.Instructions[1] = 45;
            project.Instructions[2] = 55;
            project.Instructions[3] = 67;
            project.Instructions[4] = 154;

            project.Components = new ComponentProject[UiStatics.Circuito.Components.Count];
            project.Wires = new WireProject[UiStatics.Circuito.Wires.Count];

            for (var i = 0; i < project.Components.Length; i++)
                project.Components[i] = new ComponentProject(UiStatics.Circuito.Components[i]);
            for (var i = 0; i < project.Wires.Length; i++)
                project.Wires[i] = new WireProject(UiStatics.Circuito.Wires[i], project);

            return SerializeObject(project, UiStatics.FilePath);
        }

        public static bool Load(string dir)
        {
            var project = DeSerializeObject(dir);
            if (project == null) return false;
            UiStatics.Depurador.SetText("");
            UiStatics.Simulador = null;
            if (project.Code != null && project.Code != "")
            {
                UiStatics.Codigo.scintilla.Text = project.Code;
            }

            UiStatics.Circuito.Wires.Clear();
            UiStatics.Circuito.Components.Clear();

            for (var i = 0; i < project.Components.Length; i++)
                UiStatics.Circuito.Components.Add(project.Components[i]);
            for (var i = 0; i < project.Wires.Length; i++)
                UiStatics.Circuito.Wires.Add(project.Wires[i].ToWire(project));

            return true;
        }

        /// <summary>
        ///     Serializes an object.
        ///     http://stackoverflow.com/questions/6115721/how-to-save-restore-serializable-object-to-from-file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        private static bool SerializeObject(FileProject serializableObject, string fileName)
        {
            try
            {
                if (serializableObject == null) return false;

                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(fileName,
                    FileMode.Create,
                    FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, serializableObject);
                stream.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Deserializes an xml file into an object list
        ///     http://stackoverflow.com/questions/6115721/how-to-save-restore-serializable-object-to-from-file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static FileProject DeSerializeObject(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            FileProject objectOut = null;


            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(fileName,
                    FileMode.Open,
                    FileAccess.Read, FileShare.None);
                objectOut = (FileProject) formatter.Deserialize(stream);
                stream.Close();
                return objectOut;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}