using System.Drawing;

namespace IDE
{
    public class ComponentDraw
    {
        public int DisplayListHandle;
        public int Height;
        public Point[] Terminals;
        public string[] TerminalsString;
        public int Width;

        public ComponentDraw(int displayListHandle, int width, int height, int terminals = 1)
        {
            DisplayListHandle = displayListHandle;
            Width = width;
            Height = height;
            Terminals = new Point[terminals];
            TerminalsString = new string[terminals];
        }
    }
}