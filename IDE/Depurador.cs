using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;

namespace IDE {
    public partial class Depurador : UserControl {
        
        public Depurador() {
            InitializeComponent();
            UIStatics.ScintillaSetStyle(scintilla);

            scintilla.ReadOnly = false;
            scintilla.Text = "asfasfafa\ngsdngjsdgnsd\nofjdsgnsdgn\ngdsngdnsj\nsdgj";
            scintilla.ReadOnly = true;

            Line l = scintilla.Lines[2];
            l.MarkerAdd(2);
            
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

        private void scintilla_MarginClick(object sender, MarginClickEventArgs e) {
            if(e.Margin == UIStatics.BREAKPOINT_MARGIN) {
                // Do we have a marker for this line?
                const uint mask = (1 << UIStatics.BREAKPOINT_MARKER);
                Line line = scintilla.Lines[scintilla.LineFromPosition(e.Position)];

                if((line.MarkerGet() & mask) > 0) {
                    // Remove existing bookmark
                    RemoveBreakpoint(line);
                } else {
                    // Add bookmark
                    AddBreakpoint(line);
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
    }
}
