using System;

namespace IDE
{
    [Serializable]
    public class WireProject
    {
        public PointProject From;
        public int FromComponent;
        public int FromIndex;
        public int RootComponent;
        public PointProject To;
        public int ToComponent;
        public int ToIndex;

        public WireProject(Wire wire, FileProject project)
        {
            From = wire.From;
            RootComponent = UiStatics.Circuito.Components.IndexOf(wire.RootComponent);
            if (wire.FromComponent != null)
            {
                FromComponent = UiStatics.Circuito.Components.IndexOf(wire.FromComponent);
                FromIndex = wire.FromIndex;
            }
            else
            {
                FromIndex = -1;
                FromComponent = -1;
            }

            To = wire.To;
            if (wire.ToComponent != null)
            {
                ToComponent = UiStatics.Circuito.Components.IndexOf(wire.ToComponent);
                ToIndex = wire.ToIndex;
            }
            else
            {
                ToIndex = -1;
                ToComponent = -1;
            }
        }

        public Wire ToWire(FileProject project)
        {
            var wire = new Wire(From, To);
            if (RootComponent != -1)
                wire.RootComponent = UiStatics.Circuito.Components[RootComponent];
            if (FromComponent != -1)
            {
                wire.FromComponent = UiStatics.Circuito.Components[FromComponent];
                wire.FromIndex = FromIndex;
            }
            else
            {
                FromIndex = -1;
            }

            if (ToComponent != -1)
            {
                wire.ToComponent = UiStatics.Circuito.Components[ToComponent];
                wire.ToIndex = ToIndex;
            }
            else
            {
                FromIndex = -1;
            }

            return wire;
        }
    }
}