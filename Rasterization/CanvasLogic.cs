using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rasterization
{
    public class CanvasLogic
    {
        public int DrawingMode { get; set; } = 0; //0-Disabled(Object Motion Mode)
                                                  //1-Circle
                                                  //2-Line
                                                  //3-Polygon(Start Recording)
                                                  //4-Rectangle
                                                  //5-Filling
                                                  //6-Filling with background
        public Circle tmpCircle { get; set; } = new Circle();
        public Line tmpLine { get; set; } = new Line();
        public Polygon tmpPolygon { get; set; } = new Polygon();
        public Rectangle tmpRectangle { get; set; } = new Rectangle();
    }
}
