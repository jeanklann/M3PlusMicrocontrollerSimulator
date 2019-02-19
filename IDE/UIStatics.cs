using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using IDE.Exporter;
using IDE.Importer;
using M3PlusMicrocontroller;
using ScintillaNET;

namespace IDE
{
    public static class UiStatics
    {
        public const int BreakpointIndexMargin = 1;
        public const int LabelMargin = 2;

        public const int BreakpointMarker = 1;
        public const int IndexMarker = 2;
        public const int LabelMarker = 3;

        private const string ProgramaVazio = "PROGRAMA_VAZIO:\r\nJMP PROGRAMA_VAZIO";
        private const string Sucesso = "Programa montado com sucesso.";
        private const string Erro = "Erros na montagem do programa.";
        private const string ErroCompilacao = "Erros de compilação.";

        private const string Executando = "Executando programa.";

        private const string Pausa = "Programa em pausa.";
        private const string Parado = "Programa parado.";
        private const string InstrucaoAtual = " Instrução atual: {0}";
        public static Codigo Codigo;
        public static Depurador Depurador;
        public static Circuito Circuito;

        public static FormularioPrincipal MainForm;

        public static Compiler Compilador;
        public static Simulator Simulador;

        private static readonly MMaisMaisLexer mMaisMaisLexer = new MMaisMaisLexer(
            "mov add sub inc jmp jmpc jmpz call ret and or xor not push pop pusha popa",
            "a b c d e out0 out1 out2 out3 in0 in1 in2 in3");

        public static bool WantExit = false; //To tell all threads to terminate

        public static string FilePath;
        private static Thread _exceptionThread;
        private static Exception _exception;
        private static bool _updateException;

        public static Thread ThreadDepurador { get; set; }

        public static void ExportLogiSim(string path)
        {
            BeforeExport();
            var exporter = new Exporter.Exporter(new LogisimExporterStrategy());
            exporter.Export(path, Simulador.Program);
        }

        public static void ImportLogiSim(string path)
        {
            var importer = new Importer.Importer(new LogisimImporterStrategy());
            Codigo.scintilla.Text = importer.Import(path);
        }

        public static void ExportHex(string path)
        {
            
            BeforeExport();
            var exporter = new Exporter.Exporter(new HexadecimalExporterStrategy());
            exporter.Export(path, Simulador.Program);
        }

        public static void ImportHex(string path)
        {
            var importer = new Importer.Importer(new HexadecimalImporterStrategy());
            Codigo.scintilla.Text = importer.Import(path);
        }

        public static void ExportBinary(string path)
        {
            BeforeExport();
            var exporter = new Exporter.Exporter(new BinaryExporterStrategy());
            exporter.Export(path, Simulador.Program);
        }

        private static void BeforeExport()
        {
            Compile();
            if (Simulador == null)
                throw new CompilerError("Não foi possível exportar o arquivo pois não foi instanciado a simulação.");
            if (Simulador.Program == null)
                throw new CompilerError("Não foi possível exportar o arquivo pois não há programa a ser exportado.");
        }

        public static void ImportBinary(string path)
        {
            var importer = new Importer.Importer(new BinaryImporterStrategy());
            Codigo.scintilla.Text = importer.Import(path);
        }



        public static void Compile()
        {
            try
            {
                Depurador.ChangedToCompile = false;
                Compilador = new Compiler();
                Simulador?.Stop();
                Simulador = new Simulator
                {
                    Frequency = Depurador.Frequency,
                    FrequencyLimit = Depurador.FrequencyLimiter,
                    InternalSimulation = Depurador.InternalSimulation
                };
                Depurador.SetText("");
                Depurador.RemoveAllLabels();
                Depurador.RemoveAllBreakpoint();
                var breakpoints = new bool[Compiler.MemoryMaxSize];
                const uint maskBreakpoint = 1 << BreakpointMarker;
                for (var i = 0; i < Codigo.scintilla.Lines.Count; i++)
                    if ((Codigo.scintilla.Lines[i].MarkerGet() & maskBreakpoint) > 0)
                        breakpoints[i] = true;

                if (string.IsNullOrEmpty(Codigo.scintilla.Text)) Codigo.scintilla.Text = ProgramaVazio;
                var instructions = Compilador.Compile(Codigo.scintilla.Text, breakpoints);
                Simulador.Program = instructions;
                var compiledProgram = new List<byte>();
                foreach (var instruction in Simulador.Program)
                {
                    if (instruction == null) continue;
                    var bytes = instruction.Bytes;
                    compiledProgram.AddRange(bytes);
                }

                Simulador.CompiledProgram = compiledProgram.ToArray();
                var text = new StringBuilder();
                Depurador.AddressToLine = new int[Compiler.MemoryMaxSize];
                Depurador.LineToAddress = new int[Compiler.MemoryMaxSize];

                for (var i = 0; i < Compilador.Instructions.Count; i++)
                {
                    text.AppendLine(Compilador.Instructions[i].Instruction.Text);
                    Depurador.AddressToLine[Compilador.Instructions[i].Address] = i;
                    Depurador.LineToAddress[i] = Compilador.Instructions[i].Address;
                }

                Depurador.SetText(text.ToString());
                for (var i = 0; i < Compilador.Instructions.Count; i++)
                    if (Compilador.Instructions[i].Instruction.HasBreakpoint)
                        Depurador.AddBreakpoint(Depurador.scintilla.Lines[i]);
                foreach (var item in Compilador.Labels) Depurador.AddLabel(Depurador.AddressToLine[item.Address]);
                MainForm.ToolStripStatusLabel.Text = Sucesso;
            }
            catch (CompilerError e1)
            {
                Compilador = null;
                Simulador = null;
                MainForm.ToolStripStatusLabel.Text = Erro;
                MessageBox.Show(MainForm, e1.Message, ErroCompilacao, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                if (!(e is CompilerError))
                    ShowExceptionMessage(e);
                Compilador = null;
                Simulador = null;
            }
        }

        public static bool Save()
        {
            return FilePath != null && FileProject.Save(FilePath);
        }

        public static void ShowExceptionMessage(Exception e = null)
        {
            _exception = e;
            if (_exceptionThread == null)
            {
                _exceptionThread = new Thread(ShowExceptionMessageThread);
                _exceptionThread.Start();
            }

            _updateException = true;
        }

        private static void ShowExceptionMessageThread()
        {
            while (!WantExit)
            {
                if (_updateException)
                {
                    _updateException = false;
                    Stop();
                    var exceptionLog = new ExceptionLog(_exception);
                    exceptionLog.ShowDialog(MainForm);
                }

                Thread.Sleep(25);
            }
        }

        public static bool Open()
        {
            return FilePath != null && FileProject.Load(FilePath);
        }

        public static void Run()
        {
            if (Simulador == null)
            {
                Compile();
            }
            else if (Depurador.ChangedToCompile)
            {
                Simulador.Stop();
                Circuito.Stop();
                Compile();
            }

            if (Simulador != null)
                if (!Simulador.Running)
                {
                    if (Simulador.Stopped)
                        Circuito.InstructionLog.Iniciar();
                    else
                        Circuito.InstructionLog.Continuar();
                    Simulador.Run();
                    MainForm.ToolStripStatusLabel.Text = Executando;
                    Circuito.Run();
                }

            if (ThreadDepurador == null)
            {
                ThreadDepurador = new Thread(Depurador.UpdateAll);
                ThreadDepurador.Start();
            }
        }

        public static void Pause()
        {
            if (Simulador == null) return;
            if (Simulador.Running)
            {
                Simulador.Pause();
                Circuito.InstructionLog.Pausar();
                MainForm.ToolStripStatusLabel.Text = Pausa;
            }
            else
            {
                Simulador.Run();
                Simulador.Pause();
                Circuito.InstructionLog.Continuar();
                Circuito.InstructionLog.Pausar();
                MainForm.ToolStripStatusLabel.Text = Pausa;
            }

            if (Simulador.Program[Simulador.NextInstruction] != null)
                MainForm.ToolStripStatusLabel.Text +=
                    string.Format(InstrucaoAtual, Simulador.Program[Simulador.NextInstruction].Text);
        }

        public static void Stop()
        {
            if (Simulador != null)
            {
                Simulador.Stop();
                Circuito.InstructionLog.Parar();
                MainForm.ToolStripStatusLabel.Text = Parado;
            }

            Circuito.Stop();
        }

        public static void StepIn()
        {
            if (Simulador == null) return;
            Simulador.Debug_StepInto();
            Circuito.InstructionLog.PassoDentro();
            MainForm.ToolStripStatusLabel.Text = Pausa;
            if (Simulador.Program[Simulador.NextInstruction] != null)
                MainForm.ToolStripStatusLabel.Text +=
                    string.Format(InstrucaoAtual, Simulador.Program[Simulador.NextInstruction].Text);
            Circuito.Run();
        }

        public static void StepOut()
        {
            if (Simulador == null) return;
            Simulador.Debug_StepOut();
            Circuito.InstructionLog.PassoFora();
            MainForm.ToolStripStatusLabel.Text = Pausa;
            if (Simulador.Program[Simulador.NextInstruction] != null)
                MainForm.ToolStripStatusLabel.Text +=
                    string.Format(InstrucaoAtual, Simulador.Program[Simulador.NextInstruction].Text);
            Circuito.Run();
        }


        public static void ScintillaSetStyle(Scintilla scintilla, bool hasLabel = false)
        {
            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 10;
            scintilla.StyleClearAll();

            scintilla.Styles[MMaisMaisLexer.CpuInstruction].ForeColor = Color.Blue;
            scintilla.Styles[MMaisMaisLexer.CpuInstruction].Bold = true;

            scintilla.Styles[MMaisMaisLexer.Register].ForeColor = Color.Navy;
            scintilla.Styles[MMaisMaisLexer.Register].Bold = true;

            scintilla.Styles[MMaisMaisLexer.Identifier].ForeColor = Color.Maroon;

            scintilla.Styles[MMaisMaisLexer.Number].ForeColor = Color.Red;
            scintilla.Styles[MMaisMaisLexer.Address].ForeColor = Color.Brown;

            scintilla.Styles[MMaisMaisLexer.Comment].ForeColor = Color.Green;


            scintilla.Lexer = Lexer.Container;

            scintilla.StyleNeeded += scintilla_StyleNeeded;

            var margin = scintilla.Margins[BreakpointIndexMargin];
            margin.Width = 16;
            margin.Sensitive = true;
            margin.Type = MarginType.Symbol;
            margin.Mask = (1 << BreakpointMarker) | (1 << IndexMarker);
            margin.Cursor = MarginCursor.Arrow;

            var breakpointMarker = scintilla.Markers[BreakpointMarker];
            breakpointMarker.Symbol = MarkerSymbol.Circle;
            breakpointMarker.SetBackColor(Color.Red);
            breakpointMarker.SetForeColor(Color.Red);

            var instructionMarker = scintilla.Markers[IndexMarker];
            instructionMarker.Symbol = MarkerSymbol.ShortArrow;
            instructionMarker.SetBackColor(Color.Yellow);
            instructionMarker.SetForeColor(Color.Black);


            if (!hasLabel) return;
            var margin2 = scintilla.Margins[LabelMargin];
            margin2.Width = 16;
            margin2.Sensitive = true;
            margin2.Type = MarginType.Symbol;
            margin2.Mask = 1 << LabelMarker;
            margin2.Cursor = MarginCursor.Arrow;

            var marker3 = scintilla.Markers[LabelMarker];
            marker3.Symbol = MarkerSymbol.Bookmark;
            marker3.SetBackColor(Color.Gray);
            marker3.SetForeColor(Color.Black);
        }

        private static void scintilla_StyleNeeded(object sender, StyleNeededEventArgs e)
        {
            var scintilla = (Scintilla) sender;
            var startPos = scintilla.GetEndStyled();
            var endPos = e.Position;

            mMaisMaisLexer.Style(scintilla, startPos, endPos);
        }
    }
}