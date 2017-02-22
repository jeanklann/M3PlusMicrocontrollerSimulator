using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;

namespace IDE {
    public partial class FormularioPrincipal : Form {
        private const int BOOKMARK_MARGIN = 1; // Conventionally the symbol margin
        private const int BOOKMARK_MARKER = 3; // Arbitrary. Any valid index would work.

        public FormularioPrincipal() {
            InitializeComponent();
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e) {
            
        }
        

        private void scintilla_Click(object sender, EventArgs e) {

        }

        private void copierToolStripMenuItem_Click(object sender, EventArgs e) {
            
        }

        private void verPróximoToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.GotoNextBreakpoint();
        }

        private void verAnteriorToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.GotoPreviousBreakpoint();
        }

        private void removerTodosToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.RemoveAllBreakpoint();
        }

        private void adicionarNaLinhaToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.AddBreakpoint();
        }

        private void removerNaLinhaToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.RemoveBreakpoint();
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void codigo1_Load(object sender, EventArgs e) {

        }

        private void códigoToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.Visible = true;
            depurador1.Visible = false;
            //circuito1.Visible = false;
        }

        private void depuraçãoToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.Visible = false;
            depurador1.Visible = true;
            //circuito1.Visible = false;
        }

        private void circuitoToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.Visible = false;
            depurador1.Visible = false;
            //circuito1.Visible = true;
        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.ZoomMore();
            depurador1.ZoomMore();
            //circuito1.ZoomMore();
        }

        private void zoomToolStripMenuItem1_Click(object sender, EventArgs e) {
            codigo1.ZoomLess();
            depurador1.ZoomLess();
            //circuito1.ZoomLess();
        }

        private void zoomOriginalToolStripMenuItem_Click(object sender, EventArgs e) {
            codigo1.ZoomReset();
            depurador1.ZoomReset();
            //circuito1.ZoomReset();
        }
    }
}
