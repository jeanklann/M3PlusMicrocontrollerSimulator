using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;



namespace IDE {
    public partial class Circuito : GLControl {
        public MouseProps MouseProps = new MouseProps();
        public Terminals Terminals = new Terminals();
        public List<Wire> Wires = new List<Wire>();
        public Wire HoverWire;
        public Wire SelectedWire;
        public Component Over;
        public Component Selected;
        

        public PointF Position = new PointF();
        public List<Component> Components = new List<Component>();
        public Circuito() {
            InitializeComponent();
        }

        private float zoom = 1;
        
        internal void ZoomMore() {
            if (zoom < 1)
                zoom *= 2;
            else
                zoom += 1;
            Circuito_Resize(this, null);
            Refresh();
        }

        internal void ZoomLess() {
            if (zoom <= 1)
                zoom /= 2;
            else
                zoom--;
            Circuito_Resize(this, null);
            Refresh();
        }

        internal void ZoomReset() {
            zoom = 1;
            Circuito_Resize(this, null);
            Refresh();
        }

        public override void Refresh() {
            base.Refresh();
            if (MouseProps.Button2Pressed) {
                Position = new PointF(Position.X+((MouseProps.LastPosition.X - MouseProps.CurrentPosition.X)/zoom), Position.Y-((MouseProps.LastPosition.Y - MouseProps.CurrentPosition.Y)/zoom));
            }
            bool foundComponent = false;
            bool foundTerminal = false;

            PointF worldMousePos = MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, zoom);
            foreach (Component item in Components) {
                int index = item.IsOnTerminal(worldMousePos);
                if (index >= 0) {
                    Terminals.HoverIndex = index;
                    Terminals.HoverComponent = item;
                    Over = null;
                    foundTerminal = true;
                    break;
                }
                if (item.IsInside(worldMousePos)){
                    Over = item;
                    foundComponent = true;
                    break;
                }
            }
            if (!foundComponent) Over = null;
            if (!foundTerminal) {
                Terminals.HoverComponent = null;
            }

            if (MouseProps.Button1Pressed) {
                if (((MouseProps.LastDownPosition.X - MouseProps.CurrentPosition.X) * (MouseProps.LastDownPosition.X - MouseProps.CurrentPosition.X) +
                    (MouseProps.LastDownPosition.Y - MouseProps.CurrentPosition.Y) * (MouseProps.LastDownPosition.Y - MouseProps.CurrentPosition.Y)) > 10*10 ) {
                    if (Selected != null) {
                        Selected.Center.X -= (MouseProps.LastPosition.X - MouseProps.CurrentPosition.X) / zoom;
                        Selected.Center.Y += (MouseProps.LastPosition.Y - MouseProps.CurrentPosition.Y) / zoom;
                    }
                }
                if(Terminals.FromComponent != null) {
                    terminalFromVertex = Terminals.FromComponent.TransformTerminal(Terminals.FromIndex);
                    terminalFromVertex.X += Terminals.FromComponent.Center.X;
                    terminalFromVertex.Y += Terminals.FromComponent.Center.Y;
                    terminalToVertex = MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, zoom);
                }
            }
            bool foundWire = false;
            
            foreach (Wire item in Wires) {
                if (item.FromComponent != null) {
                    item.From = item.FromComponent.TransformTerminal(item.FromIndex);
                    item.From.X += item.FromComponent.Center.X;
                    item.From.Y += item.FromComponent.Center.Y;
                }
                if (item.ToComponent != null) {
                    item.To = item.ToComponent.TransformTerminal(item.ToIndex);
                    item.To.X += item.ToComponent.Center.X;
                    item.To.Y += item.ToComponent.Center.Y;
                }
                
                if (!MouseProps.Button1Pressed && !MouseProps.Button2Pressed) {
                    if (OnWire(item.From, item.To, worldMousePos)) {
                        HoverWire = item;
                        foundWire = true;
                        break;
                    }
                }
            }
            if (!foundWire) HoverWire = null;


        }
        private PointF terminalFromVertex;
        private PointF terminalToVertex;
        private void Render() {
            Matrix4 lookat = Matrix4.LookAt(Position.X, Position.Y, 1, Position.X, Position.Y, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (zoom > 1f) {
                GL.LineWidth(zoom);
            } else {
                GL.LineWidth(1);
            }
            foreach (Component item in Components) {
                GL.Translate(item.Center.X, item.Center.Y, 0);
                GL.Rotate(item.Rotation, 0, 0, 1);
                GL.CallList(item.Draw.DisplayListHandle);
                foreach (Point terminal in item.Draw.Terminals) {
                    GL.Translate(terminal.X, terminal.Y, 0);
                    GL.CallList(Draws.TerminalHandle);
                    GL.Translate(-terminal.X, -terminal.Y, 0);
                }
                GL.Rotate(-item.Rotation, 0, 0, 1);
                GL.Translate(-item.Center.X, -item.Center.Y, 0);
            }

            if (MouseProps.Button1Pressed) {
                if(Terminals.FromComponent != null) {
                    GL.Color3(Color.Black);
                    GL.Begin(BeginMode.Lines);
                    GL.Vertex2(terminalFromVertex.X, terminalFromVertex.Y);
                    GL.Vertex2(terminalToVertex.X, terminalToVertex.Y);
                    GL.End();
                }
            }
            DrawWires();
            DrawBoxes();

            SwapBuffers();
            
        }

        private void Circuito_Load(object sender, EventArgs e) {
            GL.ClearColor(BackColor);
            Application.Idle += Application_Idle;
            Draws.Load();
            Components.Add(new Component(Draws.Input[0], new PointF(0, 0)));
            Components.Add(new Component(Draws.Input[0], new PointF(40, 20)));
            Components.Add(new Component(Draws.Input[0], new PointF(200, 0)));
            Components.Add(new Component(Draws.Input[0], new PointF(0, -100)));
            Components.Add(new Component(Draws.Input[1], new PointF(100, 100)));
            Components.Add(new Component(Draws.Input[1], new PointF(100, 800)));
        }
        void Application_Idle(object sender, EventArgs e) {
            while (IsIdle) {
                Render();
            }
        }
        
        private void Circuito_Resize(object sender, EventArgs e) {
            GLControl c = sender as GLControl;

            if (c.ClientSize.Height == 0)
                c.ClientSize = new Size(c.ClientSize.Width, 1);

            GL.Viewport(0, 0, c.ClientSize.Width, c.ClientSize.Height);
            Matrix4 orthographic = Matrix4.CreateOrthographic(c.ClientSize.Width/zoom, c.ClientSize.Height/zoom, 1, 10);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref orthographic);
        }

        private void Circuito_Paint(object sender, PaintEventArgs e) {
            Render();
        }

        private void Circuito_MouseDoubleClick(object sender, MouseEventArgs e) {
            MouseProps.LastDoubleClickPosition = e.Location;
            Refresh();
        }

        private void Circuito_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                MouseProps.LastClickPosition = e.Location;
            }
            Refresh();
        }

        private void Circuito_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) {
                MouseProps.LastDownPosition = e.Location;
                if (e.Button == MouseButtons.Left) {
                    MouseProps.Button1Pressed = true;
                    Selected = null;
                    if(Over != null)
                        Selected = Over;
                    if(Terminals.HoverComponent != null) {
                        Terminals.FromComponent = Terminals.HoverComponent;
                        Terminals.FromIndex = Terminals.HoverIndex;
                    } else {
                        Terminals.FromComponent = null;
                        Terminals.ToComponent = null;
                    }
                    if(HoverWire != null) {
                        SelectedWire = HoverWire;
                    } else {
                        SelectedWire = null;
                    }
                } else {
                    MouseProps.Button2Pressed = true;
                }
            }
            Refresh();
        }

        private void Circuito_MouseMove(object sender, MouseEventArgs e) {
            MouseProps.LastPosition = MouseProps.CurrentPosition;
            MouseProps.CurrentPosition = e.Location;
            Refresh();
        }

        private void Circuito_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) {
                MouseProps.LastUpPosition = e.Location;
                if (e.Button == MouseButtons.Left) {
                    MouseProps.Button1Pressed = false;
                } else {
                    MouseProps.Button2Pressed = false;
                }
            }
            if (Terminals.FromComponent != null) {
                if (Terminals.HoverComponent != null) {
                    if (Terminals.HoverComponent != Terminals.FromComponent || Terminals.HoverIndex != Terminals.FromIndex) {
                        Wires.Add(new Wire(Terminals.FromComponent, Terminals.FromIndex, Terminals.HoverComponent, Terminals.HoverIndex));
                    }
                } else {
                    Wires.Add(new Wire(Terminals.FromComponent, Terminals.FromIndex, MouseProps.ToWorld(MouseProps.LastUpPosition, ClientSize, Position, zoom)));
                }
            }
            Refresh();
        }

        private void Circuito_MouseLeave(object sender, EventArgs e) {
            /*
            MouseProps.Button1Pressed = false;
            MouseProps.Button2Pressed = false;
            */
            Refresh();
        }

        private void DrawWires() {
            foreach (Wire item in Wires) {
                if (item == HoverWire) {
                    GL.Color3(Color.Green);
                } else if (item == SelectedWire) {
                    GL.Color3(Color.Orange);
                } else {
                    GL.Color3(Color.Black);
                }
                GL.Begin(BeginMode.Lines);
                GL.Vertex2(item.From.X, item.From.Y);
                GL.Vertex2(item.To.X, item.To.Y);
                GL.End();
            }
        }
        private const int BoxPlusFactor = 5;
        private void DrawBoxes() {
            if (Terminals.HoverComponent != null) {
                GL.Color3(Color.Green);
                Vector3 pos = new Vector3(Terminals.HoverComponent.Center.X, Terminals.HoverComponent.Center.Y, 0);
                PointF transform = Terminals.HoverComponent.TransformTerminal(Terminals.HoverIndex);
                pos.X += transform.X;
                pos.Y += transform.Y;
                GL.Translate(pos);
                GL.CallList(Draws.TerminalHandle);
                GL.Translate(-pos);
            }
            if (Selected != null) { 
                GL.Color3(Color.Orange);
                GL.LineWidth(1);
                GL.LineStipple(1, 0xAAAA); // dashed line
                GL.Translate(Selected.Center.X, Selected.Center.Y, 0);
                GL.Rotate(Selected.Rotation, 0, 0, 1);
                GL.Begin(BeginMode.LineLoop);
                GL.Vertex2(- Selected.Draw.Width / 2f - BoxPlusFactor, - Selected.Draw.Height / 2f - BoxPlusFactor);
                GL.Vertex2(- Selected.Draw.Width / 2f - BoxPlusFactor, Selected.Draw.Height / 2f + BoxPlusFactor);
                GL.Vertex2(Selected.Draw.Width / 2f + BoxPlusFactor, Selected.Draw.Height / 2f + BoxPlusFactor);
                GL.Vertex2(Selected.Draw.Width / 2f + BoxPlusFactor, - Selected.Draw.Height / 2f - BoxPlusFactor);
                GL.End();
                GL.Rotate(-Selected.Rotation, 0, 0, 1);
                GL.Translate(-Selected.Center.X, -Selected.Center.Y, 0);
            }
            if(Over != null) {
                GL.Color3(Color.Green);
                GL.LineWidth(1);
                GL.LineStipple(1, 0xAAAA); // dashed line
                GL.Translate(Over.Center.X, Over.Center.Y, 0);
                GL.Rotate(Over.Rotation, 0, 0, 1);
                GL.Begin(BeginMode.LineLoop);
                GL.Vertex2(- Over.Draw.Width/2f - BoxPlusFactor, - Over.Draw.Height/ 2f - BoxPlusFactor);
                GL.Vertex2(- Over.Draw.Width / 2f - BoxPlusFactor, + Over.Draw.Height / 2f + BoxPlusFactor);
                GL.Vertex2(Over.Draw.Width / 2f + BoxPlusFactor, Over.Draw.Height / 2f + BoxPlusFactor);
                GL.Vertex2(Over.Draw.Width / 2f + BoxPlusFactor, - Over.Draw.Height / 2f - BoxPlusFactor);
                GL.End();
                GL.Rotate(-Over.Rotation, 0, 0, 1);
                GL.Translate(-Over.Center.X, -Over.Center.Y, 0);
            }
            
        }

        private void Circuito_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
            if(e.KeyChar == 'r') {
                if(Selected!= null) { 
                    Selected.Rotation += 90;
                    if (Selected.Rotation >= 360) Selected.Rotation -= 360;
                }
            }
            Refresh();
        }

        private static bool OnWire(PointF point1, PointF point2, PointF point) {
            //Calculando a equação reta
            float coefAng = (point2.Y - point1.Y) / (point2.X - point1.X);
            float coefLin = -point1.X * coefAng + point1.Y;
            //calculando a reta perpendicar passando no ponto
            float coefAngPar = -1 / coefAng * coefLin;
            float coefLinPar = point.Y - coefAngPar * point.X;
            //calculando o ponto de interceção das duas retas
            float x = (coefLin - coefLinPar) / (coefAngPar - coefAng);
            float y = x * coefAng + coefLin;
            //pegando os valores máximos
            float maxX = point1.X > point2.X ? point1.X : point2.X;
            float minX = point1.X > point2.X ? point2.X : point1.X;
            float maxY = point1.Y > point2.Y ? point1.Y : point2.Y;
            float minY = point1.Y > point2.Y ? point2.Y : point1.Y;
            
            
            //verifica se o ponto Y passa dos limites da linha
            if (y < minY) {
                y = minY;
                x = y / coefAng - coefLin;
            } else if (y > maxY) {
                y = maxY;
                x = y / coefAng - coefLin;
            }
            

            //verifica se o ponto X passa dos limites da linha
            if (x < minX) {
                x = minX;
                y = x * coefAng + coefLin;
            } else if (x > maxX) {
                x = maxX;
                y = x * coefAng + coefLin;
            }

            //Verifica se é uma reta totalmente paralela ao eixo y
            if (float.IsNaN(x) || float.IsNaN(y)) {
                x = point1.X;
                y = point.Y > maxY ? maxY : (point.Y < minY ? minY : point.Y);
            }

            float maiorX = x > point.X ? x : point.X;
            float menorX = x > point.X ? point.X : x;
            float maiorY = y > point.Y ? y : point.Y;
            float menorY = y > point.Y ? point.Y : y;

            Console.WriteLine((maiorX - menorX) * (maiorX - menorX) + (maiorY - menorY) * (maiorY - menorY));

            //calcula a distancia
            if ((maiorX - menorX) * (maiorX - menorX) + (maiorY - menorY) * (maiorY - menorY) < 5 * 5) {
                return true;
            }
            return false;
        }
    }
    public static class Draws {
        public static int TerminalHandle;

        public static ComponentDraw[] Input;
        public static ComponentDraw[] Output;


        public static Color Color_on = Color.Red;
        public static Color Color_off = Color.Black;
        public static Color Color_3rd = Color.Gray;

        public static void Load() {
            GenInput();
            GenTerminal();

        }
        private static void GenTerminal() {
            TerminalHandle = GL.GenLists(1);
            GL.NewList(TerminalHandle, ListMode.Compile);

            GL.Begin(BeginMode.LineLoop);
            GL.Vertex2(-2, -2);
            GL.Vertex2(-2, 2);
            GL.Vertex2(2, 2);
            GL.Vertex2(2, -2);
            GL.End();

            GL.EndList();
        }
        private static void GenInput() {
            Input = new ComponentDraw[2];
            Input[0] = new ComponentDraw(GL.GenLists(1), 15, 10, 1);
            Input[0].Terminals[0] = new Point(10, 0);
            GL.NewList(Input[0].DisplayListHandle, ListMode.Compile);

            GL.Begin(BeginMode.LineLoop);
            GL.Color3(Color_off);
            GL.Vertex2(-5, -5);
            GL.Vertex2(-5, 5);
            GL.Vertex2(5, 5);
            GL.Vertex2(5, -5);
            GL.End();
            
            GL.Begin(BeginMode.Quads);
            GL.Color3(Color_off);
            GL.Vertex2(-3, -3);
            GL.Vertex2(-3, 3);
            GL.Vertex2(3, 3);
            GL.Vertex2(3, -3);
            GL.End();

            GL.Begin(BeginMode.Lines);
            GL.Color3(Color_off);
            GL.Vertex2(5, 0);
            GL.Vertex2(10, 0);
            GL.End();
            
            GL.EndList();

            Input[1] = new ComponentDraw(GL.GenLists(1), 15, 10, 1);
            Input[1].Terminals[0] = new Point(10, 0);
            GL.NewList(Input[1].DisplayListHandle, ListMode.Compile);

            GL.Begin(BeginMode.LineLoop);
            GL.Color3(Color_off);
            GL.Vertex2(-5, -5);
            GL.Vertex2(-5, 5);
            GL.Vertex2(5, 5);
            GL.Vertex2(5, -5);
            GL.End();

            GL.Begin(BeginMode.Quads);
            GL.Color3(Color_on);
            GL.Vertex2(-3, -3);
            GL.Vertex2(-3, 3);
            GL.Vertex2(3, 3);
            GL.Vertex2(3, -3);
            GL.End();

            GL.Begin(BeginMode.Lines);
            GL.Color3(Color_on);
            GL.Vertex2(5, 0);
            GL.Vertex2(10, 0);
            GL.End();

            GL.EndList();
        }

    }
    public class ComponentDraw {
        public int DisplayListHandle;
        public int Width;
        public int Height;
        public Point[] Terminals;

        public ComponentDraw(int displayListHandle, int width, int height, int terminals = 1) {
            DisplayListHandle = displayListHandle;
            Width = width;
            Height = height;
            Terminals = new Point[terminals];
        }
    }

    public class Component {
        public ComponentDraw Draw;
        public PointF Center;
        public float Rotation = 0;

        public Component(ComponentDraw draw, PointF center) {
            Draw = draw;
            Center = center;
        }

        public bool IsInside(PointF Point) {
            RectangleF rectangle = new RectangleF(new PointF(Center.X- Draw.Width/2f, Center.Y - Draw.Height / 2f), new SizeF(Draw.Width, Draw.Height));
            rectangle.Inflate(2, 2);
            return rectangle.Contains(Point);
        }

        public int IsOnTerminal(PointF Point) {
            for (int i = 0; i < Draw.Terminals.Length; i++) {
                PointF terminal = TransformTerminal(i);
                float distance = (Center.X + terminal.X - Point.X) * (Center.X + terminal.X - Point.X) + (Center.Y + terminal.Y - Point.Y) * (Center.Y + terminal.Y - Point.Y);
                if(distance <= 4*4) {
                    return i;
                }
            }
            
            return -1;
        }

        public PointF TransformTerminal(int i) {
            double RotDegree = Math.PI * Rotation / 180.0;
            PointF point = new PointF(
                (float)(Draw.Terminals[i].X * Math.Cos(RotDegree) - Draw.Terminals[i].Y * Math.Sin(RotDegree)),
                (float)(Draw.Terminals[i].X * Math.Sin(RotDegree) + Draw.Terminals[i].Y * Math.Cos(RotDegree))
            );
            return point;
        }

    }
    public class Wire {
        public PointF From;
        public Component FromComponent;
        public int FromIndex;

        public PointF To;
        public Component ToComponent;
        public int ToIndex;

        public Wire(PointF from, PointF to) {
            From = from;
            To = to;
        }
        public Wire(Component from, int indexFrom, Component to, int indexTo) {
            FromComponent = from;
            FromIndex = indexFrom;
            ToComponent = to;
            ToIndex = indexTo;
            From = from.TransformTerminal(indexFrom);
            From.X += from.Center.X;
            From.Y += from.Center.Y;
            To = to.TransformTerminal(indexTo);
            To.X += to.Center.X;
            To.Y += to.Center.Y;
        }
        public Wire(Component from, int indexFrom, PointF to) {
            FromComponent = from;
            FromIndex = indexFrom;
            From = from.TransformTerminal(indexFrom);
            From.X += from.Center.X;
            From.Y += from.Center.Y;
            To = to;
        }
        public Wire(PointF from, Component to, int indexTo) {
            From = from;
            ToComponent = to;
            ToIndex = indexTo;
            To = to.TransformTerminal(indexTo);
            To.X += to.Center.X;
            To.Y += to.Center.Y;
        }
    }
    public struct Terminals {
        public int FromIndex;
        public Component FromComponent;
        public int ToIndex;
        public Component ToComponent;
        public int HoverIndex;
        public Component HoverComponent;
    }
    public struct MouseProps {
        public Point CurrentPosition;
        public Point LastPosition;
        public Point LastClickPosition;
        public Point LastDoubleClickPosition;
        public Point LastDownPosition;
        public Point LastUpPosition;
        public int Delta;
        public bool Button1Pressed;
        public bool Button2Pressed;

        public static PointF ToWorld(Point Point, Size ClientSize, PointF Position, float Zoom) {
            PointF worldPos = new PointF();

            worldPos.X = Point.X - ClientSize.Width / 2f;
            worldPos.Y = - Point.Y + ClientSize.Height/ 2f;

            worldPos.X = worldPos.X / Zoom;
            worldPos.Y = worldPos.Y / Zoom;

            worldPos.X += Position.X;
            worldPos.Y += Position.Y;

            return worldPos;
        }
    }
}
