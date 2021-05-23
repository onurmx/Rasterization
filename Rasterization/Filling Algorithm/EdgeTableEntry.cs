using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rasterization
{
    public class EdgeTableEntry
    {
        public int Ymin { get; set; }
        public int Ymax { get; set; }
        public float Xmin { get; set; }
        public float OneOverM { get; set; }
    }
}
