using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;



namespace IDE {
    public partial class Circuito : GLControl {
        public Circuito() {
            InitializeComponent();
        }

        private float zoom = 1;

        internal void ZoomMore() {
            if (zoom < 1)
                zoom *= 2;
            else
                zoom += 1;
            Refresh();
        }

        internal void ZoomLess() {
            if (zoom <= 1)
                zoom /= 2;
            else
                zoom--;
            Refresh();
        }

        internal void ZoomReset() {
            zoom = 1;
            Refresh();
        }
        
        
        private void Render() {
            /*
            Matrix4 lookat = Matrix4.LookAt(0, 0, 5, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);


            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            GL.CallList(Draws.Input[1].DisplayListHandle);
            

            SwapBuffers();
            */
        }

        private void Circuito_Load(object sender, EventArgs e) {
            //GL.ClearColor(Color.White);
            //Application.Idle += Application_Idle;
            //Draws.Load();
        }
        void Application_Idle(object sender, EventArgs e) {
            while (IsIdle) {
                Render();
            }
        }

        private void Circuito_Resize(object sender, EventArgs e) {
            /*
            GLControl c = sender as GLControl;

            if (c.ClientSize.Height == 0)
                c.ClientSize = new Size(c.ClientSize.Width, 1);

            GL.Viewport(0, 0, c.ClientSize.Width, c.ClientSize.Height);

            Matrix4 orthographic = Matrix4.CreateOrthographic(c.ClientSize.Width, c.ClientSize.Height, 1, 10);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref orthographic);*/
        }

        private void Circuito_Paint(object sender, PaintEventArgs e) {
            //Render();
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
            GL.Vertex2(-3, 4);
            GL.Vertex2(3, 4);
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
            GL.Vertex2(-3, 4);
            GL.Vertex2(3, 4);
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
}
