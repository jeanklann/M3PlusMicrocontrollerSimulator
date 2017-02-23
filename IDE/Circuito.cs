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
            VSync = true;
        }

        private void Circuito_Load(object sender, EventArgs e) {
            /*
            GL.ClearColor(Color.MidnightBlue);
            GL.Enable(EnableCap.DepthTest);
            Circuito_Resize(this, EventArgs.Empty);*/
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

        void Application_Idle(object sender, EventArgs e) {
            while (IsIdle) {
                Render();
            }
        }

        private void Circuito_MouseDown(object sender, MouseEventArgs e) {

        }

        private void Circuito_MouseUp(object sender, MouseEventArgs e) {

        }

        private void Circuito_MouseClick(object sender, MouseEventArgs e) {

        }

        private void Circuito_MouseDoubleClick(object sender, MouseEventArgs e) {

        }

        private void Circuito_MouseMove(object sender, MouseEventArgs e) {

        }
        

        private void Circuito_Paint(object sender, PaintEventArgs e) {
            Render();
            
        }
        private void Render() {
            Matrix4 lookat = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);


            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Begin(BeginMode.Quads);
            GL.Color3(Color.Blue);
            GL.Vertex3(new Vector3(0, 0, 0));
            GL.Vertex3(new Vector3(10, 0, 0));
            GL.Vertex3(new Vector3(10, 10, 0));
            GL.Vertex3(new Vector3(00, 10, 0));
            GL.End();

            SwapBuffers();

        }

        private void Circuito_Resize(object sender, EventArgs e) {
            OpenTK.GLControl c = sender as OpenTK.GLControl;

            if (c.ClientSize.Height == 0)
                c.ClientSize = new System.Drawing.Size(c.ClientSize.Width, 1);

            GL.Viewport(0, 0, c.ClientSize.Width, c.ClientSize.Height);

            float aspect_ratio = Width / (float)Height;
            Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perpective);
        }
    }
}
