using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
        private bool wantClose = false;
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
            int total = tableLayoutPanel1.RowCount;
            for (int i = 1; i < total; i++) {
                tableLayoutPanel1.RowStyles.RemoveAt(i);
            }


            Size oldSize = Size;
            Size = new Size(250, 100);
            for (int i = 0; i < 256; i++) {
                Label label = new Label();
                label.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                label.Text = i.ToString("X2");

                Components.DataField field = new Components.DataField();
                field.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                field.comboBox1.SelectedIndex = 2;
                field.Selected = Components.DataFieldType.HEX;
                if (UIStatics.Simulador == null) {
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
        private bool SimuladorLastStopped = (UIStatics.Simulador == null || UIStatics.Simulador.Stopped);
        private static byte[] RAMTemp = new byte[256];
        private static byte[] StackTemp = new byte[256];

        private void Run_thread() {
            while (!wantClose) {
                try {
                    if(SimuladorLastStopped && (UIStatics.Simulador != null)) {
                        for (int i = 0; i < 256; i++) {
                            if (type == FormRamType.RAM) {
                                UIStatics.Simulador.Ram[i] = RAMTemp[i];
                            } else {
                                UIStatics.Simulador.Stack[i] = StackTemp[i];
                            }
                        }
                    }
                    SimuladorLastStopped = (UIStatics.Simulador == null || UIStatics.Simulador.Stopped);
                    if (UIStatics.Simulador != null) {
                        if (UIStatics.Simulador.Running) {
                            for (int i = 0; i < Fields.Count; i++) {
                                Fields[i].Value = type == FormRamType.RAM ? UIStatics.Simulador.Ram[i] : UIStatics.Simulador.Stack[i];
                                Fields[i].Refresh();
                            }
                        }
                        Thread.Sleep(SLEEP);

                        for (int i = 0; i < Fields.Count; i++) {
                            if (type == FormRamType.RAM) {
                                if (Fields[i].UserInput) UIStatics.Simulador.Ram[i] = (byte)Fields[i].Value;
                                RAMTemp[i] = UIStatics.Simulador.Ram[i];
                            } else {
                                if (Fields[i].UserInput) UIStatics.Simulador.Stack[i] = (byte)Fields[i].Value;
                                StackTemp[i] = UIStatics.Simulador.Stack[i];
                            }
                            Fields[i].UserInput = false;
                        }
                    } else {
                        for (int i = 0; i < Fields.Count; i++) {
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
