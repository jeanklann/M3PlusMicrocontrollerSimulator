using System;
using System.Windows.Forms;
using ScintillaNET;

namespace IDE
{
    public partial class Codigo : UserControl
    {
        public bool Changed;

        public Codigo()
        {
            InitializeComponent();
            UiStatics.ScintillaSetStyle(scintilla);
        }

        public void AddBreakpoint(Line line = null)
        {
            try
            {
                if (line == null) line = scintilla.Lines[scintilla.CurrentLine];
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

        private void scintilla_MarginClick(object sender, MarginClickEventArgs e)
        {
            if (e.Margin == UiStatics.BreakpointIndexMargin)
            {
                // Do we have a marker for this line?
                const uint mask = 1 << UiStatics.BreakpointMarker;
                var line = scintilla.Lines[scintilla.LineFromPosition(e.Position)];

                if ((line.MarkerGet() & mask) > 0)
                    RemoveBreakpoint(line);
                else
                    AddBreakpoint(line);
            }
        }

        private void scintilla_TextChanged(object sender, EventArgs e)
        {
            UpdateLineNumber();
            Changed = true;
            UiStatics.Depurador.ChangedToCompile = true;
        }

        public void GotoNextBreakpoint()
        {
            try
            {
                var line = scintilla.LineFromPosition(scintilla.CurrentPosition);
                var nextLine = scintilla.Lines[++line].MarkerNext(1 << UiStatics.BreakpointMarker);
                if (nextLine != -1)
                    scintilla.Lines[nextLine].Goto();
            }
            catch (Exception e)
            {
                UiStatics.ShowExceptionMessage(e);
            }
        }

        public void GotoPreviousBreakpoint()
        {
            try
            {
                var line = scintilla.LineFromPosition(scintilla.CurrentPosition);
                var prevLine = scintilla.Lines[--line].MarkerPrevious(1 << UiStatics.BreakpointMarker);
                if (prevLine != -1)
                    scintilla.Lines[prevLine].Goto();
            }
            catch (Exception e)
            {
                UiStatics.ShowExceptionMessage(e);
            }
        }

        private void UpdateLineNumber(int padding = 2)
        {
            var maxLineNumberCharLength = scintilla.Lines.Count.ToString().Length;
            scintilla.Margins[0].Width =
                scintilla.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + padding;
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
    }
}