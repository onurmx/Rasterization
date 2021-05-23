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
        private object InitialTargetObjectProperty2 { get; set; }
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
            foreach (Rectangle rectangle in database.Rectangles)
            {
                if (InitialMouseLocation == rectangle.TopLeft)
                {
                    TargetObject = rectangle;
                    InitialTargetObjectProperty = rectangle.TopLeft;
                    LineMotionMode = 1;
                    isLocked = true;
                    return;
                }
                if (InitialMouseLocation == rectangle.BottomRight)
                {
                    TargetObject = rectangle;
                    InitialTargetObjectProperty = rectangle.BottomRight;
                    LineMotionMode = 2;
                    isLocked = true;
                    return;
                }
            }
        }

        public void LockTargetRectangle_MoveVertice(Database database)
        {
            foreach (Rectangle rectangle in database.Rectangles)
            {
                if ((InitialMouseLocation.Y == rectangle.TopLeft.Y) &&
                     rectangle.TopLeft.X < InitialMouseLocation.X &&
                     InitialMouseLocation.X < rectangle.BottomRight.X)
                {
                    TargetObject = rectangle;
                    InitialTargetObjectProperty = rectangle.TopLeft;
                    LineMotionMode = 1;
                    isLocked = true;
                    return;
                }
                if ((InitialMouseLocation.Y == rectangle.BottomRight.Y) &&
                     rectangle.TopLeft.X < InitialMouseLocation.X &&
                     InitialMouseLocation.X < rectangle.BottomRight.X)
                {
                    TargetObject = rectangle;
                    InitialTargetObjectProperty = rectangle.BottomRight;
                    LineMotionMode = 2;
                    isLocked = true;
                    return;
                }
                if ((InitialMouseLocation.X == rectangle.TopLeft.X) &&
                     rectangle.TopLeft.Y < InitialMouseLocation.Y &&
                     InitialMouseLocation.Y < rectangle.BottomRight.Y)
                {
                    TargetObject = rectangle;
                    InitialTargetObjectProperty = rectangle.TopLeft;
                    LineMotionMode = 3;
                    isLocked = true;
                    return;
                }
                if ((InitialMouseLocation.X == rectangle.BottomRight.X) &&
                     rectangle.TopLeft.Y < InitialMouseLocation.Y &&
                     InitialMouseLocation.Y < rectangle.BottomRight.Y)
                {
                    TargetObject = rectangle;
                    InitialTargetObjectProperty = rectangle.BottomRight;
                    LineMotionMode = 4;
                    isLocked = true;
                    return;
                }
            }
        }

        public void LockTargetRectangle_MoveRectangle(Database database)
        {
            foreach (Rectangle rectangle in database.Rectangles)
            {
                if ((InitialMouseLocation.Y == rectangle.TopLeft.Y) &&
                     rectangle.TopLeft.X < InitialMouseLocation.X &&
                     InitialMouseLocation.X < rectangle.BottomRight.X)
                {
                    TargetObject = rectangle;
                    InitialTargetObjectProperty = rectangle.TopLeft;
                    InitialTargetObjectProperty2 = rectangle.BottomRight;
                    isLocked = true;
                    return;
                }
                if ((InitialMouseLocation.Y == rectangle.BottomRight.Y) &&
                     rectangle.TopLeft.X < InitialMouseLocation.X &&
                     InitialMouseLocation.X < rectangle.BottomRight.X)
                {
                    TargetObject = rectangle;
                    InitialTargetObjectProperty = rectangle.TopLeft;
                    InitialTargetObjectProperty2 = rectangle.BottomRight;
                    isLocked = true;
                    return;
                }
                if ((InitialMouseLocation.X == rectangle.TopLeft.X) &&
                     rectangle.TopLeft.Y < InitialMouseLocation.Y &&
                     InitialMouseLocation.Y < rectangle.BottomRight.Y)
                {
                    TargetObject = rectangle;
                    InitialTargetObjectProperty = rectangle.TopLeft;
                    InitialTargetObjectProperty2 = rectangle.BottomRight;
                    isLocked = true;
                    return;
                }
                if ((InitialMouseLocation.X == rectangle.BottomRight.X) &&
                     rectangle.TopLeft.Y < InitialMouseLocation.Y &&
                     InitialMouseLocation.Y < rectangle.BottomRight.Y)
                {
                    TargetObject = rectangle;
                    InitialTargetObjectProperty = rectangle.TopLeft;
                    InitialTargetObjectProperty2 = rectangle.BottomRight;
                    isLocked = true;
                    return;
                }
            }
        }

        public void MoveTargetObject(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (TargetObject is Circle)
                {
                    ((Circle)TargetObject).Center = new Point(((Point)InitialTargetObjectProperty).X + Difference(e.Location).X,
                                                              ((Point)InitialTargetObjectProperty).Y + Difference(e.Location).Y);
                }
                if (TargetObject is Line)
                {
                    if (LineMotionMode == 1)
                    {
                        ((Line)TargetObject).StartPoint = new Point(((Point)InitialTargetObjectProperty).X + Difference(e.Location).X,
                                                                    ((Point)InitialTargetObjectProperty).Y + Difference(e.Location).Y);
                    }
                    if (LineMotionMode == 2)
                    {
                        ((Line)TargetObject).EndPoint = new Point(((Point)InitialTargetObjectProperty).X + Difference(e.Location).X,
                                                                  ((Point)InitialTargetObjectProperty).Y + Difference(e.Location).Y);
                    }
                }
                if (TargetObject is Polygon)
                {
                    ((Polygon)TargetObject).Points = ((List<Point>)InitialTargetObjectProperty).Select(p =>
                    {
                        return new Point(p.X + Difference(e.Location).X, p.Y + Difference(e.Location).Y);
                    }).ToList();
                }
                if (TargetObject is Rectangle)
                {
                    if (LineMotionMode == 1)
                    {
                        ((Rectangle)TargetObject).TopLeft = new Point(((Point)InitialTargetObjectProperty).X + Difference(e.Location).X,
                                                                      ((Point)InitialTargetObjectProperty).Y + Difference(e.Location).Y);
                    }
                    if (LineMotionMode == 2)
                    {
                        ((Rectangle)TargetObject).BottomRight = new Point(((Point)InitialTargetObjectProperty).X + Difference(e.Location).X,
                                                                          ((Point)InitialTargetObjectProperty).Y + Difference(e.Location).Y);
                    }
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                if (LineMotionMode == 1)
                {
                    ((Rectangle)TargetObject).TopLeft = new Point(((Point)InitialTargetObjectProperty).X,
                                                                  ((Point)InitialTargetObjectProperty).Y + Difference(e.Location).Y);
                }
                if (LineMotionMode == 2)
                {
                    ((Rectangle)TargetObject).BottomRight = new Point(((Point)InitialTargetObjectProperty).X,
                                                                      ((Point)InitialTargetObjectProperty).Y + Difference(e.Location).Y);
                }
                if (LineMotionMode == 3)
                {
                    ((Rectangle)TargetObject).TopLeft = new Point(((Point)InitialTargetObjectProperty).X + Difference(e.Location).X,
                                                                  ((Point)InitialTargetObjectProperty).Y);
                }
                if (LineMotionMode == 4)
                {
                    ((Rectangle)TargetObject).BottomRight = new Point(((Point)InitialTargetObjectProperty).X + Difference(e.Location).X,
                                                                      ((Point)InitialTargetObjectProperty).Y);
                }
            }
            if (e.Button == MouseButtons.Middle)
            {
                ((Rectangle)TargetObject).TopLeft = new Point(((Point)InitialTargetObjectProperty).X + Difference(e.Location).X,
                                                              ((Point)InitialTargetObjectProperty).Y + Difference(e.Location).Y);
                ((Rectangle)TargetObject).BottomRight = new Point(((Point)InitialTargetObjectProperty2).X + Difference(e.Location).X,
                                                                  ((Point)InitialTargetObjectProperty2).Y + Difference(e.Location).Y);

            }
        }

        public void DeleteRectangle(Database database)
        {
            foreach (Rectangle rectangle in database.Rectangles)
            {
                if ((InitialMouseLocation.Y == rectangle.TopLeft.Y) &&
                     rectangle.TopLeft.X < InitialMouseLocation.X &&
                     InitialMouseLocation.X < rectangle.BottomRight.X)
                {
                    TargetObject = rectangle;
                    break;
                }
                if ((InitialMouseLocation.Y == rectangle.BottomRight.Y) &&
                     rectangle.TopLeft.X < InitialMouseLocation.X &&
                     InitialMouseLocation.X < rectangle.BottomRight.X)
                {
                    TargetObject = rectangle;
                    break;
                }
                if ((InitialMouseLocation.X == rectangle.TopLeft.X) &&
                     rectangle.TopLeft.Y < InitialMouseLocation.Y &&
                     InitialMouseLocation.Y < rectangle.BottomRight.Y)
                {
                    TargetObject = rectangle;
                    break;
                }
                if ((InitialMouseLocation.X == rectangle.BottomRight.X) &&
                     rectangle.TopLeft.Y < InitialMouseLocation.Y &&
                     InitialMouseLocation.Y < rectangle.BottomRight.Y)
                {
                    TargetObject = rectangle;
                    break;
                }
            }

            database.Rectangles.Remove((Rectangle)TargetObject);
        }
    }
}
