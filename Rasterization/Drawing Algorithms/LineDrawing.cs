using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rasterization
{
    public class LineDrawing
    {
        public Image lineDDA(Line line, Image image)
        {
            Bitmap bitmap = new Bitmap(image);

            int dx = line.EndPoint.X - line.StartPoint.X;
            int dy = line.EndPoint.Y - line.StartPoint.Y;
            int steps = Math.Abs(dx) > Math.Abs(dy) ? Math.Abs(dx) : Math.Abs(dy);
            float Xinc = dx / (float)steps;
            float Yinc = dy / (float)steps;
            float x = line.StartPoint.X;
            float y = line.StartPoint.Y;
            for (int i = 0; i <= steps; i++)
            {
                bitmap.SetPixel((int)Math.Round(x), (int)Math.Round(y), line.Color);
                x += Xinc;
                y += Yinc;
            }

            return bitmap;
        }

        public Image lineDDA_thick(Line line, Image image)
        {
            Bitmap bitmap = new Bitmap(image);

            int dx = line.EndPoint.X - line.StartPoint.X;
            int dy = line.EndPoint.Y - line.StartPoint.Y;
            int steps = Math.Abs(dx) > Math.Abs(dy) ? Math.Abs(dx) : Math.Abs(dy);
            float Xinc = dx / (float)steps;
            float Yinc = dy / (float)steps;
            float x = line.StartPoint.X;
            float y = line.StartPoint.Y;
            if (Math.Abs(dy) > Math.Abs(dx))
            {
                for (int i = 0; i <= steps; i++)
                {
                    bitmap.SetPixel((int)Math.Round(x), (int)Math.Round(y), line.Color);
                    for (int j = 0; j <= ((line.Thickness - 1) / 2); j++)
                    {
                        bitmap.SetPixel((int)Math.Round(x) + j, (int)Math.Round(y), line.Color);
                        bitmap.SetPixel((int)Math.Round(x) - j, (int)Math.Round(y), line.Color);
                    }
                    x += Xinc;
                    y += Yinc;
                }
            }
            else
            {
                for (int i = 0; i <= steps; i++)
                {
                    bitmap.SetPixel((int)Math.Round(x), (int)Math.Round(y), line.Color);
                    for (int j = 0; j <= ((line.Thickness - 1) / 2); j++)
                    {
                        bitmap.SetPixel((int)Math.Round(x), (int)Math.Round(y) + j, line.Color);
                        bitmap.SetPixel((int)Math.Round(x), (int)Math.Round(y) - j, line.Color);
                    }
                    x += Xinc;
                    y += Yinc;
                }
            }

            return bitmap;
        }

        public Image WuLine(Line line, Image image)
        {
            Bitmap bitmap = new Bitmap(image);

            var L = line.Color;
            var B = Color.FromArgb(255, 255, 255, 255);
            if (Math.Abs(line.EndPoint.X - line.StartPoint.X) >= Math.Abs(line.EndPoint.Y - line.StartPoint.Y))
            {
                var dy = (float)(line.EndPoint.Y - line.StartPoint.Y) / Math.Abs(line.EndPoint.X - line.StartPoint.X);
                var dx = line.EndPoint.X > line.StartPoint.X ? 1 : -1;
                float y = line.StartPoint.Y;
                for (var x = line.StartPoint.X; x != line.EndPoint.X; x += dx)
                {
                    Color c1 = Color.FromArgb((int)(L.A * (1 - modf(y)) + (B.A * modf(y))),
                                              (int)(L.R * (1 - modf(y)) + (B.R * modf(y))),
                                              (int)(L.G * (1 - modf(y)) + (B.G * modf(y))),
                                              (int)(L.B * (1 - modf(y)) + (B.B * modf(y))));

                    Color c2 = Color.FromArgb((int)((L.A * modf(y)) + (B.A * (1 - modf(y)))),
                                               (int)((L.R * modf(y)) + (B.R * (1 - modf(y)))),
                                               (int)((L.G * modf(y)) + (B.G * (1 - modf(y)))),
                                               (int)((L.B * modf(y)) + (B.B * (1 - modf(y)))));


                    bitmap.SetPixel(x, (int)Math.Floor(y), c1);
                    bitmap.SetPixel(x, (int)Math.Floor(y) + 1, c2);

                    y += dy;
                }
            }
            else
            {
                int dx = line.EndPoint.X - line.StartPoint.X;
                int dy = line.EndPoint.Y - line.StartPoint.Y;
                int steps;
                if (Math.Abs(dx) > Math.Abs(dy)) steps = Math.Abs(dx);
                else steps = Math.Abs(dy);
                float Xinc = dx / (float)steps;
                float Yinc = dy / (float)steps;
                float x = line.StartPoint.X;
                float y = line.StartPoint.Y;
                for (int i = 0; i <= steps; i++)
                {
                    Color c1 = Color.FromArgb((int)(L.A * (1 - modf(y)) + (B.A * modf(y))),
                                              (int)(L.R * (1 - modf(y)) + (B.R * modf(y))),
                                              (int)(L.G * (1 - modf(y)) + (B.G * modf(y))),
                                              (int)(L.B * (1 - modf(y)) + (B.B * modf(y))));

                    Color c2 = Color.FromArgb((int)((L.A * modf(y)) + (B.A * (1 - modf(y)))),
                                               (int)((L.R * modf(y)) + (B.R * (1 - modf(y)))),
                                               (int)((L.G * modf(y)) + (B.G * (1 - modf(y)))),
                                               (int)((L.B * modf(y)) + (B.B * (1 - modf(y)))));

                    bitmap.SetPixel((int)Math.Floor(x), (int)y, c1);
                    bitmap.SetPixel((int)Math.Floor(x), (int)y + 1, c2);

                    x += Xinc;
                    y += Yinc;
                }
            }

            return bitmap;
        }

        public float modf(float number)
        {
            return number - (float)Math.Floor(number);
        }
    }
}
