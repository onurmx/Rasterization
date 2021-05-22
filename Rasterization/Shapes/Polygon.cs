using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rasterization
{
    public class Polygon
    {
        public List<Point> Points { get; set; }
        public Color Color { get; set; }
        public bool Antialiasing { get; set; }
    }
}
