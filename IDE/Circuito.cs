using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using CircuitSimulator;
using System.Threading;



namespace IDE {
    public partial class Circuito : GLControl {
        public MouseProps MouseProps = new MouseProps();
        public Terminals Terminals = new Terminals();
        public List<Wire> Wires = new List<Wire>();
        public Wire HoverWire;
        public Wire SelectedWire;
        public Component Over;
        public Component Selected;
        private bool canDrag = false;
        private Circuit Circuit;
        private Thread thread;
        public PointF Position = new PointF();
        public List<Component> Components = new List<Component>();
        public List<CircuitComponentToDrawComponent> CircuitComponentToDrawComponents = new List<CircuitComponentToDrawComponent>();
        public List<CircuitWireToDrawWire> CircuitWireToDrawWire = new List<CircuitWireToDrawWire>();
        public bool Running = false;
        public Circuito() {
            InitializeComponent();
        }

        public void Run() {
            if(Circuit == null) {
                MountCircuit();
            }
            Running = true;
        }
        public void Pause() {
            Running = false;
        }
        public void Tick() {
            if (Circuit == null) {
                MountCircuit();
            }
            Circuit.Tick();
        }
        public void Stop() {
            Running = false;
            Circuit = null;
        }

        private void MountCircuit() {
            Circuit = new Circuit();
            CircuitComponentToDrawComponents.Clear();
            CircuitWireToDrawWire.Clear();

            foreach (Component item in Components) {
                if (item.Draw.DisplayListHandle == Draws.And[0].DisplayListHandle ||
                    item.Draw.DisplayListHandle == Draws.And[1].DisplayListHandle) {
                    CircuitComponentToDrawComponents.Add( 
                        new CircuitComponentToDrawComponent(
                            Circuit.AddComponent(new AndGate()),
                            item));
                } else if (item.Draw.DisplayListHandle == Draws.Nand[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Nand[1].DisplayListHandle) {
                    CircuitComponentToDrawComponents.Add(
                        new CircuitComponentToDrawComponent(
                            Circuit.AddComponent(new NandGate()),
                            item));
                } else if (item.Draw.DisplayListHandle == Draws.Or[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Or[1].DisplayListHandle) {
                    CircuitComponentToDrawComponents.Add(
                        new CircuitComponentToDrawComponent(
                            Circuit.AddComponent(new OrGate()),
                            item));
                } else if (item.Draw.DisplayListHandle == Draws.Nor[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Nor[1].DisplayListHandle) {
                    CircuitComponentToDrawComponents.Add(
                        new CircuitComponentToDrawComponent(
                            Circuit.AddComponent(new NorGate()),
                            item));
                } else if (item.Draw.DisplayListHandle == Draws.Xor[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Xor[1].DisplayListHandle) {
                    CircuitComponentToDrawComponents.Add(
                        new CircuitComponentToDrawComponent(
                            Circuit.AddComponent(new XorGate()),
                            item));
                } else if (item.Draw.DisplayListHandle == Draws.Xnor[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Xnor[1].DisplayListHandle) {
                    CircuitComponentToDrawComponents.Add(
                        new CircuitComponentToDrawComponent(
                            Circuit.AddComponent(new XnorGate()),
                            item));
                } else if (item.Draw.DisplayListHandle == Draws.Not[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Not[1].DisplayListHandle) {
                    CircuitComponentToDrawComponents.Add(
                        new CircuitComponentToDrawComponent(
                            Circuit.AddComponent(new NotGate()),
                            item));
                } else if (item.Draw.DisplayListHandle == Draws.Input[0].DisplayListHandle ||
                             item.Draw.DisplayListHandle == Draws.Input[1].DisplayListHandle) {
                    CircuitComponentToDrawComponents.Add(
                        new CircuitComponentToDrawComponent(
                            Circuit.AddComponent(new LogicInput()),
                            item));
                } else if (item.Draw.DisplayListHandle == Draws.Output[0].DisplayListHandle ||
                             item.Draw.DisplayListHandle == Draws.Output[1].DisplayListHandle) {
                    CircuitComponentToDrawComponents.Add(
                        new CircuitComponentToDrawComponent(
                            Circuit.AddComponent(new LogicOutput()),
                            item));
                } else if (item.Draw.DisplayListHandle == Draws.Disable[0].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Disable[1].DisplayListHandle ||
                            item.Draw.DisplayListHandle == Draws.Disable[2].DisplayListHandle) {
                    throw new NotImplementedException();
                } else {
                    throw new NotImplementedException();
                }
            }
            foreach (Wire item in Wires) {
                CircuitWireToDrawWire.Add(
                    new CircuitWireToDrawWire(
                        new CircuitSimulator.Components.Wire(), item));
            }


            foreach (CircuitWireToDrawWire item in CircuitWireToDrawWire) {
                if(item.DrawWire.FromComponent != null && item.DrawWire.ToComponent != null) {

                } else if (item.DrawWire.FromComponent != null && item.DrawWire.ToComponent == null) {

                } else if (item.DrawWire.FromComponent == null && item.DrawWire.ToComponent != null) {

                } else if (item.DrawWire.FromComponent == null && item.DrawWire.ToComponent == null) {

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
            if (MouseProps.Button2Pressed) {
                Position = new PointF(Position.X+((MouseProps.LastPosition.X - MouseProps.CurrentPosition.X)/zoom), Position.Y-((MouseProps.LastPosition.Y - MouseProps.CurrentPosition.Y)/zoom));
            }
            bool foundComponent = false;
            bool foundTerminal = false;

            PointF worldMousePos = MouseProps.ToWorld(MouseProps.CurrentPosition, ClientSize, Position, zoom);
            foreach (Component item in Components) {
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

            if (MouseProps.Button1Pressed) {
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

            if (zoom > 1f) {
                GL.LineWidth(zoom);
            } else {
                GL.LineWidth(1);
            }
            foreach (Component item in Components) {
                GL.Translate(item.Center.X, item.Center.Y, 0);
                GL.Rotate(item.Rotation, 0, 0, 1);
                GL.CallList(item.Draw.DisplayListHandle);
                GL.Color3(Color.Black);
                foreach (Point terminal in item.Draw.Terminals) {
                    GL.Translate(terminal.X, terminal.Y, 0);
                    GL.CallList(Draws.TerminalHandle);
                    GL.Translate(-terminal.X, -terminal.Y, 0);
                }
                GL.Rotate(-item.Rotation, 0, 0, 1);
                GL.Translate(-item.Center.X, -item.Center.Y, 0);
            }

            if (MouseProps.Button1Pressed) {
                if(Terminals.FromComponent != null) {
                    GL.Color3(Color.Black);
                    GL.Begin(BeginMode.Lines);
                    GL.Vertex2(terminalFromVertex.X, terminalFromVertex.Y);
                    GL.Vertex2(terminalToVertex.X, terminalToVertex.Y);
                    GL.End();
                } else if (Terminals.FromNotComponent) {
                    GL.Color3(Color.Black);
                    GL.Begin(BeginMode.Lines);
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
            GL.ClearColor(BackColor);
            Application.Idle += Application_Idle;
            Draws.Load();
            /*
            Components.Add(new Component(Draws.Input[0], new PointF(0, 0)));
            Components.Add(new Component(Draws.Input[0], new PointF(40, 20)));
            Components.Add(new Component(Draws.Input[0], new PointF(200, 0)));
            Components.Add(new Component(Draws.Input[0], new PointF(0, -100)));
            Components.Add(new Component(Draws.Input[1], new PointF(100, 100)));
            Components.Add(new Component(Draws.Input[1], new PointF(100, 800)));
            Components.Add(new Component(Draws.Output[0], new PointF(20, 20)));
            Components.Add(new Component(Draws.Output[1], new PointF(20, 50)));
            */
            Components.Add(new Component(Draws.Nor[0], new PointF(-100, -100)));
            Components.Add(new Component(Draws.Or[0], new PointF(-50, -100)));
            Components.Add(new Component(Draws.Not[0], new PointF(0, -100)));
            Components.Add(new Component(Draws.Disable[0], new PointF(50, -100)));
            Components.Add(new Component(Draws.Input[0], new PointF(100, -100)));
            Components.Add(new Component(Draws.Output[0], new PointF(-100, -50)));
            Components.Add(new Component(Draws.And[0], new PointF(-50, -50)));
            Components.Add(new Component(Draws.Nand[0], new PointF(0, -50)));
            Components.Add(new Component(Draws.Xor[0], new PointF(50, -50)));
            Components.Add(new Component(Draws.Xnor[0], new PointF(100, -50)));
            Components.Add(new Component(Draws.Circuit[8,16], new PointF(200, 0)));
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

        private void Circuito_MouseDoubleClick(object sender, MouseEventArgs e) {
            MouseProps.LastDoubleClickPosition = e.Location;
            Refresh();
        }

        private void Circuito_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                MouseProps.LastClickPosition = e.Location;
            }
            Refresh();
        }

        private void Circuito_MouseDown(object sender, MouseEventArgs e) {
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

        private void Circuito_MouseMove(object sender, MouseEventArgs e) {
            MouseProps.LastPosition = MouseProps.CurrentPosition;
            MouseProps.CurrentPosition = e.Location;
            Refresh();
        }

        private void Circuito_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) {
                MouseProps.LastUpPosition = e.Location;
                if (e.Button == MouseButtons.Left) {
                    MouseProps.Button1Pressed = false;
                } else {
                    MouseProps.Button2Pressed = false;
                }
            }
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
                if (item == HoverWire) {
                    GL.Color3(Color.Green);
                } else if (item == SelectedWire) {
                    GL.Color3(Color.Orange);
                } else {
                    GL.Color3(Color.Black);
                }
                GL.Begin(BeginMode.Lines);
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
                GL.Begin(BeginMode.LineLoop);
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
                GL.Begin(BeginMode.LineLoop);
                GL.Vertex2(- Over.Draw.Width/2f - BoxPlusFactor, - Over.Draw.Height/ 2f - BoxPlusFactor);
                GL.Vertex2(- Over.Draw.Width / 2f - BoxPlusFactor, + Over.Draw.Height / 2f + BoxPlusFactor);
                GL.Vertex2(Over.Draw.Width / 2f + BoxPlusFactor, Over.Draw.Height / 2f + BoxPlusFactor);
                GL.Vertex2(Over.Draw.Width / 2f + BoxPlusFactor, - Over.Draw.Height / 2f - BoxPlusFactor);
                GL.End();
                GL.Rotate(-Over.Rotation, 0, 0, 1);
                GL.Translate(-Over.Center.X, -Over.Center.Y, 0);
            }
            
        }

        private void Circuito_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
            if(e.KeyChar == 'r') {
                if(Selected!= null) { 
                    Selected.Rotation += 90;
                    if (Selected.Rotation >= 360) Selected.Rotation -= 360;
                }
            } else if(e.KeyChar == 'd') {
                if(Selected != null) {
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
                } else if(SelectedWire != null) {
                    Wires.Remove(SelectedWire);
                    SelectedWire = null;
                    HoverWire = null;
                }
            }
            Refresh();
        }

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
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex2(-width / 2f + 10, -height / 2f);
            GL.Vertex2(width / 2f - 10, -height / 2f);
            GL.Vertex2(width / 2f - 10, height / 2f);
            GL.Vertex2(-width / 2f + 10, height / 2f);
            GL.End();
            GL.Begin(BeginMode.Lines);
            for (int i = 0; i < inputs; i++) {
                GL.Vertex2(-width / 2, height / 2 - (5 + i * 10));
                GL.Vertex2(-width / 2 + 10f, height / 2 - (5 + i * 10));
            }
            for (int i = inputs; i < inputs + outputs; i++) {
                GL.Vertex2(width / 2, height / 2 - (5 + (i - inputs) * 10));
                GL.Vertex2(width / 2 - 10f, height / 2 - (5 + (i - inputs) * 10));
            }
            GL.End();
            GL.Begin(BeginMode.LineStrip);
            GL.Vertex2(-width / 2f + 10, -5);
            GL.Vertex2(-width / 2f + 15, 0);
            GL.Vertex2(-width / 2f + 10, 5);
            GL.End();
            GL.EndList();

            return componentDraw;
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
        public static CircuitDraw Circuit;


        public static Color Color_on = Color.Red;
        public static Color Color_off = Color.Black;
        public static Color Color_3rd = Color.Gray;

        public static void Load() {
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
            Circuit = new CircuitDraw();
        }
        private static void Gen7SegDisplay() { }
        private static void GenKeyboard() { }
        

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
            GL.Begin(BeginMode.LineLoop);
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
            GL.Begin(BeginMode.LineStrip);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(halfCircle3[i]);
            }
            GL.End();
            GL.Begin(BeginMode.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(circle[i]);
            }
            GL.End();
            GL.Begin(BeginMode.Lines);
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
            GL.Begin(BeginMode.LineLoop);
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
            GL.Begin(BeginMode.LineStrip);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(halfCircle3[i]);
            }
            GL.End();
            GL.Begin(BeginMode.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(circle[i]);
            }
            GL.End();
            GL.Begin(BeginMode.Lines);
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
            GL.Begin(BeginMode.LineLoop);
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
            GL.Begin(BeginMode.LineStrip);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(halfCircle3[i]);
            }
            GL.End();
            GL.Begin(BeginMode.Lines);
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
            GL.Begin(BeginMode.LineLoop);
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
            GL.Begin(BeginMode.LineStrip);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(halfCircle3[i]);
            }
            GL.End();
            GL.Begin(BeginMode.Lines);
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
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex2(-15, 10);
            GL.Vertex2(0, 10);
            for (int i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(0, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(BeginMode.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(circle[i]);
            }
            GL.End();
            GL.Begin(BeginMode.Lines);
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
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (int i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(BeginMode.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(circle[i]);
            }
            GL.End();
            GL.Begin(BeginMode.Lines);
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
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex2(-15, 10);
            GL.Vertex2(0, 10);
            for (int i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(0, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(BeginMode.Lines);
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
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex2(-15, 10);
            GL.Vertex2(-10, 10);
            for (int i = total - 1; i >= 0; i--) {
                GL.Vertex2(halfCircle1[i]);
            }
            GL.Vertex2(-10, -10);
            GL.Vertex2(-15, -10);
            GL.End();
            GL.Begin(BeginMode.Lines);
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
            GL.Begin(BeginMode.LineLoop);
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
            GL.Begin(BeginMode.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(circle[i]);
            }
            GL.End();
            GL.Begin(BeginMode.Lines);
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
            GL.Begin(BeginMode.LineLoop);
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
            GL.Begin(BeginMode.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(circle[i]);
            }
            GL.End();
            GL.Begin(BeginMode.Lines);
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
            GL.Begin(BeginMode.LineLoop);
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
            GL.Begin(BeginMode.Lines);
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
            GL.Begin(BeginMode.LineLoop);
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
            GL.Begin(BeginMode.Lines);
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
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex2(10, 0);
            GL.Vertex2(-10, 10);
            GL.Vertex2(-10, -10);
            GL.End();
            GL.Begin(BeginMode.Lines);
            GL.Vertex2(-10, 0);
            GL.Vertex2(-20, 0);
            GL.Color3(Color_on);
            GL.Vertex2(16, 0);
            GL.Vertex2(20, 0);
            GL.End();
            GL.Begin(BeginMode.LineLoop);
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
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex2(10, 0);
            GL.Vertex2(-10, 10);
            GL.Vertex2(-10, -10);
            GL.End();
            GL.Begin(BeginMode.Lines);
            GL.Vertex2(-10, 0);
            GL.Vertex2(-20, 0);
            GL.Color3(Color_off);
            GL.Vertex2(16, 0);
            GL.Vertex2(20, 0);
            GL.End();
            GL.Begin(BeginMode.LineLoop);
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
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex2(10, 0);
            GL.Vertex2(-10, 10);
            GL.Vertex2(-10, -10);
            GL.End();
            GL.Begin(BeginMode.Lines);
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
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex2(10, 0);
            GL.Vertex2(-10, 10);
            GL.Vertex2(-10, -10);
            GL.End();
            GL.Begin(BeginMode.Lines);
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
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex2(10, 0);
            GL.Vertex2(-10, 10);
            GL.Vertex2(-10, -10);
            GL.End();
            GL.Begin(BeginMode.Lines);
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

            GL.Begin(BeginMode.LineLoop);
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
            GL.Begin(BeginMode.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(outerVertexes[i]);
            }
            GL.End();

            GL.Begin(BeginMode.TriangleFan);
            GL.Color3(Color_off);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(innerVertexes[i]);
            }
            GL.End();

            GL.Begin(BeginMode.Lines);
            GL.Vertex2(new Vector2(-5, 0));
            GL.Vertex2(new Vector2(-10, 0));
            GL.End();
            GL.EndList();

            Output[1] = new ComponentDraw(GL.GenLists(1), 15, 10, 1);
            Output[1].Terminals[0] = new Point(-10, 0);
            GL.NewList(Output[1].DisplayListHandle, ListMode.Compile);
            GL.Color3(Color_off);
            GL.Begin(BeginMode.LineLoop);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(outerVertexes[i]);
            }
            GL.End();
            GL.Color3(Color_on);
            GL.Begin(BeginMode.TriangleFan);
            for (int i = 0; i < total; i++) {
                GL.Vertex2(innerVertexes[i]);
            }
            GL.End();

            GL.Begin(BeginMode.Lines);
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

            GL.Begin(BeginMode.LineLoop);
            GL.Color3(Color_off);
            GL.Vertex2(-5, -5);
            GL.Vertex2(-5, 5);
            GL.Vertex2(5, 5);
            GL.Vertex2(5, -5);
            GL.End();
            
            GL.Begin(BeginMode.Quads);
            GL.Color3(Color_off);
            GL.Vertex2(-3, -3);
            GL.Vertex2(-3, 3);
            GL.Vertex2(3, 3);
            GL.Vertex2(3, -3);
            GL.End();

            GL.Begin(BeginMode.Lines);
            GL.Color3(Color_off);
            GL.Vertex2(5, 0);
            GL.Vertex2(10, 0);
            GL.End();
            
            GL.EndList();

            Input[1] = new ComponentDraw(GL.GenLists(1), 15, 10, 1);
            Input[1].Terminals[0] = new Point(10, 0);
            GL.NewList(Input[1].DisplayListHandle, ListMode.Compile);

            GL.Begin(BeginMode.LineLoop);
            GL.Color3(Color_off);
            GL.Vertex2(-5, -5);
            GL.Vertex2(-5, 5);
            GL.Vertex2(5, 5);
            GL.Vertex2(5, -5);
            GL.End();

            GL.Begin(BeginMode.Quads);
            GL.Color3(Color_on);
            GL.Vertex2(-3, -3);
            GL.Vertex2(-3, 3);
            GL.Vertex2(3, 3);
            GL.Vertex2(3, -3);
            GL.End();

            GL.Begin(BeginMode.Lines);
            GL.Color3(Color_on);
            GL.Vertex2(5, 0);
            GL.Vertex2(10, 0);
            GL.End();

            GL.EndList();
        }

    }
    public class CircuitComponentToDrawComponent {
        public CircuitSimulator.Component CircuitComponent;
        public Component DrawComponent;
        public CircuitComponentToDrawComponent(CircuitSimulator.Component CircuitComponent, Component DrawComponent) {
            this.CircuitComponent = CircuitComponent;
            this.DrawComponent = DrawComponent;
        }
    }
    public class CircuitWireToDrawWire {
        public CircuitSimulator.Components.Wire CircuitWire;
        public Wire DrawWire;
        public CircuitWireToDrawWire(CircuitSimulator.Components.Wire CircuitWire, Wire DrawWire) {
            this.CircuitWire = CircuitWire;
            this.DrawWire = DrawWire;
        }
    }
    public class ComponentDraw {
        public int DisplayListHandle;
        public int Width;
        public int Height;
        public Point[] Terminals;

        public ComponentDraw(int displayListHandle, int width, int height, int terminals = 1) {
            DisplayListHandle = displayListHandle;
            Width = width;
            Height = height;
            Terminals = new Point[terminals];
        }
    }

    public class Component {
        public ComponentDraw Draw;
        public PointF Center;
        public float Rotation = 0;

        public Component(ComponentDraw draw, PointF center) {
            Draw = draw;
            Center = center;
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
    public class Wire {
        public PointF From;
        public Component FromComponent;
        public int FromIndex;
        public Color Color = Color.Black;
        public PointF To;
        public Component ToComponent;
        public int ToIndex;

        public Wire(PointF from, PointF to) {
            From = from;
            To = to;
        }
        public Wire(Component from, int indexFrom, Component to, int indexTo) {
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
        }
        public Wire(Component from, int indexFrom, PointF to) {
            FromComponent = from;
            FromIndex = indexFrom;
            From = from.TransformTerminal(indexFrom);
            From.X += from.Center.X;
            From.Y += from.Center.Y;
            To = to;
        }
        public Wire(PointF from, Component to, int indexTo) {
            From = from;
            ToComponent = to;
            ToIndex = indexTo;
            To = to.TransformTerminal(indexTo);
            To.X += to.Center.X;
            To.Y += to.Center.Y;
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
}
