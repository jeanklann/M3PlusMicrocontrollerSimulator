using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Drawing;
using ScintillaNET;
using M3PlusMicrocontroller;
using System.Windows.Forms;
using System.IO;


namespace IDE {
    public static class UIStatics {
        public static Codigo Codigo;
        public static Depurador Depurador;
        public static Circuito Circuito;

        public static Form MainForm;

        public static Compiler Compilador;
        public static Simulator Simulador;
        

        public const int BREAKPOINT_INDEX_MARGIN = 1;
        public const int LABEL_MARGIN = 2;

        public const int BREAKPOINT_MARKER = 1;
        public const int INDEX_MARKER = 2;
        public const int LABEL_MARKER = 3;

        public static bool WantExit = false; //To tell all threads to terminate

        public static Thread threadDepurador;

        public static string FilePath;

        public static void Compile() {
            try {
                Compilador = new Compiler();
                if (Simulador != null) Simulador.Stop();
                Simulador = new Simulator();
                Simulador.Frequency = Depurador.Frequency;
                Simulador.FrequencyLimit = Depurador.FrequencyLimiter;
                Depurador.SetText("");
                Depurador.RemoveAllLabels();
                Depurador.RemoveAllBreakpoint();
                bool[] breakpoints = new bool[Compiler.MEMORY_MAX_SIZE];
                const uint maskBreakpoint = (1 << BREAKPOINT_MARKER);
                for (int i = 0; i < Codigo.scintilla.Lines.Count; i++) {
                    if ((Codigo.scintilla.Lines[i].MarkerGet() & maskBreakpoint) > 0) {
                        breakpoints[i] = true;
                    }
                }

                Instruction[] instructions = Compilador.Compile(Codigo.scintilla.Text, breakpoints);
                Simulador.Program = instructions;
                StringBuilder text = new StringBuilder();
                Depurador.AddressToLine = new int[Compiler.MEMORY_MAX_SIZE];
                /*
                for (int i = 0; i < Codigo.scintilla.Lines.Count; i++) {
                    if ((Codigo.scintilla.Lines[i].MarkerGet() & maskBreakpoint) > 0) {

                        Depurador.AddBreakpoint(Depurador.scintilla.Lines[i]);
                    }
                }
                foreach (Line item in Codigo.scintilla.Lines) {
                    if((item.MarkerGet() & maskBreakpoint) > 0) {
                        
                    }
                }*/
                for (int i = 0; i < Compilador.Instructions.Count; i++) {
                    text.AppendLine(Compilador.Instructions[i].Instruction.Text);
                    Depurador.AddressToLine[Compilador.Instructions[i].Address] = i;
                }
                Depurador.SetText(text.ToString());
                for (int i = 0; i < Compilador.Instructions.Count; i++) {
                    if (Compilador.Instructions[i].Instruction.HasBreakpoint) {
                        Depurador.AddBreakpoint(Depurador.scintilla.Lines[i]);
                    }
                }
                foreach (M3PlusMicrocontroller.Label item in Compilador.Labels) {
                    Depurador.AddLabel(Depurador.AddressToLine[item.Address]);
                }
                
                MessageBox.Show(MainForm, "Programa montado com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (CompilerError e1) {
                MessageBox.Show(MainForm, e1.Message, "Erro de compilação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } catch (Exception e2) {
                MessageBox.Show(MainForm, "Erro interno: \n" + e2.Message, "Erro interno", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool Save() {
            if(FilePath != null) {
                return FileProject.Save(FilePath);
            } else {
                return false;
            }
        }
        public static bool Open() {
            if (FilePath != null) {
                return FileProject.Load(FilePath);
            } else {
                return false;
            }
        }

        public static void Run() {
            if (Simulador != null) {
                if (!Simulador.Running) {
                    if (Codigo.Visible) {
                        Codigo.Visible = false;
                        Circuito.Visible = false;
                        Depurador.Visible = true;
                    }
                    Simulador.Run();
                    Circuito.Run();
                }
            }
            if (threadDepurador == null) {
                threadDepurador = new Thread(Depurador.UpdateAll);
                threadDepurador.Start();
            }
        }
        public static void Pause() {
            if (Simulador != null) {
                if (Simulador.Running) {
                    Simulador.Pause();
                } else {
                    Simulador.Run();
                    Simulador.Pause();
                }
            }
            
        }
        public static void Stop() {
            if(Simulador != null) {
                Simulador.Stop();
            }
            Circuito.Stop();
        }
        public static void StepIn() {
            if (Simulador != null) {
                Simulador.Debug_StepInto();
                Circuito.Tick();
            }
        }
        public static void StepOut() {
            if (Simulador != null) {
                Simulador.Debug_StepOut();
                Circuito.Tick();
            }
        }


        public static void ScintillaSetStyle(Scintilla scintilla, bool hasLabel = false) {
            scintilla.Styles[Style.Asm.CpuInstruction].ForeColor = System.Drawing.Color.Blue;
            scintilla.Styles[Style.Asm.CpuInstruction].Bold = true;

            scintilla.Styles[Style.Asm.Register].ForeColor = System.Drawing.Color.Navy;
            scintilla.Styles[Style.Asm.Register].Bold = true;

            scintilla.Styles[Style.Asm.Identifier].ForeColor = System.Drawing.Color.Maroon;

            scintilla.Styles[Style.Asm.Number].ForeColor = System.Drawing.Color.Red;



            scintilla.Styles[Style.Asm.Comment].ForeColor = System.Drawing.Color.Green;




            scintilla.Lexer = Lexer.Asm;

            scintilla.SetKeywords(0, "mov add sub inc jmp jmpc jmpz call ret and or xor not");
            scintilla.SetKeywords(1, "");
            scintilla.SetKeywords(2, "a b c d e out0 out1 out2 out3 in0 in1 in2 in3");
            
            Margin margin = scintilla.Margins[BREAKPOINT_INDEX_MARGIN];
            margin.Width = 16;
            margin.Sensitive = true;
            margin.Type = MarginType.Symbol;
            margin.Mask = (1<<BREAKPOINT_MARKER) | (1<<INDEX_MARKER);
            margin.Cursor = MarginCursor.Arrow;
            
            Marker marker = scintilla.Markers[BREAKPOINT_MARKER];
            marker.Symbol = MarkerSymbol.Circle;
            marker.SetBackColor(Color.Red);
            marker.SetForeColor(Color.Red);
            
            Marker marker2 = scintilla.Markers[INDEX_MARKER];
            marker2.Symbol = MarkerSymbol.ShortArrow;
            marker2.SetBackColor(Color.Yellow);
            marker2.SetForeColor(Color.Black);
            

            if (hasLabel) {
                
                Margin margin2 = scintilla.Margins[LABEL_MARGIN];
                margin2.Width = 16;
                margin2.Sensitive = true;
                margin2.Type = MarginType.Symbol;
                margin2.Mask = 1<<LABEL_MARKER;
                margin2.Cursor = MarginCursor.Arrow;

                Marker marker3 = scintilla.Markers[LABEL_MARKER];
                marker3.Symbol = MarkerSymbol.Bookmark;
                marker3.SetBackColor(Color.Gray);
                marker3.SetForeColor(Color.Black);
            }
        }
    }
}
