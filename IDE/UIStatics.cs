using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using ScintillaNET;
using M3PlusMicrocontroller;
using System.Windows.Forms;


namespace IDE {
    public static class UIStatics {
        public static Codigo Codigo;
        public static Depurador Depurador;
        public static Circuito Circuito;

        public static Compiler Compilador;
        public static Simulator Simulador;

        public static List<InstructionCompiler> Instructions;
        public static List<M3PlusMicrocontroller.Label> Labels;

        public const int BREAKPOINT_INDEX_MARGIN = 1;
        public const int LABEL_MARGIN = 2;

        public const int BREAKPOINT_MARKER = 1;
        public const int INDEX_MARKER = 2;
        public const int LABEL_MARKER = 3;

        public static bool WantExit = false; //To tell all threads to terminate

        public static Thread threadDepurador;

        public static void Compile() {
            try {
                Instruction[] instructions = Compilador.Compile(Codigo.scintilla.Text);
                Simulador = new Simulator();
                Simulador.Program = instructions;

                Instructions = Compilador.Instructions;
                Labels = Compilador.Labels;

                StringBuilder text = new StringBuilder();

                Depurador.AddressToLine = new int[Compiler.MEMORY_MAX_SIZE];
                for (int i = 0; i < Instructions.Count; i++) {
                    text.AppendLine(Instructions[i].Instruction.Text);
                    Depurador.AddressToLine[Instructions[i].Address] = i;
                }
                Depurador.SetText(text.ToString());

                MessageBox.Show("Programa montado com sucesso.");
            } catch (CompilerError e1) {
                MessageBox.Show(e1.Message);
            } catch (Exception e2) {
                MessageBox.Show("Erro interno: " + e2.Message);
            }
        }

        public static void Run() {
            if (Simulador != null) {
                if (!Simulador.Running) {
                    Codigo.Visible = false;
                    Circuito.Visible = false;
                    Depurador.Visible = true;
                    Simulador.Run();
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
        }
        public static void StepIn() {
            if (Simulador != null) {
                Simulador.Debug_StepInto();
            }
        }
        public static void StepOut() {
            if (Simulador != null) {
                Simulador.Debug_StepOut();
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
            scintilla.SetKeywords(2, "a b c d e OUT4 out1 out2 out3 IN4 in1 in2 in3");
            
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
