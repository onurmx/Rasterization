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
        public Image DrawPolygon_DDA(Polygon polygon, Image imageSource)
        {
            Image image = imageSource;
            Line line = new Line();
            line.Color = polygon.Color;
            line.Thickness = 1;
            line.Antialiasing = false;

            for (int t = 0; t < polygon.Points.Count - 1; t++)
            {
                line.StartPoint = new Point(polygon.Points[t].X, polygon.Points[t].Y);
                line.EndPoint = new Point(polygon.Points[t + 1].X, polygon.Points[t + 1].Y);
                image = LineDrawing.lineDDA(line, image);
            }
            line.StartPoint = new Point(polygon.Points.Last().X, polygon.Points.Last().Y);
            line.EndPoint = new Point(polygon.Points.First().X, polygon.Points.First().Y);
            image = LineDrawing.lineDDA(line, image);

            return image;
        }

        public Image DrawPolygon_Antialiasing(Polygon polygon, Image imageSource)
        {
            Image image = imageSource;
            Line line = new Line();
            line.Color = polygon.Color;
            line.Thickness = 1;
            line.Antialiasing = false;

            for (int t = 0; t < polygon.Points.Count - 1; t++)
            {
                line.StartPoint = new Point(polygon.Points[t].X, polygon.Points[t].Y);
                line.EndPoint = new Point(polygon.Points[t + 1].X, polygon.Points[t + 1].Y);
                image = LineDrawing.WuLine(line, image);
            }
            line.StartPoint = new Point(polygon.Points.Last().X, polygon.Points.Last().Y);
            line.EndPoint = new Point(polygon.Points.First().X, polygon.Points.First().Y);
            image = LineDrawing.WuLine(line, image);

            return image;
        }
    }
}
