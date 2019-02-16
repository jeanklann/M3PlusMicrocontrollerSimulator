using System.Drawing;

namespace IDE
{
    public struct MouseProps
    {
        public Point CurrentPosition;
        public Point LastPosition;
        public Point LastClickPosition;
        public Point LastDoubleClickPosition;
        public Point LastDownPosition;
        public Point LastUpPosition;
        public int Delta;
        public bool Button1Pressed;
        public bool Button2Pressed;

        public static PointF ToWorld(Point point, Size clientSize, PointF position, float zoom)
        {
            var worldPos = new PointF();

            worldPos.X = point.X - clientSize.Width / 2f;
            worldPos.Y = -point.Y + clientSize.Height / 2f;

            worldPos.X = worldPos.X / zoom;
            worldPos.Y = worldPos.Y / zoom;

            worldPos.X += position.X;
            worldPos.Y += position.Y;

            return worldPos;
        }
    }
}