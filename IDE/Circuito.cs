using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace IDE {
    public partial class Circuito : UserControl {
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
            */

        }
    }
}
