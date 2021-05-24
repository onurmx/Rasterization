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
        private int[] CalculateNumerators(int x0, int y0, int xMin, int yMin, int xMax, int yMax)
        {
            int q1 = x0 - xMin;
            int q2 = xMax - x0;
            int q3 = y0 - yMin;
            int q4 = yMax - y0;
            int[] returnArray = { q1, q2, q3, q4 };
            return returnArray;
        }

        private int[] CalculateDenominators(int x0, int y0, int x1, int y1)
        {
            int dX = x1 - x0;
            int dY = y1 - y0;
            int[] returnArray = { -dX, dX, -dY, dY };
            return returnArray;
        }

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

            double tE = 0.0;
            double tL = 1.0;

            if ((xMin < p1.X && p1.X < xMax) &&
                (yMin < p1.Y && p1.Y < yMax) &&
                (xMin < p2.X && p2.X < xMax) &&
                (yMin < p2.Y && p2.Y < yMax))
            {
                newLine.StartPoint = new Point(p1.X, p1.Y);
                newLine.EndPoint = new Point(p2.X, p2.Y);

                lineDrawing.lineDDA(newLine, bitmap);
                return bitmap;
            }

            var Denominators = CalculateDenominators(p1.X, p1.Y, p2.X, p2.Y);
            var Numerators = CalculateNumerators(p1.X, p1.Y, xMin, yMin, xMax, yMax);

            List<double> lessThanZeroList = new List<double>();
            List<double> biggerThanZeroList = new List<double>();
            lessThanZeroList.Add(tE);
            biggerThanZeroList.Add(tL);

            for (int i = 0; i < Denominators.Length; i++)
            {
                double t = (double)Numerators[i] / (double)Denominators[i];
                if (Denominators[i] < 0)
                {
                    lessThanZeroList.Add(t);
                }
                else
                {
                    biggerThanZeroList.Add(t);
                }
            }

            double newtE = lessThanZeroList.Max();
            double newtL = biggerThanZeroList.Min();

            if (tE != newtE && tL == newtL)
            {
                int x1 = p1.X + (int)(newtE * (p2.X - p1.X));
                int y1 = p1.Y + (int)(newtE * (p2.Y - p1.Y));

                newLine.StartPoint = new Point(x1, y1);
                newLine.EndPoint = new Point(p2.X, p2.Y);

                lineDrawing.lineDDA(newLine, bitmap);
            }
            else if (tE == newtE && tL != newtL)
            {
                int x2 = p2.X + (int)(newtL * (p2.X - p1.X));
                int y2 = p2.Y + (int)(newtL * (p2.Y - p1.Y));

                newLine.StartPoint = new Point(p1.X, p1.Y);
                newLine.EndPoint = new Point(x2, y2);

                lineDrawing.lineDDA(newLine, bitmap);
            }
            else if (tE != newtE && tL != newtL)
            {
                int x1 = p1.X + (int)(newtE * (float)(p2.X - p1.X));
                int y1 = p1.Y + (int)(newtE * (float)(p2.Y - p1.Y));
                int x2 = p1.X + (int)(newtL * (float)(p2.X - p1.X));
                int y2 = p1.Y + (int)(newtL * (float)(p2.Y - p1.Y));
                if ((x1 < bitmap.Width && x2 < bitmap.Width) && (y1 < bitmap.Height && y2 < bitmap.Height))
                {
                    newLine.StartPoint = new Point(x1, y1);
                    newLine.EndPoint = new Point(x2, y2);

                    lineDrawing.lineDDA(newLine, bitmap);
                }
            }
            return bitmap;
        }
    }
}
