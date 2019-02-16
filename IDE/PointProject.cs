using System;
using System.Drawing;

namespace IDE
{
    [Serializable]
    public class PointProject
    {
        public float X, Y;

        public PointProject(PointF point)
        {
            X = point.X;
            Y = point.Y;
        }

        public PointProject(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator PointF(PointProject p)
        {
            return new PointF(p.X, p.Y);
        }

        public static implicit operator PointProject(PointF p)
        {
            return new PointProject(p);
        }
    }
}