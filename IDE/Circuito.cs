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
            bool found = false;
            foreach (Component item in Components) {
                if(item.IsInside(MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, zoom))){
                    found = true;
                    Over = item;
                    break;
                }
            }
            if (!found) Over = null;

            if (MouseProps.Button1Pressed) {
                if (((MouseProps.LastDownPosition.X - MouseProps.CurrentPosition.X) * (MouseProps.LastDownPosition.X - MouseProps.CurrentPosition.X) +
                    (MouseProps.LastDownPosition.Y - MouseProps.CurrentPosition.Y) * (MouseProps.LastDownPosition.Y - MouseProps.CurrentPosition.Y)) > 10*10 ) {
                    if (Selected != null) {
                        Selected.Center.X -= (MouseProps.LastPosition.X - MouseProps.CurrentPosition.X) / zoom;
                        Selected.Center.Y += (MouseProps.LastPosition.Y - MouseProps.CurrentPosition.Y) / zoom;
                    }
                }
            }
        }
        
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
            Refresh();
        }

        private void Circuito_MouseLeave(object sender, EventArgs e) {
            /*
            MouseProps.Button1Pressed = false;
            MouseProps.Button2Pressed = false;
            */
            Refresh();
        }
        private const int BoxPlusFactor = 5;
        private void DrawBoxes() {
            if(Selected != null) { 
                GL.Color3(Color.Orange);
                GL.LineWidth(1);
                GL.LineStipple(1, 0xAAAA); // dashed line
                GL.Begin(BeginMode.LineLoop);
                GL.Vertex2(Selected.Center.X - Selected.Draw.Width / 2f - BoxPlusFactor, Selected.Center.Y - Selected.Draw.Height / 2f - BoxPlusFactor);
                GL.Vertex2(Selected.Center.X - Selected.Draw.Width / 2f - BoxPlusFactor, Selected.Center.Y + Selected.Draw.Height / 2f + BoxPlusFactor);
                GL.Vertex2(Selected.Center.X + Selected.Draw.Width / 2f + BoxPlusFactor, Selected.Center.Y + Selected.Draw.Height / 2f + BoxPlusFactor);
                GL.Vertex2(Selected.Center.X + Selected.Draw.Width / 2f + BoxPlusFactor, Selected.Center.Y - Selected.Draw.Height / 2f - BoxPlusFactor);
                GL.End();
            }
            if(Over != null) {
                GL.Color3(Color.Green);
                GL.LineWidth(1);
                GL.LineStipple(1, 0xAAAA); // dashed line
                GL.Begin(BeginMode.LineLoop);
                GL.Vertex2(Over.Center.X - Over.Draw.Width/2f - BoxPlusFactor, Over.Center.Y - Over.Draw.Height/ 2f - BoxPlusFactor);
                GL.Vertex2(Over.Center.X - Over.Draw.Width / 2f - BoxPlusFactor, Over.Center.Y + Over.Draw.Height / 2f + BoxPlusFactor);
                GL.Vertex2(Over.Center.X + Over.Draw.Width / 2f + BoxPlusFactor, Over.Center.Y + Over.Draw.Height / 2f + BoxPlusFactor);
                GL.Vertex2(Over.Center.X + Over.Draw.Width / 2f + BoxPlusFactor, Over.Center.Y - Over.Draw.Height / 2f - BoxPlusFactor);
                GL.End();
            }
        }

        private void Circuito_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
            if(e.KeyChar == 'r') {
                if(Selected!= null) { 
                    Selected.Rotation += 90;
                    if (Selected.Rotation >= 360) Selected.Rotation -= 360;
                }
            }
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

        public PointF TransformTerminal(int i) {
            PointF point = new PointF(
                (float)(Draw.Terminals[i].X * Math.Cos(Rotation) - Draw.Terminals[i].Y * Math.Sin(Rotation)),
                (float)(Draw.Terminals[i].X * Math.Sin(Rotation) + Draw.Terminals[i].Y * Math.Cos(Rotation))
            );
            return point;
        }
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
