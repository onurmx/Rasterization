using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Rasterization
{
    public class ObjectMotion
    {
        private object TargetObject { get; set; }
        private object InitialTargetObjectProperty { get; set; }
        private int LineMotionMode { get; set; } = 0; // 1-StartPoint, 2-EndPoint
        public Point InitialMouseLocation { get; set; } = new Point();
        public bool isLocked { get; set; } = false;


        private int FindDistance(Point p1, Point p2)
        {
            return (int)(Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow(p2.Y - p1.Y, 2)));
        }

        private Point Difference(Point MousePosition)
        {
            return new Point(MousePosition.X - InitialMouseLocation.X, MousePosition.Y - InitialMouseLocation.Y);
        }

        public void LockTargetObject(Database database)
        {
            foreach (Circle circle in database.Circles)
            {
                if (FindDistance(circle.Center, InitialMouseLocation) == circle.Radius)
                {
                    TargetObject = circle;
                    InitialTargetObjectProperty = circle.Center;
                    isLocked = true;
                    return;
                }
            }
            foreach (Line line in database.Lines)
            {
                if (InitialMouseLocation == line.StartPoint)
                {
                    TargetObject = line;
                    InitialTargetObjectProperty = line.StartPoint;
                    LineMotionMode = 1;
                    isLocked = true;
                    return;
                }
                if (InitialMouseLocation == line.EndPoint)
                {
                    TargetObject = line;
                    InitialTargetObjectProperty = line.EndPoint;
                    LineMotionMode = 2;
                    isLocked = true;
                    return;
                }
            }
            foreach (Polygon polygon in database.Polygons)
            {
                foreach (Point point in polygon.Points)
                {
                    if (InitialMouseLocation == point)
                    {
                        TargetObject = polygon;
                        InitialTargetObjectProperty = polygon.Points;
                        isLocked = true;
                        return;
                    }
                }
            }
        }

        public void MoveTargetObject(Point MousePosition)
        {
            if (TargetObject is Circle)
            {
                ((Circle)TargetObject).Center = new Point(((Point)InitialTargetObjectProperty).X + Difference(MousePosition).X,
                                                          ((Point)InitialTargetObjectProperty).Y + Difference(MousePosition).Y);
            }
            if (TargetObject is Line)
            {
                if (LineMotionMode == 1)
                {
                    ((Line)TargetObject).StartPoint = new Point(((Point)InitialTargetObjectProperty).X + Difference(MousePosition).X,
                                                                ((Point)InitialTargetObjectProperty).Y + Difference(MousePosition).Y);
                }
                if (LineMotionMode == 2)
                {
                    ((Line)TargetObject).EndPoint = new Point(((Point)InitialTargetObjectProperty).X + Difference(MousePosition).X,
                                                              ((Point)InitialTargetObjectProperty).Y + Difference(MousePosition).Y);
                }
            }
            if (TargetObject is Polygon)
            {
                for (int i = 0; i < ((Polygon)TargetObject).Points.Count; i++)
                {
                    ((Polygon)TargetObject).Points[i] = new Point(((List<Point>)InitialTargetObjectProperty)[i].X + Difference(MousePosition).X,
                                                                  ((List<Point>)InitialTargetObjectProperty)[i].Y + Difference(MousePosition).Y);
                }
            }
        }
    }
}
