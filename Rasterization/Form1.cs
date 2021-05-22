﻿using System;
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

            pictureBox1.Image = Redrawer();
        }

        private void clearCanvasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
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
            if (CanvasLogic.tmpPolygon.Antialiasing == false)
            {
                pictureBox1.Image = PolygonDrawing.DrawPolygon_DDA(CanvasLogic.tmpPolygon, pictureBox1.Image);
            }
            if (CanvasLogic.tmpPolygon.Antialiasing == true)
            {
                pictureBox1.Image = PolygonDrawing.DrawPolygon_Antialiasing(CanvasLogic.tmpPolygon, pictureBox1.Image);
            }

            Database.Polygons.Add(CanvasLogic.tmpPolygon);
            CanvasLogic.tmpPolygon = new Polygon();
            CanvasLogic.DrawingMode = 0;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
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
                        e.Y - GetValueFromTextBox(textBox1) > 0)
                    {
                        CanvasLogic.tmpCircle.Center = new Point(e.X, e.Y);
                        CanvasLogic.tmpCircle.Radius = int.Parse(textBox1.Text.ToString());
                        CanvasLogic.tmpCircle.Color = GetSelectedColor();
                        CanvasLogic.tmpCircle.Antialiasing = checkBox2.Checked ? true : false;

                        if (CanvasLogic.tmpCircle.Antialiasing)
                        {
                            pictureBox1.Image = CircleDrawing.WuCircle(CanvasLogic.tmpCircle, pictureBox1.Image);
                        }
                        else
                        {
                            pictureBox1.Image = CircleDrawing.MidpointCircle(CanvasLogic.tmpCircle, pictureBox1.Image);
                        }

                        Database.Circles.Add(CanvasLogic.tmpCircle);
                        CanvasLogic.tmpCircle = new Circle();
                        CanvasLogic.DrawingMode = 0;
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

                        if (CanvasLogic.tmpLine.Antialiasing == false &&
                            comboBox2.SelectedIndex == 0)
                        {
                            pictureBox1.Image = LineDrawing.lineDDA(CanvasLogic.tmpLine, pictureBox1.Image);
                        }
                        else if (CanvasLogic.tmpLine.Antialiasing == false &&
                                 comboBox2.SelectedIndex != 0)
                        {
                            pictureBox1.Image = LineDrawing.lineDDA_thick(CanvasLogic.tmpLine, pictureBox1.Image);
                        }
                        else if (CanvasLogic.tmpLine.Antialiasing == true &&
                                 comboBox2.SelectedIndex == 0)
                        {
                            pictureBox1.Image = LineDrawing.WuLine(CanvasLogic.tmpLine, pictureBox1.Image);
                        }

                        Database.Lines.Add(CanvasLogic.tmpLine);
                        CanvasLogic.tmpLine = new Line();
                        CanvasLogic.DrawingMode = 0;
                    }
                    break;
                case 3:
                    CanvasLogic.tmpPolygon.Points.Add(new Point(e.X, e.Y));
                    break;
                default:
                    break;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (ObjectMotion.isLocked)
            {
                ObjectMotion.MoveTargetObject(e.Location);
                pictureBox1.Image = Redrawer();
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
            return int.Parse(textBox.Text.ToString());
        }

        private Image Redrawer()
        {
            Image tmpImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            foreach (Circle circle in Database.Circles)
            {
                if (circle.Antialiasing == true)
                {
                    tmpImage = CircleDrawing.WuCircle(circle, tmpImage);
                }
                else
                {
                    tmpImage = CircleDrawing.MidpointCircle(circle, tmpImage);
                }
            }
            foreach (Line line in Database.Lines)
            {
                if (line.Antialiasing == false && line.Thickness == 1)
                {
                    tmpImage = LineDrawing.lineDDA(line, tmpImage);
                }
                if (line.Antialiasing == false && line.Thickness != 1)
                {
                    tmpImage = LineDrawing.lineDDA_thick(line, tmpImage);
                }
                if (line.Antialiasing = true && line.Thickness == 1)
                {
                    tmpImage = LineDrawing.WuLine(line, tmpImage);
                }
            }
            foreach (Polygon polygon in Database.Polygons)
            {
                if (polygon.Antialiasing == true)
                {
                    tmpImage = PolygonDrawing.DrawPolygon_Antialiasing(polygon, tmpImage);
                }
                else
                {
                    tmpImage = PolygonDrawing.DrawPolygon_DDA(polygon, tmpImage);
                }
            }

            return tmpImage;
        }
    }
}
