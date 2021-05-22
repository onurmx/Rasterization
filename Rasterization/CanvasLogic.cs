﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rasterization
{
    public class CanvasLogic
    {
        public int DrawingMode { get; set; } = 0; //0-Disabled
                                                  //1-Circle
                                                  //2-Line
                                                  //3-Polygon(Start Recording)
        public Circle tmpCircle { get; set; } = new Circle();
        public Line tmpLine { get; set; } = new Line();
        public Polygon tmpPolygon { get; set; } = new Polygon();
    }
}
