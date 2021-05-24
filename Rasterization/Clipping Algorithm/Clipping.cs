using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rasterization
{
    public class Clipping
    {
        private float dx { get; set; }
        private float dy { get; set; }
        private float tE { get; set; }
        private float tL { get; set; }



        public Image LiangBarskyClipping(Point p1, Point p2, Rectangle clip, Image image)
        {
            LineDrawing lineDrawing = new LineDrawing();
            Line newLine = new Line();
            newLine.Color = Color.Red;
            newLine.Thickness = 1;
            newLine.Antialiasing = false;

            Bitmap bitmap = new Bitmap(image);

            int xMin = clip.TopLeft.X <= clip.BottomRight.X ? clip.TopLeft.X : clip.BottomRight.X;
            int yMin = clip.TopLeft.Y <= clip.BottomRight.Y ? clip.TopLeft.Y : clip.BottomRight.Y;
            int xMax = clip.TopLeft.X >= clip.BottomRight.X ? clip.TopLeft.X : clip.BottomRight.X;
            int yMax = clip.TopLeft.Y >= clip.BottomRight.Y ? clip.TopLeft.Y : clip.BottomRight.Y;

            dx = p2.X - p1.X;
            dy = p2.Y - p1.Y;

            tE = 0;
            tL = 1;

            if (clipT(xMin - p1.X, dx))
            {
                if (clipT(p1.X - xMax, -dx))
                {
                    if (clipT(yMin - p1.Y, dy))
                    {
                        if (clipT(p1.Y - yMax, -dy))
                        {
                            if (tL < 1)
                            {
                                newLine.EndPoint = new Point((int)((float)p1.X + tL * dx), (int)((float)p1.Y + tL * dy));
                            }
                            else //My improvement
                            {
                                newLine.EndPoint = p2;
                            }
                            if (tE > 0)
                            {
                                newLine.StartPoint = new Point((int)((float)p1.X + (tE * dx)), (int)((float)p1.Y + (tE * dy)));
                            }
                            else //My improvement
                            {
                                newLine.StartPoint = p1;
                            }
                        }
                    }
                }
            }

            lineDrawing.lineDDA(newLine, bitmap);

            return bitmap;
        }

        public bool clipT(float numer, float denom)
        {
            if (denom == 0)
            {
                if (numer > 0)
                {
                    return false;
                }
                return true;
            }
            float t = numer / denom;
            if (denom > 0)
            {
                if (t > tL)
                    return false;
                if (t > tE)
                    tE = t;
            }
            else
            {
                if (t < tE)
                    return false;
                if (t < tL)
                    tL = t;
            }
            return true;
        }
    }
}
