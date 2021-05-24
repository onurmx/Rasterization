using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace Rasterization
{
    public partial class Form1 : Form
    {
        Database Database = new Database();

        CanvasLogic CanvasLogic = new CanvasLogic();
        ObjectMotion ObjectMotion = new ObjectMotion();

        CircleDrawing CircleDrawing = new CircleDrawing();
        LineDrawing LineDrawing = new LineDrawing();
        PolygonDrawing PolygonDrawing = new PolygonDrawing();
        RectangleDrawing RectangleDrawing = new RectangleDrawing();
        Filling Filling = new Filling();
        Clipping Clipping = new Clipping();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            comboBox1.Items.Add("Black");
            comboBox1.Items.Add("Red");
            comboBox1.Items.Add("Green");
            comboBox1.Items.Add("Blue");
            comboBox1.SelectedIndex = 0;

            comboBox2.Items.Add("1");
            comboBox2.Items.Add("3");
            comboBox2.Items.Add("5");
            comboBox2.Items.Add("7");
            comboBox2.SelectedIndex = 0;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Serialization serialization = new Serialization();
            serialization.Formatter(Database.Circles, Database.Lines, Database.Polygons);
            XmlSerializer serializer = new XmlSerializer(typeof(ShapesSerializableStruct));
            TextWriter filestream = new StreamWriter(@".\output.xml");
            serializer.Serialize(filestream, serialization.shapesSerializableStruct);
            filestream.Close();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Serialization serialization = new Serialization();
            XmlSerializer serializer = new XmlSerializer(typeof(ShapesSerializableStruct));
            TextReader filestream = new StreamReader(@".\output.xml");

            ShapesSerializableStruct shapesSerializableStruct = (ShapesSerializableStruct)serializer.Deserialize(filestream);
            filestream.Close();

            serialization.DeFormatter(shapesSerializableStruct);
            Database.Circles = serialization.circles;
            Database.Lines = serialization.lines;
            Database.Polygons = serialization.polygons;

            Redrawer();
        }

        private void clearCanvasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Database.Circles.Clear();
            Database.Lines.Clear();
            Database.Polygons.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CanvasLogic.DrawingMode = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CanvasLogic.DrawingMode = 2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CanvasLogic.DrawingMode = 3;
            CanvasLogic.tmpPolygon.Points = new List<Point>();
            CanvasLogic.tmpPolygon.Color = GetSelectedColor();
            CanvasLogic.tmpPolygon.Antialiasing = checkBox3.Checked ? true : false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Database.Polygons.Add(CanvasLogic.tmpPolygon);
            CanvasLogic.tmpPolygon = new Polygon();
            CanvasLogic.DrawingMode = 0;

            Redrawer();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CanvasLogic.DrawingMode = 4;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Clipping.LiangBarskyClipping(Database.Lines[0].StartPoint,
                                                             Database.Lines[0].EndPoint,
                                                             Database.Rectangles[0],
                                                             pictureBox1.Image);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (CanvasLogic.DrawingMode)
                {
                    case 0:
                        ObjectMotion.InitialMouseLocation = e.Location;
                        ObjectMotion.LockTargetObject(Database);
                        break;
                    case 1:
                        if (e.X + GetValueFromTextBox(textBox1) < pictureBox1.Width &&
                            e.X - GetValueFromTextBox(textBox1) > 0 &&
                            e.Y + GetValueFromTextBox(textBox1) < pictureBox1.Height &&
                            e.Y - GetValueFromTextBox(textBox1) > 0 &&
                            textBox1.Text != "")
                        {
                            CanvasLogic.tmpCircle.Center = new Point(e.X, e.Y);
                            CanvasLogic.tmpCircle.Radius = int.Parse(textBox1.Text.ToString());
                            CanvasLogic.tmpCircle.Color = GetSelectedColor();
                            CanvasLogic.tmpCircle.Antialiasing = checkBox2.Checked ? true : false;

                            Database.Circles.Add(CanvasLogic.tmpCircle);
                            CanvasLogic.tmpCircle = new Circle();
                            CanvasLogic.DrawingMode = 0;

                            Redrawer();
                        }
                        break;
                    case 2:
                        if (CanvasLogic.tmpLine.StartPoint == ((new Line()).StartPoint))
                        {
                            CanvasLogic.tmpLine.StartPoint = new Point(e.X, e.Y);
                        }
                        else
                        {
                            CanvasLogic.tmpLine.EndPoint = new Point(e.X, e.Y);
                            CanvasLogic.tmpLine.Color = GetSelectedColor();
                            CanvasLogic.tmpLine.Thickness = int.Parse(comboBox2.Items[comboBox2.SelectedIndex].ToString());
                            CanvasLogic.tmpLine.Antialiasing = checkBox1.Checked ? true : false;

                            Database.Lines.Add(CanvasLogic.tmpLine);
                            CanvasLogic.tmpLine = new Line();
                            CanvasLogic.DrawingMode = 0;

                            Redrawer();
                        }
                        break;
                    case 3:
                        CanvasLogic.tmpPolygon.Points.Add(new Point(e.X, e.Y));
                        break;
                    case 4:
                        if (CanvasLogic.tmpRectangle.TopLeft == (new Rectangle()).TopLeft)
                        {
                            CanvasLogic.tmpRectangle.TopLeft = e.Location;
                        }
                        else
                        {
                            CanvasLogic.tmpRectangle.BottomRight = e.Location;
                            CanvasLogic.tmpRectangle.Color = GetSelectedColor();

                            Database.Rectangles.Add(CanvasLogic.tmpRectangle);
                            CanvasLogic.tmpRectangle = new Rectangle();
                            CanvasLogic.DrawingMode = 0;

                            Redrawer();
                        }
                        break;
                    default:
                        break;
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                ObjectMotion.InitialMouseLocation = e.Location;
                ObjectMotion.LockTargetRectangle_MoveVertice(Database);
            }
            if (e.Button == MouseButtons.Middle)
            {
                ObjectMotion.InitialMouseLocation = e.Location;
                ObjectMotion.LockTargetRectangle_MoveRectangle(Database);
            }
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ObjectMotion.InitialMouseLocation = e.Location;
                ObjectMotion.DeleteRectangle(Database);

                ObjectMotion = new ObjectMotion();
                Redrawer();
            }
            if (e.Button == MouseButtons.Right)
            {
                foreach (Polygon polygon in Database.Polygons)
                {
                    foreach (Point point in polygon.Points)
                    {
                        if (point == e.Location)
                        {
                            polygon.FillColor = GetSelectedColor();
                            Redrawer();
                            break;
                        }
                    }
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (ObjectMotion.isLocked &&
                0 < e.Location.X &&
                e.Location.X < pictureBox1.Width &&
                0 < e.Location.Y &&
                e.Location.Y < pictureBox1.Height)
            {
                ObjectMotion.MoveTargetObject(e);
                Redrawer();
            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            switch (CanvasLogic.DrawingMode)
            {
                case 0:
                    ObjectMotion = new ObjectMotion();
                    break;
                default:
                    break;
            }
        }

        private Color GetSelectedColor()
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    return Color.FromArgb(255, 0, 0, 0);
                case 1:
                    return Color.FromArgb(255, 255, 0, 0);
                case 2:
                    return Color.FromArgb(255, 0, 255, 0);
                case 3:
                    return Color.FromArgb(255, 0, 0, 255);
                default:
                    return Color.FromArgb(255, 0, 0, 0);
            }
        }

        private int GetValueFromTextBox(TextBox textBox)
        {
            if (textBox.Text != "")
            {
                return int.Parse(textBox.Text.ToString());
            }
            return 0;
        }

        private void Redrawer()
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            foreach (Circle circle in Database.Circles)
            {
                switch (circle.Antialiasing)
                {
                    case false:
                        CircleDrawing.MidpointCircle(circle, bitmap);
                        break;
                    case true:
                        CircleDrawing.WuCircle(circle, bitmap);
                        break;
                    default:
                        break;
                }
            }
            foreach (Line line in Database.Lines)
            {
                switch (line.Antialiasing)
                {
                    case false:
                        switch (line.Thickness)
                        {
                            case 1:
                                LineDrawing.lineDDA(line, bitmap);
                                break;
                            default:
                                LineDrawing.lineDDA_thick(line, bitmap);
                                break;
                        }
                        break;
                    case true:
                        LineDrawing.WuLine(line, bitmap);
                        break;
                    default:
                        break;
                }
            }
            foreach (Polygon polygon in Database.Polygons)
            {
                switch (polygon.Antialiasing)
                {
                    case false:
                        PolygonDrawing.DrawPolygon_DDA(polygon, bitmap);
                        if (polygon.FillColor != (new Polygon()).FillColor)
                        {
                            Filling.FillPolygon(polygon, bitmap);
                        }
                        break;
                    case true:
                        PolygonDrawing.DrawPolygon_Antialiasing(polygon, bitmap);
                        break;
                    default:
                        break;
                }
            }
            foreach (Rectangle rectangle in Database.Rectangles)
            {
                RectangleDrawing.DrawRectangle_DDA(rectangle, bitmap);
            }

            pictureBox1.Image = bitmap;
        }
    }
}
