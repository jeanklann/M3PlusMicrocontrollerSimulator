using System;
using System.IO;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace IDE {
    [Serializable]
    public class FileProject {
        public Project Project;
        public int Frequency = 1;
        public string Code;
        public byte[] Instructions;
        
        public Wire_Project[] Wires;
        public Component_Project[] Components;
        
        public static bool Save(string dir) {
            var project = new FileProject();
            if (UiStatics.Simulador != null) {
                project.Frequency = UiStatics.Simulador.Frequency;
            }
            project.Code = UiStatics.Codigo.scintilla.Text;
            project.Instructions = new byte[5];
            project.Instructions[0] = 13;
            project.Instructions[1] = 45;
            project.Instructions[2] = 55;
            project.Instructions[3] = 67;
            project.Instructions[4] = 154;

            project.Components = new Component_Project[UiStatics.Circuito.Components.Count];
            project.Wires = new Wire_Project[UiStatics.Circuito.Wires.Count];

            for (var i = 0; i < project.Components.Length; i++) {
                project.Components[i] = new Component_Project(UiStatics.Circuito.Components[i]);
            }
            for (var i = 0; i < project.Wires.Length; i++) {
                project.Wires[i] = new Wire_Project(UiStatics.Circuito.Wires[i], project);
            }
            
            return SerializeObject(project, UiStatics.FilePath);
        }
        public static bool Load(string dir) {
            var project = DeSerializeObject(dir);
            if (project == null) return false;
            UiStatics.Depurador.SetText("");
            UiStatics.Simulador = null;
            if (project.Code != null && project.Code != "") {
                UiStatics.Codigo.scintilla.Text = project.Code;
            } else {
                
            }
            
            UiStatics.Circuito.Wires.Clear();
            UiStatics.Circuito.Components.Clear();

            for (var i = 0; i < project.Components.Length; i++) {
                UiStatics.Circuito.Components.Add(project.Components[i]);
            }
            for (var i = 0; i < project.Wires.Length; i++) {
                UiStatics.Circuito.Wires.Add(project.Wires[i].ToWire(project));
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
        private static bool SerializeObject(FileProject serializableObject, string fileName) {
            try {
                if (serializableObject == null) { return false; }

                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(fileName,
                                         FileMode.Create,
                                         FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, serializableObject);
                stream.Close();
                return true;
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
        private static FileProject DeSerializeObject(string fileName) {
            if (string.IsNullOrEmpty(fileName)) { return null; }

            FileProject objectOut = null;


            try {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(fileName,
                                         FileMode.Open,
                                         FileAccess.Read, FileShare.None);
                objectOut = (FileProject) formatter.Deserialize(stream);
                stream.Close();
                return objectOut;
            } catch (Exception) {
                return null;
            }
        }
    }
    [Serializable]
    public class Point_Project {
        public float X, Y;
        public Point_Project(PointF point) {
            X = point.X;
            Y = point.Y;
        }
        public Point_Project(float X, float Y) {
            this.X = X;
            this.Y = Y;
        }
        public static implicit operator PointF(Point_Project p) {
            return new PointF(p.X, p.Y);
        }
        public static implicit operator Point_Project(PointF p) {
            return new Point_Project(p);
        }
    }
    [Serializable]
    public class Component_Project {
        public Point_Project Center;
        public float Rotation;
        public ComponentType Type;
        public int RootComponent;

        public Component_Project(Component c) {
            Center = c.Center;
            Rotation = c.Rotation;
            Type = c.Type;
            RootComponent = UiStatics.Circuito.Components.IndexOf(c.RootComponent);
        }
        public static implicit operator Component(Component_Project c) {
            ComponentDraw draw = null;

            switch (c.Type) {
                case ComponentType.None:
                    break;
                case ComponentType.Input:
                    draw = Draws.Input[0];
                    break;
                case ComponentType.Output:
                    draw = Draws.Output[0];
                    break;
                case ComponentType.Disable:
                    draw = Draws.Disable[0];
                    break;
                case ComponentType.Not:
                    draw = Draws.Not[0];
                    break;
                case ComponentType.And:
                    draw = Draws.And[0];
                    break;
                case ComponentType.Nand:
                    draw = Draws.Nand[0];
                    break;
                case ComponentType.Or:
                    draw = Draws.Or[0];
                    break;
                case ComponentType.Nor:
                    draw = Draws.Nor[0];
                    break;
                case ComponentType.Xor:
                    draw = Draws.Xor[0];
                    break;
                case ComponentType.Xnor:
                    draw = Draws.Xnor[0];
                    break;
                case ComponentType.Keyboard:
                    draw = Draws.Keyboard;
                    break;
                case ComponentType.Display7Seg:
                    draw = Draws.Display7SegBase;
                    break;
                case ComponentType.Circuit:
                    break;
                case ComponentType.Microcontroller:
                    draw = Draws.Microcontroller;
                    break;
                case ComponentType.Osciloscope:
                    draw = Draws.Osciloscope;
                    break;
                case ComponentType.BlackTerminal:
                    break;
                case ComponentType.JkFlipFlop:
                    draw = Draws.JkFlipFlop;
                    break;
                case ComponentType.RsFlipFlop:
                    draw = Draws.RsFlipFlop;
                    break;
                case ComponentType.DFlipFlop:
                    draw = Draws.DFlipFlop;
                    break;
                case ComponentType.FlipFlop:
                    draw = Draws.FlipFlop;
                    break;
                case ComponentType.ControlModule:
                    draw = Draws.ControlModule;
                    break;
                case ComponentType.PortBank:
                    draw = Draws.PortBank;
                    break;
                case ComponentType.RamMemory:
                    draw = Draws.RamMemory;
                    break;
                case ComponentType.RomMemory:
                    draw = Draws.RomMemory;
                    break;
                case ComponentType.Stack:
                    break;
                case ComponentType.Tristate:
                    draw = Draws.Disable[0];
                    break;
                case ComponentType.Ula:
                    draw = Draws.Ula;
                    break;
                case ComponentType.Registrer8Bit:
                    draw = Draws.Registrer8Bit;
                    break;
                case ComponentType.Registrer8BitSg:
                    draw = Draws.Registrer8BitSg;
                    break;
                case ComponentType.Registrers:
                    draw = Draws.Registrers;
                    break;
                case ComponentType.Disable8Bit:
                    draw = Draws.Disable8Bit;
                    break;
                case ComponentType.BinTo7Seg:
                    draw = Draws.BinTo7Seg;
                    break;
                case ComponentType.Registrer8BitCBuffer:
                    draw = Draws.Registrer8BitCBuffer;
                    break;
                case ComponentType.Counter8Bit:
                    draw = Draws.Counter8Bit;
                    break;
                case ComponentType.RomAddresser:
                    draw = Draws.RomAddresser;
                    break;
                case ComponentType.Clock:
                    draw = Draws.Clock;
                    break;
                default:
                    break;
            }

            var component = new Component(draw, c.Center);
            component.Rotation = c.Rotation;
            component.Type = c.Type;
            if(c.RootComponent != -1)
                component.RootComponent = UiStatics.Circuito.Components[c.RootComponent];

            return component;
        }
    }
    [Serializable]
    public class Wire_Project {
        public Point_Project From;
        public int FromComponent;
        public int FromIndex;
        public Point_Project To;
        public int ToComponent;
        public int ToIndex;
        public int RootComponent;
        
        public Wire_Project(Wire wire, FileProject project) {
            From = wire.From;
            RootComponent = UiStatics.Circuito.Components.IndexOf(wire.RootComponent);
            if (wire.FromComponent != null) {
                FromComponent = UiStatics.Circuito.Components.IndexOf(wire.FromComponent);
                FromIndex = wire.FromIndex;
            } else {
                FromIndex = -1;
                FromComponent = -1;
            }
            To = wire.To;
            if (wire.ToComponent != null) {
                ToComponent = UiStatics.Circuito.Components.IndexOf(wire.ToComponent);
                ToIndex = wire.ToIndex;
            } else {
                ToIndex = -1;
                ToComponent = -1;
            }
        }
        public Wire ToWire(FileProject project) {
            var wire = new Wire(From, To);
            if (RootComponent != -1)
                wire.RootComponent = UiStatics.Circuito.Components[RootComponent];
            if (FromComponent != -1) {
                wire.FromComponent = UiStatics.Circuito.Components[FromComponent];
                wire.FromIndex = FromIndex;
            } else {
                FromIndex = -1;
            }
            if (ToComponent != -1) {
                wire.ToComponent = UiStatics.Circuito.Components[ToComponent];
                wire.ToIndex = ToIndex;
            } else {
                FromIndex = -1;
            }
            return wire;
        }
    }
}
