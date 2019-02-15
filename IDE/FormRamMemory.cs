using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace IDE {
    public partial class FormRamMemory : Form {
        public const int HEIGHT = 30;
        public const int SLEEP = 30;
        public FormRamMemory() {
            InitializeComponent();
        }
        public List<Components.DataField> Fields = new List<Components.DataField>();
        private Thread thread;
        private bool wantClose;
        private FormRamType type;

        public void Build(FormRamType type) {
            this.type = type;
            switch (type) {
                case FormRamType.RAM:
                    Text = "Visualizador e editor da memória RAM";
                    break;
                case FormRamType.Stack:
                    Text = "Visualizador e editor da memória de pilha";
                    break;
            }
            Fields.Clear();
            tableLayoutPanel1.RowStyles[0].Height = HEIGHT;
            var total = tableLayoutPanel1.RowCount;
            for (var i = 1; i < total; i++) {
                tableLayoutPanel1.RowStyles.RemoveAt(i);
            }


            var oldSize = Size;
            Size = new Size(250, 100);
            for (var i = 0; i < 256; i++) {
                var label = new Label();
                label.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                label.Text = i.ToString("X2");

                var field = new Components.DataField();
                field.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                field.comboBox1.SelectedIndex = 2;
                field.Selected = Components.DataFieldType.HEX;
                if (UiStatics.Simulador == null) {
                    if (type == FormRamType.RAM) {
                        field.Value = RAMTemp[i];
                    } else {
                        field.Value = StackTemp[i];
                    }
                }
                field.Refresh();
                Fields.Add(field);

                tableLayoutPanel1.RowCount = tableLayoutPanel1.RowCount + 1;

                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, HEIGHT));
                tableLayoutPanel1.Controls.Add(label, 0, tableLayoutPanel1.RowCount - 1);
                tableLayoutPanel1.Controls.Add(field, 1, tableLayoutPanel1.RowCount - 1);
                
                Application.DoEvents();
            }
            Size = oldSize;
            loading.Hide();


            thread = new Thread(Run_thread);
            thread.Start();
        }
        private bool SimuladorLastStopped = (UiStatics.Simulador == null || UiStatics.Simulador.Stopped);
        private static byte[] RAMTemp = new byte[256];
        private static byte[] StackTemp = new byte[256];

        private void Run_thread() {
            while (!wantClose) {
                try {
                    if(SimuladorLastStopped && (UiStatics.Simulador != null)) {
                        for (var i = 0; i < 256; i++) {
                            if (type == FormRamType.RAM) {
                                UiStatics.Simulador.Ram[i] = RAMTemp[i];
                            } else {
                                UiStatics.Simulador.Stack[i] = StackTemp[i];
                            }
                        }
                    }
                    SimuladorLastStopped = (UiStatics.Simulador == null || UiStatics.Simulador.Stopped);
                    if (UiStatics.Simulador != null) {
                        if (UiStatics.Simulador.Running) {
                            for (var i = 0; i < Fields.Count; i++) {
                                Fields[i].Value = type == FormRamType.RAM ? UiStatics.Simulador.Ram[i] : UiStatics.Simulador.Stack[i];
                                Fields[i].Refresh();
                            }
                        }
                        Thread.Sleep(SLEEP);

                        for (var i = 0; i < Fields.Count; i++) {
                            if (type == FormRamType.RAM) {
                                if (Fields[i].UserInput) UiStatics.Simulador.Ram[i] = (byte)Fields[i].Value;
                                RAMTemp[i] = UiStatics.Simulador.Ram[i];
                            } else {
                                if (Fields[i].UserInput) UiStatics.Simulador.Stack[i] = (byte)Fields[i].Value;
                                StackTemp[i] = UiStatics.Simulador.Stack[i];
                            }
                            Fields[i].UserInput = false;
                        }
                    } else {
                        for (var i = 0; i < Fields.Count; i++) {
                            if (type == FormRamType.RAM)
                                RAMTemp[i] = (byte) Fields[i].Value;
                            else
                                StackTemp[i] = (byte) Fields[i].Value;
                        }
                        Thread.Sleep(100);
                    }
                } catch (Exception) { }
            }
            foreach (var item in Fields) {
                item.Dispose();
            }
            Fields.Clear();
        }

        private void FormRamMemory_FormClosing(object sender, FormClosingEventArgs e) {
            wantClose = true;
        }
    }
    public enum FormRamType {
        RAM, Stack
    }
}
