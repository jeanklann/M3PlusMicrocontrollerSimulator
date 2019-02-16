using System;
using System.Threading;
using System.Windows.Forms;

namespace IDE {
    public partial class InstructionLogForm : Form {
        public InstructionLogForm() {
            InitializeComponent();
        }
        private Thread _thread;
        private bool _closing;
        private void InstructionLogForm_Load(object sender, EventArgs e) {
            _thread = new Thread(UpdateThread);
            _thread.Start();
        }

        private void UpdateThread() {
            while(!UiStatics.WantExit && !_closing) {
                textBox1.Text = UiStatics.Circuito.InstructionLog.ToString();
                Thread.Sleep(20);
            }
        }

        private void InstructionLogForm_FormClosing(object sender, FormClosingEventArgs e) {
            _closing = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
