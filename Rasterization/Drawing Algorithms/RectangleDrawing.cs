using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rasterization
{
    public class RectangleDrawing
    {
        public void DrawRectangle_DDA(Rectangle rectangle, Bitmap bitmap)
        {
            LineDrawing lineDrawing = new LineDrawing();

            Line line = new Line();
            line.Color = rectangle.Color;
            line.Thickness = 1;
            line.Antialiasing = false;

            line.StartPoint = new Point(rectangle.TopLeft.X, rectangle.TopLeft.Y);
            line.EndPoint = new Point(rectangle.BottomRight.X, rectangle.TopLeft.Y);
            lineDrawing.lineDDA(line, bitmap);

            line.StartPoint = new Point(rectangle.BottomRight.X, rectangle.TopLeft.Y);
            line.EndPoint = new Point(rectangle.BottomRight.X, rectangle.BottomRight.Y);
            lineDrawing.lineDDA(line, bitmap);

            line.StartPoint = new Point(rectangle.BottomRight.X, rectangle.BottomRight.Y);
            line.EndPoint = new Point(rectangle.TopLeft.X, rectangle.BottomRight.Y);
            lineDrawing.lineDDA(line, bitmap);

            line.StartPoint = new Point(rectangle.TopLeft.X, rectangle.BottomRight.Y);
            line.EndPoint = new Point(rectangle.TopLeft.X, rectangle.TopLeft.Y);
            lineDrawing.lineDDA(line, bitmap);
        }
    }
}
