using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rasterization
{
    public class Database
    {
        public List<Circle> Circles { get; set; } = new List<Circle>();
        public List<Line> Lines { get; set; } = new List<Line>();
        public List<Polygon> Polygons { get; set; } = new List<Polygon>();
    }
}
