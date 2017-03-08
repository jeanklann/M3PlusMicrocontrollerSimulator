using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace IDE {
    [Serializable]
    public class Project {
        public bool Autosave = false; //not developed
        public int AutosaveInterval = 1000*60*2; //2 minutes
    }
    [Serializable]
    public class FileProject {
        public Project Project;
        public int Frequency = 1;
        public string Code;
        public byte[] Instructions;

        public static bool Save(string dir) {
            FileProject project = new FileProject();
            if(UIStatics.Simulador != null) {
                project.Frequency = UIStatics.Simulador.Frequency;
            }
            project.Code = UIStatics.Codigo.scintilla.Text;
            return SerializeObject(project, UIStatics.FilePath);
        }
        public static bool Load(string dir) {
            FileProject project = DeSerializeObject<FileProject>(UIStatics.FilePath);
            if (project == null) return false;
            if(project.Code != null && project.Code != "") {
                UIStatics.Depurador.SetText("");
                UIStatics.Simulador = null;
                UIStatics.Codigo.scintilla.Text = project.Code;
            } else {
                //bytes of project
            }


            return true;
        }

        /// <summary>
        /// Serializes an object.
        /// http://stackoverflow.com/questions/6115721/how-to-save-restore-serializable-object-to-from-file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        private static bool SerializeObject<T>(T serializableObject, string fileName) {
            if (serializableObject == null) { return false; }

            try {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream()) {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                    return true;
                }
            } catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Deserializes an xml file into an object list
        /// http://stackoverflow.com/questions/6115721/how-to-save-restore-serializable-object-to-from-file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static T DeSerializeObject<T>(string fileName) {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;
                using (StringReader read = new StringReader(xmlString)) {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read)) {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }
                    read.Close();
                }
            } catch (Exception) {
                return default(T);
            }

            return objectOut;
        }
    }
}
