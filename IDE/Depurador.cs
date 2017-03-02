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

        public Depurador() {
            InitializeComponent();
            UIStatics.ScintillaSetStyle(scintilla, true);
            scintilla.CallTipSetPosition(true);
        }

        public void SetText(string text) {
            scintilla.ReadOnly = false;
            scintilla.Text = text;
            scintilla.ReadOnly = true;
        }

        public void AddBreakpoint(Line line = null) {
            if(line == null) {
                line = scintilla.Lines[scintilla.CurrentLine];
            }
            line.MarkerAdd(UIStatics.BREAKPOINT_MARKER);
        }
        public void RemoveBreakpoint(Line line = null) {
            if(line == null) {
                line = scintilla.Lines[scintilla.CurrentLine];
            }
            line.MarkerDelete(UIStatics.BREAKPOINT_MARKER);
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
                    
                    //toolTip1.Show("Label ", this);
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
            var maxLineNumberCharLength = scintilla.Lines.Count.ToString().Length;
            scintilla.Margins[0].Width = scintilla.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + padding;
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
            CheckForIllegalCrossThreadCalls = false;
            while (!UIStatics.WantExit) {
                if (UIStatics.Simulador != null) {

                    aField.Value = UIStatics.Simulador.Reg[0];
                    bField.Value = UIStatics.Simulador.Reg[1];
                    cField.Value = UIStatics.Simulador.Reg[2];
                    dField.Value = UIStatics.Simulador.Reg[3];
                    eField.Value = UIStatics.Simulador.Reg[4];

                    IN4Field.Value = UIStatics.Simulador.In[0];
                    in1Field.Value = UIStatics.Simulador.In[1];
                    in2Field.Value = UIStatics.Simulador.In[2];
                    in3Field.Value = UIStatics.Simulador.In[3];

                    OUT4Field.Value = UIStatics.Simulador.Out[0];
                    out1Field.Value = UIStatics.Simulador.Out[1];
                    out2Field.Value = UIStatics.Simulador.Out[2];
                    out3Field.Value = UIStatics.Simulador.Out[3];

                    cCheck.Value = UIStatics.Simulador.Flag_C;
                    zCheck.Value = UIStatics.Simulador.Flag_Z;

                    aField.Refresh();
                    bField.Refresh();
                    cField.Refresh();
                    dField.Refresh();
                    eField.Refresh();
                    IN4Field.Refresh();
                    in1Field.Refresh();
                    in2Field.Refresh();
                    in3Field.Refresh();
                    OUT4Field.Refresh();
                    out1Field.Refresh();
                    out2Field.Refresh();
                    out3Field.Refresh();
                    cCheck.Refresh();
                    zCheck.Refresh();

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

                    if (IN4Field.UserInput) UIStatics.Simulador.In[0] = (byte)IN4Field.Value;
                    if (in1Field.UserInput) UIStatics.Simulador.In[1] = (byte)in1Field.Value;
                    if (in2Field.UserInput) UIStatics.Simulador.In[2] = (byte)in2Field.Value;
                    if (in3Field.UserInput) UIStatics.Simulador.In[3] = (byte)in3Field.Value;

                    if (OUT4Field.UserInput) UIStatics.Simulador.Out[0] = (byte)OUT4Field.Value;
                    if (out1Field.UserInput) UIStatics.Simulador.Out[1] = (byte)out1Field.Value;
                    if (out2Field.UserInput) UIStatics.Simulador.Out[2] = (byte)out2Field.Value;
                    if (out3Field.UserInput) UIStatics.Simulador.Out[3] = (byte)out3Field.Value;

                    if (cCheck.UserInput) UIStatics.Simulador.Flag_C = cCheck.Value;
                    if (zCheck.UserInput) UIStatics.Simulador.Flag_Z = zCheck.Value;

                    
                } else {
                    Thread.Sleep(100);
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
    }
}
