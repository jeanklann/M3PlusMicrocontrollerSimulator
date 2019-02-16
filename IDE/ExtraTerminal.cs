using System.Drawing;

namespace IDE
{
    public struct ExtraTerminal {
        public PointF Point;
        public Component Root;
        public ExtraTerminal(PointF point, Component root) {
            Point = point;
            Root = root;
        }
    }
}