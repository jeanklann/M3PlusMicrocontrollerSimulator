using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace IDE
{
    public static class Draws {
        public static int TerminalHandle;

        public static ComponentDraw[] Input;
        public static ComponentDraw[] Output;
        public static ComponentDraw[] Disable;
        public static ComponentDraw[] Not;
        public static ComponentDraw[] And;
        public static ComponentDraw[] Nand;
        public static ComponentDraw[] Or;
        public static ComponentDraw[] Nor;
        public static ComponentDraw[] Xor;
        public static ComponentDraw[] Xnor;
        public static ComponentDraw Keyboard;
        public static ComponentDraw Microcontroller;
        public static ComponentDraw JkFlipFlop;
        public static ComponentDraw RsFlipFlop;
        public static ComponentDraw FlipFlop;
        public static ComponentDraw DFlipFlop;
        public static ComponentDraw Display7SegBase;
        public static int[] Display7SegPart;
        public static ComponentDraw BinTo7Seg;
        public static ComponentDraw Osciloscope;
        public static ComponentDraw HalfAdder;
        public static ComponentDraw FullAdder;
        public static ComponentDraw Ula;
        public static ComponentDraw ControlModule;
        public static ComponentDraw Registrer8Bit;
        public static ComponentDraw Registrer8BitCBuffer;
        public static ComponentDraw Registrers;
        public static ComponentDraw Counter8Bit;
        public static ComponentDraw Disable8Bit;
        public static ComponentDraw RamMemory;
        public static ComponentDraw RomMemory;
        public static ComponentDraw PortBank;
        public static ComponentDraw Registrer8BitSg;
        public static ComponentDraw LedMatrix;
        public static ComponentDraw RomAddresser;
        public static ComponentDraw Clock;




        public static CircuitDraw Circuit;


        public static Color ColorOn = Color.Red;
        public static Color ColorOff = Color.Black;
        public static Color Color_3Rd = Color.Gray;

        public static void Load() {
            Circuit = new CircuitDraw();
            GenInput();
            GenOutput();
            GenTerminal();
            GenDisable();
            GenNot();
            GenAnd();
            GenNand();
            GenOr();
            GenNor();
            GenXor();
            GenXnor();
            GenKeyboard();
            Gen7SegDisplay();
            GenMicrocontroller();
            GenOsciloscope();
            GenBlackTerminal();
            GenJkFlipFlop();
            GenRsFlipFlop();
            GenDFlipFlop();
            GenTFlipFlop();
            GenDisable8Bit();
            GenAdders();
            GenMemories();
            GenRegistrers();
            GenUla();
            GenControlModule();
            GenPortBank();
            GenCounter8Bit();
            GenRomAddresser();
            GenClock();
        }
        private static void GenLedMatrix() { }
        private static void GenBlackTerminal() { }
        private static void GenClock() {
            Clock = new ComponentDraw(GL.GenLists(1), 30, 20);
            Clock.Terminals[0] = new Point(-15, 0);
            GL.NewList(Clock.DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOff);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-15, 0);
            GL.Vertex2(-10, 0);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-10, -10);
            GL.Vertex2(-10, 10);
            GL.Vertex2(15, 10);
            GL.Vertex2(15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex2(-6, -6);
            GL.Vertex2(2, -6);
            GL.Vertex2(2, 6);
            GL.Vertex2(10, 6);
            GL.End();

            GL.EndList();
        }
        private static void GenRomAddresser() {
            var drawCircuit = Circuit[14, 24];
            TextRenderer.DrawText("ROM\nAddr", Color.Black, new PointF(0, 0));
            RomAddresser = new ComponentDraw(GL.GenLists(1), drawCircuit.Width, drawCircuit.Height, drawCircuit.Terminals.Length);
            for (var i = 0; i < drawCircuit.Terminals.Length; i++) {
                RomAddresser.Terminals[i] = drawCircuit.Terminals[i];
            }
            for (var i = 0; i < 8; i++) {
                RomAddresser.TerminalsString[i] = "in" + i;
            }
            RomAddresser.TerminalsString[8] = "S";
            RomAddresser.TerminalsString[9] = "CH";
            RomAddresser.TerminalsString[10] = "CL";
            RomAddresser.TerminalsString[11] = "PCH";
            RomAddresser.TerminalsString[12] = "PCL";
            RomAddresser.TerminalsString[13] = "R";
            for (var i = 14; i < 30; i++) {
                RomAddresser.TerminalsString[i] = "outAddr" + (i - 14);
            }
            for (var i = 30; i < 38; i++) {
                RomAddresser.TerminalsString[i] = "outBus" + (i - 30);
            }

            GL.NewList(RomAddresser.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("ROM\nAddr", Color.Black, new PointF(0, 0));
            GL.CallList(drawCircuit.DisplayListHandle);
            GL.EndList();
        }
        private static void GenCounter8Bit() {
            var drawCircuit = Circuit[4, 8];
            TextRenderer.DrawText("C\nn\nt", Color.Black, new PointF(0, 0));
            Counter8Bit = new ComponentDraw(GL.GenLists(1), drawCircuit.Width, drawCircuit.Height, drawCircuit.Terminals.Length);
            for (var i = 0; i < drawCircuit.Terminals.Length; i++) {
                Counter8Bit.Terminals[i] = drawCircuit.Terminals[i];
                if (i >= 4) {
                    Counter8Bit.TerminalsString[i] = "out" + (i - 4);
                }
            }
            Counter8Bit.TerminalsString[0] = "E";
            Counter8Bit.TerminalsString[1] = "+/-";
            Counter8Bit.TerminalsString[2] = "R";
            Counter8Bit.TerminalsString[3] = "C";

            GL.NewList(Counter8Bit.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("C\nn\nt", Color.Black, new PointF(0, 0));
            GL.CallList(drawCircuit.DisplayListHandle);
            GL.EndList();
        }
        private static void GenPortBank() {
            TextRenderer.DrawText("Port\nBank", Color.Black, new PointF(0, 0));
            var drawCircuit = Circuit[45, 40];
            PortBank = new ComponentDraw(GL.GenLists(1), drawCircuit.Width, drawCircuit.Height, drawCircuit.Terminals.Length);
            for (var i = 0; i < drawCircuit.Terminals.Length; i++) {
                PortBank.Terminals[i] = drawCircuit.Terminals[i];
                if (i < 8 * 4) {
                    PortBank.TerminalsString[i] = ("in" + (i / 8) + "[" + (i % 8) + "]");
                } else if (i < 8 * 5 ) {
                    PortBank.TerminalsString[i] = ("inBus" + (i % 8));
                } else if (i < 8 * 6 + 5) {
                    PortBank.TerminalsString[i] = ("outBus" + ((i-5) % 8));
                } else {
                    PortBank.TerminalsString[i] = ("out" + (((i-5) / 8) - 6) + "[" + ((i-5) % 8) + "]");
                }
            }
            PortBank.TerminalsString[8 * 5 + 0] = ("S2");
            PortBank.TerminalsString[8 * 5 + 1] = ("S1");
            PortBank.TerminalsString[8 * 5 + 2] = ("I");
            PortBank.TerminalsString[8 * 5 + 3] = ("O");
            PortBank.TerminalsString[8 * 5 + 4] = ("R");

            GL.NewList(PortBank.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("Port\nBank", Color.Black, new PointF(0, 0));
            GL.CallList(drawCircuit.DisplayListHandle);

            GL.EndList();
        }
        private static void GenControlModule() {
            TextRenderer.DrawText("Control\nModule", Color.Black, new PointF(0, 0));
            var drawCircuit = Circuit[11, 30];
            ControlModule = new ComponentDraw(GL.GenLists(1), drawCircuit.Width, drawCircuit.Height, drawCircuit.Terminals.Length);
            for (var i = 0; i < drawCircuit.Terminals.Length; i++) {
                ControlModule.Terminals[i] = drawCircuit.Terminals[i];
            }
            for (var i = 0; i < 8; i++) {
                ControlModule.TerminalsString[i] = "In"+i;
            }
            ControlModule.TerminalsString[8] = "Clock";
            ControlModule.TerminalsString[9] = "Flag IN carry";
            ControlModule.TerminalsString[10] = "Flag IN zero";
            ControlModule.TerminalsString[11] = "EOI";
            ControlModule.TerminalsString[12] = "ROM rd";
            ControlModule.TerminalsString[13] = "ROM cs";
            ControlModule.TerminalsString[14] = "PCH bus";
            ControlModule.TerminalsString[15] = "PCL bus";
            ControlModule.TerminalsString[16] = "PCH clock";
            ControlModule.TerminalsString[17] = "PCL clock";
            ControlModule.TerminalsString[18] = "Data PC sel";
            ControlModule.TerminalsString[19] = "DIR clock";
            ControlModule.TerminalsString[20] = "SP clock";
            ControlModule.TerminalsString[21] = "SP IncDec";
            ControlModule.TerminalsString[22] = "SP sel";
            ControlModule.TerminalsString[23] = "SP en";
            ControlModule.TerminalsString[24] = "Reset";
            ControlModule.TerminalsString[25] = "ULA bus";
            ControlModule.TerminalsString[26] = "Buf clk";
            ControlModule.TerminalsString[27] = "AC bus";
            ControlModule.TerminalsString[28] = "AC clk";
            ControlModule.TerminalsString[29] = "RG bus";
            ControlModule.TerminalsString[30] = "RG/PC clk";
            ControlModule.TerminalsString[31] = "RAM rd";
            ControlModule.TerminalsString[32] = "RAM wr";
            ControlModule.TerminalsString[33] = "RAM cs";
            ControlModule.TerminalsString[34] = "In bus";
            ControlModule.TerminalsString[35] = "Out clk";
            ControlModule.TerminalsString[36] = "ULA OP sel 0";
            ControlModule.TerminalsString[37] = "ULA OP sel 1";
            ControlModule.TerminalsString[38] = "ULA OP sel 2";
            ControlModule.TerminalsString[39] = "RG/PB sel 0";
            ControlModule.TerminalsString[40] = "RG/PB sel 1";


            GL.NewList(ControlModule.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("Control\nModule", Color.Black, new PointF(0, 0));
            GL.CallList(drawCircuit.DisplayListHandle);

            GL.EndList();
        }
        private static void GenUla() {
            TextRenderer.DrawText("ULA", Color.Black, new PointF(0, 0));
            Ula = new ComponentDraw(GL.GenLists(1), 220, 100, 30);
            GL.NewList(Ula.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("ULA", Color.Black, new PointF(0, 0));

            GL.Color3(ColorOff);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-105, 40);
            GL.Vertex2(-15, 40);
            GL.Vertex2(0, 20);
            GL.Vertex2(15, 40);
            GL.Vertex2(105, 40);
            GL.Vertex2(45, -40);
            GL.Vertex2(-45, -40);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            for (var i = 0; i < 8; i++) {
                GL.Vertex2(i * 10 - 95, 40);
                GL.Vertex2(i * 10 - 95, 50);

                Ula.Terminals[i] = new Point(i * 10 - 95, 50);
                Ula.TerminalsString[i] = "A[" + i + "]";
                GL.Vertex2(i * 10 + 25, 40);
                GL.Vertex2(i * 10 + 25, 50);

                Ula.Terminals[i+8] = new Point(i * 10 + 25, 50);
                Ula.TerminalsString[i+8] = "B[" + (i) + "]";
                GL.Vertex2(i * 10 - 35, -40);
                GL.Vertex2(i * 10 - 35, -50);

                Ula.Terminals[i + 20] = new Point(i * 10 - 35, -50);
                Ula.TerminalsString[i + 20] = "O[" + (i) + "]";

            }

            GL.Vertex2(-90, 20);
            GL.Vertex2(-110, 20);
            Ula.Terminals[16] = new Point(-110, 20);
            Ula.TerminalsString[16] = "S0";

            GL.Vertex2(-82, 10);
            GL.Vertex2(-110, 10);
            Ula.Terminals[17] = new Point(-110, 10);
            Ula.TerminalsString[17] = "S1";

            GL.Vertex2(-75, 0);
            GL.Vertex2(-110, 0);
            Ula.Terminals[18] = new Point(-110, 0);
            Ula.TerminalsString[18] = "S2";

            GL.Vertex2(-60, -20);
            GL.Vertex2(-110, -20);
            Ula.Terminals[19] = new Point(-110, -20);
            Ula.TerminalsString[19] = "Enable";
            
            GL.Vertex2(90, 20);
            GL.Vertex2(110, 20);
            Ula.Terminals[28] = new Point(110, 20);
            Ula.TerminalsString[28] = "Flag Z";

            GL.Vertex2(82, 10);
            GL.Vertex2(110, 10);
            Ula.Terminals[29] = new Point(110, 10);
            Ula.TerminalsString[29] = "Flag C";

            GL.End();

            GL.EndList();
        }
        private static void GenRegistrers() {
            TextRenderer.DrawText("R\ne\ng", Color.Black, new PointF(0, 0));
            TextRenderer.DrawText("R\ne\ng\nS\nG", Color.Black, new PointF(0, 0));
            TextRenderer.DrawText("R\ne\ng\ns", Color.Black, new PointF(0, 0));
            TextRenderer.DrawText("R\ne\ng\nC\n/\nB\nu\nf", Color.Black, new PointF(0, 0));
            var drawCircuit = Circuit[11, 8];
            Registrer8Bit = new ComponentDraw(GL.GenLists(1), drawCircuit.Width, drawCircuit.Height, drawCircuit.Terminals.Length);
            for (var i = 0; i < drawCircuit.Terminals.Length; i++) {
                Registrer8Bit.Terminals[i] = drawCircuit.Terminals[i];
                if (i < 8) {
                    Registrer8Bit.TerminalsString[i] = ("In" + (i % 8));
                } else if (i > 10) {
                    Registrer8Bit.TerminalsString[i] = ("Out" + ((i - 3) % 8));
                } else {
                    switch (i) {
                        case 8:
                            Registrer8Bit.TerminalsString[i] = "Enable";
                            break;
                        case 9:
                            Registrer8Bit.TerminalsString[i] = "Clock";
                            break;
                        case 10:
                            Registrer8Bit.TerminalsString[i] = "Reset";
                            break;
                    }

                }
            }
            GL.NewList(Registrer8Bit.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("R\ne\ng", Color.Black, new PointF(0, 0));
            GL.CallList(drawCircuit.DisplayListHandle);
            GL.EndList();

            drawCircuit = Circuit[10, 8];
            Registrer8BitSg = new ComponentDraw(GL.GenLists(1), drawCircuit.Width, drawCircuit.Height, drawCircuit.Terminals.Length);
            for (var i = 0; i < drawCircuit.Terminals.Length; i++) {
                Registrer8BitSg.Terminals[i] = drawCircuit.Terminals[i];
                if (i < 8) {
                    Registrer8BitSg.TerminalsString[i] = ("In" + (i % 8));
                } else if (i > 9) {
                    Registrer8BitSg.TerminalsString[i] = ("Out" + ((i - 2) % 8));
                } else {
                    switch (i) {
                        case 8:
                            Registrer8BitSg.TerminalsString[i] = "Clock";
                            break;
                        case 9:
                            Registrer8BitSg.TerminalsString[i] = "Reset";
                            break;
                    }

                }
            }
            GL.NewList(Registrer8BitSg.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("R\ne\ng\nS\nG", Color.Black, new PointF(0, 0));
            GL.CallList(drawCircuit.DisplayListHandle);
            GL.EndList();

            drawCircuit = Circuit[11, 16];
            Registrer8BitCBuffer = new ComponentDraw(GL.GenLists(1), drawCircuit.Width, drawCircuit.Height, drawCircuit.Terminals.Length);
            for (var i = 0; i < drawCircuit.Terminals.Length; i++) {
                Registrer8BitCBuffer.Terminals[i] = drawCircuit.Terminals[i];
                if (i < 8) {
                    Registrer8BitCBuffer.TerminalsString[i] = ("In" + (i % 8));
                } else if (i > 18) {
                    Registrer8BitCBuffer.TerminalsString[i] = ("OutBus" + ((i - 3) % 8));
                } else if (i > 10) {
                    Registrer8BitCBuffer.TerminalsString[i] = ("OutAcc" + ((i - 3) % 8));
                } else {
                    switch (i) {
                        case 8:
                            Registrer8BitCBuffer.TerminalsString[i] = "C";
                            break;
                        case 9:
                            Registrer8BitCBuffer.TerminalsString[i] = "E";
                            break;
                        case 10:
                            Registrer8BitCBuffer.TerminalsString[i] = "R";
                            break;
                    }

                }
            }
            GL.NewList(Registrer8BitCBuffer.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("R\ne\ng\nC\n/\nB\nu\nf", Color.Black, new PointF(0, 0));
            GL.CallList(drawCircuit.DisplayListHandle);
            GL.EndList();

            drawCircuit = Circuit[13, 8];
            Registrers = new ComponentDraw(GL.GenLists(1), drawCircuit.Width, drawCircuit.Height, drawCircuit.Terminals.Length);
            for (var i = 0; i < drawCircuit.Terminals.Length; i++) {
                Registrers.Terminals[i] = drawCircuit.Terminals[i];
                if (i < 8) {
                    Registrers.TerminalsString[i] = ("In" + i);
                } else if (i >= 13) {
                    Registrers.TerminalsString[i] = ("Out" + ((i - 5) % 8));
                } else {
                    switch (i) {
                        case 8:
                            Registrers.TerminalsString[i] = "E";
                            break;
                        case 9:
                            Registrers.TerminalsString[i] = "C";
                            break;
                        case 10:
                            Registrers.TerminalsString[i] = "R";
                            break;
                        case 11:
                            Registrers.TerminalsString[i] = "S0";
                            break;
                        case 12:
                            Registrers.TerminalsString[i] = "S1";
                            break;
                    }

                }
            }
            GL.NewList(Registrers.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("R\ne\ng\ns", Color.Black, new PointF(0, 0));
            GL.CallList(drawCircuit.DisplayListHandle);
            GL.EndList();
        }
        private static void GenMemories() {
            TextRenderer.DrawText("R\na\nm", Color.Black, new PointF(0, 0));
            TextRenderer.DrawText("R\no\nm", Color.Black, new PointF(0, 0));
            var drawCircuit = Circuit[11, 8];
            RamMemory = new ComponentDraw(GL.GenLists(1), drawCircuit.Width, drawCircuit.Height, drawCircuit.Terminals.Length);
            for (var i = 0; i < drawCircuit.Terminals.Length; i++) {
                RamMemory.Terminals[i] = drawCircuit.Terminals[i];
                if (i < 8) {
                    RamMemory.TerminalsString[i] = ("A" + (i % 8));
                } else if (i > 10) {
                    RamMemory.TerminalsString[i] = ("D" + ((i - 3) % 8));
                } else {
                    switch (i) {
                        case 8:
                            RamMemory.TerminalsString[i] = "sel";
                            break;
                        case 9:
                            RamMemory.TerminalsString[i] = "ld";
                            break;
                        case 10:
                            RamMemory.TerminalsString[i] = "clr";
                            break;
                    }
                    
                }
            }
            GL.NewList(RamMemory.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("R\na\nm", Color.Black, new PointF(0, 0));
            GL.CallList(drawCircuit.DisplayListHandle);
            GL.EndList();


            drawCircuit = Circuit[17, 8];
            RomMemory = new ComponentDraw(GL.GenLists(1), drawCircuit.Width, drawCircuit.Height, drawCircuit.Terminals.Length);
            for (var i = 0; i < drawCircuit.Terminals.Length; i++) {
                RomMemory.Terminals[i] = drawCircuit.Terminals[i];
                if (i < 16) {
                    RomMemory.TerminalsString[i] = ("A" + (i % 16));
                } else if (i > 16) {
                    RomMemory.TerminalsString[i] = ("D" + ((i - 1) % 8));
                } else {
                    RomMemory.TerminalsString[i] = "sel";

                }
            }
            GL.NewList(RomMemory.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("R\no\nm", Color.Black, new PointF(0, 0));
            GL.CallList(drawCircuit.DisplayListHandle);
            GL.EndList();
        }
        private static void GenAdders() {
            TextRenderer.DrawText("Full", Color.Black, new PointF(0, 0));
            TextRenderer.DrawText("Half", Color.Black, new PointF(0, 0));
            HalfAdder = new ComponentDraw(GL.GenLists(1), 60, 40, 4);
            HalfAdder.TerminalsString[0] = "A";
            HalfAdder.TerminalsString[1] = "B";
            HalfAdder.TerminalsString[2] = "S";
            HalfAdder.TerminalsString[3] = "C";
            HalfAdder.Terminals[0] = new Point(-30, 10);
            HalfAdder.Terminals[1] = new Point(-30, -10);

            HalfAdder.Terminals[2] = new Point(30, 10);
            HalfAdder.Terminals[3] = new Point(30, -10);

            GL.NewList(HalfAdder.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("Half", Color.Black, new PointF(0, 0));
            GL.Begin(PrimitiveType.Lines);

            GL.Vertex2(-30, -10);
            GL.Vertex2(-20, -10);

            GL.Vertex2(-30, 10);
            GL.Vertex2(-20, 10);

            GL.Vertex2(30, 10);
            GL.Vertex2(20, 10);

            GL.Vertex2(30, -10);
            GL.Vertex2(20, -10);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-20, -20);
            GL.Vertex2(-20, 20);
            GL.Vertex2(20, 20);
            GL.Vertex2(20, -20);
            GL.End();

            GL.EndList();


            FullAdder = new ComponentDraw(GL.GenLists(1), 60, 40, 5);
            FullAdder.TerminalsString[0] = "A";
            FullAdder.TerminalsString[1] = "B";
            FullAdder.TerminalsString[2] = "Cin";
            FullAdder.TerminalsString[3] = "S";
            FullAdder.TerminalsString[4] = "Cout";
            FullAdder.Terminals[0] = new Point(-30, 10);
            FullAdder.Terminals[1] = new Point(-30, 0);
            FullAdder.Terminals[2] = new Point(-30, -10);

            FullAdder.Terminals[3] = new Point(30, 10);
            FullAdder.Terminals[4] = new Point(30, -10);

            GL.NewList(FullAdder.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("Full", Color.Black, new PointF(0, 0));
            GL.Begin(PrimitiveType.Lines);

            GL.Vertex2(-30, -10);
            GL.Vertex2(-20, -10);

            GL.Vertex2(-30, 0);
            GL.Vertex2(-20, 0);

            GL.Vertex2(-30, 10);
            GL.Vertex2(-20, 10);

            GL.Vertex2(30, 10);
            GL.Vertex2(20, 10);

            GL.Vertex2(30, -10);
            GL.Vertex2(20, -10);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-20, -20);
            GL.Vertex2(-20, 20);
            GL.Vertex2(20, 20);
            GL.Vertex2(20, -20);
            GL.End();

            GL.EndList();
        }
        private static void GenDisable8Bit() {
            TextRenderer.DrawText("T\nr\ni", Color.Black, new PointF(0, 0));
            var drawCircuit = Circuit[9, 8];
            Disable8Bit = new ComponentDraw(GL.GenLists(1), drawCircuit.Width, drawCircuit.Height, drawCircuit.Terminals.Length);
            for (var i = 0; i < drawCircuit.Terminals.Length; i++) {
                Disable8Bit.Terminals[i] = drawCircuit.Terminals[i];
                if (i < 8) {
                    Disable8Bit.TerminalsString[i] = ("in" + (i % 8));
                } else if (i > 8) {
                    Disable8Bit.TerminalsString[i] = ("out"+((i-1) % 8));
                } else {
                    Disable8Bit.TerminalsString[i] = "Enable";
                }
            }
            GL.NewList(Disable8Bit.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("T\nr\ni", Color.Black, new PointF(0, 0));
            GL.CallList(drawCircuit.DisplayListHandle);

            GL.EndList();
        }
        private static void GenDFlipFlop() {
            TextRenderer.DrawText("D", Color.Black, new PointF(0, 0));
            DFlipFlop = new ComponentDraw(GL.GenLists(1), 60, 40, 4);
            DFlipFlop.TerminalsString[0] = "D";
            DFlipFlop.TerminalsString[1] = "CLK";
            DFlipFlop.TerminalsString[2] = "Q";
            DFlipFlop.TerminalsString[3] = "Q'";
            DFlipFlop.Terminals[0] = new Point(-30, 10);
            DFlipFlop.Terminals[1] = new Point(-30, -10);

            DFlipFlop.Terminals[2] = new Point(30, 10);
            DFlipFlop.Terminals[3] = new Point(30, -10);

            GL.NewList(DFlipFlop.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("D", Color.Black, new PointF(0, 0));
            GL.Begin(PrimitiveType.Lines);

            GL.Vertex2(-30, -10);
            GL.Vertex2(-20, -10);

            GL.Vertex2(-30, 10);
            GL.Vertex2(-20, 10);

            GL.Vertex2(30, 10);
            GL.Vertex2(20, 10);

            GL.Vertex2(30, -10);
            GL.Vertex2(20, -10);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-20, -20);
            GL.Vertex2(-20, 20);
            GL.Vertex2(20, 20);
            GL.Vertex2(20, -20);
            GL.End();

            GL.EndList();
        }
        private static void GenTFlipFlop() {
            TextRenderer.DrawText("T", Color.Black, new PointF(0, 0));
            FlipFlop = new ComponentDraw(GL.GenLists(1), 60, 40, 4);
            FlipFlop.TerminalsString[0] = "T";
            FlipFlop.TerminalsString[1] = "CLK";
            FlipFlop.TerminalsString[2] = "Q";
            FlipFlop.TerminalsString[3] = "Q'";
            FlipFlop.Terminals[0] = new Point(-30, 10);
            FlipFlop.Terminals[1] = new Point(-30, -10);

            FlipFlop.Terminals[2] = new Point(30, 10);
            FlipFlop.Terminals[3] = new Point(30, -10);

            GL.NewList(FlipFlop.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("T", Color.Black, new PointF(0, 0));
            GL.Begin(PrimitiveType.Lines);

            GL.Vertex2(-30, -10);
            GL.Vertex2(-20, -10);

            GL.Vertex2(-30, 10);
            GL.Vertex2(-20, 10);

            GL.Vertex2(30, 10);
            GL.Vertex2(20, 10);

            GL.Vertex2(30, -10);
            GL.Vertex2(20, -10);

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-20, -20);
            GL.Vertex2(-20, 20);
            GL.Vertex2(20, 20);
            GL.Vertex2(20, -20);
            GL.End();

            GL.EndList();
        }
        private static void GenRsFlipFlop() {
            TextRenderer.DrawText("SR", Color.Black, new PointF(0, 0));
            RsFlipFlop = new ComponentDraw(GL.GenLists(1), 60, 60, 7);
            RsFlipFlop.TerminalsString[0] = "S";
            RsFlipFlop.TerminalsString[1] = "R";
            RsFlipFlop.TerminalsString[2] = "CLK";
            RsFlipFlop.TerminalsString[3] = "Force set";
            RsFlipFlop.TerminalsString[4] = "Force reset";
            RsFlipFlop.TerminalsString[5] = "Q";
            RsFlipFlop.TerminalsString[6] = "Q'";
            RsFlipFlop.Terminals[0] = new Point(-30, 10);
            RsFlipFlop.Terminals[1] = new Point(-30, -10);
            RsFlipFlop.Terminals[2] = new Point(-30, 0);
            RsFlipFlop.Terminals[3] = new Point(0, 30);
            RsFlipFlop.Terminals[4] = new Point(0, -30);

            RsFlipFlop.Terminals[5] = new Point(30, 10);
            RsFlipFlop.Terminals[6] = new Point(30, -10);

            GL.NewList(RsFlipFlop.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("SR", Color.Black, new PointF(0, 0));
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Lines);

            GL.Vertex2(-30, -10);
            GL.Vertex2(-20, -10);

            GL.Vertex2(-30, 0);
            GL.Vertex2(-20, 0);

            GL.Vertex2(-30, 10);
            GL.Vertex2(-20, 10);

            GL.Vertex2(30, 10);
            GL.Vertex2(20, 10);

            GL.Vertex2(30, -10);
            GL.Vertex2(20, -10);

            GL.Vertex2(0, -20);
            GL.Vertex2(0, -30);

            GL.Vertex2(0, 20);
            GL.Vertex2(0, 30);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-20, -20);
            GL.Vertex2(-20, 20);
            GL.Vertex2(20, 20);
            GL.Vertex2(20, -20);
            GL.End();

            GL.EndList();
        }
        private static void GenJkFlipFlop() {
            TextRenderer.DrawText("JK", Color.Black, new PointF(0, 0));
            JkFlipFlop = new ComponentDraw(GL.GenLists(1), 60, 60, 7);
            JkFlipFlop.TerminalsString[0] = "J";
            JkFlipFlop.TerminalsString[1] = "K";
            JkFlipFlop.TerminalsString[2] = "CLK";
            JkFlipFlop.TerminalsString[3] = "S";
            JkFlipFlop.TerminalsString[4] = "R";
            JkFlipFlop.TerminalsString[5] = "Q";
            JkFlipFlop.TerminalsString[6] = "Q'";
            JkFlipFlop.Terminals[0] = new Point(-30, 10);
            JkFlipFlop.Terminals[1] = new Point(-30, -10);
            JkFlipFlop.Terminals[2] = new Point(-30, 0);
            JkFlipFlop.Terminals[3] = new Point(0, 30);
            JkFlipFlop.Terminals[4] = new Point(0, -30);

            JkFlipFlop.Terminals[5] = new Point(30, 10);
            JkFlipFlop.Terminals[6] = new Point(30, -10);

            GL.NewList(JkFlipFlop.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("JK", Color.Black, new PointF(0, 0));
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Lines);

            GL.Vertex2(-30, -10);
            GL.Vertex2(-20, -10);

            GL.Vertex2(-30, 0);
            GL.Vertex2(-20, 0);

            GL.Vertex2(-30, 10);
            GL.Vertex2(-20, 10);

            GL.Vertex2(30, 10);
            GL.Vertex2(20, 10);

            GL.Vertex2(30, -10);
            GL.Vertex2(20, -10);

            GL.Vertex2(0, -20);
            GL.Vertex2(0, -30);

            GL.Vertex2(0, 20);
            GL.Vertex2(0, 30);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-20, -20);
            GL.Vertex2(-20, 20);
            GL.Vertex2(20, 20);
            GL.Vertex2(20, -20);
            GL.End();

            GL.EndList();
        }
        private static void GenOsciloscope() {
            TextRenderer.DrawText("Osc", Color.Black, new PointF(0, 0));
            Osciloscope = new ComponentDraw(GL.GenLists(1), 30, 20);
            Osciloscope.Terminals[0] = new Point(-15, 0);
            GL.NewList(Osciloscope.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("Osc", Color.Black, new PointF(0, 0));
            GL.Color3(ColorOff);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-15, 0);
            GL.Vertex2(-10, 0);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-10, -10);
            GL.Vertex2(-10, 10);
            GL.Vertex2(15, 10);
            GL.Vertex2(15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex2(-6, -6);
            GL.Vertex2(2, -6);
            GL.Vertex2(2, 6);
            GL.Vertex2(10, 6);
            GL.End();

            GL.EndList();
        }
        private static void Gen7SegDisplay() {
            Display7SegPart = new int[8];
            Display7SegPart[0] = GL.GenLists(1);
            GL.NewList(Display7SegPart[0], ListMode.Compile); //a
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color3(ColorOn);
            GL.Vertex2(-16, 35);
            GL.Vertex2(-14, 37);
            GL.Vertex2(20, 37);
            GL.Vertex2(22, 35);
            GL.Vertex2(20, 33);
            GL.Vertex2(-14, 33);
            GL.Vertex2(-16, 35);
            GL.End();
            GL.EndList();

            Display7SegPart[1] = GL.GenLists(1);
            GL.NewList(Display7SegPart[1], ListMode.Compile); //b
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color3(ColorOn);
            GL.Vertex2(22, 35);
            GL.Vertex2(24, 33);
            GL.Vertex2(24, 2);
            GL.Vertex2(22, 0);
            GL.Vertex2(20, 2);
            GL.Vertex2(20, 33);
            GL.Vertex2(22, 35);
            GL.End();
            GL.EndList();

            Display7SegPart[2] = GL.GenLists(1);
            GL.NewList(Display7SegPart[2], ListMode.Compile); //c
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color3(ColorOn);
            GL.Vertex2(22, -35);
            GL.Vertex2(24, -33);
            GL.Vertex2(24, -2);
            GL.Vertex2(22, 0);
            GL.Vertex2(20, -2);
            GL.Vertex2(20, -33);
            GL.Vertex2(22, -35);
            GL.End();
            GL.EndList();

            Display7SegPart[3] = GL.GenLists(1);
            GL.NewList(Display7SegPart[3], ListMode.Compile); //d
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color3(ColorOn);
            GL.Vertex2(-16, -35);
            GL.Vertex2(-14, -37);
            GL.Vertex2(20, -37);
            GL.Vertex2(22, -35);
            GL.Vertex2(20, -33);
            GL.Vertex2(-14, -33);
            GL.Vertex2(-16, -35);
            GL.End();
            GL.EndList();

            Display7SegPart[4] = GL.GenLists(1);
            GL.NewList(Display7SegPart[4], ListMode.Compile); //e
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color3(ColorOn);
            GL.Vertex2(-15, -35);
            GL.Vertex2(-17, -33);
            GL.Vertex2(-17, -2);
            GL.Vertex2(-15, 0);
            GL.Vertex2(-13, -2);
            GL.Vertex2(-13, -33);
            GL.Vertex2(-15, -35);
            GL.End();
            GL.EndList();

            Display7SegPart[5] = GL.GenLists(1);
            GL.NewList(Display7SegPart[5], ListMode.Compile); //f
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color3(ColorOn);
            GL.Vertex2(-15, 35);
            GL.Vertex2(-17, 33);
            GL.Vertex2(-17, 2);
            GL.Vertex2(-15, 0);
            GL.Vertex2(-13, 2);
            GL.Vertex2(-13, 33);
            GL.Vertex2(-15, 35);
            GL.End();
            GL.EndList();

            Display7SegPart[6] = GL.GenLists(1);
            GL.NewList(Display7SegPart[6], ListMode.Compile); //g
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color3(ColorOn);
            GL.Vertex2(-16, 0);
            GL.Vertex2(-14, 2);
            GL.Vertex2(20, 2);
            GL.Vertex2(22, 0);
            GL.Vertex2(20, -2);
            GL.Vertex2(-14, -2);
            GL.Vertex2(-16, 0);
            GL.End();
            GL.EndList();

            Display7SegPart[7] = GL.GenLists(1);
            GL.NewList(Display7SegPart[7], ListMode.Compile); //dot
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color3(ColorOn);
            GL.Vertex2(24, -33);
            GL.Vertex2(24, -38);
            GL.Vertex2(29, -38);
            GL.Vertex2(29, -33);
            GL.Vertex2(24, -33);
            GL.End();
            GL.EndList();
            
            Display7SegBase = new ComponentDraw(GL.GenLists(1), 60, 80, 8);
            for (var i = 0; i < 8; i++) {
                Display7SegBase.Terminals[i] = new Point(-30, -i * 10 + 35);
                if(i < 7)
                    Display7SegBase.TerminalsString[i] = ((char)('a' + i)).ToString();
                else
                    Display7SegBase.TerminalsString[i] = "dot";
            }
            GL.NewList(Display7SegBase.DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOff);
            GL.Begin(PrimitiveType.Lines);
            for (var i = 0; i < 8; i++) {
                GL.Vertex2(-30, -i * 10 + 35);
                GL.Vertex2(-20, -i * 10 + 35);
            }
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-20, -40);
            GL.Vertex2(30, -40);
            GL.Vertex2(30, 40);
            GL.Vertex2(-20, 40);
            GL.End();
            GL.EndList();


            TextRenderer.DrawText("B\nC\nD", Color.Black, new PointF(0, 0));
            var drawCircuit = Circuit[5, 7];
            BinTo7Seg = new ComponentDraw(GL.GenLists(1), drawCircuit.Width, drawCircuit.Height, drawCircuit.Terminals.Length);
            for (var i = 0; i < drawCircuit.Terminals.Length; i++) {
                BinTo7Seg.Terminals[i] = drawCircuit.Terminals[i];
                if (i < 4) {
                    BinTo7Seg.TerminalsString[i] = ("" + (char)('A' + i));
                } else {
                    if(i == 4)
                        BinTo7Seg.TerminalsString[i] = ("Enable");
                    else
                        BinTo7Seg.TerminalsString[i] = ("" + (char)('a'+i-5));
                }
            }
            GL.NewList(BinTo7Seg.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("B\nC\nD", Color.Black, new PointF(0, 0));
            GL.CallList(drawCircuit.DisplayListHandle);

            GL.EndList();



        }
        private static void GenMicrocontroller() {
            TextRenderer.DrawText("M+++", Color.Black, new PointF(0, 0));
            var drawCircuit = Circuit[32, 32];
            Microcontroller = new ComponentDraw(GL.GenLists(1), drawCircuit.Width, drawCircuit.Height, drawCircuit.Terminals.Length);
            for (var i = 0; i < drawCircuit.Terminals.Length; i++) {
                Microcontroller.Terminals[i] = drawCircuit.Terminals[i];
                if(i < 8 * 4) {
                    Microcontroller.TerminalsString[i] = ("in" + (i / 8) + "[" + (i % 8) + "]");
                } else {
                    Microcontroller.TerminalsString[i] = ("out" + ((i / 8)-4) + "[" + (i % 8) + "]");
                }
            }
            GL.NewList(Microcontroller.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("M+++", Color.Black, new PointF(0, 0));
            GL.CallList(drawCircuit.DisplayListHandle);

            GL.EndList();
        }
        private static void GenKeyboard() {
            Keyboard = new ComponentDraw(GL.GenLists(1), 90, 50, 8);
            for (var i = 0; i < Keyboard.Terminals.Length; i++) {
                Keyboard.Terminals[i] = new Point(-Keyboard.Width / 2+i*10+10, Keyboard.Height / 2);
            }

            GL.NewList(Keyboard.DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOff);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-Keyboard.Width / 2, -Keyboard.Height / 2);
            GL.Vertex2(Keyboard.Width / 2, -Keyboard.Height / 2);
            GL.Vertex2(Keyboard.Width / 2, Keyboard.Height / 2 - 10);
            GL.Vertex2(-Keyboard.Width / 2, Keyboard.Height / 2 - 10);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            for (var i = 0; i < Keyboard.Terminals.Length; i++) {
                GL.Vertex2(-Keyboard.Width / 2 + i * 10 + 10, Keyboard.Height / 2);
                GL.Vertex2(-Keyboard.Width / 2 + i * 10 + 10, Keyboard.Height / 2-10);
            }
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            for (var y = 0; y < 3; y++) {
                for (var x = 0; x < 8; x++) {
                    if (y % 2 == 0) {
                        GL.Vertex2(-40 + 10 * x, 10 - 10 * y); GL.Vertex2(-35 + 10 * x, 10 - 10 * y); GL.Vertex2(-35 + 10 * x, 5 - 10 * y); GL.Vertex2(-40 + 10 * x, 5 - 10 * y);
                    } else {
                        GL.Vertex2(-35 + 10 * x, 10 - 10 * y); GL.Vertex2(-30 + 10 * x, 10 - 10 * y); GL.Vertex2(-30 + 10 * x, 5 - 10 * y); GL.Vertex2(-35 + 10 * x, 5 - 10 * y);
                    }
                }
                if (y == 2) {
                    GL.Vertex2(-40 + 10 * 5, 10 - 10 * y); GL.Vertex2(-35 + 10 * 2, 10 - 10 * y); GL.Vertex2(-35 + 10 * 2, 5 - 10 * y); GL.Vertex2(-40 + 10 * 5, 5 - 10 * y);
                }
            }
            
            GL.End();

            GL.EndList();
        }
        private static void GenXnor() {
            var total = 8;
            var halfCircle1 = new Vector2[total];
            var halfCircle2 = new Vector2[total];
            var halfCircle3 = new Vector2[total];
            var circle = new Vector2[total];
            for (var i = 0; i < total; i++) {
                var rotDegree = Math.PI * ((360d / ((total + 1) * 2d)) * (i + 1)) / 180.0;
                var rotDegree2 = Math.PI * ((360d / total) * i) / 180.0;
                double x = 0;
                double y = -10;
                halfCircle1[i] = new Vector2();
                halfCircle1[i].X = (float)(x * Math.Cos(rotDegree) - y * Math.Sin(rotDegree)) * 2;
                halfCircle1[i].Y = (float)(x * Math.Sin(rotDegree) + y * Math.Cos(rotDegree));
                halfCircle1[i].X -= 10;
                halfCircle2[i] = new Vector2();
                halfCircle2[i].X = (float)(x * Math.Cos(rotDegree) - y * Math.Sin(rotDegree)) / 2;
                halfCircle2[i].Y = (float)(x * Math.Sin(rotDegree) + y * Math.Cos(rotDegree));
                halfCircle2[i].X -= 15;
                halfCircle3[i] = new Vector2();
                halfCircle3[i].X = (float)(x * Math.Cos(rotDegree) - y * Math.Sin(rotDegree)) / 2;
                halfCircle3[i].Y = (float)(x * Math.Sin(rotDegree) + y * Math.Cos(rotDegree));
                halfCircle3[i].X -= 17;
                x = 3;
                y = 0;
                circle[i].X = (float)(x * Math.Cos(rotDegree2) - y * Math.Sin(rotDegree2));
                circle[i].Y = (float)(x * Math.Sin(rotDegree2) + y * Math.Cos(rotDegree2));
                circle[i].X += 13;
            }
            Xnor = new ComponentDraw[2];
            Xnor[0] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Xnor[0].Terminals[0] = new Point(-20, 5);
            Xnor[0].Terminals[1] = new Point(-20, -5);
            Xnor[0].Terminals[2] = new Point(20, 0);
            GL.NewList(Xnor[0].DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOff);
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(halfCircle2[i]);
            }
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (var i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineStrip);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(halfCircle3[i]);
            }
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(circle[i]);
            }
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-20, 5);
            GL.Vertex2(-13, 5);
            GL.Vertex2(-20, -5);
            GL.Vertex2(-13, -5);
            GL.Vertex2(20, 0);
            GL.Vertex2(16, 0);
            GL.End();
            GL.EndList();

            Xnor[1] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Xnor[1].Terminals[0] = new Point(-20, 5);
            Xnor[1].Terminals[1] = new Point(-20, -5);
            Xnor[1].Terminals[2] = new Point(20, 0);
            GL.NewList(Xnor[1].DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOn);
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(halfCircle2[i]);
            }
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (var i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineStrip);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(halfCircle3[i]);
            }
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(circle[i]);
            }
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-20, 5);
            GL.Vertex2(-13, 5);
            GL.Vertex2(-20, -5);
            GL.Vertex2(-13, -5);
            GL.Vertex2(20, 0);
            GL.Vertex2(16, 0);
            GL.End();
            GL.EndList();
        }
        private static void GenXor() {

            var total = 8;
            var halfCircle1 = new Vector2[total];
            var halfCircle2 = new Vector2[total];
            var halfCircle3 = new Vector2[total];
            for (var i = 0; i < total; i++) {
                var rotDegree = Math.PI * ((360d / ((total+1) * 2d)) * (i+1)) / 180.0;
                double x = 0;
                double y = -10;
                halfCircle1[i] = new Vector2();
                halfCircle1[i].X = (float)(x * Math.Cos(rotDegree) - y * Math.Sin(rotDegree)) * 2;
                halfCircle1[i].Y = (float)(x * Math.Sin(rotDegree) + y * Math.Cos(rotDegree));
                halfCircle1[i].X -= 10;
                halfCircle2[i] = new Vector2();
                halfCircle2[i].X = (float)(x * Math.Cos(rotDegree) - y * Math.Sin(rotDegree)) / 2;
                halfCircle2[i].Y = (float)(x * Math.Sin(rotDegree) + y * Math.Cos(rotDegree));
                halfCircle2[i].X -= 15;
                halfCircle3[i] = new Vector2();
                halfCircle3[i].X = (float)(x * Math.Cos(rotDegree) - y * Math.Sin(rotDegree)) / 2;
                halfCircle3[i].Y = (float)(x * Math.Sin(rotDegree) + y * Math.Cos(rotDegree));
                halfCircle3[i].X -= 17;
            }
            Xor = new ComponentDraw[2];
            Xor[0] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Xor[0].Terminals[0] = new Point(-20, 5);
            Xor[0].Terminals[1] = new Point(-20, -5);
            Xor[0].Terminals[2] = new Point(20, 0);
            GL.NewList(Xor[0].DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOff);
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(halfCircle2[i]);
            }
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (var i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineStrip);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(halfCircle3[i]);
            }
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-20, 5);
            GL.Vertex2(-13, 5);
            GL.Vertex2(-20, -5);
            GL.Vertex2(-13, -5);
            GL.Vertex2(20, 0);
            GL.Vertex2(10, 0);
            GL.End();
            GL.EndList();

            Xor[1] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Xor[1].Terminals[0] = new Point(-20, 5);
            Xor[1].Terminals[1] = new Point(-20, -5);
            Xor[1].Terminals[2] = new Point(20, 0);
            GL.NewList(Xor[1].DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOn);
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(halfCircle2[i]);
            }
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (var i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineStrip);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(halfCircle3[i]);
            }
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-20, 5);
            GL.Vertex2(-13, 5);
            GL.Vertex2(-20, -5);
            GL.Vertex2(-13, -5);
            GL.Vertex2(20, 0);
            GL.Vertex2(10, 0);
            GL.End();
            GL.EndList();
        }
        private static void GenNand() {
            var total = 8;
            var halfCircle1 = new Vector2[total];
            var circle = new Vector2[total];
            for (var i = 0; i < total; i++) {
                var rotDegree = Math.PI * ((360d / ((total+1) * 2d)) * (i + 1)) / 180.0;
                var rotDegree2 = Math.PI * ((360d / total) * i) / 180.0;
                double x = 0;
                double y = -10;
                halfCircle1[i] = new Vector2();
                halfCircle1[i].X = (float)(x * Math.Cos(rotDegree) - y * Math.Sin(rotDegree));
                halfCircle1[i].Y = (float)(x * Math.Sin(rotDegree) + y * Math.Cos(rotDegree));
                x = 3;
                y = 0;
                circle[i].X = (float)(x * Math.Cos(rotDegree2) - y * Math.Sin(rotDegree2));
                circle[i].Y = (float)(x * Math.Sin(rotDegree2) + y * Math.Cos(rotDegree2));
                circle[i].X += 13;
            }
            Nand = new ComponentDraw[2];
            Nand[0] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Nand[0].Terminals[0] = new Point(-20, 5);
            Nand[0].Terminals[1] = new Point(-20, -5);
            Nand[0].Terminals[2] = new Point(20, 0);
            GL.NewList(Nand[0].DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOff);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-15, 10);
            GL.Vertex2(0, 10);
            for (var i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(0, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(circle[i]);
            }
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-20, 5);
            GL.Vertex2(-15, 5);
            GL.Vertex2(-20, -5);
            GL.Vertex2(-15, -5);
            GL.Vertex2(20, 0);
            GL.Vertex2(16, 0);
            GL.End();
            GL.EndList();

            Nand[1] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Nand[1].Terminals[0] = new Point(-20, 5);
            Nand[1].Terminals[1] = new Point(-20, -5);
            Nand[1].Terminals[2] = new Point(20, 0);
            GL.NewList(Nand[1].DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOn);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (var i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(circle[i]);
            }
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-20, 5);
            GL.Vertex2(-15, 5);
            GL.Vertex2(-20, -5);
            GL.Vertex2(-15, -5);
            GL.Vertex2(20, 0);
            GL.Vertex2(16, 0);
            GL.End();
            GL.EndList();
        }
        private static void GenAnd() {
            var total = 8;
            var halfCircle1 = new Vector2[total];
            for (var i = 0; i < total; i++) {
                var rotDegree = Math.PI * ((360d / ((total + 1) * 2d)) * (i + 1)) / 180.0;
                double x = 0;
                double y = -10;
                halfCircle1[i] = new Vector2();
                halfCircle1[i].X = (float)(x * Math.Cos(rotDegree) - y * Math.Sin(rotDegree));
                halfCircle1[i].Y = (float)(x * Math.Sin(rotDegree) + y * Math.Cos(rotDegree));
            }
            And = new ComponentDraw[2];
            And[0] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            And[0].Terminals[0] = new Point(-20, 5);
            And[0].Terminals[1] = new Point(-20, -5);
            And[0].Terminals[2] = new Point(20, 0);
            GL.NewList(And[0].DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOff);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-15, 10);
            GL.Vertex2(0, 10);
            for (var i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(0, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-20, 5);
            GL.Vertex2(-15, 5);
            GL.Vertex2(-20, -5);
            GL.Vertex2(-15, -5);
            GL.Vertex2(20, 0);
            GL.Vertex2(10, 0);
            GL.End();
            GL.EndList();

            And[1] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            And[1].Terminals[0] = new Point(-20, 5);
            And[1].Terminals[1] = new Point(-20, -5);
            And[1].Terminals[2] = new Point(20, 0);
            GL.NewList(And[1].DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOn);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (var i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-20, 5);
            GL.Vertex2(-15, 5);
            GL.Vertex2(-20, -5);
            GL.Vertex2(-15, -5);
            GL.Vertex2(20, 0);
            GL.Vertex2(10, 0);
            GL.End();
            GL.EndList();
        }
        private static void GenNor() {
            var total = 8;
            var halfCircle1 = new Vector2[total];
            var halfCircle2 = new Vector2[total];
            var circle = new Vector2[total];
            for (var i = 0; i < total; i++) {
                var rotDegree = Math.PI * ((360d / ((total + 1) * 2d)) * (i + 1)) / 180.0;
                var rotDegree2 = Math.PI * ((360d / total) * i) / 180.0;
                double x = 0;
                double y = -10;
                halfCircle1[i] = new Vector2();
                halfCircle1[i].X = (float)(x * Math.Cos(rotDegree) - y * Math.Sin(rotDegree)) * 2;
                halfCircle1[i].Y = (float)(x * Math.Sin(rotDegree) + y * Math.Cos(rotDegree));
                halfCircle1[i].X -= 10;
                halfCircle2[i] = new Vector2();
                halfCircle2[i].X = (float)(x * Math.Cos(rotDegree) - y * Math.Sin(rotDegree)) / 2;
                halfCircle2[i].Y = (float)(x * Math.Sin(rotDegree) + y * Math.Cos(rotDegree));
                halfCircle2[i].X -= 15;
                x = 3;
                y = 0;
                circle[i].X = (float)(x * Math.Cos(rotDegree2) - y * Math.Sin(rotDegree2));
                circle[i].Y = (float)(x * Math.Sin(rotDegree2) + y * Math.Cos(rotDegree2));
                circle[i].X += 13;
            }
            Nor = new ComponentDraw[2];
            Nor[0] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Nor[0].Terminals[0] = new Point(-20, 5);
            Nor[0].Terminals[1] = new Point(-20, -5);
            Nor[0].Terminals[2] = new Point(20, 0);
            GL.NewList(Nor[0].DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOff);
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(halfCircle2[i]);
            }
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (var i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(circle[i]);
            }
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-20, 5);
            GL.Vertex2(-11, 5);
            GL.Vertex2(-20, -5);
            GL.Vertex2(-11, -5);
            GL.Vertex2(20, 0);
            GL.Vertex2(16, 0);
            GL.End();
            GL.EndList();

            Nor[1] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Nor[1].Terminals[0] = new Point(-20, 5);
            Nor[1].Terminals[1] = new Point(-20, -5);
            Nor[1].Terminals[2] = new Point(20, 0);
            GL.NewList(Nor[1].DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOn);
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(halfCircle2[i]);
            }
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (var i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(circle[i]);
            }
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-20, 5);
            GL.Vertex2(-11, 5);
            GL.Vertex2(-20, -5);
            GL.Vertex2(-11, -5);
            GL.Vertex2(20, 0);
            GL.Vertex2(16, 0);
            GL.End();
            GL.EndList();
        }
        private static void GenOr() {
            var total = 8;
            var halfCircle1 = new Vector2[total];
            var halfCircle2 = new Vector2[total];
            for (var i = 0; i < total; i++) {
                var rotDegree = Math.PI * ((360d / ((total + 1) * 2d)) * (i+1)) / 180.0;
                double x = 0;
                double y = -10;
                halfCircle1[i] = new Vector2();
                halfCircle1[i].X = (float)(x * Math.Cos(rotDegree) - y * Math.Sin(rotDegree))*2;
                halfCircle1[i].Y = (float)(x * Math.Sin(rotDegree) + y * Math.Cos(rotDegree));
                halfCircle1[i].X -= 10;
                halfCircle2[i] = new Vector2();
                halfCircle2[i].X = (float)(x * Math.Cos(rotDegree) - y * Math.Sin(rotDegree))/2;
                halfCircle2[i].Y = (float)(x * Math.Sin(rotDegree) + y * Math.Cos(rotDegree));
                halfCircle2[i].X -= 15;
            }
            Or = new ComponentDraw[2];
            Or[0] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Or[0].Terminals[0] = new Point(-20, 5);
            Or[0].Terminals[1] = new Point(-20, -5);
            Or[0].Terminals[2] = new Point(20, 0);
            GL.NewList(Or[0].DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOff);
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(halfCircle2[i]);
            }
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (var i = total-1; i >= 0 ; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-20, 5);
            GL.Vertex2(-11, 5);
            GL.Vertex2(-20, -5);
            GL.Vertex2(-11, -5);
            GL.Vertex2(20, 0);
            GL.Vertex2(10, 0);
            GL.End();
            GL.EndList();

            Or[1] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Or[1].Terminals[0] = new Point(-20, 5);
            Or[1].Terminals[1] = new Point(-20, -5);
            Or[1].Terminals[2] = new Point(20, 0);
            GL.NewList(Or[1].DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOn);
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(halfCircle2[i]);
            }
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (var i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-20, 5);
            GL.Vertex2(-11, 5);
            GL.Vertex2(-20, -5);
            GL.Vertex2(-11, -5);
            GL.Vertex2(20, 0);
            GL.Vertex2(10, 0);
            GL.End();
            GL.EndList();
        }
        private static void GenNot() {
            var total = 8;
            var circle = new Vector2[total];
            for (var i = 0; i < total; i++) {
                var rotDegree = Math.PI * ((360d / total) * i) / 180.0;
                double x = 3;
                double y = 0;
                circle[i] = new Vector2();
                circle[i].X = (float)(x * Math.Cos(rotDegree) - y * Math.Sin(rotDegree));
                circle[i].Y = (float)(x * Math.Sin(rotDegree) + y * Math.Cos(rotDegree));
                circle[i].X += 13;
            }
            Not = new ComponentDraw[3];
            Not[0] = new ComponentDraw(GL.GenLists(1), 40, 20, 2);
            Not[0].Terminals[0] = new Point(-20, 0);
            Not[0].Terminals[1] = new Point(20, 0);
            GL.NewList(Not[0].DisplayListHandle, ListMode.Compile);

            GL.Color3(ColorOff);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(10, 0);
            GL.Vertex2(-10, 10);
            GL.Vertex2(-10, -10);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-10, 0);
            GL.Vertex2(-20, 0);
            GL.Color3(ColorOn);
            GL.Vertex2(16, 0);
            GL.Vertex2(20, 0);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(circle[i]);
            }
            GL.End();

            GL.EndList();

            Not[1] = new ComponentDraw(GL.GenLists(1), 40, 20, 2);
            Not[1].Terminals[0] = new Point(-20, 0);
            Not[1].Terminals[1] = new Point(20, 0);
            GL.NewList(Not[1].DisplayListHandle, ListMode.Compile);

            GL.Color3(ColorOn);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(10, 0);
            GL.Vertex2(-10, 10);
            GL.Vertex2(-10, -10);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-10, 0);
            GL.Vertex2(-20, 0);
            GL.Color3(ColorOff);
            GL.Vertex2(16, 0);
            GL.Vertex2(20, 0);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(circle[i]);
            }
            GL.End();

            GL.EndList();
        }

        private static void GenDisable() {
            Disable = new ComponentDraw[3];
            Disable[0] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Disable[0].Terminals[0] = new Point(-20, 0);
            Disable[0].Terminals[1] = new Point(0, 10);
            Disable[0].Terminals[2] = new Point(20, 0);
            GL.NewList(Disable[0].DisplayListHandle, ListMode.Compile);

            GL.Color3(ColorOff);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(10, 0);
            GL.Vertex2(-10, 10);
            GL.Vertex2(-10, -10);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(10, 0);
            GL.Vertex2(20, 0);
            GL.Vertex2(-10, 0);
            GL.Vertex2(-20, 0);
            GL.Color3(ColorOn);
            GL.Vertex2(0, 5);
            GL.Vertex2(0, 10);
            GL.End();

            GL.EndList();


            Disable[1] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Disable[1].Terminals[0] = new Point(-20, 0);
            Disable[1].Terminals[1] = new Point(0, 10);
            Disable[1].Terminals[2] = new Point(20, 0);
            GL.NewList(Disable[1].DisplayListHandle, ListMode.Compile);

            GL.Color3(ColorOn);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(10, 0);
            GL.Vertex2(-10, 10);
            GL.Vertex2(-10, -10);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(10, 0);
            GL.Vertex2(20, 0);
            GL.Vertex2(-10, 0);
            GL.Vertex2(-20, 0);
            GL.Vertex2(0, 5);
            GL.Vertex2(0, 10);
            GL.End();

            GL.EndList();

            Disable[2] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Disable[2].Terminals[0] = new Point(-20, 0);
            Disable[2].Terminals[1] = new Point(0, 10);
            Disable[2].Terminals[2] = new Point(20, 0);
            GL.NewList(Disable[2].DisplayListHandle, ListMode.Compile);

            GL.Color3(Color_3Rd);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(10, 0);
            GL.Vertex2(-10, 10);
            GL.Vertex2(-10, -10);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(10, 0);
            GL.Vertex2(20, 0);
            GL.Vertex2(-10, 0);
            GL.Vertex2(-20, 0);
            GL.Color3(ColorOff);
            GL.Vertex2(0, 5);
            GL.Vertex2(0, 10);
            GL.End();

            GL.EndList();
        }
        private static void GenTerminal() {
            TerminalHandle = GL.GenLists(1);
            GL.NewList(TerminalHandle, ListMode.Compile);

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-2, -2);
            GL.Vertex2(-2, 2);
            GL.Vertex2(2, 2);
            GL.Vertex2(2, -2);
            GL.End();

            GL.EndList();
        }
        private static void GenOutput() {
            var total = 8;
            var outerVertexes = new Vector2[total];
            var innerVertexes = new Vector2[total];
            for (var i = 0; i < total; i++) {
                var rotDegree = Math.PI * ((360d / total) * i) / 180.0;
                double x = 5;
                double y = 0;
                outerVertexes[i] = new Vector2();
                outerVertexes[i].X = (float)(x * Math.Cos(rotDegree) - y * Math.Sin(rotDegree));
                outerVertexes[i].Y = (float)(x * Math.Sin(rotDegree) + y * Math.Cos(rotDegree));
                innerVertexes[i] = new Vector2();
                x = 3;
                y = 0;
                innerVertexes[i].X = (float)(x * Math.Cos(rotDegree) - y * Math.Sin(rotDegree));
                innerVertexes[i].Y = (float)(x * Math.Sin(rotDegree) + y * Math.Cos(rotDegree));
            }
            Output = new ComponentDraw[2];
            Output[0] = new ComponentDraw(GL.GenLists(1), 15, 10, 1);
            Output[0].Terminals[0] = new Point(-10, 0);
            GL.NewList(Output[0].DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOff);
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(outerVertexes[i]);
            }
            GL.End();

            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color3(ColorOff);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(innerVertexes[i]);
            }
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(new Vector2(-5, 0));
            GL.Vertex2(new Vector2(-10, 0));
            GL.End();
            GL.EndList();

            Output[1] = new ComponentDraw(GL.GenLists(1), 15, 10, 1);
            Output[1].Terminals[0] = new Point(-10, 0);
            GL.NewList(Output[1].DisplayListHandle, ListMode.Compile);
            GL.Color3(ColorOff);
            GL.Begin(PrimitiveType.LineLoop);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(outerVertexes[i]);
            }
            GL.End();
            GL.Color3(ColorOn);
            GL.Begin(PrimitiveType.TriangleFan);
            for (var i = 0; i < total; i++) {
                GL.Vertex2(innerVertexes[i]);
            }
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(new Vector2(-5, 0));
            GL.Vertex2(new Vector2(-10, 0));
            GL.End();
            GL.EndList();

        }
        private static void GenInput() {
            Input = new ComponentDraw[2];
            Input[0] = new ComponentDraw(GL.GenLists(1), 15, 10, 1);
            Input[0].Terminals[0] = new Point(10, 0);
            GL.NewList(Input[0].DisplayListHandle, ListMode.Compile);

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(ColorOff);
            GL.Vertex2(-5, -5);
            GL.Vertex2(-5, 5);
            GL.Vertex2(5, 5);
            GL.Vertex2(5, -5);
            GL.End();
            
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(ColorOff);
            GL.Vertex2(-3, -3);
            GL.Vertex2(-3, 3);
            GL.Vertex2(3, 3);
            GL.Vertex2(3, -3);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Color3(ColorOff);
            GL.Vertex2(5, 0);
            GL.Vertex2(10, 0);
            GL.End();
            
            GL.EndList();

            Input[1] = new ComponentDraw(GL.GenLists(1), 15, 10, 1);
            Input[1].Terminals[0] = new Point(10, 0);
            GL.NewList(Input[1].DisplayListHandle, ListMode.Compile);

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(ColorOff);
            GL.Vertex2(-5, -5);
            GL.Vertex2(-5, 5);
            GL.Vertex2(5, 5);
            GL.Vertex2(5, -5);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Color3(ColorOn);
            GL.Vertex2(-3, -3);
            GL.Vertex2(-3, 3);
            GL.Vertex2(3, 3);
            GL.Vertex2(3, -3);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Color3(ColorOn);
            GL.Vertex2(5, 0);
            GL.Vertex2(10, 0);
            GL.End();

            GL.EndList();
        }

    }
}