using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rasterization
{
    public struct CircleSerializableStruct
    {
        public Point Center;
        public int Radius;
        public int cA;
        public int cR;
        public int cG;
        public int cB;
        public bool Antialiasing;
    }

    public struct LineSerializableStruct
    {
        public Point StartPoint;
        public Point EndPoint;
        public int cA;
        public int cR;
        public int cG;
        public int cB;
        public int Thickness;
        public bool Antialiasing;
    }

    public struct PolygonSerializableStruct
    {
        public List<Point> Points;
        public int cA;
        public int cR;
        public int cG;
        public int cB;
        public int fA;
        public int fR;
        public int fG;
        public int fB;
        public bool FillBackgroundImage;
        public bool Antialiasing;
    }

    public struct RectangleSerializableStruct
    {
        public Point TopLeft;
        public Point BottomRight;
        public int cA;
        public int cR;
        public int cG;
        public int cB;
    }

    public struct ShapesSerializableStruct
    {
        public List<CircleSerializableStruct> circleSerializableStructs;
        public List<LineSerializableStruct> lineSerializableStructs;
        public List<PolygonSerializableStruct> polygonSerializableStructs;
        public List<RectangleSerializableStruct> rectangleSerializableStructs;
    }

    public class Serialization
    {
        public ShapesSerializableStruct shapesSerializableStruct = new ShapesSerializableStruct();

        public List<Circle> circles = new List<Circle>();
        public List<Line> lines = new List<Line>();
        public List<Polygon> polygons = new List<Polygon>();
        public List<Rectangle> rectangles = new List<Rectangle>();

        public void Formatter(List<Circle> circles,
                              List<Line> lines,
                              List<Polygon> polygons,
                              List<Rectangle> rectangles)
        {
            shapesSerializableStruct.circleSerializableStructs = new List<CircleSerializableStruct>();
            shapesSerializableStruct.lineSerializableStructs = new List<LineSerializableStruct>();
            shapesSerializableStruct.polygonSerializableStructs = new List<PolygonSerializableStruct>();
            shapesSerializableStruct.rectangleSerializableStructs = new List<RectangleSerializableStruct>();

            foreach (Circle circle in circles)
            {
                CircleSerializableStruct circleSerializableStruct = new CircleSerializableStruct();
                circleSerializableStruct.Center = circle.Center;
                circleSerializableStruct.Radius = circle.Radius;
                circleSerializableStruct.cA = circle.Color.A;
                circleSerializableStruct.cR = circle.Color.R;
                circleSerializableStruct.cG = circle.Color.G;
                circleSerializableStruct.cB = circle.Color.B;
                circleSerializableStruct.Antialiasing = circle.Antialiasing;

                shapesSerializableStruct.circleSerializableStructs.Add(circleSerializableStruct);
            }

            foreach (Line line in lines)
            {
                LineSerializableStruct lineSerializableStruct = new LineSerializableStruct();
                lineSerializableStruct.StartPoint = line.StartPoint;
                lineSerializableStruct.EndPoint = line.EndPoint;
                lineSerializableStruct.cA = line.Color.A;
                lineSerializableStruct.cR = line.Color.R;
                lineSerializableStruct.cG = line.Color.G;
                lineSerializableStruct.cB = line.Color.B;
                lineSerializableStruct.Thickness = line.Thickness;
                lineSerializableStruct.Antialiasing = line.Antialiasing;

                shapesSerializableStruct.lineSerializableStructs.Add(lineSerializableStruct);
            }

            foreach (Polygon polygon in polygons)
            {
                PolygonSerializableStruct polygonSerializableStruct = new PolygonSerializableStruct();
                polygonSerializableStruct.Points = polygon.Points;
                polygonSerializableStruct.cA = polygon.Color.A;
                polygonSerializableStruct.cR = polygon.Color.R;
                polygonSerializableStruct.cG = polygon.Color.G;
                polygonSerializableStruct.cB = polygon.Color.B;
                polygonSerializableStruct.fA = polygon.FillColor.A;
                polygonSerializableStruct.fR = polygon.FillColor.R;
                polygonSerializableStruct.fG = polygon.FillColor.G;
                polygonSerializableStruct.fB = polygon.FillColor.B;
                polygonSerializableStruct.FillBackgroundImage = polygon.FillBackgroundImage;
                polygonSerializableStruct.Antialiasing = polygon.Antialiasing;

                shapesSerializableStruct.polygonSerializableStructs.Add(polygonSerializableStruct);
            }

            foreach (Rectangle rectangle in rectangles)
            {
                RectangleSerializableStruct rectangleSerializableStruct = new RectangleSerializableStruct();
                rectangleSerializableStruct.TopLeft = rectangle.TopLeft;
                rectangleSerializableStruct.BottomRight = rectangle.BottomRight;
                rectangleSerializableStruct.cA = rectangle.Color.A;
                rectangleSerializableStruct.cR = rectangle.Color.R;
                rectangleSerializableStruct.cG = rectangle.Color.G;
                rectangleSerializableStruct.cB = rectangle.Color.B;
            }
        }

        public void DeFormatter(ShapesSerializableStruct shapesSerializableStruct)
        {
            foreach (CircleSerializableStruct circleSerializableStruct in shapesSerializableStruct.circleSerializableStructs)
            {
                Circle circle = new Circle();
                circle.Center = circleSerializableStruct.Center;
                circle.Radius = circleSerializableStruct.Radius;
                circle.Color = Color.FromArgb(circleSerializableStruct.cA,
                                              circleSerializableStruct.cR,
                                              circleSerializableStruct.cG,
                                              circleSerializableStruct.cB);
                circle.Antialiasing = circleSerializableStruct.Antialiasing;

                circles.Add(circle);
            }

            foreach (LineSerializableStruct lineSerializableStruct in shapesSerializableStruct.lineSerializableStructs)
            {
                Line line = new Line();
                line.StartPoint = lineSerializableStruct.StartPoint;
                line.EndPoint = lineSerializableStruct.EndPoint;
                line.Color = Color.FromArgb(lineSerializableStruct.cA,
                                            lineSerializableStruct.cR,
                                            lineSerializableStruct.cG,
                                            lineSerializableStruct.cB);
                line.Thickness = lineSerializableStruct.Thickness;
                line.Antialiasing = lineSerializableStruct.Antialiasing;

                lines.Add(line);
            }

            foreach (PolygonSerializableStruct polygonSerializableStruct in shapesSerializableStruct.polygonSerializableStructs)
            {
                Polygon polygon = new Polygon();
                polygon.Points = polygonSerializableStruct.Points;
                polygon.Color = Color.FromArgb(polygonSerializableStruct.cA,
                                               polygonSerializableStruct.cR,
                                               polygonSerializableStruct.cG,
                                               polygonSerializableStruct.cB);
                polygon.FillColor = Color.FromArgb(polygonSerializableStruct.fA,
                                                   polygonSerializableStruct.fR,
                                                   polygonSerializableStruct.fG,
                                                   polygonSerializableStruct.fB);
                polygon.FillBackgroundImage = polygonSerializableStruct.FillBackgroundImage;
                polygon.Antialiasing = polygonSerializableStruct.Antialiasing;

                polygons.Add(polygon);
            }

            foreach (RectangleSerializableStruct rectangleSerializableStruct in shapesSerializableStruct.rectangleSerializableStructs)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.TopLeft = rectangleSerializableStruct.TopLeft;
                rectangle.BottomRight = rectangleSerializableStruct.BottomRight;
                rectangle.Color = Color.FromArgb(rectangleSerializableStruct.cA,
                                                 rectangleSerializableStruct.cR,
                                                 rectangleSerializableStruct.cG,
                                                 rectangleSerializableStruct.cB);

                rectangles.Add(rectangle);
            }
        }
    }
}
