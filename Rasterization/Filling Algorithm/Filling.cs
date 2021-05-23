using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rasterization
{
    public class Filling
    {
        private List<EdgeTableEntry> GetEdgeTable(List<Point> points)
        {
            List<EdgeTableEntry> edgeTableEntries = new List<EdgeTableEntry>();
            Point tmpPoint = points.Last();

            foreach (Point point in points)
            {
                EdgeTableEntry edgeTableEntry = new EdgeTableEntry();

                int dx = (point.X - tmpPoint.X);
                int dy = (point.Y - tmpPoint.Y);

                edgeTableEntry.Ymin = Math.Min(point.Y, tmpPoint.Y);
                edgeTableEntry.Ymax = Math.Max(point.Y, tmpPoint.Y);
                edgeTableEntry.Xmin = point.Y < tmpPoint.Y ? point.X : tmpPoint.X;
                edgeTableEntry.OneOverM = dy == 0 ? 0 : (float)dx / (float)dy;

                edgeTableEntries.Add(edgeTableEntry);
                tmpPoint = point;
            }

            edgeTableEntries.Sort((p, q) => p.Ymin.CompareTo(q.Ymin));
            return edgeTableEntries;
        }

        public void FillPolygon(Polygon polygon, Bitmap bitmap)
        {
            List<EdgeTableEntry> ET = GetEdgeTable(polygon.Points);
            int y = ET.First().Ymin;
            List<EdgeTableEntry> AET = new List<EdgeTableEntry>();
            while (ET.Any() || AET.Any())
            {
                AET.AddRange(ET.Select(entry => entry).Where(entry => entry.Ymin == y).ToList());
                ET.RemoveAll(entry => entry.Ymin == y);
                AET.Sort((p, q) => p.Xmin.CompareTo(q.Xmin));
                for (int i = 0; i < AET.Count; i += 2)
                {
                    for (int x = (int)AET[i].Xmin; x <= AET[i + 1].Xmin; x++)
                    {
                        bitmap.SetPixel(x, y, Color.DarkRed);
                    }
                }
                ++y;
                AET.RemoveAll(e => e.Ymax == y);
                AET.Select(e => { e.Xmin += e.OneOverM; return e; }).ToList();
            }
        }
    }
}
