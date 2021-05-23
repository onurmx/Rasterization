using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rasterization
{
    public class PolygonDrawing
    {
        private LineDrawing LineDrawing = new LineDrawing();
        public void DrawPolygon_DDA(Polygon polygon, Bitmap bitmap)
        {
            Line line = new Line();
            line.Color = polygon.Color;
            line.Thickness = 1;
            line.Antialiasing = false;

            for (int t = 0; t < polygon.Points.Count - 1; t++)
            {
                line.StartPoint = new Point(polygon.Points[t].X, polygon.Points[t].Y);
                line.EndPoint = new Point(polygon.Points[t + 1].X, polygon.Points[t + 1].Y);
                LineDrawing.lineDDA(line, bitmap);
            }
            line.StartPoint = new Point(polygon.Points.Last().X, polygon.Points.Last().Y);
            line.EndPoint = new Point(polygon.Points.First().X, polygon.Points.First().Y);
            LineDrawing.lineDDA(line, bitmap);
        }

        public void DrawPolygon_Antialiasing(Polygon polygon, Bitmap bitmap)
        {
            Line line = new Line();
            line.Color = polygon.Color;
            line.Thickness = 1;
            line.Antialiasing = false;

            for (int t = 0; t < polygon.Points.Count - 1; t++)
            {
                line.StartPoint = new Point(polygon.Points[t].X, polygon.Points[t].Y);
                line.EndPoint = new Point(polygon.Points[t + 1].X, polygon.Points[t + 1].Y);
                LineDrawing.WuLine(line, bitmap);
            }
            line.StartPoint = new Point(polygon.Points.Last().X, polygon.Points.Last().Y);
            line.EndPoint = new Point(polygon.Points.First().X, polygon.Points.First().Y);
            LineDrawing.WuLine(line, bitmap);
        }
    }
}
