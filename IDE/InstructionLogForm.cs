using System;
using System.Threading;
using System.Windows.Forms;

namespace IDE {
    public partial class InstructionLogForm : Form {
        public InstructionLogForm() {
            InitializeComponent();
        }
        private Thread thread;
        private bool closing;
        private void InstructionLogForm_Load(object sender, EventArgs e) {
            thread = new Thread(UpdateThread);
            thread.Start();
        }

        private void UpdateThread() {
            while(!UiStatics.WantExit && !closing) {
                textBox1.Text = UiStatics.Circuito.InstructionLog.ToString();
                Thread.Sleep(20);
            }
        }

        private void InstructionLogForm_FormClosing(object sender, FormClosingEventArgs e) {
            closing = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
