using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp50
{
    public class Rectangle
    {
        private Point2D start;
        private int width;
        private int height;

        public Point2D Start { get { return start; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public Rectangle(Point2D start, int width, int height)
        {
            this.start = start;
            this.width = width;
            this.height = height;
        }
    }
}
