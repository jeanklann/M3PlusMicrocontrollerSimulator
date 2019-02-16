using System.Drawing;

namespace IDE
{
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
}