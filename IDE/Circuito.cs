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
        public PointF Position = new PointF();
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

            GL.CallList(Draws.Input[1].DisplayListHandle);
            

            SwapBuffers();
            
        }

        private void Circuito_Load(object sender, EventArgs e) {
            GL.ClearColor(BackColor);
            Application.Idle += Application_Idle;
            Draws.Load();
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
    }
    public static class Draws {

        public static Component[] Input;
        public static Component[] Output;


        public static Color Color_on = Color.Red;
        public static Color Color_off = Color.Black;
        public static Color Color_3rd = Color.Gray;

        public static void Load() {
            GenInput();

        }

        private static void GenInput() {
            Input = new Component[2];

            Input[0].Width = 15;
            Input[0].Height = 10;
            Input[0].DisplayListHandle = GL.GenLists(1);
            GL.NewList(Input[0].DisplayListHandle, ListMode.Compile);

            GL.Begin(BeginMode.LineStrip);
            GL.Color3(Color_off);
            GL.Vertex2(-5, -5);
            GL.Vertex2(-5, 5);
            GL.Vertex2(5, 5);
            GL.Vertex2(5, -5);
            GL.Vertex2(-5, -5);
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

            Input[1].Width = 15;
            Input[1].Height = 10;
            Input[1].DisplayListHandle = GL.GenLists(1);
            GL.NewList(Input[1].DisplayListHandle, ListMode.Compile);

            GL.Begin(BeginMode.LineStrip);
            GL.Color3(Color_off);
            GL.Vertex2(-5, -5);
            GL.Vertex2(-5, 5);
            GL.Vertex2(5, 5);
            GL.Vertex2(5, -5);
            GL.Vertex2(-5, -5);
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
    public struct Component {
        public int DisplayListHandle;
        public int Width;
        public int Height;
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
    }
}
