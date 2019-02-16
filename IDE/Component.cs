using System;
using System.Drawing;

namespace IDE
{
    public class Component
    {
        public bool[] ActiveExtraHandlers;
        public PointF Center;
        public ComponentDraw Draw;
        public int[] ExtraHandlers; //Used to display things on the component
        public Component RootComponent;
        public float Rotation;
        public ComponentType Type = ComponentType.None;

        public Component(ComponentDraw draw, PointF center, Component rootComponent = null)
        {
            Draw = draw;
            Center = center;
            if (rootComponent == null)
                RootComponent = UiStatics.Circuito.InsideComponent;
            else
                RootComponent = rootComponent;

            if (draw.DisplayListHandle == Draws.And[0].DisplayListHandle ||
                draw.DisplayListHandle == Draws.And[1].DisplayListHandle)
            {
                Type = ComponentType.And;
            }
            else if (draw.DisplayListHandle == Draws.Nand[0].DisplayListHandle ||
                     draw.DisplayListHandle == Draws.Nand[1].DisplayListHandle)
            {
                Type = ComponentType.Nand;
            }
            else if (draw.DisplayListHandle == Draws.Or[0].DisplayListHandle ||
                     draw.DisplayListHandle == Draws.Or[1].DisplayListHandle)
            {
                Type = ComponentType.Or;
            }
            else if (draw.DisplayListHandle == Draws.Nor[0].DisplayListHandle ||
                     draw.DisplayListHandle == Draws.Nor[1].DisplayListHandle)
            {
                Type = ComponentType.Nor;
            }
            else if (draw.DisplayListHandle == Draws.Xor[0].DisplayListHandle ||
                     draw.DisplayListHandle == Draws.Xor[1].DisplayListHandle)
            {
                Type = ComponentType.Xor;
            }
            else if (draw.DisplayListHandle == Draws.Xnor[0].DisplayListHandle ||
                     draw.DisplayListHandle == Draws.Xnor[1].DisplayListHandle)
            {
                Type = ComponentType.Xnor;
            }
            else if (draw.DisplayListHandle == Draws.Not[0].DisplayListHandle ||
                     draw.DisplayListHandle == Draws.Not[1].DisplayListHandle)
            {
                Type = ComponentType.Not;
            }
            else if (draw.DisplayListHandle == Draws.Input[0].DisplayListHandle ||
                     draw.DisplayListHandle == Draws.Input[1].DisplayListHandle)
            {
                Type = ComponentType.Input;
            }
            else if (draw.DisplayListHandle == Draws.Output[0].DisplayListHandle ||
                     draw.DisplayListHandle == Draws.Output[1].DisplayListHandle)
            {
                Type = ComponentType.Output;
            }
            else if (draw.DisplayListHandle == Draws.Keyboard.DisplayListHandle)
            {
                Type = ComponentType.Keyboard;
            }
            else if (draw.DisplayListHandle == Draws.Microcontroller.DisplayListHandle)
            {
                Type = ComponentType.Microcontroller;
            }
            else if (draw.DisplayListHandle == Draws.Disable[0].DisplayListHandle ||
                     draw.DisplayListHandle == Draws.Disable[1].DisplayListHandle ||
                     draw.DisplayListHandle == Draws.Disable[2].DisplayListHandle)
            {
                Type = ComponentType.Disable;
            }
            else if (draw.DisplayListHandle == Draws.Display7SegBase.DisplayListHandle)
            {
                Type = ComponentType.Display7Seg;
                ExtraHandlers = Draws.Display7SegPart;
                ActiveExtraHandlers = new bool[ExtraHandlers.Length];
            }
            else if (draw.DisplayListHandle == Draws.Osciloscope.DisplayListHandle)
            {
                Type = ComponentType.Osciloscope;
            }
            else if (draw.DisplayListHandle == Draws.JkFlipFlop.DisplayListHandle)
            {
                Type = ComponentType.JkFlipFlop;
            }
            else if (draw.DisplayListHandle == Draws.RsFlipFlop.DisplayListHandle)
            {
                Type = ComponentType.RsFlipFlop;
            }
            else if (draw.DisplayListHandle == Draws.FlipFlop.DisplayListHandle)
            {
                Type = ComponentType.FlipFlop;
            }
            else if (draw.DisplayListHandle == Draws.DFlipFlop.DisplayListHandle)
            {
                Type = ComponentType.DFlipFlop;
            }
            else if (draw.DisplayListHandle == Draws.Disable8Bit.DisplayListHandle)
            {
                Type = ComponentType.Disable8Bit;
            }
            else if (draw.DisplayListHandle == Draws.FullAdder.DisplayListHandle)
            {
                Type = ComponentType.FullAdder;
            }
            else if (draw.DisplayListHandle == Draws.HalfAdder.DisplayListHandle)
            {
                Type = ComponentType.HalfAdder;
            }
            else if (draw.DisplayListHandle == Draws.RamMemory.DisplayListHandle)
            {
                Type = ComponentType.RamMemory;
            }
            else if (draw.DisplayListHandle == Draws.RomMemory.DisplayListHandle)
            {
                Type = ComponentType.RomMemory;
            }
            else if (draw.DisplayListHandle == Draws.Ula.DisplayListHandle)
            {
                Type = ComponentType.Ula;
            }
            else if (draw.DisplayListHandle == Draws.Registrers.DisplayListHandle)
            {
                Type = ComponentType.Registrers;
            }
            else if (draw.DisplayListHandle == Draws.Registrer8Bit.DisplayListHandle)
            {
                Type = ComponentType.Registrer8Bit;
            }
            else if (draw.DisplayListHandle == Draws.Registrer8BitSg.DisplayListHandle)
            {
                Type = ComponentType.Registrer8BitSg;
            }
            else if (draw.DisplayListHandle == Draws.Registrer8BitCBuffer.DisplayListHandle)
            {
                Type = ComponentType.Registrer8BitCBuffer;
            }
            else if (draw.DisplayListHandle == Draws.ControlModule.DisplayListHandle)
            {
                Type = ComponentType.ControlModule;
            }
            else if (draw.DisplayListHandle == Draws.BinTo7Seg.DisplayListHandle)
            {
                Type = ComponentType.BinTo7Seg;
            }
            else if (draw.DisplayListHandle == Draws.PortBank.DisplayListHandle)
            {
                Type = ComponentType.PortBank;
            }
            else if (draw.DisplayListHandle == Draws.RomAddresser.DisplayListHandle)
            {
                Type = ComponentType.RomAddresser;
            }
            else if (draw.DisplayListHandle == Draws.Counter8Bit.DisplayListHandle)
            {
                Type = ComponentType.Counter8Bit;
            }
            else if (draw.DisplayListHandle == Draws.Clock.DisplayListHandle)
            {
                Type = ComponentType.Clock;
            }
            else
            {
                throw new ComponentNotImplementedException();
            }
        }

        public override string ToString()
        {
            var roots = "";
            var root = RootComponent;
            while (root != null)
            {
                roots += root.Type + ".";
                root = root.RootComponent;
            }

            return roots + Type + " in X: " + Center.X + ", Y: " + Center.Y;
        }

        public bool IsInside(PointF point)
        {
            var rectangle = new RectangleF(new PointF(Center.X - Draw.Width / 2f, Center.Y - Draw.Height / 2f),
                new SizeF(Draw.Width, Draw.Height));
            rectangle.Inflate(2, 2);
            return rectangle.Contains(point);
        }

        public int IsOnTerminal(PointF point)
        {
            for (var i = 0; i < Draw.Terminals.Length; i++)
            {
                var terminal = TransformTerminal(i);
                var distance = (Center.X + terminal.X - point.X) * (Center.X + terminal.X - point.X) +
                               (Center.Y + terminal.Y - point.Y) * (Center.Y + terminal.Y - point.Y);
                if (distance <= 4 * 4) return i;
            }

            return -1;
        }

        public PointF TransformTerminal(int i)
        {
            var rotDegree = Math.PI * Rotation / 180.0;
            var point = new PointF(
                (float) (Draw.Terminals[i].X * Math.Cos(rotDegree) - Draw.Terminals[i].Y * Math.Sin(rotDegree)),
                (float) (Draw.Terminals[i].X * Math.Sin(rotDegree) + Draw.Terminals[i].Y * Math.Cos(rotDegree))
            );
            return point;
        }
    }
}