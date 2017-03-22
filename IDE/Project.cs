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
        public Wire[] Wires;
        public Component[] Components;

        public static bool Save(string dir) {
            FileProject project = new FileProject();
            if(UIStatics.Simulador != null) {
                project.Frequency = UIStatics.Simulador.Frequency;
            }
            project.Code = UIStatics.Codigo.scintilla.Text;
            
            project.Components = UIStatics.Circuito.Components.ToArray();
            project.Wires = UIStatics.Circuito.Wires.ToArray();
            
            foreach (Component item in project.Components) {
                if (item.Draw.DisplayListHandle == Draws.And[0].DisplayListHandle ||
                    item.Draw.DisplayListHandle == Draws.And[1].DisplayListHandle) {
                    item.Type = ComponentType.And;
                } else if (item.Draw.DisplayListHandle == Draws.Nand[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Nand[1].DisplayListHandle) {
                    item.Type = ComponentType.Nand;
                } else if (item.Draw.DisplayListHandle == Draws.Or[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Or[1].DisplayListHandle) {
                    item.Type = ComponentType.Or;
                } else if (item.Draw.DisplayListHandle == Draws.Nor[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Nor[1].DisplayListHandle) {
                    item.Type = ComponentType.Nor;
                } else if (item.Draw.DisplayListHandle == Draws.Xor[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Xor[1].DisplayListHandle) {
                    item.Type = ComponentType.Xor;
                } else if (item.Draw.DisplayListHandle == Draws.Xnor[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Xnor[1].DisplayListHandle) {
                    item.Type = ComponentType.Xnor;
                } else if (item.Draw.DisplayListHandle == Draws.Not[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Not[1].DisplayListHandle) {
                    item.Type = ComponentType.Not;
                } else if (item.Draw.DisplayListHandle == Draws.Input[0].DisplayListHandle ||
                             item.Draw.DisplayListHandle == Draws.Input[1].DisplayListHandle) {
                    item.Type = ComponentType.Input;
                } else if (item.Draw.DisplayListHandle == Draws.Output[0].DisplayListHandle ||
                             item.Draw.DisplayListHandle == Draws.Output[1].DisplayListHandle) {
                    item.Type = ComponentType.Output;
                } else if (item.Draw.DisplayListHandle == Draws.Keyboard.DisplayListHandle) {
                    item.Type = ComponentType.Circuit;
                } else if (item.Draw.DisplayListHandle == Draws.Microcontroller.DisplayListHandle) {
                    item.Type = ComponentType.Microcontroller;
                } else if (item.Draw.DisplayListHandle == Draws.Disable[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Disable[1].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Disable[2].DisplayListHandle) {
                    throw new NotImplementedException();
                } else {
                    throw new NotImplementedException();
                }
            }
            
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
            
            foreach (Component item in project.Components) {
                switch (item.Type) {
                    case ComponentType.None:
                        break;
                    case ComponentType.Input:
                        item.Draw = Draws.Input[0];
                        break;
                    case ComponentType.Output:
                        item.Draw = Draws.Output[0];
                        break;
                    case ComponentType.Disable:
                        item.Draw = Draws.Disable[0];
                        break;
                    case ComponentType.Not:
                        item.Draw = Draws.Not[0];
                        break;
                    case ComponentType.And:
                        item.Draw = Draws.And[0];
                        break;
                    case ComponentType.Nand:
                        item.Draw = Draws.Nand[0];
                        break;
                    case ComponentType.Or:
                        item.Draw = Draws.Or[0];
                        break;
                    case ComponentType.Nor:
                        item.Draw = Draws.Nor[0];
                        break;
                    case ComponentType.Xor:
                        item.Draw = Draws.Xor[0];
                        break;
                    case ComponentType.Xnor:
                        item.Draw = Draws.Xnor[0];
                        break;
                    case ComponentType.Keyboard:
                        item.Draw = Draws.Keyboard;
                        break;
                    case ComponentType.Display7Seg:
                        break;
                    case ComponentType.Circuit:
                        break;
                    case ComponentType.Microcontroller:
                        item.Draw = Draws.Microcontroller;
                        break;
                    case ComponentType.Osciloscope:
                        break;
                    case ComponentType.BlackTerminal:
                        break;
                    case ComponentType.JKFlipFlop:
                        break;
                    case ComponentType.RSFlipFlop:
                        break;
                    case ComponentType.DFlipFlop:
                        break;
                    case ComponentType.TFlipFlop:
                        break;
                    default:
                        break;
                }
            }
            UIStatics.Circuito.Wires.Clear();
            UIStatics.Circuito.Components.Clear();

            
            UIStatics.Circuito.Components.AddRange(project.Components);
            UIStatics.Circuito.Wires.AddRange(project.Wires);
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
