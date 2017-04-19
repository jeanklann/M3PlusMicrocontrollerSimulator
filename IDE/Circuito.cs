using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using CircuitSimulator;
using System.Threading;
using CircuitSimulator.Components.Digital;
using OpenTK.Graphics;

namespace IDE {
    public partial class Circuito : GLControl {
        public MouseProps MouseProps = new MouseProps();
        public Terminals Terminals = new Terminals();
        public List<Wire> Wires = new List<Wire>();
        public byte KeyDown_byte = 0;
        public Wire HoverWire;
        public Wire SelectedWire;
        public Component Over;
        public Component Selected;
        private bool canDrag = false;
        private Circuit Circuit;
        private Thread thread;
        public PointF Position = new PointF();
        public PointF PositionCreatingComponent = new PointF();
        public Component InsideComponent;
        public List<Component> Components = new List<Component>();
        public Dictionary<Component, CircuitSimulator.Component> CircuitComponentToDrawComponents = new Dictionary<Component, CircuitSimulator.Component>();
        public Dictionary<Wire, CircuitSimulator.Components.Wire> CircuitWireToDrawWire = new Dictionary<Wire, CircuitSimulator.Components.Wire>();
        public Dictionary<CircuitSimulator.Component, Component> DrawComponentsToCircuitComponent = new Dictionary<CircuitSimulator.Component, Component>();
        public Dictionary<CircuitSimulator.Components.Wire, Wire> DrawWireToCircuitWire = new Dictionary<CircuitSimulator.Components.Wire, Wire>();
        public Color ClearColor;
        public List<ExtraTerminal> ExtraTerminals;
        public Circuito() {
            InitializeComponent();
        }

        public void Run() {
            if(Circuit == null) {
                MountCircuit();
                thread = new Thread(ThreadRun);
                thread.Start();
            }
        }
        public void Tick() {
            if (Circuit == null) {
                MountCircuit();
                thread = new Thread(ThreadRun);
                thread.Start();
            }
            Circuit.Ticks(5);
        }
        public void Stop() {
            Circuit = null;
            ResetColors();
        }
        private void RefreshExtraTerminals() {
            ExtraTerminals = new List<ExtraTerminal>();
            for (int i = 0; i < Wires.Count; i++) {
                if(Wires[i].FromComponent == null) {
                    RefreshExtraTerminals_internal(Wires[i].From, Wires[i].RootComponent);
                }
                if(Wires[i].ToComponent == null) {
                    RefreshExtraTerminals_internal(Wires[i].To, Wires[i].RootComponent);
                }
            }
        }
        private void RefreshExtraTerminals_internal(PointF point, Component root) {
            int count = 0;
            for (int i = 0; i < Wires.Count; i++) {
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
            foreach (Wire item in Wires) {
                item.Color = Draws.Color_off;
            }
            foreach (Component item in Components) {
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
            while (Circuit != null || !UIStatics.WantExit) {
                List<CircuitSimulator.Component> copy;
                try {
                    copy = Circuit.Components;
                } catch (Exception) {
                    break;
                }
                float halfCut = Pin.HIGH + Pin.LOW / 2f;
                foreach (CircuitSimulator.Component item in copy) {
                    if(item is CircuitSimulator.Components.Wire) {
                        Wire DrawComponent = DrawWireToCircuitWire[(CircuitSimulator.Components.Wire)item];
                        if (item.Pins[0].IsOpen) {
                            DrawComponent.Color = Draws.Color_3rd;
                        } else if (item.Pins[0].Value >= halfCut) {
                            DrawComponent.Color = Draws.Color_on;
                        } else {
                            DrawComponent.Color = Draws.Color_off;
                        }
                    } else {
                        Component DrawComponent = DrawComponentsToCircuitComponent[item];
                        if(item is AndGate) {
                            if(((AndGate)item).Output.Value >= halfCut) {
                                DrawComponent.Draw = Draws.And[1];
                            } else {
                                DrawComponent.Draw = Draws.And[0];
                            }
                        } else if (item is NandGate) {
                            if (((NandGate)item).Output.Value >= halfCut) {
                                DrawComponent.Draw = Draws.Nand[1];
                            } else {
                                DrawComponent.Draw = Draws.Nand[0];
                            }
                        } else if (item is OrGate) {
                            if (((OrGate)item).Output.Value >= halfCut) {
                                DrawComponent.Draw = Draws.Or[1];
                            } else {
                                DrawComponent.Draw = Draws.Or[0];
                            }
                        } else if (item is NorGate) {
                            if (((NorGate)item).Output.Value >= halfCut) {
                                DrawComponent.Draw = Draws.Nor[1];
                            } else {
                                DrawComponent.Draw = Draws.Nor[0];
                            }
                        }
                        else if(item is XorGate) {
                            if (((XorGate)item).Output.Value >= halfCut) {
                                DrawComponent.Draw = Draws.Xor[1];
                            } else {
                                DrawComponent.Draw = Draws.Xor[0];
                            }
                        } else if (item is XnorGate) {
                            if (((XnorGate)item).Output.Value >= halfCut) {
                                DrawComponent.Draw = Draws.Xnor[1];
                            } else {
                                DrawComponent.Draw = Draws.Xnor[0];
                            }
                        } else if (item is NotGate) {
                            if (((NotGate)item).Output.Value >= halfCut) {
                                DrawComponent.Draw = Draws.Not[1];
                            } else {
                                DrawComponent.Draw = Draws.Not[0];
                            }
                        } else if (item is LogicInput) {
                            if (DrawComponent.Draw.DisplayListHandle == Draws.Input[0].DisplayListHandle) {
                                ((LogicInput)item).Value = Pin.LOW;
                            } else if (DrawComponent.Draw.DisplayListHandle == Draws.Input[1].DisplayListHandle) {
                                ((LogicInput)item).Value = Pin.HIGH;
                            } else {
                                throw new Exception("Erro.");
                            }
                        } else if (item is LogicOutput) {
                            if (((LogicOutput)item).Pins[0].Value >= halfCut) {
                                DrawComponent.Draw = Draws.Output[1];
                            } else {
                                DrawComponent.Draw = Draws.Output[0];
                            }
                        } else if (item is CircuitSimulator.Components.Digital.Keyboard) {
                            ((CircuitSimulator.Components.Digital.Keyboard)item).Value = KeyDown_byte;
                        } else if (item is Microcontroller) {
                            if (UIStatics.Simulador != null) {
                                Microcontroller microcontroller = (Microcontroller)item;
                                for (int i = 0; i < 4; i++) {
                                    UIStatics.Simulador.In[i] = microcontroller.PinValuesToByteValue(i);
                                    microcontroller.SetOutput(UIStatics.Simulador.Out[i], i);
                                }
                            }
                        } else if (item is Display7Seg) {
                            Display7Seg display = ((Display7Seg)item);
                            for (int i = 0; i < 7; i++) {
                                DrawComponent.ActiveExtraHandlers[i] = display.Pins[i].Value >= halfCut;
                            }
                        } else {
                            //throw new NotImplementedException();
                        }
                    }
                }
                Circuit.Tick();
                Thread.Sleep(16);
            }
        }

        private void MountCircuit() {
            Circuit = new Circuit();
            CircuitComponentToDrawComponents.Clear();
            CircuitWireToDrawWire.Clear();
            DrawComponentsToCircuitComponent.Clear();
            DrawWireToCircuitWire.Clear();
            foreach (Component item in Components) {
                if (item.Draw.DisplayListHandle == Draws.And[0].DisplayListHandle ||
                    item.Draw.DisplayListHandle == Draws.And[1].DisplayListHandle) {
                    CircuitSimulator.Component component = Circuit.AddComponent(new AndGate());
                    CircuitComponentToDrawComponents.Add(item, component);
                    DrawComponentsToCircuitComponent.Add(component, item);
                } else if (item.Draw.DisplayListHandle == Draws.Nand[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Nand[1].DisplayListHandle) {
                    CircuitSimulator.Component component = Circuit.AddComponent(new NandGate());
                    CircuitComponentToDrawComponents.Add(item, component);
                    DrawComponentsToCircuitComponent.Add(component, item);
                } else if (item.Draw.DisplayListHandle == Draws.Or[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Or[1].DisplayListHandle) {
                    CircuitSimulator.Component component = Circuit.AddComponent(new OrGate());
                    CircuitComponentToDrawComponents.Add(item, component);
                    DrawComponentsToCircuitComponent.Add(component, item);
                } else if (item.Draw.DisplayListHandle == Draws.Nor[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Nor[1].DisplayListHandle) {
                    CircuitSimulator.Component component = Circuit.AddComponent(new NorGate());
                    CircuitComponentToDrawComponents.Add(item, component);
                    DrawComponentsToCircuitComponent.Add(component, item);
                } else if (item.Draw.DisplayListHandle == Draws.Xor[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Xor[1].DisplayListHandle) {
                    CircuitSimulator.Component component = Circuit.AddComponent(new XorGate());
                    CircuitComponentToDrawComponents.Add(item, component);
                    DrawComponentsToCircuitComponent.Add(component, item);
                } else if (item.Draw.DisplayListHandle == Draws.Xnor[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Xnor[1].DisplayListHandle) {
                    CircuitSimulator.Component component = Circuit.AddComponent(new XnorGate());
                    CircuitComponentToDrawComponents.Add(item, component);
                    DrawComponentsToCircuitComponent.Add(component, item);
                } else if (item.Draw.DisplayListHandle == Draws.Not[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Not[1].DisplayListHandle) {
                    CircuitSimulator.Component component = Circuit.AddComponent(new NotGate());
                    CircuitComponentToDrawComponents.Add(item, component);
                    DrawComponentsToCircuitComponent.Add(component, item);
                } else if (item.Draw.DisplayListHandle == Draws.Input[0].DisplayListHandle ||
                             item.Draw.DisplayListHandle == Draws.Input[1].DisplayListHandle) {
                    CircuitSimulator.Component component = Circuit.AddComponent(new LogicInput());
                    CircuitComponentToDrawComponents.Add(item, component);
                    DrawComponentsToCircuitComponent.Add(component, item);
                } else if (item.Draw.DisplayListHandle == Draws.Output[0].DisplayListHandle ||
                             item.Draw.DisplayListHandle == Draws.Output[1].DisplayListHandle) {
                    CircuitSimulator.Component component = Circuit.AddComponent(new LogicOutput());
                    CircuitComponentToDrawComponents.Add(item, component);
                    DrawComponentsToCircuitComponent.Add(component, item);
                } else if (item.Draw.DisplayListHandle == Draws.Keyboard.DisplayListHandle) {
                    CircuitSimulator.Component component = Circuit.AddComponent(new CircuitSimulator.Components.Digital.Keyboard());
                    CircuitComponentToDrawComponents.Add(item, component);
                    DrawComponentsToCircuitComponent.Add(component, item);
                } else if (item.Draw.DisplayListHandle == Draws.Microcontroller.DisplayListHandle) {
                    CircuitSimulator.Component component = Circuit.AddComponent(new Microcontroller());
                    CircuitComponentToDrawComponents.Add(item, component);
                    DrawComponentsToCircuitComponent.Add(component, item);
                } else if (item.Draw.DisplayListHandle == Draws.Disable[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Disable[1].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Disable[2].DisplayListHandle) {
                    throw new NotImplementedException();
                } else if (item.Draw.DisplayListHandle == Draws.Display7SegBase.DisplayListHandle) {
                    CircuitSimulator.Component component = Circuit.AddComponent(new Display7Seg());
                    CircuitComponentToDrawComponents.Add(item, component);
                    DrawComponentsToCircuitComponent.Add(component, item);
                } else if (item.Draw.DisplayListHandle == Draws.BinTo7Seg.DisplayListHandle) {
                    CircuitSimulator.Component component = Circuit.AddComponent(new BinTo7Seg());
                    CircuitComponentToDrawComponents.Add(item, component);
                    DrawComponentsToCircuitComponent.Add(component, item);
                } else {
                    throw new NotImplementedException();
                }
            }
            foreach (Wire item in Wires) {
                CircuitSimulator.Components.Wire wire = Circuit.AddComponent(new CircuitSimulator.Components.Wire());
                CircuitWireToDrawWire.Add(item, wire);
                DrawWireToCircuitWire.Add(wire, item);
            }


            foreach (Wire item in Wires) {
                if(item.FromComponent != null && item.ToComponent != null) {//from and to is component
                    CircuitSimulator.Component From = CircuitComponentToDrawComponents[item.FromComponent];
                    CircuitSimulator.Component To = CircuitComponentToDrawComponents[item.ToComponent];
                    CircuitSimulator.Components.Wire Wire = CircuitWireToDrawWire[item];
                    Wire.Pins[0].Connect(From.Pins[item.FromIndex]);
                    Wire.Pins[1].Connect(To.Pins[item.ToIndex]);
                } else if (item.FromComponent != null && item.ToComponent == null) {// from is and to is not a component
                    CircuitSimulator.Components.Wire Wire = CircuitWireToDrawWire[item];
                    CircuitSimulator.Component From = CircuitComponentToDrawComponents[item.FromComponent];
                    List<Pin> TempPins = new List<Pin>();
                    foreach (Wire item2 in Wires) {
                        if(item.To == item2.From) {
                            if (item != item2)
                                TempPins.Add(CircuitWireToDrawWire[item2].Pins[0]);
                        } else if(item.To == item2.To) {
                            if (item != item2)
                                TempPins.Add(CircuitWireToDrawWire[item2].Pins[1]);
                        }
                    }
                    Wire.Pins[0].Connect(From.Pins[item.FromIndex]);
                    foreach (Pin pin in TempPins) {
                        Wire.Pins[1].Connect(pin);
                    }
                } else if (item.FromComponent == null && item.ToComponent != null) {//from is not and to is a component
                    CircuitSimulator.Components.Wire Wire = CircuitWireToDrawWire[item];
                    CircuitSimulator.Component To = CircuitComponentToDrawComponents[item.ToComponent];
                    List<Pin> TempPins = new List<Pin>();
                    foreach (Wire item2 in Wires) {
                        if (item.From == item2.From) {
                            if (item != item2)
                                TempPins.Add(CircuitWireToDrawWire[item2].Pins[0]);
                        } else if (item.From == item2.To) {
                            if (item != item2)
                                TempPins.Add(CircuitWireToDrawWire[item2].Pins[1]);
                        }
                    }
                    Wire.Pins[1].Connect(To.Pins[item.FromIndex]);
                    foreach (Pin pin in TempPins) {
                        Wire.Pins[0].Connect(pin);
                    }
                } else if (item.FromComponent == null && item.ToComponent == null) {//from and to is not a component
                    CircuitSimulator.Components.Wire Wire = CircuitWireToDrawWire[item];
                    List<Pin> TempPinsFrom = new List<Pin>();
                    List<Pin> TempPinsTo = new List<Pin>();
                    foreach (Wire item2 in Wires) {
                        if (item.From == item2.From) {
                            if (item != item2)
                                TempPinsFrom.Add(CircuitWireToDrawWire[item2].Pins[0]);
                        } else if (item.From == item2.To) {
                            if (item != item2)
                                TempPinsFrom.Add(CircuitWireToDrawWire[item2].Pins[1]);
                        } else if (item.To == item2.From) {
                            if (item != item2)
                                TempPinsTo.Add(CircuitWireToDrawWire[item2].Pins[0]);
                        } else if (item.To == item2.To) {
                            if (item != item2)
                                TempPinsTo.Add(CircuitWireToDrawWire[item2].Pins[1]);
                        }
                    }
                    foreach (Pin pin in TempPinsFrom) {
                        Wire.Pins[0].Connect(pin);
                    }
                    foreach (Pin pin in TempPinsTo) {
                        Wire.Pins[1].Connect(pin);
                    }
                }
            }

        }

        private float zoom = 1;
        
        internal void ZoomMore() {
            if (zoom < 1)
                zoom *= 2;
            else
                zoom += 1;
            Circuito_Resize(this, null);
            Refresh();
        }

        internal void ZoomLess() {
            if (zoom <= 1)
                zoom /= 2;
            else
                zoom--;
            Circuito_Resize(this, null);
            Refresh();
        }

        internal void ZoomReset() {
            zoom = 1;
            Circuito_Resize(this, null);
            Refresh();
        }

        public override void Refresh() {
            base.Refresh();

            RefreshExtraTerminals();
            if (MouseProps.Button2Pressed) {
                Position = new PointF(Position.X+((MouseProps.LastPosition.X - MouseProps.CurrentPosition.X)/zoom), Position.Y-((MouseProps.LastPosition.Y - MouseProps.CurrentPosition.Y)/zoom));
            }
            bool foundComponent = false;
            bool foundTerminal = false;

            PointF worldMousePos = MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, zoom);
            foreach (Component item in Components) {
                if (item.RootComponent != InsideComponent)
                    continue;
                int index = item.IsOnTerminal(worldMousePos);
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
                foreach (Wire item in Wires) {
                    if (item.RootComponent != InsideComponent)
                        continue;
                    float distance;
                    if (item.FromComponent == null) {
                        distance = 
                            (item.From.X - MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, zoom).X) * 
                            (item.From.X - MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, zoom).X) +
                            (item.From.Y - MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, zoom).Y) * 
                            (item.From.Y - MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, zoom).Y);
                        if(distance <= 4 * 4) {
                            Terminals.Hover = item.From;
                            Terminals.HoverNotComponent = true;
                            foundTerminal = true;
                            break;
                        }
                    }
                    if(item.ToComponent == null) {
                        distance = 
                            (item.To.X - MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, zoom).X) * 
                            (item.To.X - MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, zoom).X) +
                            (item.To.Y - MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, zoom).Y) * 
                            (item.To.Y - MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, zoom).Y);
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

            if (MouseProps.Button1Pressed && Circuit == null) {
                if (Selected != null) {
                    if (canDrag == false) {
                        //float distance = (Center.X + terminal.X - Point.X) * (Center.X + terminal.X - Point.X) + (Center.Y + terminal.Y - Point.Y) * (Center.Y + terminal.Y - Point.Y);
                        float distance =
                            (MouseProps.LastDownPosition.X - MouseProps.CurrentPosition.X) * (MouseProps.LastDownPosition.X - MouseProps.CurrentPosition.X) +
                            (MouseProps.LastDownPosition.Y - MouseProps.CurrentPosition.Y) * (MouseProps.LastDownPosition.Y - MouseProps.CurrentPosition.Y);
                        if (distance > 25*25) {
                            canDrag = true;
                        }
                    }
                    if (canDrag) {
                        Selected.Center = MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, zoom);
                        Selected.Center.X = (float)Math.Round(Selected.Center.X / 5) * 5;
                        Selected.Center.Y = (float)Math.Round(Selected.Center.Y / 5) * 5;
                    }
                }
                if(Terminals.FromComponent != null) {
                    terminalFromVertex = Terminals.FromComponent.TransformTerminal(Terminals.FromIndex);
                    terminalFromVertex.X += Terminals.FromComponent.Center.X;
                    terminalFromVertex.Y += Terminals.FromComponent.Center.Y;
                    terminalToVertex = MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, zoom);
                    terminalToVertex.X = (float)Math.Round(terminalToVertex.X / 5) * 5;
                    terminalToVertex.Y = (float)Math.Round(terminalToVertex.Y / 5) * 5;
                } else if (Terminals.FromNotComponent) {
                    terminalFromVertex = Terminals.From;
                    terminalToVertex = MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, zoom);
                    terminalToVertex.X = (float)Math.Round(terminalToVertex.X / 5) * 5;
                    terminalToVertex.Y = (float)Math.Round(terminalToVertex.Y / 5) * 5;
                }
            } else {
                canDrag = false;
            }
            bool foundWire = false;
            
            foreach (Wire item in Wires) {
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
        private PointF terminalFromVertex;
        private PointF terminalToVertex;
        private void Render() {
            Matrix4 lookat = Matrix4.LookAt(Position.X, Position.Y, 1, Position.X, Position.Y, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.Texture2D);
            if (zoom > 1f) {
                GL.LineWidth(zoom);
            } else {
                GL.LineWidth(1);
            }
            if (ExtraTerminals != null) {
                GL.Color3(Color.Black);
                foreach (ExtraTerminal item in ExtraTerminals) {
                    if (item.Root != InsideComponent) continue;
                    GL.Translate(item.Point.X, item.Point.Y, 0);
                    GL.CallList(Draws.TerminalHandle);
                    GL.Translate(-item.Point.X, -item.Point.Y, 0);
                }
            }
            foreach (Component item in Components) {
                if (item.RootComponent != InsideComponent)
                    continue;
                GL.Translate(item.Center.X, item.Center.Y, 0);
                GL.Rotate(item.Rotation, 0, 0, 1);
                GL.CallList(item.Draw.DisplayListHandle);
                if (item.ActiveExtraHandlers != null) {
                    for (int i = 0; i < item.ActiveExtraHandlers.Length; i++) {
                        if(item.ActiveExtraHandlers[i]) GL.CallList(item.ExtraHandlers[i]);
                    }
                }
                GL.Color3(Color.Black);
                foreach (Point terminal in item.Draw.Terminals) {
                    GL.Translate(terminal.X, terminal.Y, 0);
                    GL.CallList(Draws.TerminalHandle);
                    GL.Translate(-terminal.X, -terminal.Y, 0);
                }
                GL.Rotate(-item.Rotation, 0, 0, 1);
                GL.Translate(-item.Center.X, -item.Center.Y, 0);
            }

            if (MouseProps.Button1Pressed && Circuit == null) {
                if(Terminals.FromComponent != null) {
                    GL.Color3(Color.Black);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex2(terminalFromVertex.X, terminalFromVertex.Y);
                    GL.Vertex2(terminalToVertex.X, terminalToVertex.Y);
                    GL.End();
                } else if (Terminals.FromNotComponent) {
                    GL.Color3(Color.Black);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex2(terminalFromVertex.X, terminalFromVertex.Y);
                    GL.Vertex2(terminalToVertex.X, terminalToVertex.Y);
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
            
            Application.Idle += Application_Idle;
            
            Draws.Load();
            Console.WriteLine(Application.StartupPath + "\\Default.m3mprj");
            Console.WriteLine(FileProject.Load(Application.StartupPath+"\\Default.m3mprj"));
            /*
            MountMicrocontroller();

            Components.Add(new Component(Draws.BinTo7Seg, new PointF(-200, 40)));
            Components.Add(new Component(Draws.Display7SegBase, new PointF(-100, 40)));*/
            //Components.Add(new Component(Draws.Circuit[8,16], new PointF(200, 0)));
            
            

        }

        public void LoadControl() {
            Visible = true;
            Visible = false;

        }
        void Application_Idle(object sender, EventArgs e) {
            while (IsIdle) {
                Render();
            }
        }
        
        private void Circuito_Resize(object sender, EventArgs e) {
            GLControl c = sender as GLControl;

            if (c.ClientSize.Height == 0)
                c.ClientSize = new Size(c.ClientSize.Width, 1);

            GL.Viewport(0, 0, c.ClientSize.Width, c.ClientSize.Height);
            Matrix4 orthographic = Matrix4.CreateOrthographic(c.ClientSize.Width/zoom, c.ClientSize.Height/zoom, 1, 10);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref orthographic);
        }

        private void Circuito_Paint(object sender, PaintEventArgs e) {
            Render();
        }

        private void Circuito_KeyUp(object sender, KeyEventArgs e) {
            if(KeyDown_byte == (byte)e.KeyValue)
                KeyDown_byte = 0;
        }

        private void Circuito_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e) {
            MouseProps.LastDoubleClickPosition = e.Location;
            if(Over != null) {
                if(Over.Draw.DisplayListHandle == Draws.Input[0].DisplayListHandle) {
                    Over.Draw = Draws.Input[1];
                } else if (Over.Draw.DisplayListHandle == Draws.Input[1].DisplayListHandle) {
                    Over.Draw = Draws.Input[0];
                } else if (Over.Draw.DisplayListHandle == Draws.Microcontroller.DisplayListHandle) {
                    Component Mic = Over;
                    Selected = null;
                    SelectedWire = null;
                    Over = null;
                    InsideComponent = Mic;
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
            if (e.Button == MouseButtons.Left) {
                MouseProps.LastClickPosition = e.Location;
            } else if(e.Button == MouseButtons.Right) {
                if(((MouseProps.LastDownPosition.X-e.X)* (MouseProps.LastDownPosition.X - e.X))+((MouseProps.LastDownPosition.Y - e.Y) * (MouseProps.LastDownPosition.Y - e.Y)) < 10 * 10) {
                    PositionCreatingComponent = MouseProps.ToWorld(e.Location, ClientSize, Position, zoom);
                    contextMenuStrip1.Show(this, e.Location);
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
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) {
                MouseProps.LastUpPosition = e.Location;
                if (e.Button == MouseButtons.Left) {
                    MouseProps.Button1Pressed = false;
                } else {
                    MouseProps.Button2Pressed = false;
                }
            }
            if (Circuit == null) {
                if (Terminals.FromComponent != null) {
                    if (Terminals.HoverComponent != null) {
                        if (Terminals.HoverComponent != Terminals.FromComponent || Terminals.HoverIndex != Terminals.FromIndex) {
                            Wires.Add(new Wire(Terminals.FromComponent, Terminals.FromIndex, Terminals.HoverComponent, Terminals.HoverIndex));
                        }
                    } else {
                        PointF point = MouseProps.ToWorld(MouseProps.LastUpPosition, ClientSize, Position, zoom);
                        point.X = (float)Math.Round(point.X / 5) * 5;
                        point.Y = (float)Math.Round(point.Y / 5) * 5;
                        Wires.Add(new Wire(Terminals.FromComponent, Terminals.FromIndex, point));
                    }
                } else if (Terminals.FromNotComponent) {
                    if (Terminals.HoverComponent != null) {
                        Wires.Add(new Wire(Terminals.From, Terminals.HoverComponent, Terminals.HoverIndex));
                    } else {
                        PointF point = MouseProps.ToWorld(MouseProps.LastUpPosition, ClientSize, Position, zoom);
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
            foreach (Wire item in Wires) {
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
                Vector3 pos = new Vector3(Terminals.HoverComponent.Center.X, Terminals.HoverComponent.Center.Y, 0);
                PointF transform = Terminals.HoverComponent.TransformTerminal(Terminals.HoverIndex);
                pos.X += transform.X;
                pos.Y += transform.Y;
                GL.Translate(pos);
                GL.CallList(Draws.TerminalHandle);
                string tmp = Terminals.HoverComponent.Draw.TerminalsString[Terminals.HoverIndex];
                if (tmp != null && tmp != "") {
                    GL.Translate(0, 20, 0);
                    TextRenderer.DrawText(tmp, Color.Red, new PointF(0, 0));
                    GL.Translate(0, -20, 0);
                }
                
                GL.Translate(-pos);
            } else if (Terminals.HoverNotComponent) {
                GL.Color3(Color.Green);
                Vector3 pos = new Vector3(Terminals.Hover.X, Terminals.Hover.Y, 0);
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
            float maxX = point1.X > point2.X ? point1.X : point2.X;
            float minX = point1.X > point2.X ? point2.X : point1.X;
            float maxY = point1.Y > point2.Y ? point1.Y : point2.Y;
            float minY = point1.Y > point2.Y ? point2.Y : point1.Y;
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

            float maiorX = x > point.X ? x : point.X;
            float menorX = x > point.X ? point.X : x;
            float maiorY = y > point.Y ? y : point.Y;
            float menorY = y > point.Y ? point.Y : y;
            
            //calcula a distancia
            if ((maiorX - menorX) * (maiorX - menorX) + (maiorY - menorY) * (maiorY - menorY) < 5 * 5) {
                return true;
            }
            return false;
        }

        private void Circuito_KeyDown(object sender, KeyEventArgs e) {
            KeyDown_byte = (byte) e.KeyValue;
            if (Circuit == null) {
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
                        int length = Wires.Count;
                        for (int i = 0; i < length; i++) {
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
                    Console.WriteLine("Enter");
                } else if(e.KeyCode == Keys.C) {
                    string r = Microsoft.VisualBasic.Interaction.InputBox("Nome do componente a ser criado:", "Criar componente [DEBUG]", "");
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
                                Components.Add(new Component(Draws.JKFlipFlop, new PointF(0, 0), InsideComponent));
                                break;
                            case "RSFlipFlop":
                                Components.Add(new Component(Draws.RSFlipFlop, new PointF(0, 0), InsideComponent));
                                break;
                            case "DFlipFlop":
                                Components.Add(new Component(Draws.DFlipFlop, new PointF(0, 0), InsideComponent));
                                break;
                            case "TFlipFlop":
                                Components.Add(new Component(Draws.TFlipFlop, new PointF(0, 0), InsideComponent));
                                break;
                            case "BinTo7SHalfAddereg":
                                Components.Add(new Component(Draws.HalfAdder, new PointF(0, 0), InsideComponent));
                                break;
                            case "FullAdder":
                                Components.Add(new Component(Draws.FullAdder, new PointF(0, 0), InsideComponent));
                                break;
                            case "ULA":
                                Components.Add(new Component(Draws.ULA, new PointF(0, 0), InsideComponent));
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
                                Components.Add(new Component(Draws.Registrer8BitSG, new PointF(0, 0), InsideComponent));
                                break;
                            case "LedMatrix":
                                Components.Add(new Component(Draws.LedMatrix, new PointF(0, 0), InsideComponent));
                                break;
                            case "Disable8Bit":
                                Components.Add(new Component(Draws.Disable8Bit, new PointF(0, 0), InsideComponent));
                                break;
                            /*
                            None, Input, Output, Disable, Not, And, Nand, Or, Nor, Xor, Xnor, Keyboard, Display7Seg, BinTo7Seg, Circuit,
        Microcontroller, Osciloscope, BlackTerminal, JKFlipFlop, RSFlipFlop, DFlipFlop, TFlipFlop,
        HalfAdder, FullAdder, ULA, ControlModule, Registrer8Bit, Registrers, Tristate, Stack, RamMemory,
        RomMemory, PortBank, Registrer8BitSG, LedMatrix, Disable8Bit*/
                            default:
                                MessageBox.Show("Componente "+ r +" inexistente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }
                }
            }
            Refresh();
        }

        private string DebugGenerateNewComponentsString() {
            string res = "";

            for (int i = 0; i < Components.Count; i++) {
                res += "Component c_" + i + "_" + Components[i].Type.ToString()+" = new Component(";
                res += "Draws." + Components[i].Type.ToString();
                switch (Components[i].Type) {
                    case ComponentType.And:
                    case ComponentType.Nand:
                    case ComponentType.Or:
                    case ComponentType.Nor:
                    case ComponentType.Xor:
                    case ComponentType.Not:
                    case ComponentType.Disable:
                    case ComponentType.Input:
                    case ComponentType.Output:
                        res += "[0], ";
                        break;
                    default:
                        res += ", ";
                        break;
                }
                res += "new PointF(" + Components[i].Center.X + ", " + Components[i].Center.Y + ")";
                if(Components[i].RootComponent != null) {
                    res += ", c_" + Components.IndexOf(Components[i].RootComponent) + "_" + Components[i].RootComponent.Type.ToString();
                }
                res += ");\r\n";
                res += "Components.Add(c_" + i + "_" + Components[i].Type.ToString() + ")";
            }
            for (int i = 0; i < Wires.Count; i++) {
                res += "Component c_" + i + "_" + Components[i].Type.ToString() + " = new Component(";
                res += "Draws." + Components[i].Type.ToString();
                switch (Components[i].Type) {
                    case ComponentType.And:
                    case ComponentType.Nand:
                    case ComponentType.Or:
                    case ComponentType.Nor:
                    case ComponentType.Xor:
                    case ComponentType.Not:
                    case ComponentType.Disable:
                    case ComponentType.Input:
                    case ComponentType.Output:
                        res += "[0], ";
                        break;
                    default:
                        res += ", ";
                        break;
                }
                res += "new PointF(" + Components[i].Center.X + ", " + Components[i].Center.Y + ")";
                if (Components[i].RootComponent != null) {
                    res += ", c_" + Components.IndexOf(Components[i].RootComponent) + "_" + Components[i].RootComponent.Type.ToString();
                }
                res += ");\r\n";
            }
            return res;
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

        }

        private void MountMicrocontroller() {
            /*
            Component microcontroller = new Component(Draws.Microcontroller, new PointF(0, 0));
            Components.Add(microcontroller);

            Component controlModule = new Component(Draws.ControlModule, new PointF(-500, 0), microcontroller);
            Components.Add(controlModule);
            for (int i = 0; i < 8*4; i++) {
                Components.Add(new Component(Draws.Input[0], new PointF(-1600, 145 - i * 10), microcontroller));
                Components.Add(new Component(Draws.Output[0], new PointF(-1500, 145 - i * 10), microcontroller));
            }
            Components.Add(new Component(Draws.PortBank, new PointF(-1600, -500), microcontroller));*/
            /*
            Component in0 = new Component(Draws.Input[0], new PointF(-800, 145), microcontroller);
            Component in1 = new Component(Draws.Input[0], new PointF(-800, 135), microcontroller);
            Component in2 = new Component(Draws.Input[0], new PointF(-800, 125), microcontroller);
            Component in3 = new Component(Draws.Input[0], new PointF(-800, 115), microcontroller);
            Component in4 = new Component(Draws.Input[0], new PointF(-800, 105), microcontroller);
            Component in5 = new Component(Draws.Input[0], new PointF(-800, 95), microcontroller);
            Component in6 = new Component(Draws.Input[0], new PointF(-800, 85), microcontroller);
            Component in7 = new Component(Draws.Input[0], new PointF(-800, 75), microcontroller);
            Components.Add(in0);
            Components.Add(in1);
            Components.Add(in2);
            Components.Add(in3);
            Components.Add(in4);
            Components.Add(in5);
            Components.Add(in6);
            Components.Add(in7);

            Component tristatein = new Component(Draws.Disable8Bit, new PointF(-750, 105), microcontroller);
            Components.Add(tristatein);

            Component tristateout = new Component(Draws.Disable8Bit, new PointF(-750, 0), microcontroller);
            Components.Add(tristateout);

            Wires.Add(new Wire(in0, 0, tristatein, 0, microcontroller));
            Wires.Add(new Wire(in1, 0, tristatein, 1, microcontroller));
            Wires.Add(new Wire(in2, 0, tristatein, 2, microcontroller));
            Wires.Add(new Wire(in3, 0, tristatein, 3, microcontroller));
            Wires.Add(new Wire(in4, 0, tristatein, 4, microcontroller));
            Wires.Add(new Wire(in5, 0, tristatein, 5, microcontroller));
            Wires.Add(new Wire(in6, 0, tristatein, 6, microcontroller));
            Wires.Add(new Wire(in7, 0, tristatein, 7, microcontroller));
            */

        }
    }

    public class CircuitDraw {
        private static ComponentDraw[,] Circuit;
        public CircuitDraw() {
            Circuit = new ComponentDraw[256, 256];
        }
        public ComponentDraw this[int input, int output]
        {
            get {
                if(Circuit[input, output] == null) {
                    Circuit[input, output] = GenCircuit(input, output);
                }
                return Circuit[input, output];
            }
            set {
                Circuit[input, output] = value;
            }
        }
        private static ComponentDraw GenCircuit(int inputs, int outputs) {
            int maxValue = inputs > outputs ? inputs : outputs;
            int width = maxValue * 20 / 8 + 20;
            int height = maxValue * 10;
            ComponentDraw componentDraw = new ComponentDraw(GL.GenLists(1), width, height, inputs + outputs);
            for (int i = 0; i < inputs; i++) {
                componentDraw.Terminals[i] = new Point(-width / 2, height / 2 - (5 + i * 10));
            }
            for (int i = inputs; i < inputs + outputs; i++) {
                componentDraw.Terminals[i] = new Point(width / 2, height / 2 - (5 + (i - inputs) * 10));
            }
            GL.NewList(componentDraw.DisplayListHandle, ListMode.Compile);
            GL.Color3(Draws.Color_off);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-width / 2f + 10, -height / 2f);
            GL.Vertex2(width / 2f - 10, -height / 2f);
            GL.Vertex2(width / 2f - 10, height / 2f);
            GL.Vertex2(-width / 2f + 10, height / 2f);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            for (int i = 0; i < inputs; i++) {
                GL.Vertex2(-width / 2, height / 2 - (5 + i * 10));
                GL.Vertex2(-width / 2 + 10f, height / 2 - (5 + i * 10));
            }
            for (int i = inputs; i < inputs + outputs; i++) {
                GL.Vertex2(width / 2, height / 2 - (5 + (i - inputs) * 10));
                GL.Vertex2(width / 2 - 10f, height / 2 - (5 + (i - inputs) * 10));
            }
            GL.End();
            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex2(-width / 2f + 10, -5);
            GL.Vertex2(-width / 2f + 15, 0);
            GL.Vertex2(-width / 2f + 10, 5);
            GL.End();
            GL.EndList();

            return componentDraw;
        }
    }

    /// <summary>
    /// Uses System.Drawing for 2d text rendering.
    /// </summary>
    public class TextRenderer:IDisposable {
        private static Dictionary<int, TextRenderer> textRenderers = new Dictionary<int, TextRenderer>();
        private static Font font = new Font(FontFamily.GenericMonospace, 12);
        private Bitmap bmp;
        private Graphics gfx;
        private int texture;
        private Rectangle dirty_region;
        private bool disposed;
        

        private TextRenderer() {
            if (GraphicsContext.CurrentContext == null)
                throw new InvalidOperationException("No GraphicsContext is current on the calling thread.");
        }
        

        void Dispose(bool manual) {
            if (!disposed) {
                if (manual) {
                    bmp.Dispose();
                    gfx.Dispose();
                    if (GraphicsContext.CurrentContext != null)
                        GL.DeleteTexture(texture);
                }

                disposed = true;
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TextRenderer() {
            Console.WriteLine("[Warning] Resource leaked: {0}.", typeof(TextRenderer));
        }
        public static void DrawText(string text, Color color, PointF point) {
            DrawText(text, color, point, font);
        }
        public static void DrawText(string text, Color color, PointF point, Font font) {
            TextRenderer renderer;
            int width;
            int height;
            int hash = text.GetHashCode() + color.GetHashCode() + font.GetHashCode();
            if (!textRenderers.ContainsKey(hash)) { 
                renderer = new TextRenderer();
                renderer.bmp = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                renderer.gfx = Graphics.FromImage(renderer.bmp);
                Brush brush = new SolidBrush(color);
                renderer.gfx.DrawString(text, font, brush, 0, 0);
                SizeF textSize = renderer.gfx.MeasureString(text, font);
                renderer.gfx.Dispose();
                renderer.bmp.Dispose();
                width = (int)(textSize.Width + 0.5f);
                height = (int)(textSize.Height + 0.5f);
                if (width <= 0)
                    throw new ArgumentOutOfRangeException("width");
                if (height <= 0)
                    throw new ArgumentOutOfRangeException("height ");
                renderer.bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                renderer.gfx = Graphics.FromImage(renderer.bmp);
                renderer.gfx.Clear(UIStatics.Circuito.ClearColor);
                renderer.gfx.DrawString(text, font, brush, 0, 0);
                renderer.dirty_region = new Rectangle(0, 0, renderer.bmp.Width, renderer.bmp.Height);
                renderer.texture = GL.GenTexture();
                textRenderers[hash] = renderer;
                GL.BindTexture(TextureTarget.Texture2D, renderer.texture);
                
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);


                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
                    PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);

                System.Drawing.Imaging.BitmapData data = renderer.bmp.LockBits(renderer.dirty_region,
                        System.Drawing.Imaging.ImageLockMode.ReadOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexSubImage2D(TextureTarget.Texture2D, 0,
                    renderer.dirty_region.X, renderer.dirty_region.Y, renderer.dirty_region.Width, renderer.dirty_region.Height,
                    PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                renderer.bmp.UnlockBits(data);
            } else {
                renderer = textRenderers[hash];
                GL.BindTexture(TextureTarget.Texture2D, renderer.texture);
                width = renderer.bmp.Width;
                height = renderer.bmp.Height;
            }

            GL.Color3(Color.White);
            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 1); GL.Vertex2(point.X - width/2, point.Y - height/2);
            GL.TexCoord2(1, 1); GL.Vertex2(point.X + width / 2, point.Y - height/2);
            GL.TexCoord2(1, 0); GL.Vertex2(point.X + width / 2, point.Y + height / 2);
            GL.TexCoord2(0, 0); GL.Vertex2(point.X - width/2, point.Y + height / 2);
            GL.End();
        }
    }

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
        public static ComponentDraw JKFlipFlop;
        public static ComponentDraw RSFlipFlop;
        public static ComponentDraw TFlipFlop;
        public static ComponentDraw DFlipFlop;
        public static ComponentDraw Display7SegBase;
        public static int[] Display7SegPart;
        public static ComponentDraw BinTo7Seg;
        public static ComponentDraw Osciloscope;
        public static ComponentDraw HalfAdder;
        public static ComponentDraw FullAdder;
        public static ComponentDraw ULA;
        public static ComponentDraw ControlModule;
        public static ComponentDraw Registrer8Bit;
        public static ComponentDraw Registrers;
        public static ComponentDraw Disable8Bit;
        public static ComponentDraw RamMemory;
        public static ComponentDraw RomMemory;
        public static ComponentDraw PortBank;
        public static ComponentDraw Registrer8BitSG;
        public static ComponentDraw LedMatrix;




        public static CircuitDraw Circuit;


        public static Color Color_on = Color.Red;
        public static Color Color_off = Color.Black;
        public static Color Color_3rd = Color.Gray;

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
            GenJKFlipFlop();
            GenRSFlipFlop();
            GenDFlipFlop();
            GenTFlipFlop();
            GenDisable8Bit();
            GenAdders();
            GenMemories();
            GenRegistrers();
            GenULA();
            GenControlModule();
            GenPortBank();
        }
        private static void GenLedMatrix() { }
        private static void GenBlackTerminal() { }
        private static void GenPortBank() {
            TextRenderer.DrawText("Port\nBank", Color.Black, new PointF(0, 0));
            ComponentDraw DrawCircuit = Circuit[45, 40];
            PortBank = new ComponentDraw(GL.GenLists(1), DrawCircuit.Width, DrawCircuit.Height, DrawCircuit.Terminals.Length);
            for (int i = 0; i < DrawCircuit.Terminals.Length; i++) {
                PortBank.Terminals[i] = DrawCircuit.Terminals[i];
                if (i < 8 * 4) {
                    PortBank.TerminalsString[i] = ("in" + (i / 8) + "[" + (i % 8) + "]");
                } else if (i < 8 * 5 + 5 ) {
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
            GL.CallList(DrawCircuit.DisplayListHandle);

            GL.EndList();
        }
        private static void GenControlModule() {
            TextRenderer.DrawText("Control\nModule", Color.Black, new PointF(0, 0));
            ComponentDraw DrawCircuit = Circuit[11, 30];
            ControlModule = new ComponentDraw(GL.GenLists(1), DrawCircuit.Width, DrawCircuit.Height, DrawCircuit.Terminals.Length);
            for (int i = 0; i < DrawCircuit.Terminals.Length; i++) {
                ControlModule.Terminals[i] = DrawCircuit.Terminals[i];
            }
            for (int i = 0; i < 8; i++) {
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
            GL.CallList(DrawCircuit.DisplayListHandle);

            GL.EndList();
        }
        private static void GenULA() {
            TextRenderer.DrawText("ULA", Color.Black, new PointF(0, 0));
            ULA = new ComponentDraw(GL.GenLists(1), 220, 100, 30);
            GL.NewList(ULA.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("ULA", Color.Black, new PointF(0, 0));

            GL.Color3(Color_off);
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
            for (int i = 0; i < 8; i++) {
                GL.Vertex2(i * 10 - 95, 40);
                GL.Vertex2(i * 10 - 95, 50);

                ULA.Terminals[i] = new Point(i * 10 - 95, 50);
                ULA.TerminalsString[i] = "A[" + i + "]";
                GL.Vertex2(i * 10 + 25, 40);
                GL.Vertex2(i * 10 + 25, 50);

                ULA.Terminals[i+8] = new Point(i * 10 + 25, 50);
                ULA.TerminalsString[i+8] = "B[" + (i) + "]";
                GL.Vertex2(i * 10 - 35, -40);
                GL.Vertex2(i * 10 - 35, -50);

                ULA.Terminals[i + 20] = new Point(i * 10 - 35, -50);
                ULA.TerminalsString[i + 20] = "O[" + (i) + "]";

            }

            GL.Vertex2(-90, 20);
            GL.Vertex2(-110, 20);
            ULA.Terminals[16] = new Point(-110, 20);
            ULA.TerminalsString[16] = "S0";

            GL.Vertex2(-82, 10);
            GL.Vertex2(-110, 10);
            ULA.Terminals[17] = new Point(-110, 10);
            ULA.TerminalsString[17] = "S1";

            GL.Vertex2(-75, 0);
            GL.Vertex2(-110, 0);
            ULA.Terminals[18] = new Point(-110, 0);
            ULA.TerminalsString[18] = "S2";

            GL.Vertex2(-60, -20);
            GL.Vertex2(-110, -20);
            ULA.Terminals[19] = new Point(-110, -20);
            ULA.TerminalsString[19] = "Enable";
            
            GL.Vertex2(90, 20);
            GL.Vertex2(110, 20);
            ULA.Terminals[28] = new Point(110, 20);
            ULA.TerminalsString[28] = "Flag Z";

            GL.Vertex2(82, 10);
            GL.Vertex2(110, 10);
            ULA.Terminals[29] = new Point(110, 10);
            ULA.TerminalsString[29] = "Flag C";

            GL.End();

            GL.EndList();
        }
        private static void GenRegistrers() {
            TextRenderer.DrawText("R\ne\ng", Color.Black, new PointF(0, 0));
            TextRenderer.DrawText("R\ne\ng\nS\nG", Color.Black, new PointF(0, 0));
            ComponentDraw DrawCircuit = Circuit[11, 8];
            Registrer8Bit = new ComponentDraw(GL.GenLists(1), DrawCircuit.Width, DrawCircuit.Height, DrawCircuit.Terminals.Length);
            for (int i = 0; i < DrawCircuit.Terminals.Length; i++) {
                Registrer8Bit.Terminals[i] = DrawCircuit.Terminals[i];
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
            GL.CallList(DrawCircuit.DisplayListHandle);
            GL.EndList();

            DrawCircuit = Circuit[10, 8];
            Registrer8BitSG = new ComponentDraw(GL.GenLists(1), DrawCircuit.Width, DrawCircuit.Height, DrawCircuit.Terminals.Length);
            for (int i = 0; i < DrawCircuit.Terminals.Length; i++) {
                Registrer8BitSG.Terminals[i] = DrawCircuit.Terminals[i];
                if (i < 8) {
                    Registrer8BitSG.TerminalsString[i] = ("In" + (i % 8));
                } else if (i > 9) {
                    Registrer8BitSG.TerminalsString[i] = ("Out" + ((i - 2) % 8));
                } else {
                    switch (i) {
                        case 8:
                            Registrer8BitSG.TerminalsString[i] = "Enable";
                            break;
                        case 9:
                            Registrer8BitSG.TerminalsString[i] = "Reset";
                            break;
                    }

                }
            }
            GL.NewList(Registrer8BitSG.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("R\ne\ng\nS\nG", Color.Black, new PointF(0, 0));
            GL.CallList(DrawCircuit.DisplayListHandle);
            GL.EndList();
        }
        private static void GenMemories() {
            TextRenderer.DrawText("Ram", Color.Black, new PointF(0, 0));
            TextRenderer.DrawText("Rom", Color.Black, new PointF(0, 0));
            ComponentDraw DrawCircuit = Circuit[11, 8];
            RamMemory = new ComponentDraw(GL.GenLists(1), DrawCircuit.Width, DrawCircuit.Height, DrawCircuit.Terminals.Length);
            for (int i = 0; i < DrawCircuit.Terminals.Length; i++) {
                RamMemory.Terminals[i] = DrawCircuit.Terminals[i];
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
            TextRenderer.DrawText("Ram", Color.Black, new PointF(0, 0));
            GL.CallList(DrawCircuit.DisplayListHandle);
            GL.EndList();


            DrawCircuit = Circuit[17, 8];
            RomMemory = new ComponentDraw(GL.GenLists(1), DrawCircuit.Width, DrawCircuit.Height, DrawCircuit.Terminals.Length);
            for (int i = 0; i < DrawCircuit.Terminals.Length; i++) {
                RomMemory.Terminals[i] = DrawCircuit.Terminals[i];
                if (i < 16) {
                    RomMemory.TerminalsString[i] = ("A" + (i % 16));
                } else if (i > 16) {
                    RomMemory.TerminalsString[i] = ("D" + ((i - 1) % 8));
                } else {
                    RomMemory.TerminalsString[i] = "sel";

                }
            }
            GL.NewList(RomMemory.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("Rom", Color.Black, new PointF(0, 0));
            GL.CallList(DrawCircuit.DisplayListHandle);
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
            TextRenderer.DrawText("Buf", Color.Black, new PointF(0, 0));
            ComponentDraw DrawCircuit = Circuit[9, 8];
            Disable8Bit = new ComponentDraw(GL.GenLists(1), DrawCircuit.Width, DrawCircuit.Height, DrawCircuit.Terminals.Length);
            for (int i = 0; i < DrawCircuit.Terminals.Length; i++) {
                Disable8Bit.Terminals[i] = DrawCircuit.Terminals[i];
                if (i < 8) {
                    Disable8Bit.TerminalsString[i] = ("in" + (i % 8));
                } else if (i > 8) {
                    Disable8Bit.TerminalsString[i] = ("out"+((i-1) % 8));
                } else {
                    Disable8Bit.TerminalsString[i] = "Enable";
                }
            }
            GL.NewList(Disable8Bit.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("Buf", Color.Black, new PointF(0, 0));
            GL.CallList(DrawCircuit.DisplayListHandle);

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
            TFlipFlop = new ComponentDraw(GL.GenLists(1), 60, 40, 4);
            TFlipFlop.TerminalsString[0] = "T";
            TFlipFlop.TerminalsString[1] = "CLK";
            TFlipFlop.TerminalsString[2] = "Q";
            TFlipFlop.TerminalsString[3] = "Q'";
            TFlipFlop.Terminals[0] = new Point(-30, 10);
            TFlipFlop.Terminals[1] = new Point(-30, -10);

            TFlipFlop.Terminals[2] = new Point(30, 10);
            TFlipFlop.Terminals[3] = new Point(30, -10);

            GL.NewList(TFlipFlop.DisplayListHandle, ListMode.Compile);
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
        private static void GenRSFlipFlop() {
            TextRenderer.DrawText("SR", Color.Black, new PointF(0, 0));
            RSFlipFlop = new ComponentDraw(GL.GenLists(1), 60, 60, 7);
            RSFlipFlop.TerminalsString[0] = "S";
            RSFlipFlop.TerminalsString[1] = "R";
            RSFlipFlop.TerminalsString[2] = "CLK";
            RSFlipFlop.TerminalsString[3] = "Force set";
            RSFlipFlop.TerminalsString[4] = "Force reset";
            RSFlipFlop.TerminalsString[5] = "Q";
            RSFlipFlop.TerminalsString[6] = "Q'";
            RSFlipFlop.Terminals[0] = new Point(-30, 10);
            RSFlipFlop.Terminals[1] = new Point(-30, -10);
            RSFlipFlop.Terminals[2] = new Point(-30, 0);
            RSFlipFlop.Terminals[3] = new Point(0, 30);
            RSFlipFlop.Terminals[4] = new Point(0, -30);

            RSFlipFlop.Terminals[5] = new Point(30, 10);
            RSFlipFlop.Terminals[6] = new Point(30, -10);

            GL.NewList(RSFlipFlop.DisplayListHandle, ListMode.Compile);
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
        private static void GenJKFlipFlop() {
            TextRenderer.DrawText("JK", Color.Black, new PointF(0, 0));
            JKFlipFlop = new ComponentDraw(GL.GenLists(1), 60, 60, 7);
            JKFlipFlop.TerminalsString[0] = "J";
            JKFlipFlop.TerminalsString[1] = "K";
            JKFlipFlop.TerminalsString[2] = "CLK";
            JKFlipFlop.TerminalsString[3] = "S";
            JKFlipFlop.TerminalsString[4] = "R";
            JKFlipFlop.TerminalsString[5] = "Q";
            JKFlipFlop.TerminalsString[6] = "Q'";
            JKFlipFlop.Terminals[0] = new Point(-30, 10);
            JKFlipFlop.Terminals[1] = new Point(-30, -10);
            JKFlipFlop.Terminals[2] = new Point(-30, 0);
            JKFlipFlop.Terminals[3] = new Point(0, 30);
            JKFlipFlop.Terminals[4] = new Point(0, -30);

            JKFlipFlop.Terminals[5] = new Point(30, 10);
            JKFlipFlop.Terminals[6] = new Point(30, -10);

            GL.NewList(JKFlipFlop.DisplayListHandle, ListMode.Compile);
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
            GL.Color3(Color_off);
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
            GL.Color3(Color_on);
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
            GL.Color3(Color_on);
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
            GL.Color3(Color_on);
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
            GL.Color3(Color_on);
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
            GL.Color3(Color_on);
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
            GL.Color3(Color_on);
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
            GL.Color3(Color_on);
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
            GL.Color3(Color_on);
            GL.Vertex2(24, -33);
            GL.Vertex2(24, -38);
            GL.Vertex2(29, -38);
            GL.Vertex2(29, -33);
            GL.Vertex2(24, -33);
            GL.End();
            GL.EndList();
            
            Display7SegBase = new ComponentDraw(GL.GenLists(1), 60, 80, 8);
            for (int i = 0; i < 8; i++) {
                Display7SegBase.Terminals[i] = new Point(-30, -i * 10 + 35);
                if(i < 7)
                    Display7SegBase.TerminalsString[i] = ((char)('a' + i)).ToString();
                else
                    Display7SegBase.TerminalsString[i] = "dot";
            }
            GL.NewList(Display7SegBase.DisplayListHandle, ListMode.Compile);
            GL.Color3(Color_off);
            GL.Begin(PrimitiveType.Lines);
            for (int i = 0; i < 8; i++) {
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
            ComponentDraw DrawCircuit = Circuit[5, 7];
            BinTo7Seg = new ComponentDraw(GL.GenLists(1), DrawCircuit.Width, DrawCircuit.Height, DrawCircuit.Terminals.Length);
            for (int i = 0; i < DrawCircuit.Terminals.Length; i++) {
                BinTo7Seg.Terminals[i] = DrawCircuit.Terminals[i];
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
            GL.CallList(DrawCircuit.DisplayListHandle);

            GL.EndList();



        }
        private static void GenMicrocontroller() {
            TextRenderer.DrawText("M+++", Color.Black, new PointF(0, 0));
            ComponentDraw DrawCircuit = Circuit[32, 32];
            Microcontroller = new ComponentDraw(GL.GenLists(1), DrawCircuit.Width, DrawCircuit.Height, DrawCircuit.Terminals.Length);
            for (int i = 0; i < DrawCircuit.Terminals.Length; i++) {
                Microcontroller.Terminals[i] = DrawCircuit.Terminals[i];
                if(i < 8 * 4) {
                    Microcontroller.TerminalsString[i] = ("in" + (i / 8) + "[" + (i % 8) + "]");
                } else {
                    Microcontroller.TerminalsString[i] = ("out" + ((i / 8)-4) + "[" + (i % 8) + "]");
                }
            }
            GL.NewList(Microcontroller.DisplayListHandle, ListMode.Compile);
            TextRenderer.DrawText("M+++", Color.Black, new PointF(0, 0));
            GL.CallList(DrawCircuit.DisplayListHandle);

            GL.EndList();
        }
        private static void GenKeyboard() {
            Keyboard = new ComponentDraw(GL.GenLists(1), 90, 50, 8);
            for (int i = 0; i < Keyboard.Terminals.Length; i++) {
                Keyboard.Terminals[i] = new Point(-Keyboard.Width / 2+i*10+10, Keyboard.Height / 2);
            }

            GL.NewList(Keyboard.DisplayListHandle, ListMode.Compile);
            GL.Color3(Color_off);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-Keyboard.Width / 2, -Keyboard.Height / 2);
            GL.Vertex2(Keyboard.Width / 2, -Keyboard.Height / 2);
            GL.Vertex2(Keyboard.Width / 2, Keyboard.Height / 2 - 10);
            GL.Vertex2(-Keyboard.Width / 2, Keyboard.Height / 2 - 10);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            for (int i = 0; i < Keyboard.Terminals.Length; i++) {
                GL.Vertex2(-Keyboard.Width / 2 + i * 10 + 10, Keyboard.Height / 2);
                GL.Vertex2(-Keyboard.Width / 2 + i * 10 + 10, Keyboard.Height / 2-10);
            }
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            for (int y = 0; y < 3; y++) {
                for (int x = 0; x < 8; x++) {
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
            int total = 8;
            Vector2[] halfCircle1 = new Vector2[total];
            Vector2[] halfCircle2 = new Vector2[total];
            Vector2[] halfCircle3 = new Vector2[total];
            Vector2[] circle = new Vector2[total];
            for (int i = 0; i < total; i++) {
                double RotDegree = Math.PI * ((360d / ((total + 1) * 2d)) * (i + 1)) / 180.0;
                double RotDegree2 = Math.PI * ((360d / total) * i) / 180.0;
                double x = 0;
                double y = -10;
                halfCircle1[i] = new Vector2();
                halfCircle1[i].X = (float)(x * Math.Cos(RotDegree) - y * Math.Sin(RotDegree)) * 2;
                halfCircle1[i].Y = (float)(x * Math.Sin(RotDegree) + y * Math.Cos(RotDegree));
                halfCircle1[i].X -= 10;
                halfCircle2[i] = new Vector2();
                halfCircle2[i].X = (float)(x * Math.Cos(RotDegree) - y * Math.Sin(RotDegree)) / 2;
                halfCircle2[i].Y = (float)(x * Math.Sin(RotDegree) + y * Math.Cos(RotDegree));
                halfCircle2[i].X -= 15;
                halfCircle3[i] = new Vector2();
                halfCircle3[i].X = (float)(x * Math.Cos(RotDegree) - y * Math.Sin(RotDegree)) / 2;
                halfCircle3[i].Y = (float)(x * Math.Sin(RotDegree) + y * Math.Cos(RotDegree));
                halfCircle3[i].X -= 17;
                x = 3;
                y = 0;
                circle[i].X = (float)(x * Math.Cos(RotDegree2) - y * Math.Sin(RotDegree2));
                circle[i].Y = (float)(x * Math.Sin(RotDegree2) + y * Math.Cos(RotDegree2));
                circle[i].X += 13;
            }
            Xnor = new ComponentDraw[2];
            Xnor[0] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Xnor[0].Terminals[0] = new Point(-20, 5);
            Xnor[0].Terminals[1] = new Point(-20, -5);
            Xnor[0].Terminals[2] = new Point(20, 0);
            GL.NewList(Xnor[0].DisplayListHandle, ListMode.Compile);
            GL.Color3(Color_off);
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(halfCircle2[i]);
            }
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (int i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineStrip);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(halfCircle3[i]);
            }
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
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
            GL.Color3(Color_on);
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(halfCircle2[i]);
            }
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (int i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineStrip);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(halfCircle3[i]);
            }
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
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

            int total = 8;
            Vector2[] halfCircle1 = new Vector2[total];
            Vector2[] halfCircle2 = new Vector2[total];
            Vector2[] halfCircle3 = new Vector2[total];
            for (int i = 0; i < total; i++) {
                double RotDegree = Math.PI * ((360d / ((total+1) * 2d)) * (i+1)) / 180.0;
                double x = 0;
                double y = -10;
                halfCircle1[i] = new Vector2();
                halfCircle1[i].X = (float)(x * Math.Cos(RotDegree) - y * Math.Sin(RotDegree)) * 2;
                halfCircle1[i].Y = (float)(x * Math.Sin(RotDegree) + y * Math.Cos(RotDegree));
                halfCircle1[i].X -= 10;
                halfCircle2[i] = new Vector2();
                halfCircle2[i].X = (float)(x * Math.Cos(RotDegree) - y * Math.Sin(RotDegree)) / 2;
                halfCircle2[i].Y = (float)(x * Math.Sin(RotDegree) + y * Math.Cos(RotDegree));
                halfCircle2[i].X -= 15;
                halfCircle3[i] = new Vector2();
                halfCircle3[i].X = (float)(x * Math.Cos(RotDegree) - y * Math.Sin(RotDegree)) / 2;
                halfCircle3[i].Y = (float)(x * Math.Sin(RotDegree) + y * Math.Cos(RotDegree));
                halfCircle3[i].X -= 17;
            }
            Xor = new ComponentDraw[2];
            Xor[0] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Xor[0].Terminals[0] = new Point(-20, 5);
            Xor[0].Terminals[1] = new Point(-20, -5);
            Xor[0].Terminals[2] = new Point(20, 0);
            GL.NewList(Xor[0].DisplayListHandle, ListMode.Compile);
            GL.Color3(Color_off);
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(halfCircle2[i]);
            }
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (int i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineStrip);
            for (int i = 0; i < total; i++) {
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
            GL.Color3(Color_on);
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(halfCircle2[i]);
            }
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (int i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineStrip);
            for (int i = 0; i < total; i++) {
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
            int total = 8;
            Vector2[] halfCircle1 = new Vector2[total];
            Vector2[] circle = new Vector2[total];
            for (int i = 0; i < total; i++) {
                double RotDegree = Math.PI * ((360d / ((total+1) * 2d)) * (i + 1)) / 180.0;
                double RotDegree2 = Math.PI * ((360d / total) * i) / 180.0;
                double x = 0;
                double y = -10;
                halfCircle1[i] = new Vector2();
                halfCircle1[i].X = (float)(x * Math.Cos(RotDegree) - y * Math.Sin(RotDegree));
                halfCircle1[i].Y = (float)(x * Math.Sin(RotDegree) + y * Math.Cos(RotDegree));
                x = 3;
                y = 0;
                circle[i].X = (float)(x * Math.Cos(RotDegree2) - y * Math.Sin(RotDegree2));
                circle[i].Y = (float)(x * Math.Sin(RotDegree2) + y * Math.Cos(RotDegree2));
                circle[i].X += 13;
            }
            Nand = new ComponentDraw[2];
            Nand[0] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Nand[0].Terminals[0] = new Point(-20, 5);
            Nand[0].Terminals[1] = new Point(-20, -5);
            Nand[0].Terminals[2] = new Point(20, 0);
            GL.NewList(Nand[0].DisplayListHandle, ListMode.Compile);
            GL.Color3(Color_off);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-15, 10);
            GL.Vertex2(0, 10);
            for (int i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(0, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
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
            GL.Color3(Color_on);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (int i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
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
            int total = 8;
            Vector2[] halfCircle1 = new Vector2[total];
            for (int i = 0; i < total; i++) {
                double RotDegree = Math.PI * ((360d / ((total + 1) * 2d)) * (i + 1)) / 180.0;
                double x = 0;
                double y = -10;
                halfCircle1[i] = new Vector2();
                halfCircle1[i].X = (float)(x * Math.Cos(RotDegree) - y * Math.Sin(RotDegree));
                halfCircle1[i].Y = (float)(x * Math.Sin(RotDegree) + y * Math.Cos(RotDegree));
            }
            And = new ComponentDraw[2];
            And[0] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            And[0].Terminals[0] = new Point(-20, 5);
            And[0].Terminals[1] = new Point(-20, -5);
            And[0].Terminals[2] = new Point(20, 0);
            GL.NewList(And[0].DisplayListHandle, ListMode.Compile);
            GL.Color3(Color_off);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-15, 10);
            GL.Vertex2(0, 10);
            for (int i = total - 1; i >= 0; i--) {
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
            GL.Color3(Color_on);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (int i = total - 1; i >= 0; i--) {
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
            int total = 8;
            Vector2[] halfCircle1 = new Vector2[total];
            Vector2[] halfCircle2 = new Vector2[total];
            Vector2[] circle = new Vector2[total];
            for (int i = 0; i < total; i++) {
                double RotDegree = Math.PI * ((360d / ((total + 1) * 2d)) * (i + 1)) / 180.0;
                double RotDegree2 = Math.PI * ((360d / total) * i) / 180.0;
                double x = 0;
                double y = -10;
                halfCircle1[i] = new Vector2();
                halfCircle1[i].X = (float)(x * Math.Cos(RotDegree) - y * Math.Sin(RotDegree)) * 2;
                halfCircle1[i].Y = (float)(x * Math.Sin(RotDegree) + y * Math.Cos(RotDegree));
                halfCircle1[i].X -= 10;
                halfCircle2[i] = new Vector2();
                halfCircle2[i].X = (float)(x * Math.Cos(RotDegree) - y * Math.Sin(RotDegree)) / 2;
                halfCircle2[i].Y = (float)(x * Math.Sin(RotDegree) + y * Math.Cos(RotDegree));
                halfCircle2[i].X -= 15;
                x = 3;
                y = 0;
                circle[i].X = (float)(x * Math.Cos(RotDegree2) - y * Math.Sin(RotDegree2));
                circle[i].Y = (float)(x * Math.Sin(RotDegree2) + y * Math.Cos(RotDegree2));
                circle[i].X += 13;
            }
            Nor = new ComponentDraw[2];
            Nor[0] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Nor[0].Terminals[0] = new Point(-20, 5);
            Nor[0].Terminals[1] = new Point(-20, -5);
            Nor[0].Terminals[2] = new Point(20, 0);
            GL.NewList(Nor[0].DisplayListHandle, ListMode.Compile);
            GL.Color3(Color_off);
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(halfCircle2[i]);
            }
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (int i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
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
            GL.Color3(Color_on);
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(halfCircle2[i]);
            }
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (int i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
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
            int total = 8;
            Vector2[] halfCircle1 = new Vector2[total];
            Vector2[] halfCircle2 = new Vector2[total];
            for (int i = 0; i < total; i++) {
                double RotDegree = Math.PI * ((360d / ((total + 1) * 2d)) * (i+1)) / 180.0;
                double x = 0;
                double y = -10;
                halfCircle1[i] = new Vector2();
                halfCircle1[i].X = (float)(x * Math.Cos(RotDegree) - y * Math.Sin(RotDegree))*2;
                halfCircle1[i].Y = (float)(x * Math.Sin(RotDegree) + y * Math.Cos(RotDegree));
                halfCircle1[i].X -= 10;
                halfCircle2[i] = new Vector2();
                halfCircle2[i].X = (float)(x * Math.Cos(RotDegree) - y * Math.Sin(RotDegree))/2;
                halfCircle2[i].Y = (float)(x * Math.Sin(RotDegree) + y * Math.Cos(RotDegree));
                halfCircle2[i].X -= 15;
            }
            Or = new ComponentDraw[2];
            Or[0] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Or[0].Terminals[0] = new Point(-20, 5);
            Or[0].Terminals[1] = new Point(-20, -5);
            Or[0].Terminals[2] = new Point(20, 0);
            GL.NewList(Or[0].DisplayListHandle, ListMode.Compile);
            GL.Color3(Color_off);
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(halfCircle2[i]);
            }
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (int i = total-1; i >= 0 ; i--) {
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
            GL.Color3(Color_on);
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(halfCircle2[i]);
            }
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (int i = total - 1; i >= 0; i--) {
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
            int total = 8;
            Vector2[] circle = new Vector2[total];
            for (int i = 0; i < total; i++) {
                double RotDegree = Math.PI * ((360d / total) * i) / 180.0;
                double x = 3;
                double y = 0;
                circle[i] = new Vector2();
                circle[i].X = (float)(x * Math.Cos(RotDegree) - y * Math.Sin(RotDegree));
                circle[i].Y = (float)(x * Math.Sin(RotDegree) + y * Math.Cos(RotDegree));
                circle[i].X += 13;
            }
            Not = new ComponentDraw[3];
            Not[0] = new ComponentDraw(GL.GenLists(1), 40, 20, 2);
            Not[0].Terminals[0] = new Point(-20, 0);
            Not[0].Terminals[1] = new Point(20, 0);
            GL.NewList(Not[0].DisplayListHandle, ListMode.Compile);

            GL.Color3(Color_off);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(10, 0);
            GL.Vertex2(-10, 10);
            GL.Vertex2(-10, -10);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-10, 0);
            GL.Vertex2(-20, 0);
            GL.Color3(Color_on);
            GL.Vertex2(16, 0);
            GL.Vertex2(20, 0);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(circle[i]);
            }
            GL.End();

            GL.EndList();

            Not[1] = new ComponentDraw(GL.GenLists(1), 40, 20, 2);
            Not[1].Terminals[0] = new Point(-20, 0);
            Not[1].Terminals[1] = new Point(20, 0);
            GL.NewList(Not[1].DisplayListHandle, ListMode.Compile);

            GL.Color3(Color_on);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(10, 0);
            GL.Vertex2(-10, 10);
            GL.Vertex2(-10, -10);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-10, 0);
            GL.Vertex2(-20, 0);
            GL.Color3(Color_off);
            GL.Vertex2(16, 0);
            GL.Vertex2(20, 0);
            GL.End();
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
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

            GL.Color3(Color_off);
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
            GL.Color3(Color_on);
            GL.Vertex2(0, 5);
            GL.Vertex2(0, 10);
            GL.End();

            GL.EndList();


            Disable[1] = new ComponentDraw(GL.GenLists(1), 40, 20, 3);
            Disable[1].Terminals[0] = new Point(-20, 0);
            Disable[1].Terminals[1] = new Point(0, 10);
            Disable[1].Terminals[2] = new Point(20, 0);
            GL.NewList(Disable[1].DisplayListHandle, ListMode.Compile);

            GL.Color3(Color_on);
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

            GL.Color3(Color_3rd);
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
            GL.Color3(Color_off);
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
            int total = 8;
            Vector2[] outerVertexes = new Vector2[total];
            Vector2[] innerVertexes = new Vector2[total];
            for (int i = 0; i < total; i++) {
                double RotDegree = Math.PI * ((360d / total) * i) / 180.0;
                double x = 5;
                double y = 0;
                outerVertexes[i] = new Vector2();
                outerVertexes[i].X = (float)(x * Math.Cos(RotDegree) - y * Math.Sin(RotDegree));
                outerVertexes[i].Y = (float)(x * Math.Sin(RotDegree) + y * Math.Cos(RotDegree));
                innerVertexes[i] = new Vector2();
                x = 3;
                y = 0;
                innerVertexes[i].X = (float)(x * Math.Cos(RotDegree) - y * Math.Sin(RotDegree));
                innerVertexes[i].Y = (float)(x * Math.Sin(RotDegree) + y * Math.Cos(RotDegree));
            }
            Output = new ComponentDraw[2];
            Output[0] = new ComponentDraw(GL.GenLists(1), 15, 10, 1);
            Output[0].Terminals[0] = new Point(-10, 0);
            GL.NewList(Output[0].DisplayListHandle, ListMode.Compile);
            GL.Color3(Color_off);
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(outerVertexes[i]);
            }
            GL.End();

            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color3(Color_off);
            for (int i = 0; i < total; i++) {
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
            GL.Color3(Color_off);
            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(outerVertexes[i]);
            }
            GL.End();
            GL.Color3(Color_on);
            GL.Begin(PrimitiveType.TriangleFan);
            for (int i = 0; i < total; i++) {
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
            GL.Color3(Color_off);
            GL.Vertex2(-5, -5);
            GL.Vertex2(-5, 5);
            GL.Vertex2(5, 5);
            GL.Vertex2(5, -5);
            GL.End();
            
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color_off);
            GL.Vertex2(-3, -3);
            GL.Vertex2(-3, 3);
            GL.Vertex2(3, 3);
            GL.Vertex2(3, -3);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color_off);
            GL.Vertex2(5, 0);
            GL.Vertex2(10, 0);
            GL.End();
            
            GL.EndList();

            Input[1] = new ComponentDraw(GL.GenLists(1), 15, 10, 1);
            Input[1].Terminals[0] = new Point(10, 0);
            GL.NewList(Input[1].DisplayListHandle, ListMode.Compile);

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(Color_off);
            GL.Vertex2(-5, -5);
            GL.Vertex2(-5, 5);
            GL.Vertex2(5, 5);
            GL.Vertex2(5, -5);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color_on);
            GL.Vertex2(-3, -3);
            GL.Vertex2(-3, 3);
            GL.Vertex2(3, 3);
            GL.Vertex2(3, -3);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color_on);
            GL.Vertex2(5, 0);
            GL.Vertex2(10, 0);
            GL.End();

            GL.EndList();
        }

    }

    public class ComponentDraw {
        public int DisplayListHandle;
        public int Width;
        public int Height;
        public Point[] Terminals;
        public string[] TerminalsString;

        public ComponentDraw(int displayListHandle, int width, int height, int terminals = 1) {
            DisplayListHandle = displayListHandle;
            Width = width;
            Height = height;
            Terminals = new Point[terminals];
            TerminalsString = new string[terminals];
        }
    }
    public class Component {
        public ComponentDraw Draw;
        public PointF Center;
        public float Rotation = 0;
        public ComponentType Type = ComponentType.None;
        public Component RootComponent;
        public int[] ExtraHandlers; //Used to display things on the component
        public bool[] ActiveExtraHandlers;

        public override string ToString() {
            string roots = "";
            Component root = RootComponent;
            while(root != null) {
                roots += root.Type.ToString() + ".";
                root = root.RootComponent;
            }
            return roots+Type.ToString() + " in X: " + Center.X + ", Y: "+ Center.Y;
        }

        public Component(ComponentDraw draw, PointF center, Component rootComponent = null) {
            Draw = draw;
            Center = center;
            if (rootComponent == null)
                RootComponent = UIStatics.Circuito.InsideComponent;
            else
                RootComponent = rootComponent;

            if (draw.DisplayListHandle == Draws.And[0].DisplayListHandle ||
                    draw.DisplayListHandle == Draws.And[1].DisplayListHandle) {
                Type = ComponentType.And;
            } else if (draw.DisplayListHandle == Draws.Nand[0].DisplayListHandle ||
                        draw.DisplayListHandle == Draws.Nand[1].DisplayListHandle) {
                Type = ComponentType.Nand;
            } else if (draw.DisplayListHandle == Draws.Or[0].DisplayListHandle ||
                        draw.DisplayListHandle == Draws.Or[1].DisplayListHandle) {
                Type = ComponentType.Or;
            } else if (draw.DisplayListHandle == Draws.Nor[0].DisplayListHandle ||
                        draw.DisplayListHandle == Draws.Nor[1].DisplayListHandle) {
                Type = ComponentType.Nor;
            } else if (draw.DisplayListHandle == Draws.Xor[0].DisplayListHandle ||
                        draw.DisplayListHandle == Draws.Xor[1].DisplayListHandle) {
                Type = ComponentType.Xor;
            } else if (draw.DisplayListHandle == Draws.Xnor[0].DisplayListHandle ||
                        draw.DisplayListHandle == Draws.Xnor[1].DisplayListHandle) {
                Type = ComponentType.Xnor;
            } else if (draw.DisplayListHandle == Draws.Not[0].DisplayListHandle ||
                        draw.DisplayListHandle == Draws.Not[1].DisplayListHandle) {
                Type = ComponentType.Not;
            } else if (draw.DisplayListHandle == Draws.Input[0].DisplayListHandle ||
                         draw.DisplayListHandle == Draws.Input[1].DisplayListHandle) {
                Type = ComponentType.Input;
            } else if (draw.DisplayListHandle == Draws.Output[0].DisplayListHandle ||
                         draw.DisplayListHandle == Draws.Output[1].DisplayListHandle) {
                Type = ComponentType.Output;
            } else if (draw.DisplayListHandle == Draws.Keyboard.DisplayListHandle) {
                Type = ComponentType.Keyboard;
            } else if (draw.DisplayListHandle == Draws.Microcontroller.DisplayListHandle) {
                Type = ComponentType.Microcontroller;
            } else if (draw.DisplayListHandle == Draws.Disable[0].DisplayListHandle ||
                        draw.DisplayListHandle == Draws.Disable[1].DisplayListHandle ||
                        draw.DisplayListHandle == Draws.Disable[2].DisplayListHandle) {
                Type = ComponentType.Disable;
            } else if(draw.DisplayListHandle == Draws.Display7SegBase.DisplayListHandle) {
                Type = ComponentType.Display7Seg;
                ExtraHandlers = Draws.Display7SegPart;
                ActiveExtraHandlers = new bool[ExtraHandlers.Length];
            } else if (draw.DisplayListHandle == Draws.Osciloscope.DisplayListHandle) {
                Type = ComponentType.Osciloscope;
            } else if (draw.DisplayListHandle == Draws.JKFlipFlop.DisplayListHandle) {
                Type = ComponentType.JKFlipFlop;
            } else if (draw.DisplayListHandle == Draws.RSFlipFlop.DisplayListHandle) {
                Type = ComponentType.RSFlipFlop;
            } else if (draw.DisplayListHandle == Draws.TFlipFlop.DisplayListHandle) {
                Type = ComponentType.TFlipFlop;
            } else if (draw.DisplayListHandle == Draws.DFlipFlop.DisplayListHandle) {
                Type = ComponentType.DFlipFlop;
            } else if (draw.DisplayListHandle == Draws.Disable8Bit.DisplayListHandle) {
                Type = ComponentType.Disable8Bit;
            } else if (draw.DisplayListHandle == Draws.FullAdder.DisplayListHandle) {
                Type = ComponentType.FullAdder;
            } else if (draw.DisplayListHandle == Draws.HalfAdder.DisplayListHandle) {
                Type = ComponentType.HalfAdder;
            } else if (draw.DisplayListHandle == Draws.RamMemory.DisplayListHandle) {
                Type = ComponentType.RamMemory;
            } else if (draw.DisplayListHandle == Draws.RomMemory.DisplayListHandle) {
                Type = ComponentType.RomMemory;
            } else if (draw.DisplayListHandle == Draws.ULA.DisplayListHandle) {
                Type = ComponentType.ULA;
            } else if (draw.DisplayListHandle == Draws.Registrer8Bit.DisplayListHandle) {
                Type = ComponentType.Registrer8Bit;
            } else if (draw.DisplayListHandle == Draws.Registrer8BitSG.DisplayListHandle) {
                Type = ComponentType.Registrer8BitSG;
            } else if (draw.DisplayListHandle == Draws.ControlModule.DisplayListHandle) {
                Type = ComponentType.ControlModule;
            } else if (draw.DisplayListHandle == Draws.BinTo7Seg.DisplayListHandle) {
                Type = ComponentType.BinTo7Seg;
            } else if (draw.DisplayListHandle == Draws.PortBank.DisplayListHandle) {
                Type = ComponentType.PortBank;
            } else {
                throw new NotImplementedException();
            }

        }

        public bool IsInside(PointF Point) {
            RectangleF rectangle = new RectangleF(new PointF(Center.X- Draw.Width/2f, Center.Y - Draw.Height / 2f), new SizeF(Draw.Width, Draw.Height));
            rectangle.Inflate(2, 2);
            return rectangle.Contains(Point);
        }

        public int IsOnTerminal(PointF Point) {
            for (int i = 0; i < Draw.Terminals.Length; i++) {
                PointF terminal = TransformTerminal(i);
                float distance = (Center.X + terminal.X - Point.X) * (Center.X + terminal.X - Point.X) + (Center.Y + terminal.Y - Point.Y) * (Center.Y + terminal.Y - Point.Y);
                if(distance <= 4*4) {
                    return i;
                }
            }
            
            return -1;
        }

        public PointF TransformTerminal(int i) {
            double RotDegree = Math.PI * Rotation / 180.0;
            PointF point = new PointF(
                (float)(Draw.Terminals[i].X * Math.Cos(RotDegree) - Draw.Terminals[i].Y * Math.Sin(RotDegree)),
                (float)(Draw.Terminals[i].X * Math.Sin(RotDegree) + Draw.Terminals[i].Y * Math.Cos(RotDegree))
            );
            return point;
        }

    }
    [Serializable]
    public enum ComponentType {
        None, Input, Output, Disable, Not, And, Nand, Or, Nor, Xor, Xnor, Keyboard, Display7Seg, BinTo7Seg, Circuit,
        Microcontroller, Osciloscope, BlackTerminal, JKFlipFlop, RSFlipFlop, DFlipFlop, TFlipFlop,
        HalfAdder, FullAdder, ULA, ControlModule, Registrer8Bit, Registrers, Tristate, Stack, RamMemory,
        RomMemory, PortBank, Registrer8BitSG, LedMatrix, Disable8Bit
    }

    public class Wire {
        public PointF From;
        public Component FromComponent;
        public int FromIndex;
        public Color Color = Color.Black;
        public PointF To;
        public Component ToComponent;
        public int ToIndex;
        public Component RootComponent;

        public Wire(PointF from, PointF to, Component rootComponent = null) {
            From = from;
            To = to;
            if (rootComponent == null)
                RootComponent = UIStatics.Circuito.InsideComponent;
            else
                RootComponent = rootComponent;
        }
        public Wire(Component from, int indexFrom, Component to, int indexTo, Component rootComponent = null) {
            FromComponent = from;
            FromIndex = indexFrom;
            ToComponent = to;
            ToIndex = indexTo;
            From = from.TransformTerminal(indexFrom);
            From.X += from.Center.X;
            From.Y += from.Center.Y;
            To = to.TransformTerminal(indexTo);
            To.X += to.Center.X;
            To.Y += to.Center.Y;
            if (rootComponent == null)
                RootComponent = UIStatics.Circuito.InsideComponent;
            else
                RootComponent = rootComponent;
        }
        public Wire(Component from, int indexFrom, PointF to, Component rootComponent = null) {
            FromComponent = from;
            FromIndex = indexFrom;
            From = from.TransformTerminal(indexFrom);
            From.X += from.Center.X;
            From.Y += from.Center.Y;
            To = to;
            if (rootComponent == null)
                RootComponent = UIStatics.Circuito.InsideComponent;
            else
                RootComponent = rootComponent;
        }
        public Wire(PointF from, Component to, int indexTo, Component rootComponent = null) {
            From = from;
            ToComponent = to;
            ToIndex = indexTo;
            To = to.TransformTerminal(indexTo);
            To.X += to.Center.X;
            To.Y += to.Center.Y;
            if (rootComponent == null)
                RootComponent = UIStatics.Circuito.InsideComponent;
            else
                RootComponent = rootComponent;
        }
    }
    public struct Terminals {
        public int FromIndex;
        public Component FromComponent;
        public int ToIndex;
        public Component ToComponent;
        public int HoverIndex;
        public Component HoverComponent;
        public bool HoverNotComponent;
        public PointF Hover;
        public bool FromNotComponent;
        public PointF From;
        public bool ToNotComponent;
        public PointF To;
    }
    public struct MouseProps {
        public Point CurrentPosition;
        public Point LastPosition;
        public Point LastClickPosition;
        public Point LastDoubleClickPosition;
        public Point LastDownPosition;
        public Point LastUpPosition;
        public int Delta;
        public bool Button1Pressed;
        public bool Button2Pressed;

        public static PointF ToWorld(Point Point, Size ClientSize, PointF Position, float Zoom) {
            PointF worldPos = new PointF();

            worldPos.X = Point.X - ClientSize.Width / 2f;
            worldPos.Y = - Point.Y + ClientSize.Height/ 2f;

            worldPos.X = worldPos.X / Zoom;
            worldPos.Y = worldPos.Y / Zoom;

            worldPos.X += Position.X;
            worldPos.Y += Position.Y;

            return worldPos;
        }
    }
    public struct ExtraTerminal {
        public PointF Point;
        public Component Root;
        public ExtraTerminal(PointF point, Component root) {
            Point = point;
            Root = root;
        }
    }
}

    