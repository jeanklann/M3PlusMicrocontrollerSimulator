﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace IDE {
    [Serializable]
    public class Project {
        public bool Autosave = false; //not developed
        public int AutosaveInterval = 1000 * 60 * 2; //2 minutes
    }

    [Serializable]
    public class FileProject {
        public Project Project;
        public int Frequency = 1;
        public string Code;
        public byte[] Instructions;
        
        public Wire_Project[] Wires;
        public Component_Project[] Components;
        
        public static bool Save(string dir) {
            FileProject project = new FileProject();
            if (UIStatics.Simulador != null) {
                project.Frequency = UIStatics.Simulador.Frequency;
            }
            project.Code = UIStatics.Codigo.scintilla.Text;
            project.Instructions = new byte[5];
            project.Instructions[0] = 13;
            project.Instructions[1] = 45;
            project.Instructions[2] = 55;
            project.Instructions[3] = 67;
            project.Instructions[4] = 154;

            project.Components = new Component_Project[UIStatics.Circuito.Components.Count];
            project.Wires = new Wire_Project[UIStatics.Circuito.Wires.Count];

            for (int i = 0; i < project.Components.Length; i++) {
                project.Components[i] = new Component_Project(UIStatics.Circuito.Components[i]);
            }
            for (int i = 0; i < project.Wires.Length; i++) {
                project.Wires[i] = new Wire_Project(UIStatics.Circuito.Wires[i], project);
            }
            
            return SerializeObject(project, UIStatics.FilePath);
        }
        public static bool Load(string dir) {
            FileProject project = DeSerializeObject(UIStatics.FilePath);
            if (project == null) return false;
            UIStatics.Depurador.SetText("");
            UIStatics.Simulador = null;
            if (project.Code != null && project.Code != "") {
                UIStatics.Codigo.scintilla.Text = project.Code;
            } else {
                
            }
            
            UIStatics.Circuito.Wires.Clear();
            UIStatics.Circuito.Components.Clear();

            for (int i = 0; i < project.Components.Length; i++) {
                UIStatics.Circuito.Components.Add(project.Components[i]);
            }
            for (int i = 0; i < project.Wires.Length; i++) {
                UIStatics.Circuito.Wires.Add(project.Wires[i].ToWire(project));
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
    public class Size_Project {
        public float Width, Height;
        public Size_Project(SizeF size) {
            Width = size.Width;
            Height = size.Height;
        }
        public Size_Project(float Width, float Height) {
            this.Width = Width;
            this.Height = Height;
        }
        public static implicit operator SizeF(Size_Project s) {
            return new SizeF(s.Width, s.Height);
        }
        public static implicit operator Size_Project(SizeF s) {
            return new Size_Project(s);
        }

    }
    [Serializable]
    public class Rectangle_Project {
        public Point_Project Point;
        public Size_Project Size;
        public Rectangle_Project(PointF point, SizeF size) {
            Point = point;
            Size = size;
        }
        public Rectangle_Project(float x, float y, float w, float h) {
            Point = new Point_Project(x, y);
            Size = new Size_Project(w, h);
        }
        public Rectangle_Project(RectangleF rect) {
            Point = rect.Location;
            Size = rect.Size;
        }
        public static implicit operator RectangleF(Rectangle_Project r) {
            return new RectangleF(r.Point, r.Size);
        }
        public static implicit operator Rectangle_Project(RectangleF r) {
            return new Rectangle_Project(r);
        }
    }
    [Serializable]
    public class Color_Project {
        public byte A, R, G, B;
        public Color_Project(Color color) {
            A = color.A;
            R = color.R;
            G = color.G;
            B = color.B;
        }
        public Color_Project(byte R, byte G, byte B, byte A = 255) {
            this.A = A;
            this.R = R;
            this.G = G;
            this.B = B;
        }
        public static implicit operator Color(Color_Project c) {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }
        public static implicit operator Color_Project(Color c) {
            return new Color_Project(c);
        }
    }
    [Serializable]
    public class Component_Project {
        public Point_Project Center;
        public float Rotation;
        public ComponentType Type;

        public Component_Project(Component c) {
            Center = c.Center;
            Rotation = c.Rotation;
            Type = c.Type;
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
                    break;
                case ComponentType.Circuit:
                    break;
                case ComponentType.Microcontroller:
                    draw = Draws.Microcontroller;
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

            Component component = new Component(draw, c.Center);
            component.Rotation = c.Rotation;
            component.Type = c.Type;

            return component;
        }
    }
    [Serializable]
    public class Wire_Project {
        public Point_Project From;
        public Component_Project FromComponent;
        public int FromIndex;
        public Color_Project Color;
        public Point_Project To;
        public Component_Project ToComponent;
        public int ToIndex;
        
        public Wire_Project(Wire wire, FileProject project) {
            From = wire.From;
            if (wire.FromComponent != null) {
                int i = UIStatics.Circuito.Components.IndexOf(wire.FromComponent);
                FromComponent = project.Components[i];
                FromIndex = wire.FromIndex;
            } else {
                FromIndex = -1;
            }
            To = wire.To;
            if (wire.ToComponent != null) {
                int i = UIStatics.Circuito.Components.IndexOf(wire.ToComponent);
                ToComponent = project.Components[i];
                ToIndex = wire.ToIndex;
            } else {
                ToIndex = -1;
            }
            Color = Draws.Color_off;
        }
        public Wire ToWire(FileProject project) {
            Wire wire = new Wire(From, To);
            if(FromComponent != null) {
                int index = -1;
                for (int i = 0; i < project.Components.Length; i++) {
                    if(FromComponent == project.Components[i]) {
                        index = i;
                        break;
                    }
                }
                if (index == -1) throw new Exception("Invalid component.");
                wire.FromComponent = UIStatics.Circuito.Components[index];
                wire.FromIndex = FromIndex;
            } else {
                FromIndex = -1;
            }
            if (ToComponent != null) {
                int index = -1;
                for (int i = 0; i < project.Components.Length; i++) {
                    if (ToComponent == project.Components[i]) {
                        index = i;
                        break;
                    }
                }
                if (index == -1) throw new Exception("Invalid component.");
                wire.ToComponent = UIStatics.Circuito.Components[index];
                wire.ToIndex = ToIndex;
            } else {
                FromIndex = -1;
            }
            return wire;
        }
    }
}
