using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Drawing;
using ScintillaNET;
using M3PlusMicrocontroller;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace IDE {
    public static class UIStatics {
        public static Codigo Codigo;
        public static Depurador Depurador;
        public static Circuito Circuito;

        public static Form MainForm;

        public static Compiler Compilador;
        public static Simulator Simulador;

        public static MMaisMaisLexer MMaisMaisLexer = new MMaisMaisLexer("mov add sub inc jmp jmpc jmpz call ret and or xor not push pop pusha popa", "a b c d e out0 out1 out2 out3 in0 in1 in2 in3");


        public const int BREAKPOINT_INDEX_MARGIN = 1;
        public const int LABEL_MARGIN = 2;

        public const int BREAKPOINT_MARKER = 1;
        public const int INDEX_MARKER = 2;
        public const int LABEL_MARKER = 3;

        public static bool WantExit = false; //To tell all threads to terminate

        public static Thread threadDepurador;

        public static string FilePath;

        public static bool ExportLogiSim(string path) {
            try {
                Compile();
                if (Simulador == null) return false;
                StreamWriter sw = new StreamWriter(path);
                Stream BaseStream = sw.BaseStream;
                BinaryWriter bw = new BinaryWriter(BaseStream);
                bw.Write(Encoding.ASCII.GetBytes("v2.0 raw\n"));
                for (int i = 0; i < Simulador.Program.Length; i++) {
                    if (Simulador.Program[i] == null) continue;
                    byte[] bytes = Simulador.Program[i].Convert();
                    for (int j = 0; j < bytes.Length; j++) {
                        if (j > 0)
                            bw.Write(' ');
                        bw.Write(Encoding.ASCII.GetBytes(bytes[j].ToString("x")));
                    }
                    if ((i + 1) % 16 == 0)
                        bw.Write(Encoding.ASCII.GetBytes("\n"));
                    else
                        bw.Write(' ');
                }
                bw.Close();
                return true;
            } catch (Exception) {
                return false;
            }
        }
        public static bool ImportLogiSim(string path) {
            try {
                StreamReader sr = new StreamReader(path);
                
                string all = sr.ReadToEnd();
                all = all.Replace("v2.0 raw", "");
                all = all.Replace("\r"," ");
                all = all.Replace("\n", " ");
                all = all.Replace("  ", " ");
                while (all.Contains("*")) {
                    int i = all.IndexOf("*");
                    int bi = i;
                    int ei = i;
                    char chr = all[bi];
                    while(chr != ' ') {
                        --bi;
                        chr = all[bi];
                    }
                    chr = all[ei];
                    while (chr != ' ') {
                        ++ei;
                        chr = all[ei];
                    }
                    int quant = int.Parse(all.Substring(bi, i - bi));
                    string str = all.Substring(i + 1, ei - (i + 1));
                    string tmp = "";
                    for (int j = 0; j < quant; j++) {
                        tmp += ' ';
                        tmp += str;
                    }
                    all = all.Replace(all.Substring(bi, ei - bi), tmp);
                }
                string[] hexes = all.Split(' ');
                byte[] bytes = new byte[hexes.Length];
                for (int i = 0; i < hexes.Length; i++) {
                    if (hexes[i] == "") continue;
                    bytes[i] = byte.Parse(hexes[i], System.Globalization.NumberStyles.HexNumber);
                }
                int index = 0;
                int outIndex;
                List<Instruction> InstructionList = new List<Instruction>();
                while (index < bytes.Length) {
                    Instruction instruction;
                    Instruction.Convert(bytes, index, out outIndex, out instruction);
                    InstructionList.Add(instruction);
                    index = outIndex;
                }

                Codigo.scintilla.Text = "";
                int pos = 0;
                foreach (Instruction item in InstructionList) {
                    foreach (Instruction itemLabel in InstructionList) {
                        if (itemLabel.Label != null && itemLabel.Label != "" && itemLabel.Label.StartsWith("Byte_")) {
                            if (itemLabel.Address == pos)
                                Codigo.scintilla.Text += itemLabel.Label + ":\r\n";
                        }
                    }
                    Codigo.scintilla.Text += item.Text + "\r\n";
                    pos += item.Size;
                }

                sr.Close();
                return true;
            } catch (Exception) {
                return false;
            }
        }
        public static bool ExportHex(string path) {
            try {
                Compile();
                if (Simulador == null) return false;
                StreamWriter sw = new StreamWriter(path);
                Stream BaseStream = sw.BaseStream;
                BinaryWriter bw = new BinaryWriter(BaseStream);
                for (int i = 0; i < Simulador.Program.Length; i++) {
                    if (Simulador.Program[i] == null) continue;
                    byte[] bytes = Simulador.Program[i].Convert();
                    for (int j = 0; j < bytes.Length; j++) {
                        bw.Write(Encoding.ASCII.GetBytes(bytes[j].ToString("X2")));
                    }
                }
                bw.Close();
                return true;
            } catch (Exception) {
                return false;
            }
        }
        public static bool ImportHex(string path) {
            try {
                StreamReader sr = new StreamReader(path);
                Stream BaseStream = sr.BaseStream;
                BinaryReader br = new BinaryReader(BaseStream);

                byte[] bytes = new byte[BaseStream.Length/2];
                while (BaseStream.Position < BaseStream.Length) {
                    bytes[BaseStream.Position/2] = byte.Parse(((char)br.ReadByte()) + "" + ((char)br.ReadByte()), System.Globalization.NumberStyles.HexNumber);
                }
                int index = 0;
                int outIndex;
                List<Instruction> InstructionList = new List<Instruction>();
                while (index < bytes.Length) {
                    Instruction instruction;
                    Instruction.Convert(bytes, index, out outIndex, out instruction);
                    InstructionList.Add(instruction);
                    index = outIndex;
                }
                Codigo.scintilla.Text = "";
                int pos = 0;
                foreach (Instruction item in InstructionList) {
                    foreach (Instruction itemLabel in InstructionList) {
                        if(itemLabel.Label != null && itemLabel.Label != "" && itemLabel.Label.StartsWith("Byte_")) {
                            if(itemLabel.Address == pos)
                                Codigo.scintilla.Text += itemLabel.Label+":\r\n";
                        }
                    }
                    Codigo.scintilla.Text += item.Text + "\r\n";
                    pos += item.Size;
                }
                br.Close();
                return true;
            } catch (Exception) {
                return false;
            }
        }
        public static bool ExportBinary(string path) {
            try {
                Compile();
                if (Simulador == null) return false;
                StreamWriter sw = new StreamWriter(path);
                Stream BaseStream = sw.BaseStream;
                BinaryWriter bw = new BinaryWriter(BaseStream);
                for (int i = 0; i < Simulador.Program.Length; i++) {
                    if(Simulador.Program[i] == null) continue;
                    byte[] bytes = Simulador.Program[i].Convert();
                    bw.Write(bytes);
                }
                bw.Close();
                return true;
            } catch (Exception) {
                return false;
            }
        }
        public static bool ImportBinary(string path) {
            try {
                StreamReader sr = new StreamReader(path);
                Stream BaseStream = sr.BaseStream;
                BinaryReader br = new BinaryReader(BaseStream);

                byte[] bytes = new byte[BaseStream.Length];
                while (BaseStream.Position < BaseStream.Length) {
                    bytes[BaseStream.Position] = br.ReadByte();
                }
                int index = 0;
                int outIndex;
                List<Instruction> InstructionList = new List<Instruction>();
                while (index < bytes.Length) {
                    Instruction instruction;
                    Instruction.Convert(bytes, index, out outIndex, out instruction);
                    InstructionList.Add(instruction);
                    index = outIndex;
                }

                Codigo.scintilla.Text = "";
                int pos = 0;
                foreach (Instruction item in InstructionList) {
                    foreach (Instruction itemLabel in InstructionList) {
                        if (itemLabel.Label != null && itemLabel.Label != "" && itemLabel.Label.StartsWith("Byte_")) {
                            if (itemLabel.Address == pos)
                                Codigo.scintilla.Text += itemLabel.Label + ":\r\n";
                        }
                    }
                    Codigo.scintilla.Text += item.Text + "\r\n";
                    pos += item.Size;
                }
                br.Close();
                return true;
            } catch (Exception) {
                return false;
            }
        }

        public static void Compile() {
            try {
                Compilador = new Compiler();
                if (Simulador != null) Simulador.Stop();
                Simulador = new Simulator();
                Simulador.Frequency = Depurador.Frequency;
                Simulador.FrequencyLimit = Depurador.FrequencyLimiter;
                Simulador.InternalSimulation = Depurador.InternalSimulation;
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
                List<byte> compiledProgram = new List<byte>();
                for (int i = 0; i < Simulador.Program.Length; i++) {
                    if (Simulador.Program[i] == null) continue;
                    byte[] bytes = Simulador.Program[i].Convert();
                    for (int j = 0; j < bytes.Length; j++) {
                        compiledProgram.Add(bytes[j]);
                    }
                }
                Simulador.CompiledProgram = compiledProgram.ToArray();
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
                Compilador = null;
                Simulador = null;
                MessageBox.Show(MainForm, e1.Message, "Erro de compilação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } catch (Exception e2) {
                Compilador = null;
                Simulador = null;
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
                Circuito.Run();
            }
        }
        public static void StepOut() {
            if (Simulador != null) {
                Simulador.Debug_StepOut();
                Circuito.Run();
            }
        }


        public static void ScintillaSetStyle(Scintilla scintilla, bool hasLabel = false) {
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
            
            scintilla.StyleNeeded += new System.EventHandler<ScintillaNET.StyleNeededEventArgs>(scintilla_StyleNeeded);
            Console.WriteLine();
            /*
            scintilla.SetKeywords(0, "mov add sub inc jmp jmpc jmpz call ret and or xor not");
            scintilla.SetKeywords(1, "");
            scintilla.SetKeywords(2, "a b c d e out0 out1 out2 out3 in0 in1 in2 in3");
            */
            /*
            Regex P = new Regex("[0-9a-zA-Z]?[0-9a-zA-Z]");
            
            foreach (Match m in P.Matches(scintilla.Text))
                e.GetRange(m.Index, m.Index + m.Length).SetStyle(1);
            */

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
        private static void scintilla_StyleNeeded(object sender, StyleNeededEventArgs e) {
            Scintilla scintilla = (Scintilla)sender;
            int startPos = scintilla.GetEndStyled();
            int endPos = e.Position;

            MMaisMaisLexer.Style(scintilla, startPos, endPos);

        }
    }
    public class MMaisMaisLexer {
        public const int StyleDefault = 0;
        public const int CpuInstruction = 1;
        public const int Register = 2;
        public const int Identifier = 3;
        public const int Number = 4;
        public const int Address = 5;
        public const int Comment = 6;

        private const int StateDefault = 0;
        private const int StateIdentifier = 1;
        private const int StateNumber = 2;
        private const int StateComment = 3;
        private const int StateAddress = 4;

        private HashSet<string> cpuInstructions;
        private HashSet<string> registrers;

        public MMaisMaisLexer(string CpuInstructions = "", string Registrers = "") {
            CpuInstructions = CpuInstructions.ToLower();
            Registrers = Registrers.ToLower();
            string[] cpuInstructionsList = CpuInstructions != string.Empty? CpuInstructions.Split(' '):new string[] { };
            string[] registrersList = Registrers != string.Empty ? Registrers.Split(' ') : new string[] { };
            cpuInstructions = new HashSet<string>(cpuInstructionsList);
            registrers = new HashSet<string>(registrersList);
        }

        public void Style(Scintilla scintilla, int startPos, int endPos) {
            int line = scintilla.LineFromPosition(startPos);
            startPos = scintilla.Lines[line].Position;
            int currentPos = startPos;
            int length = 0;
            int state = StateDefault;
            string text = (scintilla.Text + '\0');

            scintilla.StartStyling(startPos);
            while (currentPos < endPos+1) {
                char c = char.ToLower(text[currentPos]);
                bool reprocess = true;
                while (reprocess) {
                    reprocess = false;
                    switch (state) {
                        case StateDefault:
                            if (c == ';') {
                                scintilla.SetStyling(1, Comment);
                                state = StateComment;
                            } else if (char.IsDigit(c) || (c >= 'a' && c <= 'f')) {
                                state = StateNumber;
                                reprocess = true;
                            } else if (c == '#') {
                                length++;
                                state = StateAddress;
                            } else if (char.IsLetterOrDigit(c) || c == '_') {
                                state = StateIdentifier;
                                reprocess = true;
                            } else {
                                if(currentPos < endPos)
                                    scintilla.SetStyling(1, StyleDefault);
                            }
                            break;
                        case StateComment:
                            if(c!='\0')
                                scintilla.SetStyling(1, Comment);
                            if (c == '\n' || c == '\r') {
                                state = StateDefault;
                            }
                        break;
                        case StateNumber:
                            if (char.IsDigit(c) || (c >= 'a' && c <= 'f') || c == 'd' || c == 'h') {
                                length++;
                                if(length < 3 && c == 'h') {
                                    state = StateIdentifier;
                                }
                                if (length > 2) {
                                    if (length == 3 && c == 'h') {
                                    } else if (char.IsDigit(c)) {
                                    } else if (c == 'd') {
                                        bool valid = true;
                                        for (int i = currentPos-1; i >= currentPos-(length-1); i--) {
                                            char c2 = char.ToLower(text[i]);
                                            if (!char.IsDigit(c2)) valid = false;
                                        }
                                        if(!valid) state = StateIdentifier;
                                    } else {
                                        state = StateIdentifier;
                                    }
                                }
                            } else {
                                if (char.IsLetterOrDigit(c) || c == '_') {
                                    state = StateIdentifier;
                                    reprocess = true;
                                } else if (length < 2) {
                                    state = StateIdentifier;
                                    reprocess = true;
                                } else {
                                    scintilla.SetStyling(length, Number);
                                    length = 0;
                                    state = StateDefault;
                                    reprocess = true;
                                }
                            }
                            
                            break;
                        case StateAddress:
                            if (char.IsDigit(c) || (c >= 'a' && c <= 'f') || c == 'd' || c == 'h') {
                                length++;
                                if (length < 4 && c == 'h') {
                                    state = StateIdentifier;
                                }
                                if (length > 3) {
                                    if (length == 4 && c == 'h') {
                                    } else if (char.IsDigit(c)) {
                                    } else if (c == 'd') {
                                        bool valid = true;
                                        for (int i = currentPos - 1; i >= currentPos - (length - 2); i--) {
                                            char c2 = char.ToLower(text[i]);
                                            if (!char.IsDigit(c2)) valid = false;
                                        }
                                        if (!valid) state = StateIdentifier;
                                    } else {
                                        state = StateIdentifier;
                                    }
                                }
                            } else {
                                if (char.IsLetterOrDigit(c) || c == '_') {
                                    state = StateIdentifier;
                                    reprocess = true;
                                } else {
                                    scintilla.SetStyling(length, Number);
                                    length = 0;
                                    state = StateDefault;
                                    reprocess = true;
                                }
                            }

                            break;
                        case StateIdentifier:
                            if (char.IsLetterOrDigit(c) || c == '_') {
                                length++;
                            } else {
                                int style = Identifier;
                                string identifier = scintilla.GetTextRange(currentPos - length, length).ToLower();
                                if (cpuInstructions.Contains(identifier))
                                    style = CpuInstruction;
                                else if (registrers.Contains(identifier))
                                    style = Register;
                                scintilla.SetStyling(length, style);
                                length = 0;
                                state = StateDefault;
                                reprocess = true;
                            }
                            break;
                    }
                }
                currentPos++;
            }

        }
    }
}
