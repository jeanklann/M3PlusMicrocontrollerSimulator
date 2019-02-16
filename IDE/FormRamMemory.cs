using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace IDE {
    public partial class FormRamMemory : Form {
        public const int Height = 30;
        public const int Sleep = 30;
        public FormRamMemory() {
            InitializeComponent();
        }
        public List<Components.DataField> Fields = new List<Components.DataField>();
        private Thread _thread;
        private bool _wantClose;
        private FormRamType _type;

        public void Build(FormRamType type) {
            this._type = type;
            switch (type) {
                case FormRamType.Ram:
                    Text = "Visualizador e editor da memória RAM";
                    break;
                case FormRamType.Stack:
                    Text = "Visualizador e editor da memória de pilha";
                    break;
            }
            Fields.Clear();
            tableLayoutPanel1.RowStyles[0].Height = Height;
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
                field.Selected = Components.DataFieldType.Hex;
                if (UiStatics.Simulador == null) {
                    if (type == FormRamType.Ram) {
                        field.Value = _ramTemp[i];
                    } else {
                        field.Value = _stackTemp[i];
                    }
                }
                field.Refresh();
                Fields.Add(field);

                tableLayoutPanel1.RowCount = tableLayoutPanel1.RowCount + 1;

                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, Height));
                tableLayoutPanel1.Controls.Add(label, 0, tableLayoutPanel1.RowCount - 1);
                tableLayoutPanel1.Controls.Add(field, 1, tableLayoutPanel1.RowCount - 1);
                
                Application.DoEvents();
            }
            Size = oldSize;
            loading.Hide();


            _thread = new Thread(Run_thread);
            _thread.Start();
        }
        private bool _simuladorLastStopped = (UiStatics.Simulador == null || UiStatics.Simulador.Stopped);
        private static byte[] _ramTemp = new byte[256];
        private static byte[] _stackTemp = new byte[256];

        private void Run_thread() {
            while (!_wantClose) {
                try {
                    if(_simuladorLastStopped && (UiStatics.Simulador != null)) {
                        for (var i = 0; i < 256; i++) {
                            if (_type == FormRamType.Ram) {
                                UiStatics.Simulador.Ram[i] = _ramTemp[i];
                            } else {
                                UiStatics.Simulador.Stack[i] = _stackTemp[i];
                            }
                        }
                    }
                    _simuladorLastStopped = (UiStatics.Simulador == null || UiStatics.Simulador.Stopped);
                    if (UiStatics.Simulador != null) {
                        if (UiStatics.Simulador.Running) {
                            for (var i = 0; i < Fields.Count; i++) {
                                Fields[i].Value = _type == FormRamType.Ram ? UiStatics.Simulador.Ram[i] : UiStatics.Simulador.Stack[i];
                                Fields[i].Refresh();
                            }
                        }
                        Thread.Sleep(Sleep);

                        for (var i = 0; i < Fields.Count; i++) {
                            if (_type == FormRamType.Ram) {
                                if (Fields[i].UserInput) UiStatics.Simulador.Ram[i] = (byte)Fields[i].Value;
                                _ramTemp[i] = UiStatics.Simulador.Ram[i];
                            } else {
                                if (Fields[i].UserInput) UiStatics.Simulador.Stack[i] = (byte)Fields[i].Value;
                                _stackTemp[i] = UiStatics.Simulador.Stack[i];
                            }
                            Fields[i].UserInput = false;
                        }
                    } else {
                        for (var i = 0; i < Fields.Count; i++) {
                            if (_type == FormRamType.Ram)
                                _ramTemp[i] = (byte) Fields[i].Value;
                            else
                                _stackTemp[i] = (byte) Fields[i].Value;
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
            _wantClose = true;
        }
    }
    public enum FormRamType {
        Ram, Stack
    }
}
