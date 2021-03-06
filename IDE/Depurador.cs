﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using IDE.Components;
using ScintillaNET;

namespace IDE
{
    public partial class Depurador : UserControl
    {
        private List<IComponent> _components;
        private FormRamMemory _formRamMemory;
        private FormRomMemory _formRomMemory;
        private FormRamMemory _formStackMemory;

        private double _freq = 1;
        private int _power;
        public int[] AddressToLine;
        public bool ChangedToCompile = false;
        public int Frequency = 1;
        public bool FrequencyLimiter = true;
        public bool InternalSimulation;
        public int[] LineToAddress;

        public Depurador()
        {
            InitializeComponent();
            UiStatics.ScintillaSetStyle(scintilla, true);
            scintilla.CallTipSetPosition(true);
            programCounter.comboBox1.SelectedIndex = 2;
            programCounter.Selected = DataFieldType.Hex;
            stackPointer.comboBox1.SelectedIndex = 2;
            stackPointer.Selected = DataFieldType.Hex;

            aField.comboBox1.SelectedIndex = 2;
            aField.Selected = DataFieldType.Hex;
            bField.comboBox1.SelectedIndex = 2;
            bField.Selected = DataFieldType.Hex;
            cField.comboBox1.SelectedIndex = 2;
            cField.Selected = DataFieldType.Hex;
            dField.comboBox1.SelectedIndex = 2;
            dField.Selected = DataFieldType.Hex;
            eField.comboBox1.SelectedIndex = 2;
            eField.Selected = DataFieldType.Hex;
        }

        public void SetText(string text)
        {
            scintilla.ReadOnly = false;
            scintilla.Text = text;
            scintilla.ReadOnly = true;
        }

        public void AddBreakpoint(Line line = null)
        {
            try
            {
                if (line == null) line = scintilla.Lines[scintilla.CurrentLine];
                if (UiStatics.Simulador != null)
                    if (UiStatics.Simulador.Program != null)
                    {
                        for (var i = 0; i < AddressToLine.Length; i++)
                        {
                            if (AddressToLine[i] == line.Index)
                            {
                                if (UiStatics.Simulador.Program[i] != null)
                                    UiStatics.Simulador.Program[i].HasBreakpoint = true;
                            }
                        }
                    }

                line.MarkerAdd(UiStatics.BreakpointMarker);
            }
            catch (Exception e)
            {
                UiStatics.ShowExceptionMessage(e);
            }
        }

        public void RemoveBreakpoint(Line line = null)
        {
            try
            {
                if (line == null) line = scintilla.Lines[scintilla.CurrentLine];
                if (UiStatics.Simulador != null)
                    if (UiStatics.Simulador.Program != null)
                    {
                        for (var i = 0; i < AddressToLine.Length; i++)
                        {
                            if (AddressToLine[i] == line.Index)
                            {
                                if (UiStatics.Simulador.Program[i] != null)
                                    UiStatics.Simulador.Program[i].HasBreakpoint = false;
                            }
                        }
                    }

                line.MarkerDelete(UiStatics.BreakpointMarker);
            }
            catch (Exception e)
            {
                UiStatics.ShowExceptionMessage(e);
            }
        }

        public void RemoveAllBreakpoint()
        {
            foreach (var line in scintilla.Lines) RemoveBreakpoint(line);
        }

        public void AddLabel(int lineNumber)
        {
            var line = scintilla.Lines[lineNumber];
            line.MarkerAdd(UiStatics.LabelMarker);
        }

        public void RemoveAllLabels()
        {
            foreach (var line in scintilla.Lines) line.MarkerDelete(UiStatics.LabelMarker);
        }

        private void scintilla_MarginClick(object sender, MarginClickEventArgs e)
        {
            if (e.Margin == UiStatics.BreakpointIndexMargin)
            {
                const uint mask = 1 << UiStatics.BreakpointMarker;

                var line = scintilla.Lines[scintilla.LineFromPosition(e.Position)];

                if ((line.MarkerGet() & mask) > 0)
                    RemoveBreakpoint(line);
                else
                    AddBreakpoint(line);
            }
            else if (e.Margin == UiStatics.LabelMargin)
            {
                const uint mask = 1 << UiStatics.LabelMarker;

                var line = scintilla.Lines[scintilla.LineFromPosition(e.Position)];
                if ((line.MarkerGet() & mask) > 0)
                {
                    var linePos = scintilla.LineFromPosition(e.Position);
                    for (var i = 0; i < AddressToLine.Length; i++)
                    {
                        if (AddressToLine[i] == linePos)
                        {
                            string text = null;
                            foreach (var item in UiStatics.Compilador.Labels)
                            {
                                if (item.Address == i)
                                {
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

        private void scintilla_TextChanged(object sender, EventArgs e)
        {
            UpdateLineNumber();
        }

        public void GotoNextBreakpoint()
        {
            var line = scintilla.LineFromPosition(scintilla.CurrentPosition);
            var nextLine = scintilla.Lines[++line].MarkerNext(1 << UiStatics.BreakpointMarker);
            if (nextLine != -1)
                scintilla.Lines[nextLine].Goto();
        }

        public void GotoPreviousBreakpoint()
        {
            var line = scintilla.LineFromPosition(scintilla.CurrentPosition);
            var prevLine = scintilla.Lines[--line].MarkerPrevious(1 << UiStatics.BreakpointMarker);
            if (prevLine != -1)
                scintilla.Lines[prevLine].Goto();
        }

        private void UpdateLineNumber(int padding = 2)
        {
            scintilla.Margins[0].Type = MarginType.RightText;
            int i;
            var max = 0;

            for (i = 0; i < scintilla.Lines.Count; i++)
            {
                scintilla.Lines[i].MarginStyle = Style.LineNumber;
                if (scintilla.Lines[i].Text != "")
                {
                    scintilla.Lines[i].MarginText = LineToAddress[i].ToString("X");
                    if (LineToAddress[i].ToString().Length > max)
                        max = LineToAddress[i].ToString().Length;
                }
                else
                {
                    scintilla.Lines[i].MarginText = "";
                }
            }

            try
            {
                scintilla.Lines[scintilla.Lines.Count - 1].MarginStyle = Style.LineNumber;
                scintilla.Lines[scintilla.Lines.Count - 1].MarginText =
                    (LineToAddress[scintilla.Lines.Count - 2] +
                     UiStatics.Simulador.Program[LineToAddress[scintilla.Lines.Count - 2]].Size).ToString("X");
            }
            catch (Exception)
            {
            }

            scintilla.Margins[0].Width = scintilla.TextWidth(Style.LineNumber, new string('9', max + 1)) + padding;
        }

        public void ZoomMore()
        {
            scintilla.ZoomIn();
            UpdateLineNumber();
        }

        public void ZoomLess()
        {
            scintilla.ZoomOut();
            UpdateLineNumber();
        }

        public void ZoomReset()
        {
            scintilla.Zoom = 0;
            UpdateLineNumber();
        }

        private void Depurador_Load(object sender, EventArgs e)
        {
        }

        public void UpdateAll()
        {
            _components = new List<IComponent>();
            _components.Add(aField);
            _components.Add(bField);
            _components.Add(cField);
            _components.Add(dField);
            _components.Add(eField);
            _components.Add(in0Field);
            _components.Add(in1Field);
            _components.Add(in2Field);
            _components.Add(in3Field);
            _components.Add(out0Field);
            _components.Add(out1Field);
            _components.Add(out2Field);
            _components.Add(out3Field);
            _components.Add(stackPointer);
            _components.Add(cCheck);
            _components.Add(zCheck);
            _components.Add(programCounter);

            CheckForIllegalCrossThreadCalls = false;
            while (!UiStatics.WantExit)
                try
                {
                    if (UiStatics.Simulador != null)
                    {
                        if (UiStatics.Simulador.Running)
                            try
                            {
                                UiStatics.MainForm.ToolStripStatusLabel.Text =
                                    "Executando programa. Instrução atual: " + UiStatics.Simulador
                                        .Program[UiStatics.Simulador.NextInstruction].Text;
                            }
                            catch (Exception)
                            {
                                UiStatics.MainForm.ToolStripStatusLabel.Text = "Erro no programa.";
                            }

                        programCounter.Value = UiStatics.Simulador.NextInstruction;
                        stackPointer.Value = UiStatics.Simulador.PointerStack;

                        aField.Value = UiStatics.Simulador.Reg[0];
                        bField.Value = UiStatics.Simulador.Reg[1];
                        cField.Value = UiStatics.Simulador.Reg[2];
                        dField.Value = UiStatics.Simulador.Reg[3];
                        eField.Value = UiStatics.Simulador.Reg[4];
                        if (!ativarEdicao.Checked)
                        {
                            in0Field.Value = UiStatics.Simulador.In[0];
                            in1Field.Value = UiStatics.Simulador.In[1];
                            in2Field.Value = UiStatics.Simulador.In[2];
                            in3Field.Value = UiStatics.Simulador.In[3];
                        }

                        out0Field.Value = UiStatics.Simulador.Out[0];
                        out1Field.Value = UiStatics.Simulador.Out[1];
                        out2Field.Value = UiStatics.Simulador.Out[2];
                        out3Field.Value = UiStatics.Simulador.Out[3];

                        cCheck.Value = UiStatics.Simulador.FlagC;
                        zCheck.Value = UiStatics.Simulador.FlagZ;

                        foreach (var item in _components) item.Refresh();

                        {
                            var inicio = "Frequência real: ";
                            var mod = " Hz";
                            float frequencia = UiStatics.Simulador.CurrentFrequency;
                            if (!internalSimulation.Checked)
                            {
                                inicio = "IPS real:";
                                mod = " IPS";
                            }

                            if (frequencia >= 1000)
                            {
                                frequencia = frequencia / 1000f;
                                mod = " kIPS";
                            }

                            if (frequencia >= 1000)
                            {
                                frequencia = frequencia / 1000f;
                                mod = " MIPS";
                            }

                            frequencia = (float) (Math.Round(frequencia * 100) / 100);

                            realFrequency.Text = inicio + frequencia + mod;
                        }

                        var nextLine = scintilla.Lines[0].MarkerNext(1 << UiStatics.IndexMarker);
                        var markerLine = AddressToLine[UiStatics.Simulador.NextInstruction];
                        if (nextLine != markerLine)
                        {
                            if (nextLine != -1)
                                scintilla.Lines[nextLine].MarkerDelete(UiStatics.IndexMarker);
                            scintilla.Lines[markerLine].MarkerAdd(UiStatics.IndexMarker);
                        }

                        Thread.Sleep(30); //30ms to reaload to refresh the screen.

                        if (aField.UserInput) UiStatics.Simulador.Reg[0] = (byte) aField.Value;
                        if (bField.UserInput) UiStatics.Simulador.Reg[1] = (byte) bField.Value;
                        if (cField.UserInput) UiStatics.Simulador.Reg[2] = (byte) cField.Value;
                        if (dField.UserInput) UiStatics.Simulador.Reg[3] = (byte) dField.Value;
                        if (eField.UserInput) UiStatics.Simulador.Reg[4] = (byte) eField.Value;

                        if (ativarEdicao.Checked)
                        {
                            UiStatics.Simulador.In[0] = (byte) in0Field.Value;
                            UiStatics.Simulador.In[1] = (byte) in1Field.Value;
                            UiStatics.Simulador.In[2] = (byte) in2Field.Value;
                            UiStatics.Simulador.In[3] = (byte) in3Field.Value;
                        }

                        /*
                        if (out0Field.UserInput) UIStatics.Simulador.Out[0] = (byte)out0Field.Value;
                        if (out1Field.UserInput) UIStatics.Simulador.Out[1] = (byte)out1Field.Value;
                        if (out2Field.UserInput) UIStatics.Simulador.Out[2] = (byte)out2Field.Value;
                        if (out3Field.UserInput) UIStatics.Simulador.Out[3] = (byte)out3Field.Value;
                        */
                        if (cCheck.UserInput) UiStatics.Simulador.FlagC = cCheck.Value;
                        if (zCheck.UserInput) UiStatics.Simulador.FlagZ = zCheck.Value;


                        foreach (var item in _components) item.UserInput = false;
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
                catch (Exception e)
                {
                    Thread.Sleep(30);
                    if (UiStatics.Simulador != null) UiStatics.ShowExceptionMessage(e);
                }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            UiStatics.Run();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            UiStatics.Pause();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            UiStatics.Stop();
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            UiStatics.StepIn();
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            UiStatics.StepOut();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _freq = (double) frequencyNumeric.Value;
            UpdateFrequency();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _power = frequencyCombo.SelectedIndex;
            UpdateFrequency();
        }

        private void frequencyActive_CheckedChanged(object sender, EventArgs e)
        {
            FrequencyLimiter = frequencyActive.Checked;
            UpdateFrequency();
        }

        private void UpdateFrequency()
        {
            Frequency = (int) (_freq * Math.Pow(10, _power * 3));
            if (UiStatics.Simulador != null)
            {
                UiStatics.Simulador.FrequencyLimit = FrequencyLimiter;
                UiStatics.Simulador.Frequency = Frequency;
                UiStatics.Simulador.InternalSimulation = internalSimulation.Checked;
            }
        }

        private void abrirMemoriaRam_Click(object sender, EventArgs e)
        {
            if (_formRamMemory == null || !_formRamMemory.Visible) _formRamMemory = SplashScreen.OpenRam(this);
        }

        private void abrirMemoriaPilha_Click(object sender, EventArgs e)
        {
            if (_formStackMemory == null || !_formStackMemory.Visible) _formStackMemory = SplashScreen.OpenStack(this);
        }

        private void internalSimulation_CheckedChanged(object sender, EventArgs e)
        {
            InternalSimulation = internalSimulation.Checked;
            if (internalSimulation.Checked)
            {
                frequencyActive.Checked = true;
                frequencyActive.Enabled = false;
                frequencyCombo.Items.Clear();
                frequencyCombo.Items.Add("Hz");
                frequencyCombo.Text = "Hz";
                frequencyCombo.SelectedIndex = 0;
                if (frequencyNumeric.Value > 20.00M)
                    frequencyNumeric.Value = 20.00M;
                frequencyNumeric.Maximum = 20.00M;
                _freq = (double) frequencyNumeric.Value;
                _power = frequencyCombo.SelectedIndex;
                UpdateFrequency();
            }
            else
            {
                frequencyActive.Enabled = true;
                frequencyCombo.Items.Clear();
                frequencyCombo.Items.Add("IPS");
                frequencyCombo.Items.Add("kIPS");
                frequencyCombo.Items.Add("MIPS");
                frequencyCombo.Text = "IPS";
                frequencyCombo.SelectedIndex = 0;
                frequencyNumeric.Maximum = 999.99M;
                _freq = (double) frequencyNumeric.Value;
                _power = frequencyCombo.SelectedIndex;
                UpdateFrequency();
            }
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
        }

        private void scintilla_Leave(object sender, EventArgs e)
        {
        }

        private void scintilla_Click(object sender, EventArgs e)
        {
            var line = scintilla.LineFromPosition(scintilla.CurrentPosition);
            if (UiStatics.Simulador != null)
                UiStatics.MainForm.ToolStripStatusLabel.Text =
                    UiStatics.Simulador.Program[LineToAddress[line]].Description;
        }

        private void abrirMemoriaROM_Click(object sender, EventArgs e)
        {
            if (_formRomMemory == null || !_formRomMemory.Visible) _formRomMemory = SplashScreen.OpenRom(this);
        }
    }
}