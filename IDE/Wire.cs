using System.Drawing;

namespace IDE
{
    public class Wire
    {
        public Color Color = Color.Black;
        public PointF From;
        public Component FromComponent;
        public int FromIndex;
        public Component RootComponent;
        public PointF To;
        public Component ToComponent;
        public int ToIndex;

        public Wire(PointF from, PointF to, Component rootComponent = null)
        {
            From = from;
            To = to;
            if (rootComponent == null)
                RootComponent = UiStatics.Circuito.InsideComponent;
            else
                RootComponent = rootComponent;
        }

        public Wire(Component from, int indexFrom, Component to, int indexTo, Component rootComponent = null)
        {
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
                RootComponent = UiStatics.Circuito.InsideComponent;
            else
                RootComponent = rootComponent;
        }

        public Wire(Component from, int indexFrom, PointF to, Component rootComponent = null)
        {
            FromComponent = from;
            FromIndex = indexFrom;
            From = from.TransformTerminal(indexFrom);
            From.X += from.Center.X;
            From.Y += from.Center.Y;
            To = to;
            if (rootComponent == null)
                RootComponent = UiStatics.Circuito.InsideComponent;
            else
                RootComponent = rootComponent;
        }

        public Wire(PointF from, Component to, int indexTo, Component rootComponent = null)
        {
            From = from;
            ToComponent = to;
            ToIndex = indexTo;
            To = to.TransformTerminal(indexTo);
            To.X += to.Center.X;
            To.Y += to.Center.Y;
            if (rootComponent == null)
                RootComponent = UiStatics.Circuito.InsideComponent;
            else
                RootComponent = rootComponent;
        }
    }
}