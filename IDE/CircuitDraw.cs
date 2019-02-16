using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace IDE
{
    public class CircuitDraw {
        private static ComponentDraw[,] _circuit;
        public CircuitDraw() {
            _circuit = new ComponentDraw[256, 256];
        }
        public ComponentDraw this[int input, int output]
        {
            get {
                if(_circuit[input, output] == null) {
                    _circuit[input, output] = GenCircuit(input, output);
                }
                return _circuit[input, output];
            }
            set => _circuit[input, output] = value;
        }
        private static ComponentDraw GenCircuit(int inputs, int outputs) {
            var maxValue = inputs > outputs ? inputs : outputs;
            var width = maxValue * 20 / 8 + 20;
            var height = maxValue * 10;
            var componentDraw = new ComponentDraw(GL.GenLists(1), width, height, inputs + outputs);
            for (var i = 0; i < inputs; i++) {
                componentDraw.Terminals[i] = new Point(-width / 2, height / 2 - (5 + i * 10));
            }
            for (var i = inputs; i < inputs + outputs; i++) {
                componentDraw.Terminals[i] = new Point(width / 2, height / 2 - (5 + (i - inputs) * 10));
            }
            GL.NewList(componentDraw.DisplayListHandle, ListMode.Compile);
            GL.Color3(Draws.ColorOff);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(-width / 2f + 10, -height / 2f);
            GL.Vertex2(width / 2f - 10, -height / 2f);
            GL.Vertex2(width / 2f - 10, height / 2f);
            GL.Vertex2(-width / 2f + 10, height / 2f);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            for (var i = 0; i < inputs; i++) {
                GL.Vertex2(-width / 2, height / 2 - (5 + i * 10));
                GL.Vertex2(-width / 2 + 10f, height / 2 - (5 + i * 10));
            }
            for (var i = inputs; i < inputs + outputs; i++) {
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
}