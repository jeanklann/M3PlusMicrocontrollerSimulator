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

        private void Circuito_Load(object sender, EventArgs e) {

        }
        private float zoom = 1;
        private void Circuito_Paint(object sender, PaintEventArgs e) {
            Graphics g;
            g = CreateGraphics();
            g.ScaleTransform(zoom, zoom);
            Pen myPen = new Pen(Color.Green, 1);
            Rectangle myRectangle = new Rectangle(1, 1, 10, 10);
            g.DrawEllipse(myPen, myRectangle);
            

        }

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
    }
}
