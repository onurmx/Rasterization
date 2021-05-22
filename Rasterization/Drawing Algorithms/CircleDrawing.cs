using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rasterization
{
    public class CircleDrawing
    {
        public Image MidpointCircle(Circle circle, Image image)
        {
            Bitmap bitmap = new Bitmap(image);

            int d = 1 - circle.Radius;
            int x = 0;
            int y = circle.Radius;
            bitmap.SetPixel(circle.Center.X + circle.Radius, circle.Center.Y, circle.Color);
            bitmap.SetPixel(circle.Center.X - circle.Radius, circle.Center.Y, circle.Color);
            bitmap.SetPixel(circle.Center.X, circle.Center.Y + circle.Radius, circle.Color);
            bitmap.SetPixel(circle.Center.X, circle.Center.Y - circle.Radius, circle.Color);
            while (y > x)
            {
                if (d < 0) //move to E
                {
                    d += 2 * x + 3;
                }
                else //move to SE
                {
                    d += 2 * x - 2 * y + 5;
                    --y;
                }
                ++x;

                bitmap.SetPixel(circle.Center.X + x, circle.Center.Y + y, circle.Color);
                bitmap.SetPixel(circle.Center.X + y, circle.Center.Y + x, circle.Color);
                bitmap.SetPixel(circle.Center.X - y, circle.Center.Y + x, circle.Color);
                bitmap.SetPixel(circle.Center.X - x, circle.Center.Y + y, circle.Color);
                bitmap.SetPixel(circle.Center.X - x, circle.Center.Y - y, circle.Color);
                bitmap.SetPixel(circle.Center.X - y, circle.Center.Y - x, circle.Color);
                bitmap.SetPixel(circle.Center.X + y, circle.Center.Y - x, circle.Color);
                bitmap.SetPixel(circle.Center.X + x, circle.Center.Y - y, circle.Color);
            }

            return bitmap;
        }

        public Image WuCircle(Circle circle, Image image)
        {
            Bitmap bitmap = new Bitmap(image);

            Color L = circle.Color; /*Line color*/
            Color B = Color.FromArgb(255, 255, 255, 255); /*Background Color*/
            int x = circle.Radius;
            int y = 0;

            bitmap.SetPixel(circle.Center.X + circle.Radius, circle.Center.Y, circle.Color);
            bitmap.SetPixel(circle.Center.X - circle.Radius, circle.Center.Y, circle.Color);
            bitmap.SetPixel(circle.Center.X, circle.Center.Y + circle.Radius, circle.Color);
            bitmap.SetPixel(circle.Center.X, circle.Center.Y - circle.Radius, circle.Color);

            while (x > y)
            {
                ++y;
                x = (int)(Math.Ceiling(Math.Sqrt((circle.Radius * circle.Radius) - (y * y))));
                float T = D(circle.Radius, y);

                Color c2 = Color.FromArgb((int)(L.A * (1 - T) + B.A * T),
                                          (int)(L.R * (1 - T) + B.R * T),
                                          (int)(L.G * (1 - T) + B.G * T),
                                          (int)(L.B * (1 - T) + B.B * T));

                Color c1 = Color.FromArgb((int)(L.A * T + B.A * (1 - T)),
                                          (int)(L.R * T + B.R * (1 - T)),
                                          (int)(L.G * T + B.G * (1 - T)),
                                          (int)(L.B * T + B.B * (1 - T)));

                bitmap.SetPixel(circle.Center.X + x, circle.Center.Y + y, c2);
                bitmap.SetPixel(circle.Center.X + x - 1, circle.Center.Y + y, c1);

                bitmap.SetPixel(circle.Center.X + y, circle.Center.Y + x, c2);
                bitmap.SetPixel(circle.Center.X + y - 1, circle.Center.Y + x, c1);

                bitmap.SetPixel(circle.Center.X - y, circle.Center.Y + x, c2);
                bitmap.SetPixel(circle.Center.X - y - 1, circle.Center.Y + x, c1);

                bitmap.SetPixel(circle.Center.X - x, circle.Center.Y + y, c2);
                bitmap.SetPixel(circle.Center.X - x - 1, circle.Center.Y + y, c1);

                bitmap.SetPixel(circle.Center.X - x, circle.Center.Y - y, c2);
                bitmap.SetPixel(circle.Center.X - x - 1, circle.Center.Y - y, c1);

                bitmap.SetPixel(circle.Center.X - y, circle.Center.Y - x, c2);
                bitmap.SetPixel(circle.Center.X - y - 1, circle.Center.Y - x, c1);

                bitmap.SetPixel(circle.Center.X + y, circle.Center.Y - x, c2);
                bitmap.SetPixel(circle.Center.X + y - 1, circle.Center.Y - x, c1);

                bitmap.SetPixel(circle.Center.X + x, circle.Center.Y - y, c2);
                bitmap.SetPixel(circle.Center.X + x - 1, circle.Center.Y - y, c1);
            }

            return bitmap;
        }

        public float D(int R, int y)
        {
            var p1 = (float)Math.Sqrt((R * R) - (y * y));
            var p2 = (float)Math.Ceiling(p1);
            return p2 - p1;
        }
    }
}
