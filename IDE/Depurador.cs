using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ScintillaNET;

namespace IDE {
    public partial class Depurador : UserControl {
        public int[] AddressToLine;
        public int[] LineToAddress;
        public int Frequency = 1;
        public bool FrequencyLimiter = true;
        public bool InternalSimulation = false;
        public bool ChangedToCompile = false;

        private double freq = 1;
        private int power = 0;
        private FormRomMemory formRomMemory;
        private FormRamMemory formRamMemory;
        private FormRamMemory formStackMemory;



        private List<Components.Component> Components;

        public Depurador() {
            InitializeComponent();
            UIStatics.ScintillaSetStyle(scintilla, true);
            scintilla.CallTipSetPosition(true);
            programCounter.comboBox1.SelectedIndex = 2;
            programCounter.Selected = IDE.Components.DataFieldType.HEX;
            stackPointer.comboBox1.SelectedIndex = 2;
            stackPointer.Selected = IDE.Components.DataFieldType.HEX;
            
            aField.comboBox1.SelectedIndex = 2;
            aField.Selected = IDE.Components.DataFieldType.HEX;
            bField.comboBox1.SelectedIndex = 2;
            bField.Selected = IDE.Components.DataFieldType.HEX;
            cField.comboBox1.SelectedIndex = 2;
            cField.Selected = IDE.Components.DataFieldType.HEX;
            dField.comboBox1.SelectedIndex = 2;
            dField.Selected = IDE.Components.DataFieldType.HEX;
            eField.comboBox1.SelectedIndex = 2;
            eField.Selected = IDE.Components.DataFieldType.HEX;
        }

        public void SetText(string text) {
            scintilla.ReadOnly = false;
            scintilla.Text = text;
            scintilla.ReadOnly = true;
        }

        public void AddBreakpoint(Line line = null) {
            try { 
                if(line == null) {
                    line = scintilla.Lines[scintilla.CurrentLine];
                }
                if (UIStatics.Simulador != null) {
                    if (UIStatics.Simulador.Program != null) {
                        for (int i = 0; i < AddressToLine.Length; i++) {
                            if(AddressToLine[i] == line.Index) {
                                if(UIStatics.Simulador.Program[i] != null)
                                    UIStatics.Simulador.Program[i].HasBreakpoint = true;
                            }
                        }
                    }
                }
                line.MarkerAdd(UIStatics.BREAKPOINT_MARKER);
            } catch (Exception e) {
                UIStatics.ShowExceptionMessage(e);
            }
        }
        public void RemoveBreakpoint(Line line = null) {
            try {
                if (line == null) {
                    line = scintilla.Lines[scintilla.CurrentLine];
                }
                if (UIStatics.Simulador != null) {
                    if (UIStatics.Simulador.Program != null) {
                        for (int i = 0; i < AddressToLine.Length; i++) {
                            if (AddressToLine[i] == line.Index) {
                                if (UIStatics.Simulador.Program[i] != null)
                                    UIStatics.Simulador.Program[i].HasBreakpoint = false;
                            }
                        }
                    }
                }
                line.MarkerDelete(UIStatics.BREAKPOINT_MARKER);
            } catch (Exception e) {
                UIStatics.ShowExceptionMessage(e);
            }
        }
        public void RemoveAllBreakpoint() {
            foreach(Line line in scintilla.Lines) {
                RemoveBreakpoint(line);
            }
        }

        public void AddLabel(int lineNumber) {
            Line line = scintilla.Lines[lineNumber];
            line.MarkerAdd(UIStatics.LABEL_MARKER);
        }
        public void RemoveAllLabels() {
            foreach (Line line in scintilla.Lines) {
                line.MarkerDelete(UIStatics.LABEL_MARKER);
            }
        }

        private void scintilla_MarginClick(object sender, MarginClickEventArgs e) {
            
            if(e.Margin == UIStatics.BREAKPOINT_INDEX_MARGIN) {

                const uint mask = (1 << UIStatics.BREAKPOINT_MARKER);

                Line line = scintilla.Lines[scintilla.LineFromPosition(e.Position)];

                if((line.MarkerGet() & mask) > 0) {
                    RemoveBreakpoint(line);
                } else {
                    AddBreakpoint(line);
                }
                
            } else if(e.Margin == UIStatics.LABEL_MARGIN) {
                
                const uint mask = (1 << UIStatics.LABEL_MARKER);
                
                Line line = scintilla.Lines[scintilla.LineFromPosition(e.Position)];
                if ((line.MarkerGet() & mask) > 0) {
                    int linePos = scintilla.LineFromPosition(e.Position);
                    for (int i = 0; i < AddressToLine.Length; i++) {
                        if(AddressToLine[i] == linePos) {
                            string text = null;
                            foreach (M3PlusMicrocontroller.Label item in UIStatics.Compilador.Labels) {
                                if(item.Address == i) {
                                    if (text != null)
                                        text = text += "\n";
                                    text += item.Name + ":";
                                }
                            }
                            scintilla.CallTipShow(e.Position, text);
                            break;
                        }
                    }
                }

            }
        }
        private void scintilla_TextChanged(object sender, EventArgs e) {
            updateLineNumber();
        }

        public void GotoNextBreakpoint() {
            var line = scintilla.LineFromPosition(scintilla.CurrentPosition);
            var nextLine = scintilla.Lines[++line].MarkerNext(1 << UIStatics.BREAKPOINT_MARKER);
            if(nextLine != -1)
                scintilla.Lines[nextLine].Goto();
        }
        public void GotoPreviousBreakpoint() {
            var line = scintilla.LineFromPosition(scintilla.CurrentPosition);
            var prevLine = scintilla.Lines[--line].MarkerPrevious(1 << UIStatics.BREAKPOINT_MARKER);
            if(prevLine != -1)
                scintilla.Lines[prevLine].Goto();
        }
        private void updateLineNumber(int padding = 2) {
            scintilla.Margins[0].Type = MarginType.RightText;
            int i;
            int max = 0;
            
            for (i = 0; i < scintilla.Lines.Count; i++) {
                scintilla.Lines[i].MarginStyle = Style.LineNumber;
                if (scintilla.Lines[i].Text != "") {
                    scintilla.Lines[i].MarginText = LineToAddress[i].ToString("X");
                    if (LineToAddress[i].ToString().Length > max)
                        max = LineToAddress[i].ToString().Length;
                } else {
                    scintilla.Lines[i].MarginText = "";
                }
            }
            try {
                scintilla.Lines[scintilla.Lines.Count - 1].MarginStyle = Style.LineNumber;
                scintilla.Lines[scintilla.Lines.Count - 1].MarginText = (LineToAddress[scintilla.Lines.Count - 2] + UIStatics.Simulador.Program[LineToAddress[scintilla.Lines.Count - 2]].Size).ToString("X");
            } catch (Exception) { }
            scintilla.Margins[0].Width = scintilla.TextWidth(Style.LineNumber, new string('9', max + 1)) + padding;
        }
        public void ZoomMore() {
            scintilla.ZoomIn();
            updateLineNumber();
        }
        public void ZoomLess() {
            scintilla.ZoomOut();
            updateLineNumber();
        }
        public void ZoomReset() {
            scintilla.Zoom = 0;
            updateLineNumber();
        }

        private void Depurador_Load(object sender, EventArgs e) {
            
        }

        public void UpdateAll() {
            Components = new List<Components.Component>();
            Components.Add(aField);
            Components.Add(bField);
            Components.Add(cField);
            Components.Add(dField);
            Components.Add(eField);
            Components.Add(in0Field);
            Components.Add(in1Field);
            Components.Add(in2Field);
            Components.Add(in3Field);
            Components.Add(out0Field);
            Components.Add(out1Field);
            Components.Add(out2Field);
            Components.Add(out3Field);
            Components.Add(stackPointer);
            Components.Add(cCheck);
            Components.Add(zCheck);
            Components.Add(programCounter);

            CheckForIllegalCrossThreadCalls = false;
            while (!UIStatics.WantExit) {
                try {
                    if (UIStatics.Simulador != null) {
                        if (UIStatics.Simulador.Running) {
                            try {
                                UIStatics.MainForm.ToolStripStatusLabel.Text = "Executando programa. Instrução atual: " + UIStatics.Simulador.Program[UIStatics.Simulador.NextInstruction].Text;
                            } catch (Exception) {
                                UIStatics.MainForm.ToolStripStatusLabel.Text = "Erro no programa.";
                            }
                        }
                        programCounter.Value = UIStatics.Simulador.NextInstruction;
                        stackPointer.Value = UIStatics.Simulador.PointerStack;

                        aField.Value = UIStatics.Simulador.Reg[0];
                        bField.Value = UIStatics.Simulador.Reg[1];
                        cField.Value = UIStatics.Simulador.Reg[2];
                        dField.Value = UIStatics.Simulador.Reg[3];
                        eField.Value = UIStatics.Simulador.Reg[4];
                        if (!ativarEdicao.Checked) {
                            in0Field.Value = UIStatics.Simulador.In[0];
                            in1Field.Value = UIStatics.Simulador.In[1];
                            in2Field.Value = UIStatics.Simulador.In[2];
                            in3Field.Value = UIStatics.Simulador.In[3];
                        }
                        out0Field.Value = UIStatics.Simulador.Out[0];
                        out1Field.Value = UIStatics.Simulador.Out[1];
                        out2Field.Value = UIStatics.Simulador.Out[2];
                        out3Field.Value = UIStatics.Simulador.Out[3];

                        cCheck.Value = UIStatics.Simulador.FlagC;
                        zCheck.Value = UIStatics.Simulador.FlagZ;

                        foreach (Components.Component item in Components) {
                            item.Refresh();
                        }

                        {
                            string inicio = "Frequência real: ";
                            string mod = " Hz";
                            float frequencia = UIStatics.Simulador.CurrentFrequency;
                            if (!internalSimulation.Checked) {
                                inicio = "IPS real:";
                                mod = " IPS";
                            }
                            if (frequencia >= 1000) {
                                frequencia = frequencia / 1000f;
                                mod = " kIPS";
                            }
                            if (frequencia >= 1000) {
                                frequencia = frequencia / 1000f;
                                mod = " MIPS";
                            }
                            frequencia = (float)(Math.Round(frequencia * 100) / 100);

                            realFrequency.Text = inicio + frequencia + mod;
                        }

                        int nextLine = scintilla.Lines[0].MarkerNext(1 << UIStatics.INDEX_MARKER);
                        int markerLine = AddressToLine[UIStatics.Simulador.NextInstruction];
                        if (nextLine != markerLine) {
                            if (nextLine != -1)
                                scintilla.Lines[nextLine].MarkerDelete(UIStatics.INDEX_MARKER);
                            scintilla.Lines[markerLine].MarkerAdd(UIStatics.INDEX_MARKER);
                        }

                        Thread.Sleep(30); //30ms to reaload to refresh the screen.

                        if (aField.UserInput) UIStatics.Simulador.Reg[0] = (byte)aField.Value;
                        if (bField.UserInput) UIStatics.Simulador.Reg[1] = (byte)bField.Value;
                        if (cField.UserInput) UIStatics.Simulador.Reg[2] = (byte)cField.Value;
                        if (dField.UserInput) UIStatics.Simulador.Reg[3] = (byte)dField.Value;
                        if (eField.UserInput) UIStatics.Simulador.Reg[4] = (byte)eField.Value;

                        if (ativarEdicao.Checked) {
                            UIStatics.Simulador.In[0] = (byte)in0Field.Value;
                            UIStatics.Simulador.In[1] = (byte)in1Field.Value;
                            UIStatics.Simulador.In[2] = (byte)in2Field.Value;
                            UIStatics.Simulador.In[3] = (byte)in3Field.Value;
                        }
                        /*
                        if (out0Field.UserInput) UIStatics.Simulador.Out[0] = (byte)out0Field.Value;
                        if (out1Field.UserInput) UIStatics.Simulador.Out[1] = (byte)out1Field.Value;
                        if (out2Field.UserInput) UIStatics.Simulador.Out[2] = (byte)out2Field.Value;
                        if (out3Field.UserInput) UIStatics.Simulador.Out[3] = (byte)out3Field.Value;
                        */
                        if (cCheck.UserInput) UIStatics.Simulador.FlagC = cCheck.Value;
                        if (zCheck.UserInput) UIStatics.Simulador.FlagZ = zCheck.Value;


                        foreach (Components.Component item in Components) {
                            item.UserInput = false;
                        }

                    } else {
                        Thread.Sleep(100);
                    }
                } catch (Exception e) {
                    Thread.Sleep(30);
                    if (UIStatics.Simulador != null) {
                        UIStatics.ShowExceptionMessage(e);
                    }
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e) {
            UIStatics.Run();
        }

        private void btnPause_Click(object sender, EventArgs e) {
            UIStatics.Pause();
        }

        private void btnStop_Click(object sender, EventArgs e) {
            UIStatics.Stop();
        }

        private void btnIn_Click(object sender, EventArgs e) {
            UIStatics.StepIn();
        }

        private void btnOut_Click(object sender, EventArgs e) {
            UIStatics.StepOut();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) {
            freq = (double) frequencyNumeric.Value;
            UpdateFrequency();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            power = frequencyCombo.SelectedIndex;
            UpdateFrequency();
        }
        private void frequencyActive_CheckedChanged(object sender, EventArgs e) {
            FrequencyLimiter = frequencyActive.Checked;
            UpdateFrequency();
        }
        private void UpdateFrequency() {
            Frequency = (int) (freq * Math.Pow(10, power*3));
            if(UIStatics.Simulador != null) {
                UIStatics.Simulador.FrequencyLimit = FrequencyLimiter;
                UIStatics.Simulador.Frequency = Frequency;
                UIStatics.Simulador.InternalSimulation = internalSimulation.Checked;
            }
        }

        private void abrirMemoriaRam_Click(object sender, EventArgs e) {
            if (formRamMemory == null || !formRamMemory.Visible) {
                formRamMemory = SplashScreen.OpenRAM(this);
            }
        }

        private void abrirMemoriaPilha_Click(object sender, EventArgs e) {
            if (formStackMemory == null || !formStackMemory.Visible) {
                formStackMemory = SplashScreen.OpenStack(this);
            }
        }

        private void internalSimulation_CheckedChanged(object sender, EventArgs e) {
            InternalSimulation = internalSimulation.Checked;
            if (internalSimulation.Checked) {
                frequencyActive.Checked = true;
                frequencyActive.Enabled = false;
                frequencyCombo.Items.Clear();
                frequencyCombo.Items.Add("Hz");
                frequencyCombo.Text = "Hz";
                frequencyCombo.SelectedIndex = 0;
                if (frequencyNumeric.Value > 20.00M)
                    frequencyNumeric.Value = 20.00M;
                frequencyNumeric.Maximum = 20.00M;
                freq = (double)frequencyNumeric.Value;
                power = frequencyCombo.SelectedIndex;
                UpdateFrequency();

            } else {
                frequencyActive.Enabled = true;
                frequencyCombo.Items.Clear();
                frequencyCombo.Items.Add("IPS");
                frequencyCombo.Items.Add("kIPS");
                frequencyCombo.Items.Add("MIPS");
                frequencyCombo.Text = "IPS";
                frequencyCombo.SelectedIndex = 0;
                frequencyNumeric.Maximum = 999.99M;
                freq = (double)frequencyNumeric.Value;
                power = frequencyCombo.SelectedIndex;
                UpdateFrequency();
            }
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e) {

        }

        private void scintilla_Leave(object sender, EventArgs e) {
        }

        private void scintilla_Click(object sender, EventArgs e) {
            int line = scintilla.LineFromPosition(scintilla.CurrentPosition);
            if (UIStatics.Simulador != null) {
                UIStatics.MainForm.ToolStripStatusLabel.Text = UIStatics.Simulador.Program[LineToAddress[line]].Description;
            }
        }

        private void abrirMemoriaROM_Click(object sender, EventArgs e) {
            if (formRomMemory == null || !formRomMemory.Visible) {
                formRomMemory = SplashScreen.OpenROM(this);
            }
        }
    }
}
