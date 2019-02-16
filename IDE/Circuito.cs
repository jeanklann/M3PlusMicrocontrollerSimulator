using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using CircuitSimulator;
using System.Threading;
using CircuitSimulator.Components.Digital;
using CircuitSimulator.Components.Digital.MMaisMaisMais;
using System.Diagnostics;

namespace IDE {
    public partial class Circuito : GLControl {
        public InternalComponents InternalComponents;
        public MouseProps MouseProps;
        public Terminals Terminals;
        public List<Wire> Wires = new List<Wire>();
        public byte KeyDownByte;
        public Wire HoverWire;
        public Wire SelectedWire;
        public Component Over;
        public Component Selected;
        private bool _canDrag;
        private Circuit _circuit;
        private Thread _thread;
        private Thread _threadDraw;
        private Thread _threadStatesPanel;
        public PointF Position;
        public PointF PositionCreatingComponent;
        public Component InsideComponent;
        public List<Component> Components = new List<Component>();
        public Dictionary<Component, CircuitSimulator.Component> CircuitComponentToDrawComponents = new Dictionary<Component, CircuitSimulator.Component>();
        public Dictionary<Wire, CircuitSimulator.Components.Wire> CircuitWireToDrawWire = new Dictionary<Wire, CircuitSimulator.Components.Wire>();
        public Dictionary<CircuitSimulator.Component, Component> DrawComponentsToCircuitComponent = new Dictionary<CircuitSimulator.Component, Component>();
        public Dictionary<CircuitSimulator.Components.Wire, Wire> DrawWireToCircuitWire = new Dictionary<CircuitSimulator.Components.Wire, Wire>();
        public Color ClearColor;
        public List<ExtraTerminal> ExtraTerminals;
        public bool Loaded;
        public bool Changed;
        public InstructionLog InstructionLog = new InstructionLog();
        private bool _isDebugMode;
        private Queue<Keys> _konamiCode = new Queue<Keys>();
        private Stopwatch _syncronizer = new Stopwatch();
        private Keys[] _konamiCodeCorrect = new Keys[] { Keys.NumPad8, Keys.NumPad8, Keys.NumPad2, Keys.NumPad2, Keys.NumPad4, Keys.NumPad6, Keys.NumPad4, Keys.NumPad6, Keys.B, Keys.A };
        public Circuito() {
            InitializeComponent();
        }

        public void Run() {
            if(_circuit == null) {
                MountCircuit();
                _thread = new Thread(ThreadRun);
                _thread.Start();
            }
        }
        private void ThreadStatesUpdate() {
            //for (int i = 0; i < 8; i++) {
            //    ControlModule.TerminalsString[i] = "In" + i;
            //}
            //ControlModule.TerminalsString[8] = "Clock";
            //ControlModule.TerminalsString[9] = "Flag IN carry";
            //ControlModule.TerminalsString[10] = "Flag IN zero";
            //ControlModule.TerminalsString[11] = "EOI";
            //ControlModule.TerminalsString[12] = "ROM rd";
            //ControlModule.TerminalsString[13] = "ROM cs";
            //ControlModule.TerminalsString[14] = "PCH bus";
            //ControlModule.TerminalsString[15] = "PCL bus";
            //ControlModule.TerminalsString[16] = "PCH clock";
            //ControlModule.TerminalsString[17] = "PCL clock";
            //ControlModule.TerminalsString[18] = "Data PC sel";
            //ControlModule.TerminalsString[19] = "DIR clock";
            //ControlModule.TerminalsString[20] = "SP clock";
            //ControlModule.TerminalsString[21] = "SP IncDec";
            //ControlModule.TerminalsString[22] = "SP sel";
            //ControlModule.TerminalsString[23] = "SP en";
            //ControlModule.TerminalsString[24] = "Reset";
            //ControlModule.TerminalsString[25] = "ULA bus";
            //ControlModule.TerminalsString[26] = "Buf clk";
            //ControlModule.TerminalsString[27] = "AC bus";
            //ControlModule.TerminalsString[28] = "AC clk";
            //ControlModule.TerminalsString[29] = "RG bus";
            //ControlModule.TerminalsString[30] = "RG/PC clk";
            //ControlModule.TerminalsString[31] = "RAM rd";
            //ControlModule.TerminalsString[32] = "RAM wr";
            //ControlModule.TerminalsString[33] = "RAM cs";
            //ControlModule.TerminalsString[34] = "In bus";
            //ControlModule.TerminalsString[35] = "Out clk";
            //ControlModule.TerminalsString[36] = "ULA OP sel 0";
            //ControlModule.TerminalsString[37] = "ULA OP sel 1";
            //ControlModule.TerminalsString[38] = "ULA OP sel 2";
            //ControlModule.TerminalsString[39] = "RG/PB sel 0";
            //ControlModule.TerminalsString[40] = "RG/PB sel 1";
            
            while (!UiStatics.WantExit) {
                try {
                    Thread.Sleep(20);
                    if (UiStatics.Simulador == null || InternalComponents.ControlModule == null || 
                        InsideComponent == null || InsideComponent.Draw.DisplayListHandle != Draws.Microcontroller.DisplayListHandle ||
                        UiStatics.Simulador.InternalSimulation == false ) {
                        controlModuleStatesPanel.Visible = false;
                        continue;
                    } else {
                        controlModuleStatesPanel.Visible = true;
                    }
                    bus.Text = GetBusHexa() + "\r\nBUS";
                    clock.Checked = InternalComponents.ControlModule.Clock.Value >= Pin.Halfcut;
                    flagC.Checked = InternalComponents.ControlModule.FlagInCarry.Value >= Pin.Halfcut;
                    flagZ.Checked = InternalComponents.ControlModule.FlagInZero.Value >= Pin.Halfcut;
                    EOI.Checked = InternalComponents.ControlModule.Eoi.Value >= Pin.Halfcut;
                    ROMrd.Checked = InternalComponents.ControlModule.RoMrd.Value >= Pin.Halfcut;
                    ROMcs.Checked = InternalComponents.ControlModule.RoMcs.Value >= Pin.Halfcut;
                    PCHbus.Checked = InternalComponents.ControlModule.PcHbus.Value >= Pin.Halfcut;
                    PCLbus.Checked = InternalComponents.ControlModule.PcLbus.Value >= Pin.Halfcut;
                    PCHclk.Checked = InternalComponents.ControlModule.PcHclock.Value >= Pin.Halfcut;
                    PCLclk.Checked = InternalComponents.ControlModule.PcLclock.Value >= Pin.Halfcut;
                    DataPCsel.Checked = InternalComponents.ControlModule.DataPCsel.Value >= Pin.Halfcut;
                    DIRclk.Checked = InternalComponents.ControlModule.DiRclock.Value >= Pin.Halfcut;
                    SPclk.Checked = InternalComponents.ControlModule.SPclock.Value >= Pin.Halfcut;
                    SPen.Checked = InternalComponents.ControlModule.SPen.Value >= Pin.Halfcut;
                    SPIncDec.Checked = InternalComponents.ControlModule.SpIncDec.Value >= Pin.Halfcut;
                    SPsel.Checked = InternalComponents.ControlModule.SPsel.Value >= Pin.Halfcut;
                    SPen.Checked = InternalComponents.ControlModule.SPen.Value >= Pin.Halfcut;
                    Reset.Checked = InternalComponents.ControlModule.Reset.Value >= Pin.Halfcut;
                    ULAbus.Checked = InternalComponents.ControlModule.UlAbus.Value >= Pin.Halfcut;
                    BUFclk.Checked = InternalComponents.ControlModule.BuFclock.Value >= Pin.Halfcut;
                    ACbus.Checked = InternalComponents.ControlModule.ACbus.Value >= Pin.Halfcut;
                    ACclk.Checked = InternalComponents.ControlModule.ACclock.Value >= Pin.Halfcut;
                    RGbus.Checked = InternalComponents.ControlModule.RGbus.Value >= Pin.Halfcut;
                    RGPCclk.Checked = InternalComponents.ControlModule.RgpCclock.Value >= Pin.Halfcut;
                    RAMrd.Checked = InternalComponents.ControlModule.RaMrd.Value >= Pin.Halfcut;
                    RAMwr.Checked = InternalComponents.ControlModule.RaMwr.Value >= Pin.Halfcut;
                    RAMcs.Checked = InternalComponents.ControlModule.RaMcs.Value >= Pin.Halfcut;
                    INbus.Checked = InternalComponents.ControlModule.Nbus.Value >= Pin.Halfcut;
                    OUTclk.Checked = InternalComponents.ControlModule.OuTclock.Value >= Pin.Halfcut;
                    ULAop0.Checked = InternalComponents.ControlModule.UlaoPsel0.Value >= Pin.Halfcut;
                    ULAop1.Checked = InternalComponents.ControlModule.UlaoPsel1.Value >= Pin.Halfcut;
                    ULAop2.Checked = InternalComponents.ControlModule.UlaoPsel2.Value >= Pin.Halfcut;
                    RGPB0.Checked = InternalComponents.ControlModule.RgpBsel0.Value >= Pin.Halfcut;
                    RGPB1.Checked = InternalComponents.ControlModule.RgpBsel1.Value >= Pin.Halfcut;
                } catch (Exception e) {
                    UiStatics.ShowExceptionMessage(e);
                }
            }
        }
        private string GetBusHexa() {
            var val = 0;
            val += InternalComponents.ControlModule.In0.Value >= Pin.Halfcut ? 1 : 0;
            val += InternalComponents.ControlModule.In1.Value >= Pin.Halfcut ? 2 : 0;
            val += InternalComponents.ControlModule.In2.Value >= Pin.Halfcut ? 4 : 0;
            val += InternalComponents.ControlModule.In3.Value >= Pin.Halfcut ? 8 : 0;
            val += InternalComponents.ControlModule.In4.Value >= Pin.Halfcut ? 16 : 0;
            val += InternalComponents.ControlModule.In5.Value >= Pin.Halfcut ? 32 : 0;
            val += InternalComponents.ControlModule.In6.Value >= Pin.Halfcut ? 64 : 0;
            val += InternalComponents.ControlModule.In7.Value >= Pin.Halfcut ? 128 : 0;
            return val.ToString("X2");
        }
        public void Stop() {
            _circuit = null;
            ResetColors();
        }
        private void RefreshExtraTerminals() {
            ExtraTerminals = new List<ExtraTerminal>();
            for (var i = 0; i < Wires.Count; i++) {
                if(Wires[i].FromComponent == null) {
                    RefreshExtraTerminals_internal(Wires[i].From, Wires[i].RootComponent);
                }
                if(Wires[i].ToComponent == null) {
                    RefreshExtraTerminals_internal(Wires[i].To, Wires[i].RootComponent);
                }
            }
        }
        private void RefreshExtraTerminals_internal(PointF point, Component root) {
            var count = 0;
            for (var i = 0; i < Wires.Count; i++) {
                if (Wires[i].RootComponent != root) continue;
                if(Wires[i].FromComponent == null) {
                    if (Wires[i].From == point)
                        ++count;
                }
                if (Wires[i].ToComponent == null) {
                    if (Wires[i].To == point)
                        ++count;
                }
                if (count > 2) {
                    ExtraTerminals.Add(new ExtraTerminal(point, root));
                    break;
                }
            }
            if (count != 2) {
                ExtraTerminals.Add(new ExtraTerminal(point, root));
            }

        }
        private void ResetColors() {
            foreach (var item in Wires) {
                item.Color = Draws.ColorOff;
            }
            foreach (var item in Components) {
                if(item.Draw.DisplayListHandle == Draws.And[1].DisplayListHandle) {
                    item.Draw = Draws.And[0];
                } else if (item.Draw.DisplayListHandle == Draws.Nand[1].DisplayListHandle) {
                    item.Draw = Draws.Nand[0];
                } else if (item.Draw.DisplayListHandle == Draws.Or[1].DisplayListHandle) {
                    item.Draw = Draws.Or[0];
                } else if (item.Draw.DisplayListHandle == Draws.Nor[1].DisplayListHandle) {
                    item.Draw = Draws.Nor[0];
                } else if (item.Draw.DisplayListHandle == Draws.Xor[1].DisplayListHandle) {
                    item.Draw = Draws.Xor[0];
                } else if (item.Draw.DisplayListHandle == Draws.Xnor[1].DisplayListHandle) {
                    item.Draw = Draws.Xnor[0];
                } else if (item.Draw.DisplayListHandle == Draws.Not[1].DisplayListHandle) {
                    item.Draw = Draws.Not[0];
                } else if (item.Draw.DisplayListHandle == Draws.Output[1].DisplayListHandle) {
                    item.Draw = Draws.Output[0];
                } else if (item.Draw.DisplayListHandle == Draws.Disable[1].DisplayListHandle || item.Draw.DisplayListHandle == Draws.Disable[2].DisplayListHandle) {
                    item.Draw = Draws.Disable[0];
                }
            }
        }

        private void ThreadRun() {
            while (_circuit != null && !UiStatics.WantExit) {
                List<CircuitSimulator.Component> copy;
                try {
                    copy = _circuit.Components;
                    var halfCut = Pin.High + Pin.Low / 2f;
                    foreach (var item in copy) {
                        if (item is CircuitSimulator.Components.Wire) {
                            var drawComponent = DrawWireToCircuitWire[(CircuitSimulator.Components.Wire)item];
                            if (item.Pins[0].IsOpen || item.SimulationId != _circuit.SimulationId) {
                                drawComponent.Color = Draws.Color_3Rd;
                            } else if (item.Pins[0].Value >= halfCut) {
                                drawComponent.Color = Draws.ColorOn;
                            } else {
                                drawComponent.Color = Draws.ColorOff;
                            }
                        } else {
                            var drawComponent = DrawComponentsToCircuitComponent[item];
                            if (item is AndGate) {
                                if (((AndGate)item).Output.Value >= halfCut) {
                                    drawComponent.Draw = Draws.And[1];
                                } else {
                                    drawComponent.Draw = Draws.And[0];
                                }
                            } else if (item is NandGate) {
                                if (((NandGate)item).Output.Value >= halfCut) {
                                    drawComponent.Draw = Draws.Nand[1];
                                } else {
                                    drawComponent.Draw = Draws.Nand[0];
                                }
                            } else if (item is OrGate) {
                                if (((OrGate)item).Output.Value >= halfCut) {
                                    drawComponent.Draw = Draws.Or[1];
                                } else {
                                    drawComponent.Draw = Draws.Or[0];
                                }
                            } else if (item is NorGate) {
                                if (((NorGate)item).Output.Value >= halfCut) {
                                    drawComponent.Draw = Draws.Nor[1];
                                } else {
                                    drawComponent.Draw = Draws.Nor[0];
                                }
                            } else if (item is XorGate) {
                                if (((XorGate)item).Output.Value >= halfCut) {
                                    drawComponent.Draw = Draws.Xor[1];
                                } else {
                                    drawComponent.Draw = Draws.Xor[0];
                                }
                            } else if (item is XnorGate) {
                                if (((XnorGate)item).Output.Value >= halfCut) {
                                    drawComponent.Draw = Draws.Xnor[1];
                                } else {
                                    drawComponent.Draw = Draws.Xnor[0];
                                }
                            } else if (item is NotGate) {
                                if (((NotGate)item).Output.Value >= halfCut) {
                                    drawComponent.Draw = Draws.Not[1];
                                } else {
                                    drawComponent.Draw = Draws.Not[0];
                                }
                            } else if (item is LogicInput) {
                                if (drawComponent.Draw.DisplayListHandle == Draws.Input[0].DisplayListHandle) {
                                    ((LogicInput)item).Value = Pin.Low;
                                } else if (drawComponent.Draw.DisplayListHandle == Draws.Input[1].DisplayListHandle) {
                                    ((LogicInput)item).Value = Pin.High;
                                } else {
                                    throw new Exception("Erro.");
                                }
                            } else if (item is LogicOutput) {
                                if (((LogicOutput)item).Pins[0].Value >= halfCut) {
                                    drawComponent.Draw = Draws.Output[1];
                                } else {
                                    drawComponent.Draw = Draws.Output[0];
                                }
                            } else if (item is CircuitSimulator.Components.Digital.Keyboard) {
                                ((CircuitSimulator.Components.Digital.Keyboard)item).Value = KeyDownByte;
                            } else if (item is Microcontroller) {
                                if (UiStatics.Simulador != null) {
                                    var microcontroller = (Microcontroller)item;
                                    for (var i = 0; i < 4; i++) {
                                        if(!UiStatics.Depurador.ativarEdicao.Checked)
                                            UiStatics.Simulador.In[i] = microcontroller.PinValuesToByteValue(i);
                                        microcontroller.SetOutput(UiStatics.Simulador.Out[i], i);
                                    }
                                }
                            } else if (item is Display7Seg) {
                                var display = ((Display7Seg)item);
                                for (var i = 0; i < 8; i++) {
                                    drawComponent.ActiveExtraHandlers[i] = display.Pins[i].Value >= halfCut;
                                }
                            } else if (item is ControlModule) {
                                var module = ((ControlModule)item);
                                ProcessControlModule(module);
                            } else {
                                //throw new NotImplementedException();
                            }
                        }
                    }
                } catch (InvalidOperationException) {
                } catch (Exception e) {
                    UiStatics.ShowExceptionMessage(e);
                    break;
                }
                try {
                    _circuit.Tick();
                    if (_controlModuleLastClock != InternalComponents.ControlModule.Clock.Value && InternalComponents.ControlModule.Clock.Value == Pin.High) {
                        UpdateInstructionLog();
                    }
                    _controlModuleLastClock = InternalComponents.ControlModule.Clock.Value;
                } catch (Exception) {
                }
                Thread.Sleep(16);
            }
        }
        private void UpdateInstructionLog() {
            var item = new InstructionLogItem(UiStatics.Simulador.Program[UiStatics.Simulador.NextInstruction]);
            
            item.Bus = GetBusHexa();
            item.Clock = InternalComponents.ControlModule.Clock.Value >= Pin.Halfcut;
            item.FlagC = InternalComponents.ControlModule.FlagInCarry.Value >= Pin.Halfcut;
            item.FlagZ = InternalComponents.ControlModule.FlagInZero.Value >= Pin.Halfcut;
            item.Eoi = InternalComponents.ControlModule.Eoi.Value >= Pin.Halfcut;
            item.RoMrd = InternalComponents.ControlModule.RoMrd.Value >= Pin.Halfcut;
            item.RoMcs = InternalComponents.ControlModule.RoMcs.Value >= Pin.Halfcut;
            item.PcHbus = InternalComponents.ControlModule.PcHbus.Value >= Pin.Halfcut;
            item.PcLbus = InternalComponents.ControlModule.PcLbus.Value >= Pin.Halfcut;
            item.PcHclk = InternalComponents.ControlModule.PcHclock.Value >= Pin.Halfcut;
            item.PcLclk = InternalComponents.ControlModule.PcLclock.Value >= Pin.Halfcut;
            item.DataPCsel = InternalComponents.ControlModule.DataPCsel.Value >= Pin.Halfcut;
            item.DiRclk = InternalComponents.ControlModule.DiRclock.Value >= Pin.Halfcut;
            item.SPclk = InternalComponents.ControlModule.SPclock.Value >= Pin.Halfcut;
            item.SPen = InternalComponents.ControlModule.SPen.Value >= Pin.Halfcut;
            item.SpIncDec = InternalComponents.ControlModule.SpIncDec.Value >= Pin.Halfcut;
            item.SPsel = InternalComponents.ControlModule.SPsel.Value >= Pin.Halfcut;
            item.SPen = InternalComponents.ControlModule.SPen.Value >= Pin.Halfcut;
            item.Reset = InternalComponents.ControlModule.Reset.Value >= Pin.Halfcut;
            item.UlAbus = InternalComponents.ControlModule.UlAbus.Value >= Pin.Halfcut;
            item.BuFclk = InternalComponents.ControlModule.BuFclock.Value >= Pin.Halfcut;
            item.ACbus = InternalComponents.ControlModule.ACbus.Value >= Pin.Halfcut;
            item.ACclk = InternalComponents.ControlModule.ACclock.Value >= Pin.Halfcut;
            item.RGbus = InternalComponents.ControlModule.RGbus.Value >= Pin.Halfcut;
            item.RgpCclk = InternalComponents.ControlModule.RgpCclock.Value >= Pin.Halfcut;
            item.RaMrd = InternalComponents.ControlModule.RaMrd.Value >= Pin.Halfcut;
            item.RaMwr = InternalComponents.ControlModule.RaMwr.Value >= Pin.Halfcut;
            item.RaMcs = InternalComponents.ControlModule.RaMcs.Value >= Pin.Halfcut;
            item.Nbus = InternalComponents.ControlModule.Nbus.Value >= Pin.Halfcut;
            item.OuTclk = InternalComponents.ControlModule.OuTclock.Value >= Pin.Halfcut;
            item.UlAop0 = InternalComponents.ControlModule.UlaoPsel0.Value >= Pin.Halfcut;
            item.UlAop1 = InternalComponents.ControlModule.UlaoPsel1.Value >= Pin.Halfcut;
            item.UlAop2 = InternalComponents.ControlModule.UlaoPsel2.Value >= Pin.Halfcut;
            item.Rgpb0 = InternalComponents.ControlModule.RgpBsel0.Value >= Pin.Halfcut;
            item.Rgpb1 = InternalComponents.ControlModule.RgpBsel1.Value >= Pin.Halfcut;
            
            if(InstructionLog.Items.Count == 0) {
                item.Primeira = true;
            } else {
                if(InstructionLog.Items[InstructionLog.Items.Count-1].Instruction != item.Instruction) {
                    item.Primeira = true;
                } else {
                    if(InstructionLog.Items[InstructionLog.Items.Count - 1].Eoi) {
                        item.Primeira = true;
                    }
                }
            }


            InstructionLog.Add(item);
        }
        private float _controlModuleLastClock = Pin.Low;
        private void ProcessControlModule(ControlModule module) {
            if (UiStatics.Simulador != null && UiStatics.Simulador.InternalSimulation) {
                InternalComponents.Microcontroller.PreventUpdateInput =
                    UiStatics.Depurador.ativarEdicao.Checked;
                if (module.LowFrequencyIteraction != UiStatics.Simulador.LowFrequencyIteraction) {

                    if (InternalComponents.Microcontroller.PortBank == null) {
                        InternalComponents.Microcontroller.PortBank = InternalComponents.PortBank;
                    }
                    if (module.Clock.Value < Pin.Halfcut) {
                        module.Clock.SetDigital(Pin.High);
                    } else
                        module.Clock.SetDigital(Pin.Low);
                    module.LowFrequencyIteraction = UiStatics.Simulador.LowFrequencyIteraction;
                }
                
                if (InternalComponents.ControlModule.NeedSet) {
                    GetValuesToSimulator();
                }
                if (InternalComponents.ControlModule.NeedSet) {
                    SetValuesFromSimulator();
                }
            }
        }

        private void MountCircuit() {
            try {
                _circuit = new Circuit();
                CircuitComponentToDrawComponents.Clear();
                CircuitWireToDrawWire.Clear();
                DrawComponentsToCircuitComponent.Clear();
                DrawWireToCircuitWire.Clear();
                foreach (var item in Components) {
                    if (item.Draw.DisplayListHandle == Draws.And[0].DisplayListHandle ||
                        item.Draw.DisplayListHandle == Draws.And[1].DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new AndGate());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Nand[0].DisplayListHandle ||
                                item.Draw.DisplayListHandle == Draws.Nand[1].DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new NandGate());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Or[0].DisplayListHandle ||
                                item.Draw.DisplayListHandle == Draws.Or[1].DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new OrGate());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Nor[0].DisplayListHandle ||
                                item.Draw.DisplayListHandle == Draws.Nor[1].DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new NorGate());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Xor[0].DisplayListHandle ||
                                item.Draw.DisplayListHandle == Draws.Xor[1].DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new XorGate());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Xnor[0].DisplayListHandle ||
                                item.Draw.DisplayListHandle == Draws.Xnor[1].DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new XnorGate());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Not[0].DisplayListHandle ||
                                item.Draw.DisplayListHandle == Draws.Not[1].DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new NotGate());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Input[0].DisplayListHandle ||
                                 item.Draw.DisplayListHandle == Draws.Input[1].DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new LogicInput());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Output[0].DisplayListHandle ||
                                 item.Draw.DisplayListHandle == Draws.Output[1].DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new LogicOutput());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Keyboard.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new CircuitSimulator.Components.Digital.Keyboard());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Microcontroller.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new Microcontroller());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Display7SegBase.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new Display7Seg());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.BinTo7Seg.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new BinTo7Seg());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.ControlModule.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new ControlModule());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.PortBank.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new PortBank());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Registrers.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new Registrers());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Disable8Bit.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new Disable8Bit());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.RamMemory.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new RamMemory());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Counter8Bit.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new Counter8Bit());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.RomAddresser.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new RomAddresser());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.RomMemory.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new RomMemory());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Ula.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new Ula());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Registrer8BitSg.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new Registrer8BitSg());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Registrer8BitCBuffer.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new Registrer8BitCBuffer());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.Clock.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new InternalClock());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.JkFlipFlop.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new FlipflopJk());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.DFlipFlop.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new FlipflopD());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.RsFlipFlop.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new FlipflopSr());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else if (item.Draw.DisplayListHandle == Draws.FlipFlop.DisplayListHandle) {
                        CircuitSimulator.Component component = _circuit.AddComponent(new FlipflopT());
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                    } else {
                        CircuitSimulator.Component component = _circuit.AddComponent(new Chip("Generc chip of "+item.Type, item.Draw.Terminals.Length));
                        CircuitComponentToDrawComponents.Add(item, component);
                        DrawComponentsToCircuitComponent.Add(component, item);
                        Console.WriteLine("Componente " + item.Type + " não programado. Adicionado chip genérico.");
                    }
                }
            
                foreach (var component in _circuit.Components) {
                    if (component is ControlModule)
                        InternalComponents.ControlModule = (ControlModule)component;
                    else if (component is RomMemory)
                        InternalComponents.RomMemory = (RomMemory)component;
                    else if (component is Ula)
                        InternalComponents.Ula = (Ula)component;
                    else if (component is Registrers)
                        InternalComponents.Registrers = (Registrers)component;
                    else if (component is Registrer8BitCBuffer)
                        InternalComponents.Accumulator = (Registrer8BitCBuffer)component;
                    else if (component is Counter8Bit)
                        InternalComponents.StackCounter = (Counter8Bit)component;
                    else if (component is PortBank)
                        InternalComponents.PortBank = (PortBank)component;
                    else if (component is RomAddresser)
                        InternalComponents.RomAddresser = (RomAddresser)component;
                    else if (component is Microcontroller)
                        InternalComponents.Microcontroller = (Microcontroller)component;
                    else if (component is RamMemory) {
                        if(component.Id == 39) {
                            InternalComponents.StackMemory = (RamMemory)component;
                        } else if(component.Id == 40) {
                            InternalComponents.RamMemory = (RamMemory)component;
                        }
                    }
                }

                foreach (var item in Wires) {
                    var wire = _circuit.AddComponent(new CircuitSimulator.Components.Wire());
                    CircuitWireToDrawWire.Add(item, wire);
                    DrawWireToCircuitWire.Add(wire, item);
                }


                foreach (var item in Wires) {
                    if(item.FromComponent != null && item.ToComponent != null) {//from and to is component
                        var @from = CircuitComponentToDrawComponents[item.FromComponent];
                        var to = CircuitComponentToDrawComponents[item.ToComponent];
                        var wire = CircuitWireToDrawWire[item];
                        wire.Pins[0].Connect(@from.Pins[item.FromIndex]);
                        wire.Pins[1].Connect(to.Pins[item.ToIndex]);
                    } else if (item.FromComponent != null && item.ToComponent == null) {// from is and to is not a component
                        var wire = CircuitWireToDrawWire[item];
                        var @from = CircuitComponentToDrawComponents[item.FromComponent];
                        var tempPins = new List<Pin>();
                        foreach (var item2 in Wires) {
                            if(item.To == item2.From) {
                                if (item != item2)
                                    tempPins.Add(CircuitWireToDrawWire[item2].Pins[0]);
                            } else if(item.To == item2.To) {
                                if (item != item2)
                                    tempPins.Add(CircuitWireToDrawWire[item2].Pins[1]);
                            }
                        }
                        wire.Pins[0].Connect(@from.Pins[item.FromIndex]);
                        foreach (var pin in tempPins) {
                            wire.Pins[1].Connect(pin);
                        }
                    } else if (item.FromComponent == null && item.ToComponent != null) {//from is not and to is a component
                        var wire = CircuitWireToDrawWire[item];
                        var to = CircuitComponentToDrawComponents[item.ToComponent];
                        var tempPins = new List<Pin>();
                        foreach (var item2 in Wires) {
                            if (item.From == item2.From) {
                                if (item != item2)
                                    tempPins.Add(CircuitWireToDrawWire[item2].Pins[0]);
                            } else if (item.From == item2.To) {
                                if (item != item2)
                                    tempPins.Add(CircuitWireToDrawWire[item2].Pins[1]);
                            }
                        }
                        wire.Pins[1].Connect(to.Pins[item.ToIndex]);
                        foreach (var pin in tempPins) {
                            wire.Pins[0].Connect(pin);
                        }
                    } else if (item.FromComponent == null && item.ToComponent == null) {//from and to is not a component
                        var wire = CircuitWireToDrawWire[item];
                        var tempPinsFrom = new List<Pin>();
                        var tempPinsTo = new List<Pin>();
                        foreach (var item2 in Wires) {
                            if (item.From == item2.From) {
                                if (item != item2)
                                    tempPinsFrom.Add(CircuitWireToDrawWire[item2].Pins[0]);
                            } else if (item.From == item2.To) {
                                if (item != item2)
                                    tempPinsFrom.Add(CircuitWireToDrawWire[item2].Pins[1]);
                            } else if (item.To == item2.From) {
                                if (item != item2)
                                    tempPinsTo.Add(CircuitWireToDrawWire[item2].Pins[0]);
                            } else if (item.To == item2.To) {
                                if (item != item2)
                                    tempPinsTo.Add(CircuitWireToDrawWire[item2].Pins[1]);
                            }
                        }
                        foreach (var pin in tempPinsFrom) {
                            wire.Pins[0].Connect(pin);
                        }
                        foreach (var pin in tempPinsTo) {
                            wire.Pins[1].Connect(pin);
                        }
                    }
                }
            } catch (Exception e) {
                UiStatics.ShowExceptionMessage(e);
            }
        }
        
        private void GetValuesToSimulator() {
            Array.Copy(InternalComponents.RamMemory.InternalValue,
                UiStatics.Simulador.Ram,
                UiStatics.Simulador.Ram.Length);
            Array.Copy(InternalComponents.StackMemory.InternalValue,
                UiStatics.Simulador.Stack,
                UiStatics.Simulador.Ram.Length);
            UiStatics.Simulador.Reg[0] =
                InternalComponents.Accumulator.InternalValue;
            UiStatics.Simulador.PointerStack = InternalComponents.StackCounter.InternalValue;
            for (var i = 0; i < 4; i++) {
                UiStatics.Simulador.Reg[i+1] =
                    InternalComponents.Registrers.Reg[i];
                UiStatics.Simulador.Out[i] =
                    InternalComponents.PortBank.RegOut[i];
                if (!UiStatics.Depurador.ativarEdicao.Checked) {
                    UiStatics.Simulador.In[i] =
                    InternalComponents.PortBank.GetInput(i);
                }
            }
            UiStatics.Simulador.FlagC =
                InternalComponents.Ula.Pins[29].Value >= Pin.Halfcut;
            UiStatics.Simulador.FlagZ =
                InternalComponents.Ula.Pins[28].Value >= Pin.Halfcut;
            var nextInstruction =
                InternalComponents.RomAddresser.RegH * 256 +
                InternalComponents.RomAddresser.RegL;
            if(UiStatics.Simulador.Program[nextInstruction] != null) {
                UiStatics.Simulador.NextInstruction = nextInstruction;
            } else {
                UiStatics.ShowExceptionMessage(new Exception("ERRO NO PC: "+nextInstruction));
        }
            
        }
        private void SetValuesFromSimulator() {
            Array.Copy(UiStatics.Simulador.CompiledProgram, 
                InternalComponents.RomMemory.InternalValue, 
                UiStatics.Simulador.CompiledProgram.Length);
            
            Array.Copy(UiStatics.Simulador.Ram,
                InternalComponents.RamMemory.InternalValue,
                UiStatics.Simulador.Ram.Length);
            Array.Copy(UiStatics.Simulador.Stack,
                InternalComponents.StackMemory.InternalValue,
                UiStatics.Simulador.Ram.Length);
            InternalComponents.Accumulator.InternalValue =
                UiStatics.Simulador.Reg[0];
            InternalComponents.StackCounter.InternalValue =
                UiStatics.Simulador.PointerStack;
            for (var i = 0; i < 4; i++) {
                InternalComponents.Registrers.Reg[i] =
                UiStatics.Simulador.Reg[i+1];
                if (UiStatics.Depurador.ativarEdicao.Checked) {
                    InternalComponents.PortBank.SetInput(i, UiStatics.Simulador.In[i]);
                }
            }
            InternalComponents.Ula.Pins[29].Value =
                UiStatics.Simulador.FlagC ? Pin.High : Pin.Low;
            InternalComponents.Ula.Pins[28].Value =
                UiStatics.Simulador.FlagZ ? Pin.High : Pin.Low;

            InternalComponents.RomAddresser.RegH =
                (byte)((UiStatics.Simulador.NextInstruction) / 256);
            InternalComponents.RomAddresser.RegL =
                (byte)((UiStatics.Simulador.NextInstruction) % 256);
            InternalComponents.ControlModule.NeedSet = false;
        }

        private float _zoom = 1;
        
        internal void ZoomMore() {
            if (_zoom < 1)
                _zoom *= 2;
            else
                _zoom += 1;
            Circuito_Resize(this, null);
            Refresh();
        }

        internal void ZoomLess() {
            if (_zoom <= 1)
                _zoom /= 2;
            else
                _zoom--;
            Circuito_Resize(this, null);
            Refresh();
        }

        internal void ZoomReset() {
            _zoom = 1;
            Circuito_Resize(this, null);
            Refresh();
        }

        public override void Refresh() {
            base.Refresh();

            RefreshExtraTerminals();
            if (MouseProps.Button2Pressed) {
                Position = new PointF(Position.X+((MouseProps.LastPosition.X - MouseProps.CurrentPosition.X)/_zoom), Position.Y-((MouseProps.LastPosition.Y - MouseProps.CurrentPosition.Y)/_zoom));
            }
            var foundComponent = false;
            var foundTerminal = false;

            var worldMousePos = MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, _zoom);
            foreach (var item in Components) {
                if (item.RootComponent != InsideComponent)
                    continue;
                var index = item.IsOnTerminal(worldMousePos);
                if (index >= 0) {
                    Terminals.HoverIndex = index;
                    Terminals.HoverComponent = item;
                    Over = null;
                    foundTerminal = true;
                    break;
                }
                if (item.IsInside(worldMousePos)){
                    Over = item;
                    foundComponent = true;
                    break;
                }
            }
            if (!foundComponent) Over = null;
            if (!foundTerminal) {
                Terminals.HoverComponent = null;
            }
            if(!foundTerminal && !foundComponent) {
                /*
                float distance = (Center.X + terminal.X - Point.X) * (Center.X + terminal.X - Point.X) + (Center.Y + terminal.Y - Point.Y) * (Center.Y + terminal.Y - Point.Y);
                if (distance <= 4 * 4) {
                    return i;
                }
                */
                Terminals.HoverNotComponent = false;
                foreach (var item in Wires) {
                    if (item.RootComponent != InsideComponent)
                        continue;
                    float distance;
                    if (item.FromComponent == null) {
                        distance = 
                            (item.From.X - MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, _zoom).X) * 
                            (item.From.X - MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, _zoom).X) +
                            (item.From.Y - MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, _zoom).Y) * 
                            (item.From.Y - MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, _zoom).Y);
                        if(distance <= 4 * 4) {
                            Terminals.Hover = item.From;
                            Terminals.HoverNotComponent = true;
                            foundTerminal = true;
                            break;
                        }
                    }
                    if(item.ToComponent == null) {
                        distance = 
                            (item.To.X - MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, _zoom).X) * 
                            (item.To.X - MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, _zoom).X) +
                            (item.To.Y - MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, _zoom).Y) * 
                            (item.To.Y - MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, _zoom).Y);
                        if (distance <= 4 * 4) {
                            Terminals.Hover = item.To;
                            Terminals.HoverNotComponent = true;
                            foundTerminal = true;
                            break;
                        }
                    }
                }
                if (!foundTerminal) {
                    Terminals.HoverNotComponent = false;
                }
            }

            if (MouseProps.Button1Pressed && _circuit == null) {
                if (Selected != null) {
                    if (_canDrag == false) {
                        //float distance = (Center.X + terminal.X - Point.X) * (Center.X + terminal.X - Point.X) + (Center.Y + terminal.Y - Point.Y) * (Center.Y + terminal.Y - Point.Y);
                        float distance =
                            (MouseProps.LastDownPosition.X - MouseProps.CurrentPosition.X) * (MouseProps.LastDownPosition.X - MouseProps.CurrentPosition.X) +
                            (MouseProps.LastDownPosition.Y - MouseProps.CurrentPosition.Y) * (MouseProps.LastDownPosition.Y - MouseProps.CurrentPosition.Y);
                        if (distance > 25*25) {
                            _canDrag = true;
                        }
                    }
                    if (_canDrag) {
                        Selected.Center = MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, _zoom);
                        Selected.Center.X = (float)Math.Round(Selected.Center.X / 5) * 5;
                        Selected.Center.Y = (float)Math.Round(Selected.Center.Y / 5) * 5;
                    }
                }
                if(Terminals.FromComponent != null) {
                    _terminalFromVertex = Terminals.FromComponent.TransformTerminal(Terminals.FromIndex);
                    _terminalFromVertex.X += Terminals.FromComponent.Center.X;
                    _terminalFromVertex.Y += Terminals.FromComponent.Center.Y;
                    _terminalToVertex = MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, _zoom);
                    _terminalToVertex.X = (float)Math.Round(_terminalToVertex.X / 5) * 5;
                    _terminalToVertex.Y = (float)Math.Round(_terminalToVertex.Y / 5) * 5;
                } else if (Terminals.FromNotComponent) {
                    _terminalFromVertex = Terminals.From;
                    _terminalToVertex = MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, _zoom);
                    _terminalToVertex.X = (float)Math.Round(_terminalToVertex.X / 5) * 5;
                    _terminalToVertex.Y = (float)Math.Round(_terminalToVertex.Y / 5) * 5;
                }
            } else {
                _canDrag = false;
            }
            var foundWire = false;
            
            foreach (var item in Wires) {
                if (item.RootComponent != InsideComponent)
                    continue;
                if (item.FromComponent != null) {
                    item.From = item.FromComponent.TransformTerminal(item.FromIndex);
                    item.From.X += item.FromComponent.Center.X;
                    item.From.Y += item.FromComponent.Center.Y;
                }
                if (item.ToComponent != null) {
                    item.To = item.ToComponent.TransformTerminal(item.ToIndex);
                    item.To.X += item.ToComponent.Center.X;
                    item.To.Y += item.ToComponent.Center.Y;
                }
                
                if (!MouseProps.Button1Pressed && !MouseProps.Button2Pressed) {
                    if (OnWire(item.From, item.To, worldMousePos)) {
                        HoverWire = item;
                        foundWire = true;
                        break;
                    }
                }
            }
            if (!foundWire) HoverWire = null;


        }
        private PointF _terminalFromVertex;
        private PointF _terminalToVertex;
        private void Render() {
            var lookat = Matrix4.LookAt(Position.X, Position.Y, 1, Position.X, Position.Y, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.Texture2D);
            if (_zoom > 1f) {
                GL.LineWidth(_zoom);
            } else {
                GL.LineWidth(1);
            }
            if (ExtraTerminals != null) {
                GL.Color3(Color.Black);
                foreach (var item in ExtraTerminals) {
                    if (item.Root != InsideComponent) continue;
                    GL.Translate(item.Point.X, item.Point.Y, 0);
                    GL.CallList(Draws.TerminalHandle);
                    GL.Translate(-item.Point.X, -item.Point.Y, 0);
                }
            }
            foreach (var item in Components) {
                if (item.RootComponent != InsideComponent)
                    continue;
                GL.Translate(item.Center.X, item.Center.Y, 0);
                GL.Rotate(item.Rotation, 0, 0, 1);
                GL.CallList(item.Draw.DisplayListHandle);
                if (item.ActiveExtraHandlers != null) {
                    for (var i = 0; i < item.ActiveExtraHandlers.Length; i++) {
                        if(item.ActiveExtraHandlers[i]) GL.CallList(item.ExtraHandlers[i]);
                    }
                }
                for (var i = 0; i < item.Draw.Terminals.Length; i++) {
                    GL.Translate(item.Draw.Terminals[i].X, item.Draw.Terminals[i].Y, 0);
                    if (UiStatics.Simulador != null && (UiStatics.Simulador.Running || !UiStatics.Simulador.Stopped) && CircuitComponentToDrawComponents.ContainsKey(item)) {
                        if(CircuitComponentToDrawComponents[item].Pins[i].SimulationId != _circuit.SimulationId) {
                            GL.Color3(Color.Gray);
                        } else if(CircuitComponentToDrawComponents[item].Pins[i].Value >= Pin.Halfcut) {
                            GL.Color3(Color.Red);
                        } else {
                            GL.Color3(Color.Black);
                        }
                    } else {
                        GL.Color3(Color.Black);
                    }
                    GL.CallList(Draws.TerminalHandle);
                    GL.Translate(-item.Draw.Terminals[i].X, -item.Draw.Terminals[i].Y, 0);
                }
                GL.Rotate(-item.Rotation, 0, 0, 1);
                GL.Translate(-item.Center.X, -item.Center.Y, 0);
            }

            if (MouseProps.Button1Pressed && _circuit == null) {
                if(Terminals.FromComponent != null) {
                    GL.Color3(Color.Black);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex2(_terminalFromVertex.X, _terminalFromVertex.Y);
                    GL.Vertex2(_terminalToVertex.X, _terminalToVertex.Y);
                    GL.End();
                } else if (Terminals.FromNotComponent) {
                    GL.Color3(Color.Black);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex2(_terminalFromVertex.X, _terminalFromVertex.Y);
                    GL.Vertex2(_terminalToVertex.X, _terminalToVertex.Y);
                    GL.End();
                }
            }
            DrawWires();
            DrawBoxes();

            SwapBuffers();
        }
        private void Circuito_Load(object sender, EventArgs e) {
            ClearColor = BackColor;
            GL.ClearColor(ClearColor);
            
            Draws.Load();
            FileProject.Load(Application.StartupPath+"\\Default.m3mprj");

            Loaded = true;

            _threadDraw = new Thread(ThreadDraw);
            _threadDraw.Start();

            _threadStatesPanel = new Thread(ThreadStatesUpdate);
            _threadStatesPanel.Start();
        }
        void ThreadDraw() {
            while (!UiStatics.WantExit) {
                Invalidate();
                Thread.Sleep(10);
            }
        }
        
        private void Circuito_Resize(object sender, EventArgs e) {
            var c = sender as GLControl;

            if (c.ClientSize.Height == 0)
                c.ClientSize = new Size(c.ClientSize.Width, 1);

            GL.Viewport(0, 0, c.ClientSize.Width, c.ClientSize.Height);
            var orthographic = Matrix4.CreateOrthographic(c.ClientSize.Width/_zoom, c.ClientSize.Height/_zoom, 1, 10);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref orthographic);
        }

        private void Circuito_Paint(object sender, PaintEventArgs e) {
            if (!_syncronizer.IsRunning) {
                Render();
                _syncronizer.Start();
            } else {
                if(_syncronizer.ElapsedMilliseconds >= 10) {
                    Render();
                    _syncronizer.Reset();
                    _syncronizer.Start();
                }
            }
        }

        private void Circuito_KeyUp(object sender, KeyEventArgs e) {
            if(KeyDownByte == (byte)e.KeyValue)
                KeyDownByte = 0;
        }

        private void Circuito_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e) {
            Changed = true;
            MouseProps.LastDoubleClickPosition = e.Location;
            if(Over != null) {
                if(Over.Draw.DisplayListHandle == Draws.Input[0].DisplayListHandle) {
                    Over.Draw = Draws.Input[1];
                } else if (Over.Draw.DisplayListHandle == Draws.Input[1].DisplayListHandle) {
                    Over.Draw = Draws.Input[0];
                } else if (Over.Draw.DisplayListHandle == Draws.Microcontroller.DisplayListHandle) {
                    var mic = Over;
                    Selected = null;
                    SelectedWire = null;
                    Over = null;
                    InsideComponent = mic;
                }
            } else {
                Selected = null;
                SelectedWire = null;
                Over = null;
                InsideComponent = null;
            }
            Refresh();
        }

        private void Circuito_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e) {
            Changed = true;
            if (e.Button == MouseButtons.Left) {
                MouseProps.LastClickPosition = e.Location;
            } else if(e.Button == MouseButtons.Right) {
                if(((MouseProps.LastDownPosition.X-e.X)* (MouseProps.LastDownPosition.X - e.X))+((MouseProps.LastDownPosition.Y - e.Y) * (MouseProps.LastDownPosition.Y - e.Y)) < 10 * 10) {
                    if (InsideComponent != null && InsideComponent.Draw.DisplayListHandle != Draws.Microcontroller.DisplayListHandle ||
                        InsideComponent == null) {
                        PositionCreatingComponent = MouseProps.ToWorld(e.Location, ClientSize, Position, _zoom);
                        contextMenuStrip1.Show(this, e.Location);
                    }
                } else {
                    contextMenuStrip1.Hide();
                }
            }
            Refresh();
        }

        private void Circuito_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) {
                MouseProps.LastDownPosition = e.Location;
                if (e.Button == MouseButtons.Left) {
                    if (!_isDebugMode) {
                        if (InsideComponent != null) {
                            if (InsideComponent.Type == ComponentType.Microcontroller) return;
                        }
                    }
                    MouseProps.Button1Pressed = true;
                    Selected = null;
                    if(Over != null)
                        Selected = Over;
                    if (Terminals.HoverComponent != null) {
                        Terminals.FromComponent = Terminals.HoverComponent;
                        Terminals.FromIndex = Terminals.HoverIndex;
                    } else if (Terminals.HoverNotComponent) {
                        Terminals.FromNotComponent = true;
                        Terminals.From = Terminals.Hover;
                    } else {
                        Terminals.FromComponent = null;
                        Terminals.ToComponent = null;
                        Terminals.FromNotComponent = false;
                        Terminals.ToNotComponent = false;
                    }
                    if(HoverWire != null) {
                        SelectedWire = HoverWire;
                    } else {
                        SelectedWire = null;
                    }
                } else {
                    MouseProps.Button2Pressed = true;
                }
            }
            Refresh();
        }

        private void Circuito_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
            MouseProps.LastPosition = MouseProps.CurrentPosition;
            MouseProps.CurrentPosition = e.Location;
            Refresh();
        }

        private void Circuito_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
            Changed = true;
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) {
                MouseProps.LastUpPosition = e.Location;
                if (e.Button == MouseButtons.Left) {
                    MouseProps.Button1Pressed = false;
                } else {
                    MouseProps.Button2Pressed = false;
                }
            }
            if (_circuit == null) {
                if (Terminals.FromComponent != null) {
                    if (Terminals.HoverComponent != null) {
                        if (Terminals.HoverComponent != Terminals.FromComponent || Terminals.HoverIndex != Terminals.FromIndex) {
                            Wires.Add(new Wire(Terminals.FromComponent, Terminals.FromIndex, Terminals.HoverComponent, Terminals.HoverIndex));
                        }
                    } else {
                        var point = MouseProps.ToWorld(MouseProps.LastUpPosition, ClientSize, Position, _zoom);
                        point.X = (float)Math.Round(point.X / 5) * 5;
                        point.Y = (float)Math.Round(point.Y / 5) * 5;
                        Wires.Add(new Wire(Terminals.FromComponent, Terminals.FromIndex, point));
                    }
                } else if (Terminals.FromNotComponent) {
                    if (Terminals.HoverComponent != null) {
                        Wires.Add(new Wire(Terminals.From, Terminals.HoverComponent, Terminals.HoverIndex));
                    } else {
                        var point = MouseProps.ToWorld(MouseProps.LastUpPosition, ClientSize, Position, _zoom);
                        point.X = (float)Math.Round(point.X / 5) * 5;
                        point.Y = (float)Math.Round(point.Y / 5) * 5;
                        if ((Terminals.From.X - point.X) * (Terminals.From.X - point.X) + (Terminals.From.Y - point.Y) * (Terminals.From.Y - point.Y) >= 10 * 10) {
                            Wires.Add(new Wire(Terminals.From, point));
                        }
                    }
                }
            }
            Terminals.FromComponent = null;
            Terminals.ToComponent = null;
            Terminals.FromNotComponent = false;
            Terminals.ToNotComponent = false;
            Refresh();
        }

        private void Circuito_MouseLeave(object sender, EventArgs e) {
            /*
            MouseProps.Button1Pressed = false;
            MouseProps.Button2Pressed = false;
            */
            Refresh();
        }

        private void DrawWires() {
            foreach (var item in Wires) {
                if (item.RootComponent != InsideComponent)
                    continue;
                if (item == HoverWire) {
                    GL.Color3(Color.Green);
                } else if (item == SelectedWire) {
                    GL.Color3(Color.Orange);
                } else {
                    GL.Color3(item.Color);
                }
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(item.From.X, item.From.Y);
                GL.Vertex2(item.To.X, item.To.Y);
                GL.End();
            }
        }
        private const int BoxPlusFactor = 5;
        private void DrawBoxes() {
            if (Terminals.HoverComponent != null) {
                GL.Color3(Color.Green);
                var pos = new Vector3(Terminals.HoverComponent.Center.X, Terminals.HoverComponent.Center.Y, 0);
                var transform = Terminals.HoverComponent.TransformTerminal(Terminals.HoverIndex);
                pos.X += transform.X;
                pos.Y += transform.Y;
                GL.Translate(pos);
                GL.CallList(Draws.TerminalHandle);
                var tmp = Terminals.HoverComponent.Draw.TerminalsString[Terminals.HoverIndex];
                if (tmp != null && tmp != "") {
                    GL.Translate(0, 20, 0);
                    TextRenderer.DrawText(tmp, Color.Red, new PointF(0, 0));
                    GL.Translate(0, -20, 0);
                }
                
                GL.Translate(-pos);
            } else if (Terminals.HoverNotComponent) {
                GL.Color3(Color.Green);
                var pos = new Vector3(Terminals.Hover.X, Terminals.Hover.Y, 0);
                GL.Translate(pos);
                GL.CallList(Draws.TerminalHandle);
                GL.Translate(-pos);
            }
            if (Selected != null) { 
                GL.Color3(Color.Orange);
                GL.LineWidth(1);
                GL.LineStipple(1, 0xAAAA); // dashed line
                GL.Translate(Selected.Center.X, Selected.Center.Y, 0);
                GL.Rotate(Selected.Rotation, 0, 0, 1);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex2(- Selected.Draw.Width / 2f - BoxPlusFactor, - Selected.Draw.Height / 2f - BoxPlusFactor);
                GL.Vertex2(- Selected.Draw.Width / 2f - BoxPlusFactor, Selected.Draw.Height / 2f + BoxPlusFactor);
                GL.Vertex2(Selected.Draw.Width / 2f + BoxPlusFactor, Selected.Draw.Height / 2f + BoxPlusFactor);
                GL.Vertex2(Selected.Draw.Width / 2f + BoxPlusFactor, - Selected.Draw.Height / 2f - BoxPlusFactor);
                GL.End();
                GL.Rotate(-Selected.Rotation, 0, 0, 1);
                GL.Translate(-Selected.Center.X, -Selected.Center.Y, 0);
            }
            if(Over != null) {
                GL.Color3(Color.Green);
                GL.LineWidth(1);
                GL.LineStipple(1, 0xAAAA); // dashed line
                GL.Translate(Over.Center.X, Over.Center.Y, 0);
                GL.Rotate(Over.Rotation, 0, 0, 1);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex2(- Over.Draw.Width/2f - BoxPlusFactor, - Over.Draw.Height/ 2f - BoxPlusFactor);
                GL.Vertex2(- Over.Draw.Width / 2f - BoxPlusFactor, + Over.Draw.Height / 2f + BoxPlusFactor);
                GL.Vertex2(Over.Draw.Width / 2f + BoxPlusFactor, Over.Draw.Height / 2f + BoxPlusFactor);
                GL.Vertex2(Over.Draw.Width / 2f + BoxPlusFactor, - Over.Draw.Height / 2f - BoxPlusFactor);
                GL.End();
                GL.Rotate(-Over.Rotation, 0, 0, 1);
                GL.Translate(-Over.Center.X, -Over.Center.Y, 0);
            }
            
        }
        /*
        private void Circuito_KeyUp(object sender, System.Windows.Forms.KeyPressEventArgs e) {
            
        }
        */
        private static bool OnWire(PointF point1, PointF point2, PointF point) {
            //pegando os valores máximos
            var maxX = point1.X > point2.X ? point1.X : point2.X;
            var minX = point1.X > point2.X ? point2.X : point1.X;
            var maxY = point1.Y > point2.Y ? point1.Y : point2.Y;
            var minY = point1.Y > point2.Y ? point2.Y : point1.Y;
            float x, y, coefAng, coefLin, coefAngPar, coefLinPar;

            //Equacao linear = ax - by + c = 0

            if (maxX - minX > maxY - minY) { //funcao linear pelo eixo x
                //Calculando a equação reta
                coefAng = (point2.Y - point1.Y) / (point2.X - point1.X);
                coefLin = -point1.X * coefAng + point1.Y;
                //calculando a reta perpendicar passando no ponto
                coefAngPar = -1 / coefAng * coefLin;
                coefLinPar = point.Y - coefAngPar * point.X;
                //calculando o ponto de interceção das duas retas
                x = (coefLin - coefLinPar) / (coefAngPar - coefAng);
                y = x * coefAng + coefLin;

                //verifica se o ponto Y passa dos limites da linha
                if (y < minY) {
                    y = minY;
                    x = y / coefAng - coefLin;
                } else if (y > maxY) {
                    y = maxY;
                    x = y / coefAng - coefLin;
                }
                
                //verifica se o ponto X passa dos limites da linha
                if (x < minX) {
                    x = minX;
                    y = x * coefAng + coefLin;
                } else if (x > maxX) {
                    x = maxX;
                    y = x * coefAng + coefLin;
                }
            } else { //funcao linear pelo eixo Y
                //Calculando a equação reta
                coefAng = (point2.X - point1.X) / (point2.Y - point1.Y);
                coefLin = -point1.Y * coefAng + point1.X;
                //calculando a reta perpendicar passando no ponto
                coefAngPar = -1 / coefAng * coefLin;
                coefLinPar = point.X - coefAngPar * point.Y;
                //calculando o ponto de interceção das duas retas
                y = (coefLin - coefLinPar) / (coefAngPar - coefAng);
                x = y * coefAng + coefLin;

                //verifica se o ponto Y passa dos limites da linha
                if (x < minX) {
                    x = minX;
                    y = x / coefAng - coefLin;
                } else if (x > maxX) {
                    x = maxX;
                    y = x / coefAng - coefLin;
                }

                //verifica se o ponto X passa dos limites da linha
                if (y < minY) {
                    y = minY;
                    x = y * coefAng + coefLin;
                } else if (y > maxY) {
                    x = maxY;
                    x = y * coefAng + coefLin;
                }
            }

            //Verifica se é uma reta totalmente paralela ao eixo y ou ao eixo X
            
            if (point1.Y == point2.Y) {
                y = point1.Y;
                x = point.X > maxX ? maxX : (point.X < minX ? minX : point.X);
            } else if(point1.X == point2.X) {
                x = point1.X;
                y = point.Y > maxY ? maxY : (point.Y < minY ? minY : point.Y);
            }

            var maiorX = x > point.X ? x : point.X;
            var menorX = x > point.X ? point.X : x;
            var maiorY = y > point.Y ? y : point.Y;
            var menorY = y > point.Y ? point.Y : y;
            
            //calcula a distancia
            if ((maiorX - menorX) * (maiorX - menorX) + (maiorY - menorY) * (maiorY - menorY) < 5 * 5) {
                return true;
            }
            return false;
        }

        private void Circuito_KeyDown(object sender, KeyEventArgs e) {

            {   //KONAMI CODE
                _konamiCode.Enqueue(e.KeyCode);
                if (_konamiCode.Count > 10) _konamiCode.Dequeue();
                var arr = _konamiCode.ToArray();
                if (arr.Length == _konamiCodeCorrect.Length) {
                    var isCorrect = true;
                    for (var i = 0; i < arr.Length; i++) {
                        if (arr[i] != _konamiCodeCorrect[i]) {
                            isCorrect = false;
                            break;
                        }
                    }
                    if (isCorrect) {
                        _isDebugMode = true;
                        MessageBox.Show("Você entrou no modo debug.", "Modo debug", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
            Changed = true;
            KeyDownByte = (byte) e.KeyValue;
            if (_circuit == null) {
                if (!_isDebugMode) {
                    if (Selected != null) {
                        if (Selected.RootComponent != null) {
                            if (Selected.RootComponent.Type == ComponentType.Microcontroller) return;
                        }
                    } else if (SelectedWire != null) {
                        if (SelectedWire.RootComponent != null) {
                            if (SelectedWire.RootComponent.Type == ComponentType.Microcontroller) return;
                        }
                    }
                }
                if (e.KeyCode == Keys.R) {
                    if (Selected != null) {
                        Selected.Rotation += 90;
                        if (Selected.Rotation >= 360) Selected.Rotation -= 360;
                    }
                } else if (e.KeyCode == Keys.Delete) {
                    if (Selected != null) {
                        if (Selected.Draw.DisplayListHandle == Draws.Microcontroller.DisplayListHandle) return;
                        SelectedWire = null;
                        HoverWire = null;
                        var length = Wires.Count;
                        for (var i = 0; i < length; i++) {
                            if (Wires[i].FromComponent == Selected || Wires[i].ToComponent == Selected) {
                                Wires.Remove(Wires[i]);
                                --i;
                                --length;
                            }
                        }
                        Components.Remove(Selected);
                        Selected = null;
                        Over = null;
                    } else if (SelectedWire != null) {
                        Wires.Remove(SelectedWire);
                        SelectedWire = null;
                        HoverWire = null;
                    }
                } else if (e.KeyCode == Keys.Enter) { //DebugMode
                } else if(e.KeyCode == Keys.C && _isDebugMode) {
                    var r = Microsoft.VisualBasic.Interaction.InputBox("Nome do componente a ser criado:", "Criar componente [DEBUG]", "");
                    if (r != null && r != "") {
                        switch (r) {

                            case "Input":
                                Components.Add(new Component(Draws.Input[0], new PointF(0, 0), InsideComponent));
                                break;
                            case "Output":
                                Components.Add(new Component(Draws.Output[0], new PointF(0, 0), InsideComponent));
                                break;
                            case "Disable":
                                Components.Add(new Component(Draws.Disable[0], new PointF(0, 0), InsideComponent));
                                break;
                            case "Not":
                                Components.Add(new Component(Draws.Not[0], new PointF(0, 0), InsideComponent));
                                break;
                            case "And":
                                Components.Add(new Component(Draws.And[0], new PointF(0, 0), InsideComponent));
                                break;
                            case "Nand":
                                Components.Add(new Component(Draws.Nand[0], new PointF(0, 0), InsideComponent));
                                break;
                            case "Or":
                                Components.Add(new Component(Draws.Or[0], new PointF(0, 0), InsideComponent));
                                break;
                            case "Nor":
                                Components.Add(new Component(Draws.Nor[0], new PointF(0, 0), InsideComponent));
                                break;
                            case "Xnor":
                                Components.Add(new Component(Draws.Xnor[0], new PointF(0, 0), InsideComponent));
                                break;
                            case "Keyboard":
                                Components.Add(new Component(Draws.Keyboard, new PointF(0, 0), InsideComponent));
                                break;
                            case "Display7Seg":
                                Components.Add(new Component(Draws.Display7SegBase, new PointF(0, 0), InsideComponent));
                                break;
                            case "BinTo7Seg":
                                Components.Add(new Component(Draws.BinTo7Seg, new PointF(0, 0), InsideComponent));
                                break;
                            case "Microcontroller":
                                Components.Add(new Component(Draws.Microcontroller, new PointF(0, 0), InsideComponent));
                                break;
                            case "Osciloscope":
                                Components.Add(new Component(Draws.Osciloscope, new PointF(0, 0), InsideComponent));
                                break;
                            case "JKFlipFlop":
                                Components.Add(new Component(Draws.JkFlipFlop, new PointF(0, 0), InsideComponent));
                                break;
                            case "RSFlipFlop":
                                Components.Add(new Component(Draws.RsFlipFlop, new PointF(0, 0), InsideComponent));
                                break;
                            case "DFlipFlop":
                                Components.Add(new Component(Draws.DFlipFlop, new PointF(0, 0), InsideComponent));
                                break;
                            case "TFlipFlop":
                                Components.Add(new Component(Draws.FlipFlop, new PointF(0, 0), InsideComponent));
                                break;
                            case "BinTo7SHalfAddereg":
                                Components.Add(new Component(Draws.HalfAdder, new PointF(0, 0), InsideComponent));
                                break;
                            case "FullAdder":
                                Components.Add(new Component(Draws.FullAdder, new PointF(0, 0), InsideComponent));
                                break;
                            case "ULA":
                                Components.Add(new Component(Draws.Ula, new PointF(0, 0), InsideComponent));
                                break;
                            case "ControlModule":
                                Components.Add(new Component(Draws.ControlModule, new PointF(0, 0), InsideComponent));
                                break;
                            case "Registrer8Bit":
                                Components.Add(new Component(Draws.Registrer8Bit, new PointF(0, 0), InsideComponent));
                                break;
                            case "Registrers":
                                Components.Add(new Component(Draws.Registrers, new PointF(0, 0), InsideComponent));
                                break;
                            case "RamMemory":
                                Components.Add(new Component(Draws.RamMemory, new PointF(0, 0), InsideComponent));
                                break;
                            case "RomMemory":
                                Components.Add(new Component(Draws.RomMemory, new PointF(0, 0), InsideComponent));
                                break;
                            case "PortBank":
                                Components.Add(new Component(Draws.PortBank, new PointF(0, 0), InsideComponent));
                                break;
                            case "Registrer8BitSG":
                                Components.Add(new Component(Draws.Registrer8BitSg, new PointF(0, 0), InsideComponent));
                                break;
                            case "Registrer8BitCBuffer":
                                Components.Add(new Component(Draws.Registrer8BitCBuffer, new PointF(0, 0), InsideComponent));
                                break;
                            case "RomAddresser":
                                Components.Add(new Component(Draws.RomAddresser, new PointF(0, 0), InsideComponent));
                                break;
                            case "Counter8Bit":
                                Components.Add(new Component(Draws.Counter8Bit, new PointF(0, 0), InsideComponent));
                                break;
                            case "LedMatrix":
                                Components.Add(new Component(Draws.LedMatrix, new PointF(0, 0), InsideComponent));
                                break;
                            case "Disable8Bit":
                                Components.Add(new Component(Draws.Disable8Bit, new PointF(0, 0), InsideComponent));
                                break;
                            case "Clock":
                                Components.Add(new Component(Draws.Clock, new PointF(0, 0), InsideComponent));
                                break;
                            default:
                                MessageBox.Show("Componente "+ r +" inexistente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }
                }
            }
            Refresh();
        }

        private void aNDToolStripMenuItem_Click(object sender, EventArgs e) {
            Components.Add(new Component(Draws.And[0], PositionCreatingComponent));
        }

        private void nANDToolStripMenuItem_Click(object sender, EventArgs e) {
            Components.Add(new Component(Draws.Nand[0], PositionCreatingComponent));
        }

        private void oRToolStripMenuItem_Click(object sender, EventArgs e) {
            Components.Add(new Component(Draws.Or[0], PositionCreatingComponent));
        }

        private void nORToolStripMenuItem_Click(object sender, EventArgs e) {
            Components.Add(new Component(Draws.Nor[0], PositionCreatingComponent));
        }

        private void xORToolStripMenuItem_Click(object sender, EventArgs e) {
            Components.Add(new Component(Draws.Xor[0], PositionCreatingComponent));
        }

        private void xNORToolStripMenuItem_Click(object sender, EventArgs e) {
            Components.Add(new Component(Draws.Xnor[0], PositionCreatingComponent));
        }

        private void nOTToolStripMenuItem_Click(object sender, EventArgs e) {
            Components.Add(new Component(Draws.Not[0], PositionCreatingComponent));
        }

        private void decodificador7SegmentosToolStripMenuItem_Click(object sender, EventArgs e) {
            Components.Add(new Component(Draws.BinTo7Seg, PositionCreatingComponent));
        }

        private void entradaLógicaToolStripMenuItem_Click(object sender, EventArgs e) {
            Components.Add(new Component(Draws.Input[0], PositionCreatingComponent));
        }

        private void saídaLógicaToolStripMenuItem_Click(object sender, EventArgs e) {
            Components.Add(new Component(Draws.Output[0], PositionCreatingComponent));
        }

        private void tecladoToolStripMenuItem_Click(object sender, EventArgs e) {
            Components.Add(new Component(Draws.Keyboard, PositionCreatingComponent));
        }

        private void displayDe7SegmentosToolStripMenuItem_Click(object sender, EventArgs e) {
            Components.Add(new Component(Draws.Display7SegBase, PositionCreatingComponent));
        }

        private void flipflopJKToolStripMenuItem_Click(object sender, EventArgs e) {
            Components.Add(new Component(Draws.JkFlipFlop, PositionCreatingComponent));
        }

        private void flipflopRSToolStripMenuItem_Click(object sender, EventArgs e) {
            Components.Add(new Component(Draws.RsFlipFlop, PositionCreatingComponent));
        }
        
    }
}

    